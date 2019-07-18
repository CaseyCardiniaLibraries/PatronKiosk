using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using static CCLKiosk.Configuration;
using SHDocVw;

namespace CCLKiosk
{
    public partial class HomeForm : Form
    {
        #region Variable Declaration
        //form objects
        HomeForm HOMEFORM;
        OverlayForm OVERLAYFORM;

        public ConfigurationData CONFIG_FILE = GetConfigData();
        string ProcWindow;
        AppButton[] BUTTONLIST;

        int selectedButton = -1;

        Process OSKProc = null;
        Process ieProc = null;
        #endregion

        #region Initialisation
        public HomeForm()
        {
            //set current form object
            HOMEFORM = this;
            HOMEFORM.DoubleBuffered = true;

            InitializeComponent();

            //setup app buttons and home button
            if (ButtonSetup()) Application.Exit();

            //move buttons to correct area
            RowSetup();

            //set theme colour
            HOMEFORM.BackColor = Color.FromName(CONFIG_FILE.themeColour);

            if (CONFIG_FILE.mainBGImage != "None")
            {
                HOMEFORM.BackgroundImage = Image.FromFile(Path.Combine(CONFIG_FILE.resourceFolder, CONFIG_FILE.mainBGImage));
                HOMEFORM.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            //make app sit on top
            HOMEFORM.TopMost = true;

            SetTaskbarState();
        }

        //auto-hide task bar
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("shell32.dll")]
        public static extern UInt32 SHAppBarMessage(UInt32 dwMessage, ref APPBARDATA pData);

        public struct APPBARDATA
        {
            public UInt32 cbSize;
            public IntPtr hWnd;
            public UInt32 uCallbackMessage;
            public UInt32 uEdge;
            public Rectangle rc;
            public Int32 lParam;
        }

        public void SetTaskbarState()
        {
            APPBARDATA msgData = new APPBARDATA();
            msgData.cbSize = (UInt32)Marshal.SizeOf(msgData);
            msgData.hWnd = FindWindow("System_TrayWnd", null);
            msgData.lParam = (Int32)(0x01);
            SHAppBarMessage((UInt32)0x0a, ref msgData);
        }

        private bool ButtonSetup()
        {
            //initialise button list and set as invisible
            BUTTONLIST = new AppButton[CONFIG_FILE.numOfButtons];
            for (int i = 0; i < BUTTONLIST.Length; i++)
            {
                PictureBox tempButton = new PictureBox
                {
                    Name = "AppButton" + i,
                    BackColor = Color.White,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                if (CONFIG_FILE.appButtonsConfig[i].subOptions != null)
                {
                    PictureBox[] tempSubButtons = new PictureBox[CONFIG_FILE.appButtonsConfig[i].subOptions.Length];
                    for (int j = 0; j < CONFIG_FILE.appButtonsConfig[i].subOptions.Length; j++)
                    {
                        tempSubButtons[j] = new PictureBox
                        {
                            Name = string.Concat("App", i, "Sub", j),
                            BackColor = Color.White,
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };

                        tempSubButtons[j].Click += SubButtonClick;
                        tempSubButtons[j].Paint += SubButtonText_Paint;
                        HOMEFORM.Controls.Add(tempSubButtons[j]);
                        tempSubButtons[j].Visible = false;
                    }
                    BUTTONLIST[i] = new AppButton(tempButton, tempSubButtons, CONFIG_FILE.appButtonsConfig[i]);
                }
                else
                {
                    BUTTONLIST[i] = new AppButton(tempButton, CONFIG_FILE.appButtonsConfig[i]);
                }

                BUTTONLIST[i].mainButton.Click += ButtonClick;
                BUTTONLIST[i].mainButton.Paint += ButtonText_Paint;
                HOMEFORM.Controls.Add(BUTTONLIST[i].mainButton);
            }

            //setup all app buttons
            Image image;
            for (int i = 0; i < BUTTONLIST.Length; i++)
            {
                ButtonConfig tempConfig = BUTTONLIST[i].config;
                //get image from path
                try { image = Image.FromFile(Path.Combine(CONFIG_FILE.resourceFolder, tempConfig.backgroundImageName)); }
                catch { image = null; }
                BUTTONLIST[i].mainButton.Image = image;

                if (BUTTONLIST[i].subButtons != null)
                {
                    for (int j = 0; j < BUTTONLIST[i].subButtons.Length; j++)
                    {
                        BUTTONLIST[i].subButtons[j].Image = image;
                    }
                }
            }

            //create home button from config
            try { OVERLAYFORM = new OverlayForm(HOMEFORM); }
            catch { return true; }

            return false;
        }

        private void RowSetup()
        {
            //set home screen to be fullscreen
            Rectangle bounds = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom);
            HOMEFORM.DesktopBounds = bounds;
            HOMEFORM.Size = bounds.Size;

            //set bounds after padding
            bounds.Width -= (CONFIG_FILE.mainPaddingLeft + CONFIG_FILE.mainPaddingRight);
            bounds.Height -= (CONFIG_FILE.mainPaddingTop + CONFIG_FILE.mainPaddingBottom);
            bounds.X = CONFIG_FILE.mainPaddingLeft;
            bounds.Y = CONFIG_FILE.mainPaddingTop;

            int perRow = (int)Math.Ceiling((float)(CONFIG_FILE.numOfButtons / CONFIG_FILE.mainRows));

            //correct if size too large
            int widthCorrection = 0;
            if (bounds.Width / perRow < CONFIG_FILE.buttonWidth)
            {
                CONFIG_FILE.buttonWidth = bounds.Width / perRow;
                widthCorrection = bounds.Width % CONFIG_FILE.buttonWidth;
            }

            int heightCorrection = 0;
            if (bounds.Height / CONFIG_FILE.mainRows < CONFIG_FILE.buttonHeight)
            {
                CONFIG_FILE.buttonHeight = bounds.Height / CONFIG_FILE.mainRows;
                heightCorrection = bounds.Height % CONFIG_FILE.buttonHeight;
            }

            //set button size and position
            for (int i = 0; i < CONFIG_FILE.mainRows; i++)
            {
                for (int j = 0; j < perRow && (i * perRow) + j < CONFIG_FILE.numOfButtons; j++)
                {
                    int currentButton = (i * perRow) + j;
                    BUTTONLIST[currentButton].mainButton.Size = new Size(CONFIG_FILE.buttonWidth, CONFIG_FILE.buttonHeight);
                    BUTTONLIST[currentButton].mainButton.Location = new Point((((bounds.Width - (CONFIG_FILE.buttonWidth * perRow)) / (perRow + 1)) * (j + 1)) + (CONFIG_FILE.buttonWidth * j) + bounds.X,
                        (((bounds.Height - (CONFIG_FILE.buttonHeight * CONFIG_FILE.mainRows)) / (CONFIG_FILE.mainRows + 1)) * (i + 1)) + (CONFIG_FILE.buttonHeight * i) + bounds.Y);

                    if (j == perRow - 1) BUTTONLIST[currentButton].mainButton.Size = new Size(BUTTONLIST[currentButton].mainButton.Size.Width + widthCorrection, BUTTONLIST[currentButton].mainButton.Size.Height);
                    if (i == CONFIG_FILE.mainRows - 1) BUTTONLIST[currentButton].mainButton.Size = new Size(BUTTONLIST[currentButton].mainButton.Size.Width, BUTTONLIST[currentButton].mainButton.Size.Height + heightCorrection);
                }
            }

            foreach (AppButton a in BUTTONLIST)
            {
                if (a.subButtons != null)
                {
                    int numOfSub = a.subButtons.Length;
                    Rectangle scr = Screen.PrimaryScreen.Bounds;

                    int widthPadding = CONFIG_FILE.homeButtonConfig.buttonWidth * 3;
                    scr.Width -= widthPadding;
                    int heightPadding = scr.Height / 10;
                    scr.Height -= heightPadding;

                    int rows = 0;
                    bool done = false;
                    while (!done)
                    {
                        rows++;
                        if (scr.Width > scr.Height)
                        {
                            a.subButtonSize = scr.Height / rows;
                            if (Math.Floor((double)(scr.Width / a.subButtonSize)) * rows >= numOfSub) done = true;
                        }
                        else
                        {
                            a.subButtonSize = scr.Width / rows;
                            if (Math.Floor((double)(scr.Height / a.subButtonSize)) * rows >= numOfSub) done = true;
                        }
                    }

                    int bprow = (int)Math.Floor((double)(scr.Width / a.subButtonSize));
                    int padding = (scr.Width - (bprow * a.subButtonSize)) / 2;

                    for (int row = 0; row < rows; row++)
                    {
                        for (int b = 0; (b < bprow) && (row * bprow) + b  < numOfSub; b++)
                        {
                            a.subButtons[(row * bprow) + b].Size = new Size(a.subButtonSize - 10, a.subButtonSize - 10);
                            a.subButtons[(row * bprow) + b].Location = new Point((widthPadding/2) + padding + (b * a.subButtonSize) + 5, heightPadding + (row * a.subButtonSize) + 5);
                        }
                    }
                }
            }
        }

        private void ButtonText_Paint(object sender, PaintEventArgs e)
        {
            //get button number
            int buttonNum = Convert.ToInt32((sender as PictureBox).Name.Substring(9));

            //set formatting of text
            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            //easy access of button config
            ButtonConfig temp = BUTTONLIST[buttonNum].config;

            //paint text onto picture box (button)
            using (Font myFont = new Font("Arial", temp.fontSize))
            {
                e.Graphics.DrawString(temp.buttonText, myFont, new SolidBrush(Color.FromName(temp.textColor)), new Point(CONFIG_FILE.buttonWidth / 2, CONFIG_FILE.buttonHeight / 2), format);
            }
        }

        private void SubButtonText_Paint(object sender, PaintEventArgs e)
        {
            //get main button number
            int mainNum = Convert.ToInt32((sender as PictureBox).Name.Substring(3, 1));
            //get sub-option button number
            int buttonNum = Convert.ToInt32((sender as PictureBox).Name.Substring(7, 1));

            //set formatting of text
            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            //easy access of button config
            ButtonConfig temp = BUTTONLIST[mainNum].config;

            //paint text onto picture box (button)
            using (Font myFont = new Font("Arial", temp.fontSize))
            {
                e.Graphics.DrawString(temp.subOptions[buttonNum].text, myFont, new SolidBrush(Color.FromName(temp.textColor)), new Point(BUTTONLIST[mainNum].subButtonSize / 2, BUTTONLIST[mainNum].subButtonSize / 2), format);
            }
        }
        #endregion

        #region Navigation Handlers
        //process button click
        private void ButtonClick(object sender, EventArgs e)
        {
            //get button number
            int buttonNum = Convert.ToInt32((sender as PictureBox).Name.Substring(9));
            selectedButton = buttonNum;

            ProcWindow = BUTTONLIST[buttonNum].config.option.appProcName;

            //navigate ie using url in button arguments
            string args = BUTTONLIST[buttonNum].config.option.arguments;
            if (ProcWindow.Equals("iexplore", StringComparison.InvariantCultureIgnoreCase) && args != null)
            {
                ShellWindows explorers = new ShellWindows();
                bool found = false;
                foreach (InternetExplorer explorer in explorers)
                {
                    //use opened IE instance
                    if (explorer.Name == "Internet Explorer")
                    {
                        //explorer.Navigate(args, 0x20);
                        found = true;
                        break;
                    }
                }

                //create new instance of IE if not open
                if (!found) Process.Start("IExplore.exe", "-k " + args);
            }

            if (ProcWindow == "!subOptions")
            {
                ShowOptions();                              //show sub-options if set
                subOptionsLabel.Text = BUTTONLIST[buttonNum].config.buttonText;
            }
            else SwitchPrep();                              //switch window if no sub options
        }

        private void SubButtonClick(object sender, EventArgs e)
        {
            //get main button number
            int mainNum = Convert.ToInt32((sender as PictureBox).Name.Substring(3, 1));
            //get sub-option button number
            int buttonNum = Convert.ToInt32((sender as PictureBox).Name.Substring(7, 1));

            ProcWindow = BUTTONLIST[mainNum].config.subOptions[buttonNum].appProcName;

            //navigate ie using url in button arguments
            string args = BUTTONLIST[mainNum].config.subOptions[buttonNum].arguments;
            if (ProcWindow.Equals("iexplore", StringComparison.InvariantCultureIgnoreCase) && args != null)
            {
                ShellWindows explorers = new ShellWindows();
                bool found = false;
                foreach (InternetExplorer explorer in explorers)
                {
                    //use opened IE instance
                    if (explorer.Name == "Internet Explorer")
                    {
                        explorer.Navigate(args, 0x20);
                        found = true;
                        break;
                    }
                }

                //create new instance of IE if not open
                if (!found) ieProc = Process.Start("IExplore.exe", "-k " + args);
            }

            if (ProcWindow == "!subOptions")
            {
                ShowOptions();                              //show sub-options if set
                subOptionsLabel.Text = BUTTONLIST[mainNum].config.subOptions[buttonNum].text;
            }
            else SwitchPrep();                              //switch window if no sub options
        }

        //set main buttons to not visible and sub-options (and home button) to visible
        private void ShowOptions()
        {
            foreach (AppButton b in BUTTONLIST)
            {
                b.mainButton.Visible = false;
            }
            foreach (PictureBox p in BUTTONLIST[selectedButton].subButtons)
            {
                p.Visible = true;
            }

            subOptionsLabel.Visible = true;

            OVERLAYFORM.Visible = true;

            subOptionsLabel.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 20);
        }

        //set main buttons to visible and sub-options (and home button) to not visible
        public void ShowMain()
        {
            if (OSKProc != null)
            {
                try
                {
                    OSKProc.Kill();
                    OSKProc = null;
                }
                catch { }
            }

            if(ieProc != null)
            {
                try
                {
                    ieProc.Kill();
                    ieProc = null;
                }
                catch { }
            }
            try
            {
                foreach (Process process in Process.GetProcessesByName("iexplore"))
                {
                    process.Kill();
                }
                foreach (Process process in Process.GetProcessesByName("Internet Explorer"))
                {
                    process.Kill();
                }
            }
            catch { }

            if (selectedButton >= 0 && BUTTONLIST[selectedButton].subButtons != null)
            {
                foreach (PictureBox p in BUTTONLIST[selectedButton].subButtons)
                {
                    p.Visible = false;
                }
                foreach (AppButton b in BUTTONLIST)
                {
                    b.mainButton.Visible = true;
                }

                subOptionsLabel.Visible = false;

                OVERLAYFORM.Visible = false;
            }
        }

        private void SwitchPrep()
        {
            //switch to window in background
            SwitchWindow();
            //change visible window
            OVERLAYFORM.Visible = true;
            HOMEFORM.Visible = false;
            //set idle timer
            OVERLAYFORM.SetIdleTimer();
            if (ProcWindow != "LoanStation") OSKProc = Process.Start(new ProcessStartInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\osk.exe"));
        }

        //import dll
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

        //switch window, loop to find correct process
        private void SwitchWindow()
        {
            //get processes by name and switch to process window
            Process[] procs = Process.GetProcessesByName(ProcWindow);
            foreach (Process proc in procs)
            {
                SwitchToThisWindow(proc.MainWindowHandle, false);
                //to negate behaviour of window showing then minimising for IE
                //if (ProcWindow.Equals("iexplore", StringComparison.InvariantCultureIgnoreCase)) break;
            }
        }

        public void Refocus()
        {
            ProcWindow = "MultiAppKiosk";
            SwitchWindow();
        }
        #endregion

        //class containing relevant button info and objects
        class AppButton
        {
            //button displayed at main menu
            public PictureBox mainButton;
            //config from config file
            public ButtonConfig config;
            //buttons for sub-options if needed
            public PictureBox[] subButtons;
            //size of sub-option buttons
            public int subButtonSize = 0;

            public AppButton(PictureBox setMainButton, ButtonConfig setConfig)
            {
                mainButton = setMainButton;
                config = setConfig;
            }

            public AppButton(PictureBox setMainButton, PictureBox[] addButtons, ButtonConfig setConfig)
            {
                mainButton = setMainButton;
                subButtons = addButtons;
                config = setConfig;
            }
        }
    }
}