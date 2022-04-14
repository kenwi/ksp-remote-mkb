using System.Drawing;

namespace Server.Services
{
    public class ScreenshotService : BackgroundService, IScreenshotService
    {
        public string Image64 { get; set; } = string.Empty;

        ScreenCapture sc = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Graphics? graphics = null;
            while (true)
            {
                using var outStream = new MemoryStream();
                using var img = sc.CaptureScreen();

                if(graphics is null)
                    graphics = Graphics.FromImage(img);

                graphics.DrawImage(img, 160, 120);
                img.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                //using var resized = new Bitmap(img, 1600, 900);
                //resized.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                Image64 = "data:image/jpeg;base64," + Convert.ToBase64String(outStream.ToArray());
                await Task.Delay(1, stoppingToken);
            }
        }
    }
}