using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<InputRecorder>();

        var hook = new GlobalKeyboardHook();
        hook.KeyboardPressed += (sender, args) =>
        {
            Console.WriteLine("Received keypress");
        };
        services.AddSingleton(hook);
    })
    .Build()
    .Run();