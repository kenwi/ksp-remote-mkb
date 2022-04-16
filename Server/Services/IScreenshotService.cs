namespace Server.Services
{
    public interface IScreenshotService
    {
        public string Image64 { get; }
        public int FrameRate { get; set; }
    }
}
