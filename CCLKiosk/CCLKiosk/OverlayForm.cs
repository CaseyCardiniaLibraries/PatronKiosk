using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
        Task homeTask;
        CancellationTokenSource source = new CancellationTokenSource();
        const int IDLEINTERVAL = 5;
        int TIMEOUT;
        int MAXIDLE;
        int idleCount;
        bool idleReset = false;
        bool dialogueUp = false;
        #endregion

        #region Initialisation
        //keyboard detection
        KeyboardHook kh = new KeyboardHook();
        private IntPtr _hookID = IntPtr.Zero;

        //mouse detection
        private LowLevelMouseProc _proc;
        private const int WH_MOUSE_LL = 14;

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

            //set idle check hooks
            _proc = HookCallback;
            kh.KeyDown += Kh_KeyDown;
            _hookID = SetHook(_proc);
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
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
        private async void ScheduleAction(Action action, DateTime ExecutionTime)
        {
            //create idle timer task
            homeTask = Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds, source.Token);

            //wait for idle timer
            try { await homeTask; }
            catch { return; }

            action();
        }

        //set idle timer
        public void SetTimer()
        {
            if (dialogueUp) ScheduleAction(CheckHome, DateTime.Now.AddSeconds(TIMEOUT));
            else ScheduleAction(CheckHome, DateTime.Now.AddSeconds(IDLEINTERVAL));
        }

        private void CancelTimer()
        {
            idleCount = 0;
            source.Cancel();
            source.Dispose();
            source = new CancellationTokenSource();
        }

        private void HomeDialogue()
        {
            dialogueUp = true;
            DIALOGUEFORM = new DialogueForm(OVERLAYFORM, HOMEFORM, HOMEFORM.CONFIG_FILE.themeColour, TIMEOUT) { Visible = true };
            SetTimer();
        }
        #endregion

        #region Navigation Handlers
        //cancel idle timer and go to home screen
        private void HomeButton_Click(object sender, EventArgs e) { if(!dialogueUp) GoHome(); }

        public void ContinueSession()
        {
            CancelTimer();
            dialogueUp = false;
            SetTimer();
        }

        //return to home screen
        private void GoHome()
        {
            dialogueUp = false;
            CancelTimer();
            HOMEFORM.Visible = true;
            OVERLAYFORM.Visible = false;
        }

        private void CheckHome()
        {
            //check if timeout
            if(dialogueUp)
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
                SetTimer();
                return;
            }
            //reset timer
            if(idleReset)
            {
                idleCount = 0;
                SetTimer();
                idleReset = false;
            }
        }
        #endregion

        #region Hooks
        private void Kh_KeyDown(Keys key) { idleReset = true; }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule) return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool v = (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam || MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam || MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam || MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam || MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam || MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam );
            if (nCode >= 0 && v) idleReset = true;

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}
