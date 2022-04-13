using Server.Services;

var builder = WebApplication.CreateBuilder(args);
if(File.Exists("hub.m0b.services.pem")) 
    builder.Configuration["Kestrel:Certificates:Default:Path"] = Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.pem");
if(File.Exists("hub.m0b.services.key"))
    builder.Configuration["Kestrel:Certificates:Default:KeyPath"]= Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.key");

builder.Services.AddGrpc();

builder.Services.AddSingleton<OpenGLScreenshotService>();
builder.Services.AddHostedService<OpenGLScreenshotService>(provider => provider.GetRequiredService<OpenGLScreenshotService>());

//builder.Services.AddSingleton<IFoo, Foo>();
//builder.Services.AddSingleton<DllHookService>();
var useBlazor = Environment.GetCommandLineArgs()
    .Any(arg => arg.ToLower().Equals("useblazor"));

if (useBlazor)
{
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddSingleton<ScreenshotService>();
    builder.Services.AddHostedService<ScreenshotService>(provider => provider.GetRequiredService<ScreenshotService>());
}

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if(useBlazor)
{
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

public interface IFoo
{

}

public class Foo : IFoo
{

}
