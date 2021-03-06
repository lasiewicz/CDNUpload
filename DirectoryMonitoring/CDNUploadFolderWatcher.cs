﻿using System;
using System.IO;
using System.ServiceProcess;
using System.Configuration;
namespace DirectoryMonitoring
{
    public partial class CDNUploadFolderWatcherService : ServiceBase
    {
        protected FileSystemWatcher Watcher;



        // Directory must already exist unless you want to add your own code to create it.

        private string PathToFolder;
        private bool justwrotestuff;
        private System.Threading.Timer IntervalTimer;
        public CDNUploadFolderWatcherService()
        {

            string logfolder = "c:\\logs";
            PathToFolder = "c:\\test";
            Job.Instance.TooEmail = "lasiewicz@gmail.com";
            Log.Instance.LogPath = @logfolder;
            Log.Instance.LogFileName = "CNDUploadFolderWatcher";
            Job.Instance.Jobsdir = "c:\\jobs";
            Job.Instance.logdir = "c:\\logs";
            try
            {
                System.Configuration.Configuration config =
               ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                System.Configuration.AppSettingsSection appSettings =
                    (System.Configuration.AppSettingsSection)config.GetSection("appSettings");
                logfolder = appSettings.Settings["logs"].Value;
                PathToFolder = appSettings.Settings["watch"].Value;
                Job.Instance.TooEmail = appSettings.Settings["Sendto"].Value;
                Log.Instance.LogPath = @logfolder;
                Log.Instance.LogFileName = "CNDUploadFolderWatcher";
                Job.Instance.Jobsdir = appSettings.Settings["jobs"].Value;
                Job.Instance.logdir = logfolder;
                Watcher = new MyFileSystemWatcher(PathToFolder);
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }

           
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                IntervalTimer = new System.Threading.Timer(new System.Threading.TimerCallback(IntervalTimer_Elapsed), null, 60000, 60000);
                // Log.WriteLine("start");
                Job.Instance.CreateNewNum();
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        
        }

        protected override void OnStop()
        {
            IntervalTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            IntervalTimer.Dispose();
            IntervalTimer = null;
        }
        private void IntervalTimer_Elapsed(object state)
        {   
            if (justwrotestuff)
            {
                if (Job.Instance.InJob == false)
                {
                    
                    try
                    {
                        Job.Instance.Spitoutnewfile();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine(ex.ToString());
                    }
                    justwrotestuff = false;
                    Job.Instance.InJob = false;
                    Job.Instance.CreateNewNum();
                }
            }

            if (Job.Instance.InJob)
            {
                justwrotestuff = true;
                Job.Instance.InJob = false;
            }
            

           
        }

    }
}
