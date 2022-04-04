using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server;
using WindowsInput;
using WindowsInput.Events;

namespace Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> logger;
        private readonly Empty empty = new();
        private EventBuilder events = Simulate.Events();
        public GreeterService(ILogger<GreeterService> logger, DllHookService dllHook)
        {
            this.logger = logger;
        }

        public override Task<Empty> SendMouseEvent(MouseEvent request, ServerCallContext context)
        {
            if (request.Type == EventType.Move)
                events = events.MoveTo(request.X, request.Y);

            if (request.Type == EventType.Doubleclick)
                events = events.DoubleClick(ButtonCode.Left);

            if (request.Type == EventType.Leftdown)
                events = events.Click(ButtonCode.Left);

            if (request.Type == EventType.Rightdown)
                events = events.Click(ButtonCode.Right);

            events.Invoke();
            return Task.FromResult(empty);
        }
    }
}