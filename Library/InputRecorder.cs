using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class InputRecorder : IHostedService
{
    readonly ILogger<InputRecorder> logger;

    public InputRecorder(
        ILogger<InputRecorder> logger,
        IHostApplicationLifetime appLifetime)
    {
        this.logger = logger;
        appLifetime.ApplicationStarted.Register(OnStarted);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        logger.LogInformation("Started");
    }
}
