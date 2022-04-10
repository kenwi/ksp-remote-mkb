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
            ((System.ComponentModel.ISupportInitialize)(this.nmOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // clientGuid
            // 
            this.clientGuid.Location = new System.Drawing.Point(82, 12);
            this.clientGuid.Name = "clientGuid";
            this.clientGuid.Size = new System.Drawing.Size(320, 27);
            this.clientGuid.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 78);
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
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Client Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
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
            this.nmOpacity.Location = new System.Drawing.Point(82, 45);
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
            // ClientConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 114);
            this.Controls.Add(this.nmOpacity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clientGuid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClientConfigurationDialog";
            this.Text = "ClientConfigurationDialog";
            this.Load += new System.EventHandler(this.ClientConfigurationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox clientGuid;
        private Button button1;
        private Label label1;
        private Label label2;
        private NumericUpDown nmOpacity;
    }
}