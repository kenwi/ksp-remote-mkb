using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server;

namespace Client.WinForms
{
    public partial class ClientForm : Form
    {
        static readonly MouseEvent outboundEvent = new();
        readonly Greeter.GreeterClient? client;
        readonly Resolution? serverGameResolution;
        readonly Resolution? serverMonitorResolution;
        float xmod = 0, ymod = 0;

        public ClientForm(ILogger<ClientForm> logger)
        {
            InitializeComponent();

            using var dialog = new ClientConfigurationDialog()
            {
                ServerURI = Environment.GetCommandLineArgs()
                .Skip(1)
                .FirstOrDefault()
                ?? "https://localhost"
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var identification = new Identification() { Id = dialog.ClientId };
                    var rpcEndpoint =  dialog.ServerURI;

                    logger.LogInformation($"Connecting to RpcEndpoint: {rpcEndpoint}");
                    var channel = GrpcChannel.ForAddress(rpcEndpoint, new GrpcChannelOptions
                    {
                        HttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
                    });
                    client = new Greeter.GreeterClient(channel);
                    serverGameResolution = client.GetGameResolution(new Empty());
                    serverMonitorResolution = client.GetMonitorResolution(new Empty());
                    CalculatePerspective();

                    var clientId = client?.Identify(identification)?.Message;
                    this.Text = $"Remote Client Id: {clientId}";
                    this.Opacity = dialog.Opacity;

                    logger.LogInformation($"Received gRPC Client Id: {clientId}");
                    logger.LogInformation($"Selected Opacity: {Opacity}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    logger.LogCritical(ex.Message);
                    client = null;
                    throw;
                }
            }
        }

        void CalculatePerspective()
        {
            if (serverMonitorResolution is null || serverGameResolution is null)
                return;

            xmod = (float)serverMonitorResolution.X / DisplayRectangle.Width;
            ymod = (float)serverMonitorResolution.Y / DisplayRectangle.Height;
        }

        private void ClientForm_DoubleClick(object sender, EventArgs e)
        {
            if (e is not MouseEventArgs mouse)
                return;

            client?.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = EventType.Doubleclick
            });
        }

        private void ClientForm_MouseMove(object sender, MouseEventArgs mouse)
        {
            outboundEvent.X = (int)(mouse.X * xmod);
            outboundEvent.Y = (int)(mouse.Y * ymod);
            outboundEvent.Type = EventType.Move;
            client?.SendMouseEvent(outboundEvent);
        }

        private void ClientForm_MouseDown(object sender, MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.XButton1)
            {
                using var dialog = new ClientConfigurationDialog();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        this.Opacity = dialog.Opacity;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                }
                return;
            }

            client?.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = mouse.Button switch
                {
                    MouseButtons.Left => EventType.Leftdown,
                    MouseButtons.Right => EventType.Rightdown,
                    _ => throw new NotImplementedException()
                }
            });
        }

        private void ClientForm_MouseUp(object sender, MouseEventArgs mouse)
        {
            client?.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = mouse.Button switch
                {
                    MouseButtons.Left => EventType.Leftup,
                    MouseButtons.Right => EventType.Rightup,
                    _ => throw new NotImplementedException()
                }
            });
        }

        private void ClientForm_KeyDown(object sender, KeyEventArgs e)
        {
            client?.SendKeyboardEvent(new KeyboardEvent()
            {
                Key = e.KeyValue,
                Type = EventType.Keydown
            });
        }

        private void ClientForm_KeyUp(object sender, KeyEventArgs e)
        {
            client?.SendKeyboardEvent(new KeyboardEvent()
            {
                Key = e.KeyValue,
                Type = EventType.Keyup
            });
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new ClientConfigurationDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.Opacity = dialog.Opacity;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }

        private void ClientForm_Resize(object sender, EventArgs e)
        {
            if (serverMonitorResolution is null || serverGameResolution is null)
                return;

            CalculatePerspective();
        }
    }
}