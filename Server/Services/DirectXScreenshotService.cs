using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

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

        public string Image64 { get; set; } = string.Empty;
        public int FrameRate { get; set; } = 100;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger?.LogInformation($"Starting service at targeted rate of {FrameRate} ms");
            while (true)
            {
                await imageCapture.Capture();
                await Task.Delay(FrameRate, stoppingToken);
            }
        }
    }
