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
    public partial class Form1 : Form
    {

        NotifyIcon HddTrayIcon;
        Icon hdd_busy;
        Icon hdd_idle;
        Thread HddLedWorkerThread;

        public Form1()
        {
            InitializeComponent();
            
            //Ucitavanje ikona za stanje diska
            hdd_busy = new Icon("HDD_Busy.ico");
            hdd_idle = new Icon("HDD_Idle.ico");
            //Setovanje da po defaultu ikonica u trayu bude idle
            HddTrayIcon = new NotifyIcon();
            HddTrayIcon.Icon = hdd_idle;
            HddTrayIcon.Visible = true;

            MenuItem About = new MenuItem("Verzija 0.001 | Aleksandar Babic");
            MenuItem quitMenu = new MenuItem("Quit");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(About);
            contextMenu.MenuItems.Add(quitMenu);

            HddTrayIcon.ContextMenu = contextMenu;

            //Sakriven UI jer pravim aplikaciju koja radi samo u system trayu
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }
    }
}
