using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server;
using System.Text;

namespace Client.WinForms
{
    public partial class ClientForm : Form
    {
        readonly GrpcChannel channel;
        readonly Greeter.GreeterClient client;
        public ClientForm(ILogger<ClientForm> logger)
        {
            InitializeComponent();
            var hook = new GlobalKeyboardHook();
            hook.KeyboardPressed += (sender, args) =>
            {
                if(args.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                {
                    var builder = new StringBuilder($"Keyboard Code: {args.KeyboardData.VirtualCode}");
                    label1.Text = builder.ToString();
                }
            };

            var rpcEndpoint = "https://10.0.0.9:5000";
            channel = GrpcChannel.ForAddress(rpcEndpoint,
                new GrpcChannelOptions
                {
                    HttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                });
            client = new Greeter.GreeterClient(channel);
        }

        private void ClientForm_DoubleClick(object sender, EventArgs e)
        {
            if (e is not MouseEventArgs mouse)
                return;

            client.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = EventType.Doubleclick
            });
        }

        private void ClientForm_MouseMove(object sender, MouseEventArgs mouse)
        {
            label1.Text = $"Coordinates: {mouse.X} {mouse.Y}";
            client.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = EventType.Move
            });
        }

        private void ClientForm_MouseDown(object sender, MouseEventArgs mouse)
        {
            client.SendMouseEvent(new MouseEvent()
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
            client.SendMouseEvent(new MouseEvent()
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
    }
}