using System.Drawing;
using System.Drawing.Imaging;

namespace Server.Services
{
    public class DirectXScreenshotService : BackgroundService, IScreenshotService
    {
        private readonly ILogger<DirectXScreenshotService>? logger;
        private readonly DirectXImageCapture imageCapture = new();

        public DirectXScreenshotService(ILogger<DirectXScreenshotService>? logger)
        {
            this.logger = logger;
            imageCapture.Initialize();
            logger?.LogInformation($"Initialized");
        }

        public int FrameRate { get; set; } = 25;
        public int FrameCount => imageCapture.FrameCount;
        public string Image64 { get; set; }

        public Bitmap Bitmap { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger?.LogInformation($"Starting service at targeted rate of {FrameRate} ms");
            while (true)
            {
                await imageCapture.Capture();
                using var stream = new MemoryStream();
                using var bitmap = imageCapture.Bitmap;
                bitmap.Save(stream, ImageFormat.Jpeg);
                Image64 = "data:image/jpeg;base64," + Convert.ToBase64String(stream.ToArray());
                await Task.Delay(FrameRate, stoppingToken);
            }
        }
    }
}
