using Server.Services;

var builder = WebApplication.CreateBuilder(args);
if(File.Exists("localhost.pem")) 
    builder.Configuration["Kestrel:Certificates:Default:Path"] = Path.Combine(builder.Environment.ContentRootPath, "localhost.pem");
if(File.Exists("localhost.key"))
    builder.Configuration["Kestrel:Certificates:Default:KeyPath"]= Path.Combine(builder.Environment.ContentRootPath, "localhost.key");

builder.Services.AddGrpc();

var useBlazor = Environment.GetCommandLineArgs()
    .Any(arg => arg.ToLower().Equals("useblazor"));

var useOpenGL = Environment.GetCommandLineArgs()
    .Any(arg => arg.ToLower().Equals("useopengl"));

if (useBlazor)
{
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    if(useOpenGL)
    {
        builder.Services.AddSingleton<OpenGLScreenshotService>();
        builder.Services.AddHostedService<OpenGLScreenshotService>(provider => provider.GetRequiredService<OpenGLScreenshotService>());
    }
    else
    {
        builder.Services.AddSingleton<ScreenshotService>();
        builder.Services.AddHostedService<ScreenshotService>(provider => provider.GetRequiredService<ScreenshotService>());
    }
}

var app = builder.Build();
if(useBlazor)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
}

app.UseHttpsRedirection();
app.MapGrpcService<RemoteControlService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

var host = Environment.GetCommandLineArgs()
    .FirstOrDefault(arg => arg.ToLower().StartsWith("http"))
    ?? "https://localhost";
app.Run(host);
