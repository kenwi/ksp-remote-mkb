using Server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration["Kestrel:Certificates:Default:Path"] = Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.pem");
builder.Configuration["Kestrel:Certificates:Default:KeyPath"]= Path.Combine(builder.Environment.ContentRootPath, "hub.m0b.services.key");

builder.Services.AddGrpc();
builder.Services.AddSingleton<IFoo, Foo>();
builder.Services.AddSingleton<DllHookService>();

var app = builder.Build();
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run(args.FirstOrDefault());

public interface IFoo
{

}

public class Foo : IFoo
{

}
