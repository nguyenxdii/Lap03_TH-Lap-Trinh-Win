using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lap03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var time = DateTime.Now.ToString("hh:mm:ss tt");
            this.toolStripStatusLabel1.Text = string.Format("Hôm nay là ngày: {0} - Bây giờ là {1}", date, time);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdOpen.Filter = "Video Files|*.mp4;*.mov;*.wmv;*.avi|All Files|*.*";
            if (ofdOpen.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = ofdOpen.FileName;
                //axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
    }
}
