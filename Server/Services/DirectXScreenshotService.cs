using System.Drawing;
using System.Drawing.Imaging;

namespace Server.Services
{
    public class DirectXScreenshotService : BackgroundService, IScreenshotService
    {
        public int FrameRate { get; set; } = 25;
        public int FrameCount => imageCapture.FrameCount;
        public string Image64 { get; set; }
        public Bitmap Bitmap { get; set; }
        public byte[] Image { get; set; }
        private readonly ILogger<DirectXScreenshotService>? logger;
        private readonly DirectXImageCapture imageCapture = new();
        private MemoryStream stream = new();

        public DirectXScreenshotService(ILogger<DirectXScreenshotService>? logger)
        {
            this.logger = logger;
            imageCapture.Initialize();
            logger?.LogInformation($"Initialized");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger?.LogInformation($"Starting service at targeted rate of {FrameRate} ms");
            while (true)
            {
                await imageCapture.Capture();
                var bitmap = imageCapture.Bitmap;
                bitmap.Save(stream, ImageFormat.Jpeg);
                var bytes = stream.ToArray();
                (Image, Image64) = (bytes, $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}");
                stream.Position = 0;
                await Task.Delay(FrameRate, stoppingToken);
            }
        }
    }
}
