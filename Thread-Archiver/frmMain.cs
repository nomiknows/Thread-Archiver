using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Thread_Archiver
{
    public partial class frmMain : Form
    {
        private static string msgCaption = "Oh shit, what the fuck are you doing?";

        private ThreadDownloader downloader;
        public frmMain()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = this.folderBrowserDialog1.ShowDialog();
            if (res.Equals(DialogResult.OK))
            {
                // We need an extra \ at the end because Windows is fucking stupid
                txtImageFolder.Text = folderBrowserDialog1.SelectedPath + "\\";                
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // Make sure we can find a thread and save the images
            if (txtThreadLink.Text.Equals(String.Empty))
            {
                MessageBox.Show("You forgot the thread link.", msgCaption);
                return;
            }

            if (txtImageFolder.Text.Equals(String.Empty))
            {
                MessageBox.Show("You haven't given a folder", msgCaption);
                return;
            }

            if (!Directory.Exists(txtImageFolder.Text))
            {
                // Ask if they want to create the directory
                DialogResult res = MessageBox.Show("Did you want to create " + txtImageFolder.Text + "?", "Couldn't find that folder", MessageBoxButtons.YesNo);
                if (res.Equals(DialogResult.No))
                {
                    MessageBox.Show("That path doesn't exist.", msgCaption);
                    return;
                } 

                Directory.CreateDirectory(txtImageFolder.Text);
            }

            downloader = new ThreadDownloader(txtThreadLink.Text);
            downloader.DownloadImages(txtImageFolder.Text);
        }
    }
}
