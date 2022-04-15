namespace Server
{
    public class ImageCapture
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? DeviceName { get; set; }
        public int OffsetLeft { get; set; }
        public int OffsetTop { get; set; }
        public int FrameCount { get; set; }
    }
}
