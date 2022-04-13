using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;

namespace Server.Services
{

    public class OpenGLScreenshotService : Game, IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}