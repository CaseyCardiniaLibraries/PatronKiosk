using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using static CCLKiosk.Configuration;

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
        PictureBox[] BUTTONLIST;
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

            if(CONFIG_FILE.mainBGImage != "None")
            {
                HOMEFORM.BackgroundImage = Image.FromFile(Path.Combine(CONFIG_FILE.resourceFolder, CONFIG_FILE.mainBGImage));
                HOMEFORM.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            //make app sit on top
            //HOMEFORM.TopMost = true;
        }

        private bool ButtonSetup()
        {
            //initialise button list and set as invisible
            BUTTONLIST = new PictureBox[CONFIG_FILE.numOfButtons];
            for (int i = 0; i < BUTTONLIST.Length; i++)
            {
                BUTTONLIST[i] = new PictureBox
                {
                    Name = "AppButton" + i,
                    BackColor = Color.White,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                BUTTONLIST[i].Click += ButtonClick;
                BUTTONLIST[i].Paint += ButtonText_Paint;
                HOMEFORM.Controls.Add(BUTTONLIST[i]);
            }

            //setup all app buttons
            Image image;
            for(int i = 0; i < CONFIG_FILE.numOfButtons; i++)
            {
                ButtonConfig temp = CONFIG_FILE.appButtonsConfig[i];
                //get image from path
                try { image = Image.FromFile(Path.Combine(CONFIG_FILE.resourceFolder, temp.backgroundImageName)); }
                catch { image = null; }
                BUTTONLIST[i].Image = image;
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
            int buttonCount = 0;
            for (int i = 0; i < CONFIG_FILE.mainRows; i++)
            {
                for (int j = 0; j < perRow && buttonCount < CONFIG_FILE.numOfButtons; j++)
                {
                    int currentButton = (i * perRow) + j;
                    BUTTONLIST[currentButton].Size = new Size(CONFIG_FILE.buttonWidth, CONFIG_FILE.buttonHeight);
                    BUTTONLIST[currentButton].Location = new Point((((bounds.Width - (CONFIG_FILE.buttonWidth * perRow)) / (perRow + 1)) * (j + 1)) + (CONFIG_FILE.buttonWidth * j) + bounds.X,
                        (((bounds.Height - (CONFIG_FILE.buttonHeight * CONFIG_FILE.mainRows)) / (CONFIG_FILE.mainRows + 1)) * (i + 1)) + (CONFIG_FILE.buttonHeight * i) + bounds.Y);

                    if (j == perRow - 1) BUTTONLIST[currentButton].Size = new Size(BUTTONLIST[currentButton].Size.Width + widthCorrection, BUTTONLIST[currentButton].Size.Height);
                    if (i == CONFIG_FILE.mainRows - 1) BUTTONLIST[currentButton].Size = new Size(BUTTONLIST[currentButton].Size.Width, BUTTONLIST[currentButton].Size.Height + heightCorrection);

                    buttonCount++;
                }
            }
        }

        private void ButtonText_Paint(object sender, PaintEventArgs e)
        {
            int buttonNum = Convert.ToInt32((sender as PictureBox).Name.Substring(9));

            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            ButtonConfig temp = CONFIG_FILE.appButtonsConfig[buttonNum];

            using (Font myFont = new Font("Arial", temp.fontSize))
            {
                e.Graphics.DrawString(temp.buttonText, myFont, new SolidBrush(Color.FromName(temp.textColor)), new Point(CONFIG_FILE.buttonWidth / 2, CONFIG_FILE.buttonHeight / 2), format);
            }
        }
        #endregion

        #region Navigation Handlers
        //process button click
        private void ButtonClick(object sender, EventArgs e)
        {
            string name = (sender as PictureBox).Name;
            name = name.Substring(9);

            ProcWindow = CONFIG_FILE.appButtonsConfig[Convert.ToInt32(name)].appProcName;
            SwitchPrep();
        }

        private void SwitchPrep()
        {
            //switch to window in background
            SwitchWindow();
            //change visible screen
            OVERLAYFORM.Visible = true;
            HOMEFORM.Visible = false;
            //set idle timer
            OVERLAYFORM.SetTimer();
        }

        //import dll
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

        //switch window, loop to find correct process
        private void SwitchWindow()
        {
            //get processes by name and switch to process window
            Process[] procs = Process.GetProcessesByName(ProcWindow);
            foreach (Process proc in procs) SwitchToThisWindow(proc.MainWindowHandle, false);
        }
        #endregion
    }
}
