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
        public string? ClientId => Guid.Parse(inputClientGuid.Text).ToString();

        public string? ServerURI { get => inputServerHost.Text; set => inputServerHost.Text = value; }

        public ClientConfigurationDialog()
        {
            InitializeComponent();
        }

        private void ClientConfigurationDialog_Load(object sender, EventArgs e)
        {
            inputClientGuid.Text = $"{Guid.Empty}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Opacity = (float)numOpacity.Value;
            DialogResult = DialogResult.OK;
        }
    }
}
