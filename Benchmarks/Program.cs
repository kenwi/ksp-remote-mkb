using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Server;
using System.Drawing;

BenchmarkRunner.Run<Screenshot>();

public class Screenshot
{
    ScreenCapture sc = new ScreenCapture();
    Graphics graphics = null;

    [GlobalSetup]
    public void Setup()
    {
        using var img = sc.CaptureScreen();
        graphics = Graphics.FromImage(img);
    }

    [Benchmark]
    public MemoryStream CaptureImage()
    {
        using var stream = new MemoryStream();
        using var img = sc.CaptureScreen();
        graphics.DrawImage(img, 160, 120);
        img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        return stream;
    }
}