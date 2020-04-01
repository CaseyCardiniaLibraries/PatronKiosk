namespace CCLKiosk
{
    partial class AdminMenu
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
            this.EditConfigButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.QuitAppButton = new System.Windows.Forms.Button();
            this.RestartAppButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EditConfigButton
            // 
            this.EditConfigButton.Location = new System.Drawing.Point(106, 101);
            this.EditConfigButton.Margin = new System.Windows.Forms.Padding(2);
            this.EditConfigButton.Name = "EditConfigButton";
            this.EditConfigButton.Size = new System.Drawing.Size(115, 43);
            this.EditConfigButton.TabIndex = 0;
            this.EditConfigButton.Text = "EDIT CONFIG";
            this.EditConfigButton.UseVisualStyleBackColor = true;
            this.EditConfigButton.Click += new System.EventHandler(this.EditConfig_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.BackColor = System.Drawing.Color.Transparent;
            this.messageLabel.Location = new System.Drawing.Point(21, 27);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(0, 13);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // QuitAppButton
            // 
            this.QuitAppButton.Location = new System.Drawing.Point(24, 108);
            this.QuitAppButton.Name = "QuitAppButton";
            this.QuitAppButton.Size = new System.Drawing.Size(75, 36);
            this.QuitAppButton.TabIndex = 3;
            this.QuitAppButton.Text = "CLOSE APP";
            this.QuitAppButton.UseVisualStyleBackColor = true;
            this.QuitAppButton.Click += new System.EventHandler(this.CloseApp_Click);
            // 
            // RestartAppButton
            // 
            this.RestartAppButton.Location = new System.Drawing.Point(237, 108);
            this.RestartAppButton.Name = "RestartAppButton";
            this.RestartAppButton.Size = new System.Drawing.Size(75, 36);
            this.RestartAppButton.TabIndex = 4;
            this.RestartAppButton.Text = "RESTART APP";
            this.RestartAppButton.UseVisualStyleBackColor = true;
            this.RestartAppButton.Click += new System.EventHandler(this.RestartApp_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(315, 1);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(20, 23);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "X";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.Exit_Click);
            // 
            // AdminMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(337, 166);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.RestartAppButton);
            this.Controls.Add(this.QuitAppButton);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.EditConfigButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdminMenu";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.AdminMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EditConfigButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button QuitAppButton;
        private System.Windows.Forms.Button RestartAppButton;
        private System.Windows.Forms.Button ExitButton;
    }
}