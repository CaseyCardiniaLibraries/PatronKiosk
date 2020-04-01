using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static CCLKiosk.Configuration;
using CefSharp;
using CefSharp.WinForms;

namespace CCLKiosk
{
    public partial class OverlayForm : Form
    {
        #region Variable Declaration
        //form objects
        private HomeForm HOMEFORM;
        private OverlayForm OVERLAYFORM;
        private DialogueForm DIALOGUEFORM;
        private PictureBox HomeButton;
        private PictureBox KeyboardButton;

        //idle timer objects
        private int TIMEOUT;
        private int IDLETIMEOUT;
        private int idleCount;
        private bool dialogueUp = false;

        //browser
        private ChromiumWebBrowser browser;
        private Label border;

        //keyboard
        private Button[] keyboardButtons;
        private int[] keysPerRow = new int[] { 15, 30, 43 , 55, 56 };
        private bool caps = false;
        private bool shift = false;
        private string[,] keyboardKeys = new string[,] { {"{ESC}" , "{ESC}"} , {"`" , "~"} , {"1" , "!"} , {"2" , "@"} , {"3" , "#"} , {"4" , "$"} , {"5" , "%"} , {"6" , "^"} , {"7" , "&"} , {"8" , "*"} , {"9" , "("} , 
            {"0" , ")"} , {"-" , "_"} , {"=" , "+"} , {"{BS}" , "{BS}"} , {"{TAB}" , "{TAB}"} , {"q", "Q"} , {"w" , "W"} , {"e" , "E"} , {"r" , "R"} , {"t" , "T"} , {"y" , "Y"} , {"u" , "U"} , {"i" , "I"} , {"o" , "O"} , {"p" , "P"} , {"[" , "{"} , 
            {"]" , "}"} , {"\\" , "|"} , {"{DEL}" , "{DEL}"} , {"{CAPSLOCK}" , "{CAPSLOCK}"} , {"a" , "A"} , {"s" , "S"} , {"d" , "D"} , {"f" , "F"} , {"g" , "G"} , {"h" , "H"} , {"j" , "J"} , {"k" , "K"} , {"l" , "L"} , {";" , ":"} , {"'" , "\""} , 
            {"{ENTER}" , "{ENTER}"} , {"{SHIFT L}", "{SHIFT L}"} , {"z" , "Z"} , {"x" , "X"} , {"c" , "C"} , {"v" , "V"} , {"b" , "B"} , {"n" , "N"} , {"m" , "M"} , {"," , "<"} , {"." , ">"} , {"/" , "?"} , {"{SHIFT R}", "{SHIFT R}"} , { " " , " " } };
        private string[,] keyboardText = new string[,] { {"ESC" , "ESC"} , {"`" , "~"} , {"1" , "!"} , {"2" , "@"} , {"3" , "#"} , {"4" , "$"} , {"5" , "%"} , {"6" , "^"} , {"7" , "&"} , {"8" , "*"} , {"9" , "("} ,
            {"0" , ")"} , {"-" , "_"} , {"=" , "+"} , {"<<" , "<<"} , {"TAB" , "TAB"} , {"q", "Q"} , {"w" , "W"} , {"e" , "E"} , {"r" , "R"} , {"t" , "T"} , {"y" , "Y"} , {"u" , "U"} , {"i" , "I"} , {"o" , "O"} , {"p" , "P"} , {"[" , "{"} ,
            {"]" , "}"} , {"\\" , "|"} , {"DEL" , "DEL"} , {"CAPS" , "CAPS"} , {"a" , "A"} , {"s" , "S"} , {"d" , "D"} , {"f" , "F"} , {"g" , "G"} , {"h" , "H"} , {"j" , "J"} , {"k" , "K"} , {"l" , "L"} , {";" , ":"} , {"'" , "\""} ,
            {"ENTER" , "ENTER"} , {"SHIFT", "SHIFT"}, {"z" , "Z"} , {"x" , "X"} , {"c" , "C"} , {"v" , "V"} , {"b" , "B"} , {"n" , "N"} , {"m" , "M"} , {"," , "<"} , {"." , ">"} , {"/" , "?"}, {"SHIFT", "SHIFT"} , { "SPACE" , "SPACE" } };
        private double[] keySizes = new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1.5, 1.5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2.5, 2.5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 15.5 };
        private int[] keyFontSizes = new int[] { 14, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 14, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18 };
        private bool[] keyShadow = new bool[] { false, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, false, false };
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
            HomeButton = new PictureBox();
            OVERLAYFORM.Controls.Add(HomeButton);
            HomeButton.Image = Image.FromFile(Path.Combine(HOMEFORM.CONFIG_FILE.resourceFolder, temp.backgroundImageName));
            HomeButton.SizeMode = PictureBoxSizeMode.StretchImage;
            HomeButton.Click += HomeButton_Click;
            HomeButton.Paint += ButtonText_Paint;

            //set button size
            Rectangle bounds = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom);
            OVERLAYFORM.DesktopBounds = bounds;
            OVERLAYFORM.Size = bounds.Size;

            HomeButton.Size = new Size(temp.buttonWidth, temp.buttonHeight);
            SetButtonPos(HomeButton, temp, temp.buttonPosition);

            //timeout config
            TIMEOUT = HOMEFORM.CONFIG_FILE.timeoutTime;
            IDLETIMEOUT = HOMEFORM.CONFIG_FILE.idleTimeout;

            //browser control
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser("www.google.com.au")
            {
                Size = new Size(bounds.Size.Width - (3 * temp.buttonWidth) - 2, bounds.Size.Height - (3 * temp.buttonHeight) - 2),
                Location = new Point((int)(temp.buttonWidth * 1.5) + 1, (int)(temp.buttonHeight * 1.5) + 1),
                Dock = DockStyle.None
            };
            OVERLAYFORM.Controls.Add(browser);

            OVERLAYFORM.TransparencyKey = Color.Green;

            border = new Label
            {
                Size = new Size(bounds.Size.Width - (3 * temp.buttonWidth), bounds.Size.Height - (3 * temp.buttonHeight)),
                Location = new Point((int)(temp.buttonWidth * 1.5), (int)(temp.buttonHeight * 1.5)),
                BackColor = Color.Black
            };
            OVERLAYFORM.Controls.Add(border);

            keyboardButtons = new Button[keyboardKeys.Length / 2];
            int rowCount = 0;
            int defxPos = 0;
            for (int i = 0; i < keysPerRow[0]; i++)
            {
                defxPos += (int)(temp.buttonWidth * keySizes[i]);
            }
            defxPos = (bounds.Width / 2) - (defxPos / 2);

            int defyPos = 0;
            defyPos = keysPerRow.Length * temp.buttonHeight;
            defyPos = bounds.Height - defyPos - (temp.buttonHeight * 2);

            int xPos = defxPos;
            int yPos = defyPos;
            for (int i = 0; i < keyboardButtons.Length; i++)
            {
                if (i == keysPerRow[rowCount])
                {
                    rowCount++;
                    yPos = defyPos + (temp.buttonHeight * rowCount);
                    xPos = defxPos;
                }

                keyboardButtons[i] = new Button
                {
                    Text = keyboardText[i, 0],
                    Name = keyboardKeys[i, 0],
                    Size = new Size((int)(temp.buttonWidth * keySizes[i]), temp.buttonHeight),
                    Location = new Point(xPos, yPos),
                    Font = new Font("Arial", keyFontSizes[i])
                };
                OVERLAYFORM.Controls.Add(keyboardButtons[i]);
                keyboardButtons[i].Click += Keyboard_Click;
                keyboardButtons[i].BringToFront();

                if (keyShadow[i]) { keyboardButtons[i].Paint += Key_Paint; }

                xPos += (int)(temp.buttonWidth * keySizes[i]);
            }

            KeyboardButton = new PictureBox()
            {
                Size = HomeButton.Size,
                Image = HomeButton.Image,
                SizeMode = HomeButton.SizeMode,
                Name = "KeyboardButton"
            };

            OVERLAYFORM.Controls.Add(KeyboardButton);
            SetButtonPos(KeyboardButton, temp, "BOTTOMRIGHT");
            KeyboardButton.Click += KeyboardButton_Click;
            KeyboardButton.Paint += ButtonText_Paint;
        }

        private void SetButtonPos(PictureBox toSet, HomeConfig temp, string position)
        {
            //move home button to position
            Rectangle workingArea = Screen.GetWorkingArea(OVERLAYFORM);
            if (position == "TOPRIGHT") toSet.Location = new Point(workingArea.Right - temp.buttonWidth - temp.buttonPaddingHorizontal, workingArea.Top + temp.buttonPaddingVertical);
            else if (position == "TOPLEFT") toSet.Location = new Point(workingArea.Left + temp.buttonPaddingHorizontal, workingArea.Top + temp.buttonPaddingVertical);
            else if (position == "BOTTOMRIGHT") toSet.Location = new Point(workingArea.Right - temp.buttonWidth - temp.buttonPaddingHorizontal, workingArea.Bottom - temp.buttonHeight - (temp.buttonPaddingVertical * 2));
            else if (position == "BOTTOMLEFT") toSet.Location = new Point(workingArea.Left + temp.buttonPaddingHorizontal, workingArea.Bottom - temp.buttonHeight - (temp.buttonPaddingVertical * 2));

        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
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
            string text = temp.buttonText;
            int fontSize = temp.fontSize;

            if ((sender as PictureBox).Name == "KeyboardButton")
            {
                text = "Keyboard";
                fontSize = 10;
            }

            using (Font myFont = new Font("Arial", fontSize))
            {
                e.Graphics.DrawString(text, myFont, new SolidBrush(Color.FromName(temp.textColor)), new Point(temp.buttonWidth / 2, temp.buttonHeight / 2), format);
            }

            //set black border for button
            ControlPaint.DrawBorder(e.Graphics, (sender as PictureBox).ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void Key_Paint(object sender, PaintEventArgs e)
        {
            string keyName = (sender as Button).Name;

            string[] findArray = new string[keyboardKeys.Length / 2];
            for(int i = 0; i < findArray.Length; i++) { findArray[i] = keyboardKeys[i, Convert.ToInt32(Caps())]; }
            int resultIndex = Array.IndexOf(findArray, keyName);

            HomeConfig temp = HOMEFORM.CONFIG_FILE.homeButtonConfig;

            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            using (Font myFont = new Font("Arial", 8))
            {
                e.Graphics.DrawString(keyboardText[resultIndex, Convert.ToInt32(!Caps())], myFont, new SolidBrush(Color.Gray), new Point(temp.buttonWidth / 5, temp.buttonHeight / 5), format);
            }
        }
        #endregion

        #region Timer
        private void TimerIdle_Tick(object sender, EventArgs e)
        {
            GetLastInputInfo(ref lastInPut);
            if (lastInPut.dwTime - inputCheck == 0)
            {
                idleCount++;
            }
            else
            {
                idleCount = 0;
                inputCheck = lastInPut.dwTime;
            }

            if (idleCount >= IDLETIMEOUT && !dialogueUp)
            {
                dialogueUp = true;
                HomeDialogue();
            }
        }

        //set idle timer
        public void SetIdleTimer()
        {
            idleCount = 0;
            GetLastInputInfo(ref lastInPut);
            inputCheck = lastInPut.dwTime;
            timerIdle.Start();
        }

        private void HomeDialogue()
        {
            dialogueUp = true;
            DIALOGUEFORM = new DialogueForm(OVERLAYFORM, HOMEFORM, HOMEFORM.CONFIG_FILE.themeColour, TIMEOUT) { Visible = true };
        }
        #endregion

        #region Navigation Handlers
        //cancel idle timer and go to home screen
        private void HomeButton_Click(object sender, EventArgs e) { if(!dialogueUp) GoHome(); }
        private void KeyboardButton_Click(object sender, EventArgs e) { if (!dialogueUp) KeyboardToggle(); }

        private void KeyboardToggle()
        {
            foreach(Button k in keyboardButtons)
            {
                k.Visible = !k.Visible;
            }
        }

        public void ContinueSession() { dialogueUp = false; }

        public void FocusWindow(bool inBrowser)
        {
            OVERLAYFORM.Visible = true;

            if(inBrowser)
            {
                OVERLAYFORM.BackColor = Color.White;
                browser.Visible = true;
                border.Visible = true;
                KeyboardButton.Visible = true;
            }
            else
            {
                foreach (Button k in keyboardButtons)
                {
                    k.Visible = false;
                }
                KeyboardButton.Visible = false;
                OVERLAYFORM.BackColor = Color.Green;
                browser.Visible = false;
                border.Visible = false;
            }
        }

        public void BrowserURL(string URL)
        {
            Cef.GetGlobalCookieManager().DeleteCookies("", "");
            browser.Load(URL);
        }

        //return to home screen
        public void GoHome()
        {
            dialogueUp = false;
            timerIdle.Stop();
            HOMEFORM.Visible = true;
            HOMEFORM.ShowMain();
            OVERLAYFORM.Visible = false;
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

        private void Keyboard_Click(object sender, EventArgs e)
        {
            if((sender as Button).Name == "{CAPSLOCK}")
            {
                caps = !caps;
                if(shift) { shift = !shift; }

                KeyboardCaps();
            }
            else
            {
                if ((sender as Button).Name == "{SHIFT L}" || (sender as Button).Name == "{SHIFT R}")
                {
                    shift = !shift;
                    KeyboardCaps();
                }
                else
                {
                    browser.Focus();
                    SendKeys.Send((sender as Button).Name);

                    if(shift)
                    {
                        shift = !shift;
                        KeyboardCaps();
                    }
                }
            }

            if(shift)
            {
                keyboardButtons[43].BackColor = Color.LightGray;
                keyboardButtons[54].BackColor = Color.LightGray;
            }
            else
            {
                keyboardButtons[43].BackColor = Color.White;
                keyboardButtons[54].BackColor = Color.White;
            }
            if (caps)
            {
                keyboardButtons[30].BackColor = Color.LightGray;
            }
            else
            {
                keyboardButtons[30].BackColor = Color.White;
            }
        }

        private void KeyboardCaps()
        {
            bool tog = Caps();

            for (int i = 0; i < keyboardButtons.Length; i++)
            {
                keyboardButtons[i].Text = keyboardText[i, Convert.ToInt32(tog)];
                keyboardButtons[i].Name = keyboardKeys[i, Convert.ToInt32(tog)];
            }
        }

        private bool Caps()
        {
            bool tog = false;
            if (caps && shift) { tog = false; }
            else if (!caps && !shift) { tog = false; }
            else if (!caps && shift) { tog = true; }
            else if (caps && !shift) { tog = true; }

            return tog;
        }
    }
}