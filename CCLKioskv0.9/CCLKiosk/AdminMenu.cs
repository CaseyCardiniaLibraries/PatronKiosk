using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Printing;
using System.IO;

namespace CCLKiosk
{
    public partial class AdminMenu : Form
    {
        #region Variable Declaration
        //form objects
        HomeForm HOMEFORM;
        AdminMenu ADMINMENU;

        //timer display
        int timeLeft;
        #endregion

        #region Initialisation
        public AdminMenu(HomeForm homeForm, string theme)
        {
            //set parent form
            HOMEFORM = homeForm;
            //set current form
            ADMINMENU = this;

            InitializeComponent();

            //setstyle
            ADMINMENU.BackColor = Color.FromName(theme);
            messageLabel.Text = "Admin Menu";

            Font tempFont = new Font("Arial", homeForm.CONFIG_FILE.timeoutFontSize);
            messageLabel.Font = tempFont;
            EditConfigButton.Font = tempFont;
            RestartAppButton.Font = tempFont;
            QuitAppButton.Font = tempFont;
            ExitButton.Font = tempFont;

            ADMINMENU.Size = new Size(homeForm.CONFIG_FILE.timeoutWidth, homeForm.CONFIG_FILE.timeoutHeight);
            messageLabel.Size = new Size(ADMINMENU.Width, messageLabel.Height);
            EditConfigButton.Size = new Size(ADMINMENU.Width / 5, ADMINMENU.Height / 4);
            RestartAppButton.Size = new Size(ADMINMENU.Width / 5, ADMINMENU.Height / 4);
            QuitAppButton.Size = new Size(ADMINMENU.Width / 5, ADMINMENU.Height / 4);
            ExitButton.Size = new Size(40, 40);

            ADMINMENU.CenterToScreen();
            messageLabel.Location = new Point((ADMINMENU.Width / 2) - (messageLabel.Width / 2), (ADMINMENU.Height / 6) * 1);
            EditConfigButton.Location = new Point((ADMINMENU.Width / 2) - (EditConfigButton.Width / 2), (ADMINMENU.Height / 7) * 4);
            RestartAppButton.Location = new Point((ADMINMENU.Width / 4) - (EditConfigButton.Width / 2), (ADMINMENU.Height / 7) * 4);
            QuitAppButton.Location = new Point(((ADMINMENU.Width / 4) * 3) - (EditConfigButton.Width / 2), (ADMINMENU.Height / 7) * 4);
            ExitButton.Location = new Point(ADMINMENU.Width - ExitButton.Width - 10, 10);
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            //place on top of windows
            ADMINMENU.TopMost = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //set black border for window
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
        #endregion

        #region Dialogue Handlers
        private void EditConfig_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine((new FileInfo(Application.ExecutablePath).Directory.FullName).ToString(), "KioskConfig.exe"));
        }

        private void CloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //private void PurgePrintQueue_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LocalPrintServer ps = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer);
        //        PrintQueue pq = new PrintQueue(ps, ps.DefaultPrintQueue.FullName, PrintSystemDesiredAccess.AdministratePrinter);

        //        if (pq.NumberOfJobs > 0)
        //        {
        //            pq.Purge();
        //        }
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            PrintServer ps = new PrintServer(PrintSystemDesiredAccess.AdministrateServer);
        //            PrintQueue pq = new PrintQueue(ps, ps.DefaultSpoolDirectory, PrintSystemDesiredAccess.AdministratePrinter);

        //            if (pq.NumberOfJobs > 0)
        //            {
        //                pq.Purge();
        //            }
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RestartApp_Click(object sender, EventArgs e)
        {
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
        #endregion
    }
}