using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.WinForms
{
    public partial class ClientConfigurationDialog : Form
    {
        public string? ClientId => Guid.Parse(clientGuid.Text).ToString();

        public string? ServerURI { get => serverHost.Text; set => serverHost.Text = value; }

        public ClientConfigurationDialog()
        {
            InitializeComponent();
        }

        private void ClientConfigurationDialog_Load(object sender, EventArgs e)
        {
            clientGuid.Text = $"{Guid.Empty}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Opacity = (float)nmOpacity.Value;
            DialogResult = DialogResult.OK;
        }
    }
}
