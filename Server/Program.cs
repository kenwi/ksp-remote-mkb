using Server.Services;

var builder = WebApplication.CreateBuilder(args);
if(File.Exists("hub.m0b.services.pem")) 
    builder.Configuration["Kestrel:Certificates:Default:Path"] = Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.pem");
if(File.Exists("hub.m0b.services.key"))
    builder.Configuration["Kestrel:Certificates:Default:KeyPath"]= Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.key");

//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();

builder.Services.AddGrpc();

//builder.Services.AddSingleton<IFoo, Foo>();
//builder.Services.AddSingleton<DllHookService>();

//builder.Services.AddSingleton<ScreenshotService>();
//builder.Services.AddHostedService<ScreenshotService>(provider => provider.GetRequiredService<ScreenshotService>());

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

app.MapGrpcService<RemoteControlService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run(args.FirstOrDefault());

public interface IFoo
{

}

public class Foo : IFoo
{

}
