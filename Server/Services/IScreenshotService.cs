namespace Server.Services
{
    public interface IScreenshotService
    {
        public string Image64 { get; set; }
        public int FrameRate { get; set; }
    }
}
