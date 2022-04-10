using System.Drawing;

namespace Server.Services
{
    public class ScreenshotService : BackgroundService
    {
        public string Image64 { get; set; } = string.Empty;

        ScreenCapture sc = new();

        public ScreenshotService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                using var outStream = new MemoryStream();
                using var img = sc.CaptureScreen();
                using var resized = new Bitmap(img, 1600, 900);
                resized.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                Image64 = "data:image/jpeg;base64," + Convert.ToBase64String(outStream.ToArray());
                await Task.Delay(100);
            }
        }
    }
}