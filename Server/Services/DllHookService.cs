namespace Server.Services
{
    public class DllHookService
    {
        private readonly ILogger<DllHookService> logger;

        public DllHookService(ILogger<DllHookService> logger)
        {
            this.logger = logger;
            logger.LogInformation("DllHookService Started");
        }
        public void SendMouseMove(int x, int y)
        {

        }
    }
}