using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server;
using WindowsInput;
using WindowsInput.Events;

namespace Server.Services
{
    public class RemoteControlService : Greeter.GreeterBase
    {
        private readonly ILogger<RemoteControlService> logger;
        private readonly Empty empty = new();
        private EventBuilder events = Simulate.Events();

        public RemoteControlService(ILogger<RemoteControlService> logger)
        {
            this.logger = logger;
        }

        public override async Task Connect(IAsyncStreamReader<StreamEvent> request, IServerStreamWriter<Empty> response, ServerCallContext context)
        {
            await foreach(var req in request.ReadAllAsync())
            {
                if(req.Type == EventType.Move)
                {
                    events = events.MoveTo(req.Mouse.X, req.Mouse.Y);
                }
                
                await response.WriteAsync(empty);
            }
        }

        public override Task<Resolution> GetMonitorResolution(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Resolution()
            {
                X = 2560,
                Y = 1440
            });
        }

        public override Task<Resolution> GetGameResolution(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Resolution()
            {
                X = 1280,
                Y = 720
            });
        }

        public override Task<IdentificationResponse> Identify(Identification request, ServerCallContext context)
        {
            var screenCapture = new ScreenCapture();
            var screenData = screenCapture.CaptureScreen();
            return Task.FromResult(new IdentificationResponse() { Message = "m0b", Responsetype = IdentificationResponseType.Ok });
        }

        public override Task<Empty> SendKeyboardEvent(KeyboardEvent request, ServerCallContext context)
        {
            var code = (KeyCode)request.Key;
            var dir = (EventType)request.Type;

            if(dir == EventType.Keydown)
                events = events.Hold(code);
            if(dir == EventType.Keyup)
                events = events.Release(code);

            Console.WriteLine($"{dir} {code}");

            events.Invoke();
            return Task.FromResult(empty);
        }

        public override Task<Empty> SendMouseEvent(MouseEvent request, ServerCallContext context)
        {
            if (request.Type == EventType.Move)
                events = events.MoveTo(request.X, request.Y);

            if (request.Type == EventType.Doubleclick)
                events = events.DoubleClick(ButtonCode.Left);

            //if (request.Type == EventType.Leftdown)
            //    events = events.Click(ButtonCode.Left);

            if (request.Type == EventType.Leftdown)
                events.Hold(ButtonCode.Left);

            if (request.Type == EventType.Leftup)
                events.Release(ButtonCode.Left);

            if (request.Type == EventType.Rightdown)
                events = events.Hold(ButtonCode.Right);

            if (request.Type == EventType.Rightup)
                events = events.Release(ButtonCode.Right);

            events.Invoke();
            return Task.FromResult(empty);
        }
    }
}