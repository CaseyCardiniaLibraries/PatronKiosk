namespace CCLKiosk
{
    partial class HomeForm
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
            this.subOptionsLabel = new System.Windows.Forms.Label();
            this.ScreensaverTimer = new System.Windows.Forms.Timer(this.components);
            this.ScreensaverImg = new System.Windows.Forms.PictureBox();
            this.HomeButton = new System.Windows.Forms.PictureBox();
            this.ImgSwitchTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ScreensaverImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HomeButton)).BeginInit();
            this.SuspendLayout();
            // 
            // subOptionsLabel
            // 
            this.subOptionsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.subOptionsLabel.AutoSize = true;
            this.subOptionsLabel.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subOptionsLabel.Location = new System.Drawing.Point(505, 41);
            this.subOptionsLabel.Name = "subOptionsLabel";
            this.subOptionsLabel.Size = new System.Drawing.Size(0, 42);
            this.subOptionsLabel.TabIndex = 0;
            this.subOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScreensaverTimer
            // 
            this.ScreensaverTimer.Interval = 1000;
            this.ScreensaverTimer.Tick += new System.EventHandler(this.ScreensaverTimer_Tick);
            // 
            // ScreensaverImg
            // 
            this.ScreensaverImg.Location = new System.Drawing.Point(0, 0);
            this.ScreensaverImg.Name = "ScreensaverImg";
            this.ScreensaverImg.Size = new System.Drawing.Size(100, 50);
            this.ScreensaverImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ScreensaverImg.TabIndex = 1;
            this.ScreensaverImg.TabStop = false;
            this.ScreensaverImg.Visible = false;
            this.ScreensaverImg.Click += new System.EventHandler(this.ScreensaverImg_Click);
            // 
            // HomeButton
            // 
            this.HomeButton.Location = new System.Drawing.Point(837, 240);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(100, 50);
            this.HomeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.HomeButton.TabIndex = 2;
            this.HomeButton.TabStop = false;
            this.HomeButton.Visible = false;
            // 
            // ImgSwitchTimer
            // 
            this.ImgSwitchTimer.Interval = 1000;
            this.ImgSwitchTimer.Tick += new System.EventHandler(this.ImgSwitchTimer_Tick);
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1280, 702);
            this.Controls.Add(this.HomeButton);
            this.Controls.Add(this.ScreensaverImg);
            this.Controls.Add(this.subOptionsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HomeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.HomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ScreensaverImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HomeButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label subOptionsLabel;
        private System.Windows.Forms.Timer ScreensaverTimer;
        private System.Windows.Forms.PictureBox ScreensaverImg;
        private System.Windows.Forms.PictureBox HomeButton;
        private System.Windows.Forms.Timer ImgSwitchTimer;
    }
}