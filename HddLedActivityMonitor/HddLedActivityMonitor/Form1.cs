using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
using System.Threading;

namespace HddLedActivityMonitor
{
    public partial class noForm : Form
    {
        #region globalne promenljive
        NotifyIcon HddTrayIcon;
        Icon hdd_busy;
        Icon hdd_idle;
        Thread HddLedWorkerThread;
        #endregion
        #region sakrivanje forme,dodeljivanje menu itema, etc ..
        public noForm()
        {
            InitializeComponent();
            
            //Ucitavanje ikona za stanje diska
            hdd_busy = new Icon("HDD_Busy.ico");
            hdd_idle = new Icon("HDD_Idle.ico");
            //Setovanje da po defaultu ikonica u trayu bude idle
            HddTrayIcon = new NotifyIcon();
            HddTrayIcon.Icon = hdd_idle;
            HddTrayIcon.Visible = true;

            //Kreirani menu itemi i dodati u contextmenu
            MenuItem About = new MenuItem("Verzija 1.00 BETA | Aleksandar Babic");
            MenuItem quitMenu = new MenuItem("Izlaz");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(About);
            contextMenu.MenuItems.Add(quitMenu);
            HddTrayIcon.ContextMenu = contextMenu;

            quitMenu.Click += QuitMenu_Click;

            //Sakriven UI jer pravim aplikaciju koja radi samo u system trayu
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Pokretanje threada
            HddLedWorkerThread = new Thread(HddThreadMetoda);
            HddLedWorkerThread.Start();
        }
        /// <summary>
        /// Zatvori aplikaciju pritiskom na quit dugme 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion
        #region contextmenu Event handleri
        private void QuitMenu_Click(object sender, EventArgs e)
        {
            HddLedWorkerThread.Abort();
            HddTrayIcon.Dispose();
            this.Close();
        }


        #endregion
        #region Threadovi
        public void HddThreadMetoda() {
            ManagementClass driveDataClass = new ManagementClass("Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
            try
            {
                while (true) {
                    //Konektovanje na driveDataClass i uzimanje svih instanci
                    ManagementObjectCollection driveDataClassCollection = driveDataClass.GetInstances();
                    foreach (ManagementObject obj in driveDataClassCollection) {
                        if(obj["Name"].ToString() == "_Total") {
                            if (Convert.ToUInt64(obj["DiskBytesPerSec"]) > 0) {
                                HddTrayIcon.Icon = hdd_busy;
                            }
                            else{
                                HddTrayIcon.Icon = hdd_idle;
                            }
                        }
                    }


                    Thread.Sleep(100);
                }
            }
            catch(ThreadAbortException tbe) {
                driveDataClass.Dispose();
            }
            
        }
        #endregion
    }
}
