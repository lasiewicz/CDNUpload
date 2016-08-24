﻿using System;
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

