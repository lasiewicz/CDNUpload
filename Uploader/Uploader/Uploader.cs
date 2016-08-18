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
using System.IO;
namespace Uploader
{
    public partial class Uploader : Form
    {
        public string jobnumber;
        public Uploader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            jobnumber = jobtextbox.Text;
            getjob();
        }

     

        private void Uploader_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
           
            Log.Instance.LogPath = "c:\\logs";
            Log.Instance.LogFileName = "CNDUploader";
            if (args.Length > 1)
            {
                jobnumber = args[1];
                getjob();
            }
        }

        private void getjob()
        {
            string jobroot = "c:\\jobs\\";
            string searchroot = "\\\\172.18.89.75\\nmg01\\Upload";
            string jobtextplace = jobroot + jobnumber ;
            Jobfiles.Instance.fnames= System.IO.File.ReadAllLines(@jobtextplace);
   
            for (int x=0;x<Jobfiles.Instance.fnames.Length;x++)
            {
                try
                {
                    string apple = Jobfiles.Instance.fnames[x].Substring(searchroot.Length +1, Jobfiles.Instance.fnames[x].Length - searchroot.Length-1);
                    string cppath = "c:\\test\\" + apple;
                    int t = 0;
                    for (int y = cppath.Length-1; y > 1; y--)
                    {
                        if (cppath.Substring(y, 1) == "\\")
                        {
                            t = y + 1;
                            break;
                        }
                    }
                    string containdir = cppath.Substring(0, t);
                    if (!Directory.Exists(containdir))
                    {
                        Directory.CreateDirectory(containdir);
                    }
                    
                    File.Copy(Jobfiles.Instance.fnames[x], cppath);
                    Log.Instance.WriteLineToLog("copied " + cppath);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.ToString());
                    Log.Instance.WriteLineToLog("error copying " + Jobfiles.Instance.fnames[x]);
                }




            }
            Application.Exit();

           

        }

        private void jobtextbox_TextChanged(object sender, EventArgs e)
        {

        }
        private void processjob()
        {

        }
       
    }
}

