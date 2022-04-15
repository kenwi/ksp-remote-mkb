using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace Server
{
    public class DirectXImageCapture : ImageCapture
    {
        protected internal Device? Device;
        protected internal Texture2D? StagingTexture;
        protected internal Texture2D? BackingTexture;
        protected internal OutputDuplication? DuplicatedOutput;
        private Bitmap bitmap;
        private Rectangle boundsRect;
        protected internal DisplayModeRotation? DisplayRotation;
        protected internal Texture2D? TransformTexture;

        public Task Initialize()
        {
            Device = new Device(DriverType.Hardware, DeviceCreationFlags.VideoSupport);
            var multiThread = Device.QueryInterface<Multithread>();
            multiThread.SetMultithreadProtected(true);

            using var factory = new Factory1();
            using var output = GetOutput(factory);

            Width = output.Description.DesktopBounds.Right;
            Height = output.Description.DesktopBounds.Bottom;
            DuplicatedOutput = output.DuplicateOutput(Device);

            bitmap = new System.Drawing.Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            boundsRect = new Rectangle(0, 0, Width, Height);

            //Texture used to copy contents from the GPU to be accesible by the CPU.
            StagingTexture = new Texture2D(Device, new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                Format = Format.B8G8R8A8_UNorm,
                Width = Width,
                Height = Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Staging
            });

            //Texture that is used to recieve the pixel data from the GPU.
            BackingTexture = new Texture2D(Device, new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = Width,
                Height = Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            });

            return Task.CompletedTask;
        }

        private Output1 GetOutput(Factory1 factory)
        {
            try
            {
                //Gets the output with the bigger area being intersected.
                var output = factory.Adapters1.SelectMany(s => s.Outputs).FirstOrDefault(f => f.Description.DeviceName == DeviceName) ??
                             factory.Adapters1.SelectMany(s => s.Outputs).OrderByDescending(f =>
                             {
                                 var x = Math.Max(Left, f.Description.DesktopBounds.Left);
                                 var num1 = Math.Min(Left + Width, f.Description.DesktopBounds.Right);
                                 var y = Math.Max(Top, f.Description.DesktopBounds.Top);
                                 var num2 = Math.Min(Top + Height, f.Description.DesktopBounds.Bottom);

                                 if (num1 >= x && num2 >= y)
                                     return num1 - x + num2 - y;

                                 return 0;
                             }).FirstOrDefault();

                if (output == null)
                    throw new Exception($"Could not find a proper output device for the area of L: {Left}, T: {Top}, Width: {Width}, Height: {Height}.");

                //Position adjustments, so the correct region is captured.
                OffsetLeft = output.Description.DesktopBounds.Left;
                OffsetTop = output.Description.DesktopBounds.Top;
                DisplayRotation = output.Description.Rotation;

                if (DisplayRotation != DisplayModeRotation.Identity)
                {
                    //Texture that is used to recieve the pixel data from the GPU.
                    TransformTexture = new Texture2D(Device, new Texture2DDescription
                    {
                        ArraySize = 1,
                        BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                        CpuAccessFlags = CpuAccessFlags.None,
                        Format = Format.B8G8R8A8_UNorm,
                        Width = Height,
                        Height = Width,
                        OptionFlags = ResourceOptionFlags.None,
                        MipLevels = 1,
                        SampleDescription = new SampleDescription(1, 0),
                        Usage = ResourceUsage.Default
                    });
                }

                //Create textures in here, after detecting the orientation?

                return output.QueryInterface<Output1>();
            }
            catch (SharpDXException ex)
            {
                throw new Exception("Could not find the specified output device.", ex);
            }
        }

        public Task Capture()
        {
            var res = new Result(-1);

            try
            {
                //Try to get the duplicated output frame within given time.
                res = DuplicatedOutput.TryAcquireNextFrame(0, out var info, out var resource);

                if (FrameCount == 0 && (res.Failure || resource == null))
                {
                    //Somehow, it was not possible to retrieve the resource, frame or metadata.
                    resource?.Dispose();
                    return Task.CompletedTask;
                    //return FrameCount;
                }

                #region Process changes

                //Something on screen was moved or changed.
                if (info.TotalMetadataBufferSize > 0)
                {
                    //Copy resource into memory that can be accessed by the CPU.
                    using (var screenTexture = resource.QueryInterface<Texture2D>())
                    {
                        #region Moved rectangles

                        var movedRectangles = new OutputDuplicateMoveRectangle[info.TotalMetadataBufferSize];
                        DuplicatedOutput.GetFrameMoveRects(movedRectangles.Length, movedRectangles, out var movedRegionsLength);

                        for (var movedIndex = 0; movedIndex < movedRegionsLength / Marshal.SizeOf(typeof(OutputDuplicateMoveRectangle)); movedIndex++)
                        {
                            //Crop the destination rectangle to the screen area rectangle.
                            var left = Math.Max(movedRectangles[movedIndex].DestinationRect.Left, Left - OffsetLeft);
                            var right = Math.Min(movedRectangles[movedIndex].DestinationRect.Right, Left + Width - OffsetLeft);
                            var top = Math.Max(movedRectangles[movedIndex].DestinationRect.Top, Top - OffsetTop);
                            var bottom = Math.Min(movedRectangles[movedIndex].DestinationRect.Bottom, Top + Height - OffsetTop);

                            //Copies from the screen texture only the area which the user wants to capture.
                            if (right > left && bottom > top)
                            {
                                //Limit the source rectangle to the available size within the destination rectangle.
                                var sourceWidth = movedRectangles[movedIndex].SourcePoint.X + (right - left);
                                var sourceHeight = movedRectangles[movedIndex].SourcePoint.Y + (bottom - top);

                                Device.ImmediateContext.CopySubresourceRegion(screenTexture, 0,
                                    new ResourceRegion(movedRectangles[movedIndex].SourcePoint.X, movedRectangles[movedIndex].SourcePoint.Y, 0, sourceWidth, sourceHeight, 1),
                                    StagingTexture, 0, left - (Left - OffsetLeft), top - (Top - OffsetTop));
                            }
                        }

                        #endregion

                        #region Dirty rectangles

                        var dirtyRectangles = new RawRectangle[info.TotalMetadataBufferSize];
                        DuplicatedOutput.GetFrameDirtyRects(dirtyRectangles.Length, dirtyRectangles, out var dirtyRegionsLength);

                        for (var dirtyIndex = 0; dirtyIndex < dirtyRegionsLength / Marshal.SizeOf(typeof(RawRectangle)); dirtyIndex++)
                        {
                            //Crop screen positions and size to frame sizes.
                            var left = Math.Max(dirtyRectangles[dirtyIndex].Left, Left - OffsetLeft);
                            var right = Math.Min(dirtyRectangles[dirtyIndex].Right, Left + Width - OffsetLeft);
                            var top = Math.Max(dirtyRectangles[dirtyIndex].Top, Top - OffsetTop);
                            var bottom = Math.Min(dirtyRectangles[dirtyIndex].Bottom, Top + Height - OffsetTop);

                            //Copies from the screen texture only the area which the user wants to capture.
                            if (right > left && bottom > top)
                                Device.ImmediateContext.CopySubresourceRegion(screenTexture, 0, new ResourceRegion(left, top, 0, right, bottom, 1), StagingTexture, 0, left - (Left - OffsetLeft), top - (Top - OffsetTop));
                        }

                        #endregion
                    }
                }

                #endregion

                #region Gets the image data

                //Gets the staging texture as a stream.
                var data = Device.ImmediateContext.MapSubresource(StagingTexture, 0, MapMode.Read, MapFlags.None);

                if (data.IsEmpty)
                {
                    Device.ImmediateContext.UnmapSubresource(StagingTexture, 0);
                    resource?.Dispose();
                    return Task.CompletedTask;
                    //return FrameCount;
                }

                ////Copy pixels from screen capture Texture to the GDI bitmap.
                var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                var sourcePtr = data.DataPointer;
                var destPtr = mapDest.Scan0;

                for (var y = 0; y < Height; y++)
                {
                    //Copy a single line.
                    Utilities.CopyMemory(destPtr, sourcePtr, Width * 4);

                    //Advance pointers.
                    sourcePtr = IntPtr.Add(sourcePtr, data.RowPitch);
                    destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                }

                //Release source and dest locks.
                bitmap.UnlockBits(mapDest);

                //Set frame details.
                FrameCount++;
                //bitmap.Dispose();

                //using var stream = new MemoryStream();

                //using var scaler = new BitmapScaler(sourcePtr);
                //var decoder = new BitmapDecoder(new BitmapDecoderInfo(destPtr));
                //scaler.Initialize(decoder.GetFrame(0), 800, 600, BitmapInterpolationMode.NearestNeighbor);
                //bitmap.Save(stream, ImageFormat.Jpeg);
                //Image64 = "data:image/jpeg;base64," + Convert.ToBase64String(stream.ToArray());

                //bitmap.Save("Yoyo.jpg", ImageFormat.Jpeg);
                //frame.Path = $"{Project.FullPath}{FrameCount}.png";
                //frame.Delay = FrameRate.GetMilliseconds();
                //frame.Image = bitmap;

                //if (IsAcceptingFrames)
                //    BlockingCollection.Add(frame);

                #endregion

                Device.ImmediateContext.UnmapSubresource(StagingTexture, 0);

                resource?.Dispose();
                return Task.CompletedTask;
                //return FrameCount;
            }
            catch (SharpDXException se) when (se.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
            {
                return Task.CompletedTask;
                //return FrameCount;
            }
            catch (SharpDXException se) when (se.ResultCode.Code == SharpDX.DXGI.ResultCode.DeviceRemoved.Result.Code || se.ResultCode.Code == SharpDX.DXGI.ResultCode.DeviceReset.Result.Code)
            {
                //DisposeInternal();
                Initialize();
                //logger?.LogError("Device lost or reset. Reinitiating");
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                //logger?.LogError("It was not possible to finish capturing the frame with DirectX.");
                //MajorCrashHappened = true;
                //if (IsAcceptingFrames)
                //    Application.Current.Dispatcher.Invoke(() => OnError.Invoke(ex));
                return Task.CompletedTask;
            }
            finally
            {
                try
                {
                    //Only release the frame if there was a sucess in capturing it.
                    if (res.Success)
                        DuplicatedOutput.ReleaseFrame();
                }
                catch (Exception)
                {
                    //logger?.LogError("It was not possible to release the frame.");
                }
            }
        }
    }
}
