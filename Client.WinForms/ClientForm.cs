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

            var rpcEndpoint = "https://localhost:7278";
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

        private void ClientForm_Click(object sender, EventArgs e)
        {
            var mouse = e as MouseEventArgs;
            if (mouse == null)
                return;

            var builder = new StringBuilder($"Original Coordinates: {mouse.X} {mouse.Y}");
            label1.Text = builder.ToString();

            client.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = mouse.Button switch
                {
                    MouseButtons.Left => EventType.Leftdown,
                    MouseButtons.Right => EventType.Rightdown,
                    //MouseButtons.None => throw new NotImplementedException(),
                    //MouseButtons.Middle => throw new NotImplementedException(),
                    //MouseButtons.XButton1 => throw new NotImplementedException(),
                    //MouseButtons.XButton2 => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                }
            });
        }

        private void ClientForm_MouseMove(object sender, MouseEventArgs mouse)
        {
            var builder = new StringBuilder($"Original Coordinates: {mouse.X} {mouse.Y}");
            label2.Text = builder.ToString();
            client.SendMouseEvent(new MouseEvent()
            {
                X = mouse.X,
                Y = mouse.Y,
                Type = EventType.Move
            });
        }
    }
}