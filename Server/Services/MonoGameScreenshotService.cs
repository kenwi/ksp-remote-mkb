using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Services
{
    public class MonoGameScreenshotService : Game, IHostedService
    {
        GraphicsDeviceManager? graphics = null;
        Texture2D? backbufferTexture = null;
        int[]? backBuffer = null;
        int width, height;

        public MonoGameScreenshotService()
        {
            Task.Run(() =>
            {
                Run();
            });
        }

        protected override void Initialize()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferHeight = 1600;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();

            (width, height) = (GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            
            backbufferTexture = new Texture2D(GraphicsDevice, width, height);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        bool hasRenderedFirstFrame = false;
        protected override void Draw(GameTime gameTime)
        {
            if (hasRenderedFirstFrame)
            {
                backBuffer = new int[width * height];
                GraphicsDevice.GetBackBufferData(backBuffer);
                backbufferTexture.SetData(backBuffer);
                using var stream = File.OpenWrite("foo.jpg");
                backbufferTexture.SaveAsJpeg(stream, width, height);
                throw new Exception("Done");
                ;
                //GraphicsDevice.GetBackBufferData(backbufferTexture, 0, backBuffer.Length, backBuffer);
            }
                       
            base.Draw(gameTime);
            if(!hasRenderedFirstFrame)
                hasRenderedFirstFrame = true;
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