namespace CCLKiosk
{
    partial class DialogueForm
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
            this.components = new System.ComponentModel.Container();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.countdownLabel = new System.Windows.Forms.Label();
            this.timeOut = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(106, 101);
            this.ContinueButton.Margin = new System.Windows.Forms.Padding(2);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(115, 31);
            this.ContinueButton.TabIndex = 0;
            this.ContinueButton.Text = "Continue Session";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
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
            // countdownLabel
            // 
            this.countdownLabel.AutoSize = true;
            this.countdownLabel.BackColor = System.Drawing.Color.Transparent;
            this.countdownLabel.Location = new System.Drawing.Point(133, 66);
            this.countdownLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.countdownLabel.Name = "countdownLabel";
            this.countdownLabel.Size = new System.Drawing.Size(0, 13);
            this.countdownLabel.TabIndex = 2;
            this.countdownLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timeOut
            // 
            this.timeOut.Interval = 1000;
            this.timeOut.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // DialogueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(337, 166);
            this.Controls.Add(this.countdownLabel);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.ContinueButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DialogueForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.DialogueForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label countdownLabel;
        private System.Windows.Forms.Timer timeOut;
    }
}