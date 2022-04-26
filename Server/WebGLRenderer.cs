using Microsoft.JSInterop;
using Server.Services;

public class WebGLRenderer : IRenderer
{
    private readonly IJSRuntime js;
    private readonly DirectXScreenshotService directXScreenshotService;

    public WebGLRenderer(IJSRuntime js, DirectXScreenshotService directXScreenshotService)
    {
        this.js = js;
        this.directXScreenshotService = directXScreenshotService;
    }

    public async Task Setup()
    {
        var vertexShader = File.ReadAllText("wwwroot/shaders/fundamentals.vert");
        var fragmentShader = File.ReadAllText("wwwroot/shaders/fundamentals.frag");
        await js.InvokeVoidAsync("LoadShaders", vertexShader, fragmentShader);
    }

    public async Task Draw()
    {
        await js.InvokeVoidAsync("RenderImage", directXScreenshotService.Image64);
    }


}
