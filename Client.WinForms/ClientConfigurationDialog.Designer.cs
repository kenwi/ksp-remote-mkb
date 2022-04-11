namespace Client.WinForms
{
    partial class ClientConfigurationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clientGuid = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nmOpacity = new System.Windows.Forms.NumericUpDown();
            this.serverHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.nmOpacity)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clientGuid
            // 
            this.clientGuid.Location = new System.Drawing.Point(8, 81);
            this.clientGuid.Name = "clientGuid";
            this.clientGuid.Size = new System.Drawing.Size(302, 27);
            this.clientGuid.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Opacity";
            // 
            // nmOpacity
            // 
            this.nmOpacity.DecimalPlaces = 1;
            this.nmOpacity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmOpacity.Location = new System.Drawing.Point(8, 134);
            this.nmOpacity.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmOpacity.Name = "nmOpacity";
            this.nmOpacity.Size = new System.Drawing.Size(64, 27);
            this.nmOpacity.TabIndex = 4;
            this.nmOpacity.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // serverHost
            // 
            this.serverHost.Location = new System.Drawing.Point(8, 28);
            this.serverHost.Name = "serverHost";
            this.serverHost.Size = new System.Drawing.Size(302, 27);
            this.serverHost.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Client Id";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.serverHost);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.clientGuid);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.nmOpacity);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(322, 243);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // ClientConfigurationDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 243);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientConfigurationDialog";
            this.Text = "ClientConfigurationDialog";
            this.Load += new System.EventHandler(this.ClientConfigurationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmOpacity)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox clientGuid;
        private Button button1;
        private Label label1;
        private Label label2;
        private NumericUpDown nmOpacity;
        private TextBox serverHost;
        private Label label3;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}