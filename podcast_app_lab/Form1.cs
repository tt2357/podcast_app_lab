using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace podcast_app_lab
{
    public partial class Form1 : Form
    {
        Timer timer1 = new Timer();
        private int start = 0;
        private string nazwapliku;
        private double mduration;
        OpenFileDialog fd = new OpenFileDialog();
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fd.ShowDialog();
            string filename = fd.FileName;
            listView1.Items.Add(filename);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].Remove();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Pliki");
            listView1.Columns[0].Width = 300;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                nazwapliku = listView1.SelectedItems[0].Text;
            }
            wplayer.URL = nazwapliku;
            wplayer.controls.play();
            mduration = wplayer.currentMedia.duration;

            timer1.Tick += new EventHandler(timer1_Tick);
            label1.Text = start.ToString();
            timer1.Start();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            start++;
            label1.Text = start.ToString() + "/" + mduration.ToString();

            if (mduration == start)
            {
                timer1.Stop();
            }
        }
    }
}
