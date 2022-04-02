using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Client.WinForms;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<ClientForm>();
    })
    .Build();
using var scope = builder.Services.CreateScope();
var services = scope.ServiceProvider;
Application.Run(services.GetRequiredService<ClientForm>());