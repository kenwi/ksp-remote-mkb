using Blazor.Extensions;
using Blazor.Extensions.Canvas.WebGL;
using Microsoft.JSInterop;
using Server.Services;
using WebGL;

public class Renderer : IRenderer
{
    readonly BECanvasComponent canvas;
    private readonly DirectXScreenshotService directXScreenshotService;
    private readonly IJSRuntime jSRuntime;
    //WebGLContext? gl;
    //WebGLUniformLocation u_matrix_location;
    //WebGLProgram shader;
    //static Random rng = new Random((int)DateTime.Now.Ticks);
    //private float[] positions;
    //uint positionAttributeLocation;
    //private ShaderProgram shaderProgram;
    //private byte[] byteArr;

    public Renderer(BECanvasComponent canvas, 
        DirectXScreenshotService directXScreenshotService,
        IJSRuntime jSRuntime)
    {
        this.canvas = canvas;
        this.directXScreenshotService = directXScreenshotService;
        this.jSRuntime = jSRuntime;
    }

    public async Task Draw()
    {
        var img = directXScreenshotService.Image64;
        await jSRuntime.InvokeVoidAsync("LoadTexture", img, canvas.Width, canvas.Height);
    }

    private async Task<WebGLProgram> InitProgramAsync(WebGLContext gl, string vsSource, string fsSource)
    {
        var vertexShader = await this.LoadShaderAsync(gl, ShaderType.VERTEX_SHADER, vsSource);
        var fragmentShader = await this.LoadShaderAsync(gl, ShaderType.FRAGMENT_SHADER, fsSource);

        var program = await gl.CreateProgramAsync();
        await gl.AttachShaderAsync(program, vertexShader);
        await gl.AttachShaderAsync(program, fragmentShader);
        await gl.LinkProgramAsync(program);

        await gl.DeleteShaderAsync(vertexShader);
        await gl.DeleteShaderAsync(fragmentShader);

        if (!await gl.GetProgramParameterAsync<bool>(program, ProgramParameter.LINK_STATUS))
        {
            string info = await gl.GetProgramInfoLogAsync(program);
            throw new Exception("An error occured while linking the program: " + info);
        }
        //u_matrix_location = await gl.GetUniformLocationAsync(program, "u_matrix");
        return program;
    }
    
    private async Task<WebGLShader> LoadShaderAsync(WebGLContext gl, ShaderType type, string source)
    {
        var shader = await gl.CreateShaderAsync(type);

        await gl.ShaderSourceAsync(shader, source);
        await gl.CompileShaderAsync(shader);

        if (!await gl.GetShaderParameterAsync<bool>(shader, ShaderParameter.COMPILE_STATUS))
        {
            string info = await gl.GetShaderInfoLogAsync(shader);
            await gl.DeleteShaderAsync(shader);
            throw new Exception("An error occured while compiling the shader: " + info);
        }

        return shader;
    }

    public Task Setup()
    {
        throw new NotImplementedException();
    }
}
