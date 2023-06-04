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
        private string fileName;
        private double mediaDuration;
        OpenFileDialog fd = new OpenFileDialog();
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        bool isMediaEnded = true;
        bool isPaused = false;
        double pausedPosition = 0;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            wplayer.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wplayer_PlayStateChange);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileName = fd.FileName;
                listView1.Items.Add(fileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.SelectedItems[0].Remove();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Pliki");
            listView1.Columns[0].Width = 300;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                

                if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    button4.Text = "▷";
                    wplayer.controls.pause();
                    pausedPosition = wplayer.controls.currentPosition;
                    isPaused = true;
                }
                else if (wplayer.playState == WMPLib.WMPPlayState.wmppsPaused || wplayer.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    button4.Text = "||";
                    wplayer.controls.currentPosition = pausedPosition;
                    wplayer.controls.play();

                    if (!isPaused)
                    {
                        mediaDuration = wplayer.currentMedia.duration;
                        label2.Text = TimeSpan.FromSeconds(mediaDuration).ToString(@"hh\:mm\:ss");
                        timer1.Start();
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fileName = listView1.SelectedItems[0].Text;
            wplayer.URL = fileName;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                label1.Text = TimeSpan.FromSeconds(wplayer.controls.currentPosition).ToString(@"hh\:mm\:ss");

                double progressPercentage = (wplayer.controls.currentPosition / mediaDuration) * 100;
                progressBar1.Value = Math.Min((int)progressPercentage, 100);
            }
            else if (wplayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer1.Stop();
                progressBar1.Value = 0;
            }
        }

        private void wplayer_PlayStateChange(int newState)
        {
            if ((WMPLib.WMPPlayState)newState == WMPLib.WMPPlayState.wmppsStopped)
            {
                isMediaEnded = true;
                pausedPosition = 0;
            }

            if ((WMPLib.WMPPlayState)newState == WMPLib.WMPPlayState.wmppsPlaying && isMediaEnded)
            {
                isMediaEnded = false;
                if (isPaused)
                {
                    wplayer.controls.currentPosition = pausedPosition;
                }
                else
                {
                    mediaDuration = wplayer.currentMedia.duration;
                    label2.Text = TimeSpan.FromSeconds(mediaDuration).ToString(@"hh\:mm\:ss");
                    timer1.Start();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            wplayer.controls.stop();
            timer1.Stop();
            progressBar1.Value = 0;
            isPaused = false;
        }
    }
}
