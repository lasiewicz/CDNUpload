using System;
using System.Windows.Forms;
using DirectoryMonitoring;
using System.IO;
using System.Configuration;

namespace Uploader
{
    public partial class Uploader : Form
    {
        public BMail MailObject = new BMail();
        public string jobnumber;
        public string TooEmail;
        public string fc;
        public Uploader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            jobnumber = jobtextbox.Text;
            getjob(true);
        }

     

        private void Uploader_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            Log.Instance.LogFileName = "FileCompare";


            System.Configuration.Configuration config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Configuration.AppSettingsSection appSettings =
                (System.Configuration.AppSettingsSection)config.GetSection("appSettings");
            Log.Instance.LogPath = appSettings.Settings["logs"].Value;
            
            TooEmail = appSettings.Settings["Sendto"].Value;

         

            if (args.Length ==2)

            {
                Log.Instance.LogFileName = "CNDUploader";
                jobnumber = args[1];
                getjob(false);
            }

            if (args.Length == 3)

            {
                Log.Instance.LogFileName = "FileCompare";
                jobnumber = args[1];
                getjob(true);
            }
        }

        private void getjob(bool compare)
        {

            fc = "";
            string jobroot = "c:\\jobs\\";
            string searchroot = "\\\\172.18.89.75\\nmg01\\Upload";
            string jobtextplace = jobroot + jobnumber ;
            Jobfiles.Instance.fnames= System.IO.File.ReadAllLines(@jobtextplace);
            int unsuccfilecompare = 0;
            for (int x=0;x<Jobfiles.Instance.fnames.Length-1;x++)
            {
                try
                {
                    string apple = Jobfiles.Instance.fnames[x].Substring(searchroot.Length +1, Jobfiles.Instance.fnames[x].Length - searchroot.Length-1);
                    string cppath = "c:\\test\\" + apple;
                    int t = 0;
                    
                    FileCompare crc = new FileCompare();
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
                    if (compare==false)
                    {
                        File.Copy(Jobfiles.Instance.fnames[x], cppath);
                        Log.Instance.WriteLineToLog("copied " + cppath);
                    }
                    if (compare == true)
                    {
                        
                        if (crc.FileEquals(Jobfiles.Instance.fnames[x], cppath))
                        {
                            Log.Instance.WriteLineToLog("file compare successful for " + cppath);
                            fc = fc + "successful for " + cppath + (char)13;


                        }
                        else
                        {
                            Log.Instance.WriteLineToLog("file compare NOT successful for " + cppath);
                            fc = fc + "NOT successful file compare for " + cppath + (char)13;
                            unsuccfilecompare++;
                        }
                       
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.ToString());
                    Log.Instance.WriteLineToLog("error with " + Jobfiles.Instance.fnames[x]);
                }




            }
            if (compare == true)
            {
                string subject = "";
                string Mes = "";
                if (unsuccfilecompare > 0)
                {
                    subject = " upload for job" + jobnumber + " completed with errors";
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows "  + (char)13;
                    Mes = Mes + fc;
                }
                else
                {
                    subject = " upload for job" + jobnumber + " completed";
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows " + (char)13;
                    Mes = Mes + fc;
                }


                string test = MailObject.SendEmail(TooEmail, subject, Mes);
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

