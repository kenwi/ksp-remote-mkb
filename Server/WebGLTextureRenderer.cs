using Blazor.Extensions;
using Microsoft.JSInterop;
using Server.Services;

public class WebGLTextureRenderer : IRenderer
{
    protected readonly IJSRuntime js;
    protected readonly DirectXScreenshotService directXScreenshotService;
    private readonly byte[] imageBytes;
    int width, height;
    bool isActive;
    Random rng = new Random();

    public WebGLTextureRenderer(IJSRuntime js, DirectXScreenshotService directXScreenshotService)
    {
        (width, height) = (1280, 720);
        this.isActive = false;
        this.js = js;
        this.directXScreenshotService = directXScreenshotService;
        imageBytes = new byte[width * height * 4];
    }

    public virtual async Task Setup()
    {
        var vertexShader = await File.ReadAllTextAsync("wwwroot/shaders/fundamentals.vert");
        var fragmentShader = await File.ReadAllTextAsync("wwwroot/shaders/fundamentals.frag");
        //var imageBytes = directXScreenshotService.Image.ToArray();
        
        rng.NextBytes(imageBytes);
        await js.InvokeVoidAsync("Initialize", vertexShader, fragmentShader, imageBytes);
        isActive = true;
    }

    public virtual async Task Draw()
    {
        if (!isActive)
            return;
        rng.NextBytes(imageBytes);

        await js.InvokeVoidAsync("updateTexture", imageBytes, width, height);
    }
}
