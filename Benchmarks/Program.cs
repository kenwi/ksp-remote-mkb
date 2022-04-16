using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Server;
using Server.Services;

#if DEBUG

var imageCapture = new DirectXImageCapture();
imageCapture.Initialize();
imageCapture.Capture();
;

#else
BenchmarkRunner.Run<Screenshot>();
#endif

public class Screenshot
{
    DirectXImageCapture ds = new();
    Gdi32ImageCapture sc = new();
    MemoryStream memoryStream = new();
    Graphics graphics = null;
    Image? image;

    [GlobalSetup]
    public void Setup()
    {
        ds.Initialize();

        using var img = sc.CaptureScreen();
        graphics = Graphics.FromImage(img);
        img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        image = img;
    }

    [Benchmark]
    public void CaptureImageDirectX()
    {
        ds.Capture();
    }

    [Benchmark]
    public void CaptureImageGdi32()
    {
        using var stream = new MemoryStream();
        using var img = sc.CaptureScreen();
        graphics.DrawImage(img, 1600, 1200);
        img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
    }


}
