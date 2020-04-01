using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCLKiosk
{
    public partial class DialogueForm : Form
    {
        #region Variable Declaration
        //form objects
        OverlayForm OVERLAYFORM;
        DialogueForm DIALOGUEFORM;

        //timer display
        int timeLeft;
        #endregion

        #region Initialisation
        public DialogueForm(OverlayForm parent, HomeForm homeForm, string theme, int timerValue)
        {
            //set parent form
            OVERLAYFORM = parent;
            //set current form
            DIALOGUEFORM = this;

            InitializeComponent();

            //setstyle
            DIALOGUEFORM.BackColor = Color.FromName(theme);
            //set time
            timeLeft = timerValue;
            messageLabel.Text = homeForm.CONFIG_FILE.timeoutText;
            countdownLabel.Text = timeLeft + " seconds";

            Font tempFont = new Font("Arial", homeForm.CONFIG_FILE.timeoutFontSize);
            messageLabel.Font = tempFont;
            countdownLabel.Font = tempFont;
            ContinueButton.Font = tempFont;

            DIALOGUEFORM.Size = new Size(homeForm.CONFIG_FILE.timeoutWidth, homeForm.CONFIG_FILE.timeoutHeight);
            messageLabel.Size = new Size(DIALOGUEFORM.Width, messageLabel.Height);
            countdownLabel.Size = new Size(DIALOGUEFORM.Width, countdownLabel.Height);
            ContinueButton.Size = new Size(DIALOGUEFORM.Width / 3, DIALOGUEFORM.Height / 6);

            DIALOGUEFORM.CenterToScreen();
            messageLabel.Location = new Point((DIALOGUEFORM.Width / 2) - (messageLabel.Width / 2), (DIALOGUEFORM.Height / 6) * 1);
            countdownLabel.Location = new Point((DIALOGUEFORM.Width / 2) - (countdownLabel.Width / 2), (DIALOGUEFORM.Height / 6) * 2);
            ContinueButton.Location = new Point((DIALOGUEFORM.Width / 2) - (ContinueButton.Width / 2), (DIALOGUEFORM.Height / 6) * 4);
        }

        private void DialogueForm_Load(object sender, EventArgs e)
        {
            //place on top of windows
            DIALOGUEFORM.TopMost = true;
            //start countdown
            timeOut.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //set black border for window
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
        #endregion

        #region Dialogue Handlers
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            //remove this form
            OVERLAYFORM.ContinueSession();
            DIALOGUEFORM.Dispose();
            DIALOGUEFORM.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DIALOGUEFORM.Focus();

            //countdown
            if (timeLeft > 0)
            {
                timeLeft -= 1;
                countdownLabel.Text = timeLeft + " seconds";
            }
            else
            {
                timeOut.Stop();
                DIALOGUEFORM.Dispose();
                DIALOGUEFORM.Close();
                OVERLAYFORM.GoHome();
            }
        }
        #endregion
    }
}