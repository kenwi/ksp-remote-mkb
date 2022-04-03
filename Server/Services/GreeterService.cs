using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server;

namespace Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> logger;
        public GreeterService(ILogger<GreeterService> logger, DllHookService dllHook)
        {
            this.logger = logger;
        }

        public override Task<Empty> SendMouseEvent(MouseEvent request, ServerCallContext context)
        {
            logger.LogInformation($"Received MouseEvent Type: {request.Type} X: {request.X} Y: {request.Y}");

            return Task.FromResult(new Empty());
            //return base.SendMouseEvent(request, context);
        }

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}
    }
}