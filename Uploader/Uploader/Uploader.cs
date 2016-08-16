using System;
using System.Net;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectoryMonitoring;

namespace Uploader
{
    public partial class Uploader : Form
    {
        public Uploader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getjob();
        }

     

        private void Uploader_Load(object sender, EventArgs e)
        {

            Log.Instance.LogPath = "c:\\logs";
            Log.Instance.LogFileName = "CNDUploader";

        }

        private void getjob()
        {
            string jobroot = "c:\\jobs\\";
            string jobtextplace = jobroot + jobtextbox.Text + ".txt";
            Jobfiles.Instance.fnames= System.IO.File.ReadAllLines(@jobtextplace);
            for (int x=0;x<Jobfiles.Instance.fnames.Length;)
            {
                //do somthing

            }
           

        }

        private void jobtextbox_TextChanged(object sender, EventArgs e)
        {

        }
        private void processjob()
        {

        }
       
    }
}

