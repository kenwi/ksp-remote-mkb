using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Client.WinForms;
using System.Diagnostics;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ClientForm>();
    })
    .Build();
using var scope = builder.Services.CreateScope();
var services = scope.ServiceProvider;
Application.Run(services.GetRequiredService<ClientForm>());