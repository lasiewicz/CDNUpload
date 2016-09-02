using System;
using System.Windows.Forms;
using DirectoryMonitoring;
using System.IO;
using System.Configuration;

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

            Ftpstuff.Instance.jobnumber = jobtextbox.Text;
            Ftpstuff.Instance.getjob(false);
            Application.Exit();

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
            Ftpstuff.Instance.Login = appSettings.Settings["login"].Value;
            Ftpstuff.Instance.Password= appSettings.Settings["password"].Value;
            Ftpstuff.Instance.Host = appSettings.Settings["host"].Value;
            Ftpstuff.Instance.TooEmail = appSettings.Settings["Sendto"].Value;
            Ftpstuff.Instance.jobroot = appSettings.Settings["jobs"].Value;
            Ftpstuff.Instance.searchroot = appSettings.Settings["searchroot"].Value;
            Ftpstuff.Instance.ftptype = appSettings.Settings["ftptype"].Value;


            if (args.Length ==2)

            {
                Log.Instance.LogFileName = "CNDUploader";
                Ftpstuff.Instance.jobnumber = args[1];
                Ftpstuff.Instance.getjob(false);
            }

            if (args.Length == 3)

            {
                Log.Instance.LogFileName = "FileCompare";
                Ftpstuff.Instance.jobnumber = args[1];
                Ftpstuff.Instance.getjob(true);
            }

            //Application.Exit();
        }


        private void jobtextbox_TextChanged(object sender, EventArgs e)
        {

        }
        private void processjob()
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Ftpstuff.Instance.test();
        }
    }
}

