using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Server;
using Server.Services;

#if DEBUG
    var ds = new DirectXScreenshotService();
    ds.Initialize();
    ds.Capture();
#else
    BenchmarkRunner.Run<DirectXScreenshot>();
    BenchmarkRunner.Run<Gdi32Screenshot>();
#endif

public class DirectXScreenshot
{
    DirectXScreenshotService ds;

    [GlobalSetup]
    public void Setup()
    {
        ds = new DirectXScreenshotService();
        ds.Initialize();
    }
    
    [Benchmark]
    public void CaptureImage()
    {
        ds.Capture();
    }
}

public class Gdi32Screenshot
{
    ScreenCapture sc = new ();
    Graphics graphics = null;
    MemoryStream memoryStream = new();
    Image? image;

    [GlobalSetup]
    public void Setup()
    {
        using var img = sc.CaptureScreen();
        graphics = Graphics.FromImage(img);
        img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        image = img;
    }

    [Benchmark]
    public MemoryStream CaptureImage()
    {
        using var stream = new MemoryStream();
        using var img = sc.CaptureScreen();
        graphics.DrawImage(img, 1600, 1200);
        img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        return stream;
    }
}