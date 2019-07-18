using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static CCLKiosk.Configuration;

namespace CCLKiosk
{
    public partial class OverlayForm : Form
    {
        #region Variable Declaration
        //form objects
        HomeForm HOMEFORM;
        OverlayForm OVERLAYFORM;
        DialogueForm DIALOGUEFORM;

        //idle timer objects
        int countTime;
        const int IDLEINTERVAL = 5;
        int TIMEOUT;
        int MAXIDLE;
        int idleCount;
        bool idleReset = false;
        bool dialogueUp = false;
        #endregion

        #region Initialisation
        LASTINPUTINFO lastInPut = new LASTINPUTINFO();
        private uint inputCheck = 0;

        public OverlayForm(HomeForm parent)
        {
            //get home screen object
            HOMEFORM = parent;
            //set current form as object
            OVERLAYFORM = this;

            InitializeComponent();

            HomeConfig temp = HOMEFORM.CONFIG_FILE.homeButtonConfig;

            //set home button image
            HomeButton.Image = Image.FromFile(Path.Combine(HOMEFORM.CONFIG_FILE.resourceFolder, temp.backgroundImageName));
            HomeButton.Click += HomeButton_Click;
            HomeButton.Paint += ButtonText_Paint;

            //set button size
            OVERLAYFORM.Size = new Size(temp.buttonWidth, temp.buttonHeight);

            HomeButton.Size = new Size(temp.buttonWidth, temp.buttonHeight);
            //move home button to position
            Rectangle workingArea = Screen.GetWorkingArea(OVERLAYFORM);
            if (temp.buttonPosition == "TOPRIGHT") OVERLAYFORM.Location = new Point(workingArea.Right - temp.buttonWidth - temp.buttonPaddingHorizontal, workingArea.Top + temp.buttonPaddingVertical);
            else if (temp.buttonPosition == "TOPLEFT") OVERLAYFORM.Location = new Point(workingArea.Left + temp.buttonPaddingHorizontal, workingArea.Top + temp.buttonPaddingVertical);
            else if (temp.buttonPosition == "BOTTOMRIGHT") OVERLAYFORM.Location = new Point(workingArea.Right - temp.buttonWidth - temp.buttonPaddingHorizontal, workingArea.Bottom - temp.buttonHeight - temp.buttonPaddingVertical);
            else if (temp.buttonPosition == "BOTTOMLEFT") OVERLAYFORM.Location = new Point(workingArea.Left + temp.buttonPaddingHorizontal, workingArea.Bottom - temp.buttonHeight - temp.buttonPaddingVertical);

            //timeout config
            TIMEOUT = HOMEFORM.CONFIG_FILE.timeoutTime;
            MAXIDLE = HOMEFORM.CONFIG_FILE.idleTimeout/IDLEINTERVAL;
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);
            inputCheck = lastInPut.dwTime;

            //make the home button overlay
            OVERLAYFORM.TopMost = true;
        }

        private void ButtonText_Paint(object sender, PaintEventArgs e)
        {
            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            HomeConfig temp = HOMEFORM.CONFIG_FILE.homeButtonConfig;

            using (Font myFont = new Font("Arial", temp.fontSize))
            {
                e.Graphics.DrawString(temp.buttonText, myFont, new SolidBrush(Color.FromName(temp.textColor)), new Point(temp.buttonWidth / 2, temp.buttonHeight / 2), format);
            }

            //set black border for button
            ControlPaint.DrawBorder(e.Graphics, (sender as PictureBox).ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
        #endregion

        #region Timer
        private void TimerIdle_Tick(object sender, EventArgs e)
        {
            //countdown
            if (countTime > 0) countTime--;
            else CheckHome();
        }

        //set idle timer
        public void SetIdleTimer()
        {
            if (dialogueUp) countTime = TIMEOUT;
            else countTime = IDLEINTERVAL;
            timerIdle.Start();
        }

        private void CancelIdleTimer()
        {
            timerIdle.Stop();
            idleCount = 0;
        }

        private void HomeDialogue()
        {
            dialogueUp = true;
            DIALOGUEFORM = new DialogueForm(OVERLAYFORM, HOMEFORM, HOMEFORM.CONFIG_FILE.themeColour, TIMEOUT) { Visible = true };
            SetIdleTimer();
        }
        #endregion

        #region Navigation Handlers
        //cancel idle timer and go to home screen
        private void HomeButton_Click(object sender, EventArgs e) { if(!dialogueUp) GoHome(); }

        public void ContinueSession()
        {
            CancelIdleTimer();
            dialogueUp = false;
            SetIdleTimer();
        }

        //return to home screen
        private void GoHome()
        {
            dialogueUp = false;
            CancelIdleTimer();
            HOMEFORM.Visible = true;
            HOMEFORM.ShowMain();
            OVERLAYFORM.Visible = false;
        }

        private void CheckHome()
        {
            GetLastInputInfo(ref lastInPut);
            if(inputCheck != lastInPut.dwTime)
            {
                idleReset = true;
            }

            inputCheck = lastInPut.dwTime;

            //check if timeout
            if (dialogueUp)
            {
                DIALOGUEFORM.Dispose();
                DIALOGUEFORM.Close();
                GoHome();
                return;
            }
            //idle time up
            if (!idleReset && idleCount == MAXIDLE-1)
            {
                HomeDialogue();
                return;
            }
            //increment idle timer
            if (!idleReset && idleCount != MAXIDLE-1)
            {
                idleCount += 1;
                SetIdleTimer();
                return;
            }
            //reset timer
            if(idleReset)
            {
                idleCount = 0;
                SetIdleTimer();
                idleReset = false;
            }
        }
        #endregion

        #region Hooks
        [DllImport("User32.dll")]
        private static extern bool
        GetLastInputInfo(ref LASTINPUTINFO plii);

        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
        #endregion
    }
}