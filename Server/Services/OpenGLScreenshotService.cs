using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Services
{
    public class OpenGLScreenshotService : Game, IHostedService
    {
        GraphicsDeviceManager? graphics;

        public OpenGLScreenshotService()
        {
            Task.Run(() =>
            {
                graphics = new GraphicsDeviceManager(this);
                Run();
            });
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

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