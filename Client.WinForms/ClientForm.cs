using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client.WinForms
{
    public partial class ClientForm : Form, IHostedService
    {
        public ClientForm(ILogger<ClientForm> logger)
        {
            InitializeComponent();
            var hook = new GlobalKeyboardHook();
            hook.KeyboardPressed += (sender, args) =>
            {
                if(args.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                {
                    MessageBox.Show($"Received Keyboard Event: {args.KeyboardState}");
                }
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}