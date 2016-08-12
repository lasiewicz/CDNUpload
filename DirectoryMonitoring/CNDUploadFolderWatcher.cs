using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Configuration;
namespace DirectoryMonitoring
{
    public partial class CNDUploadFolderWatcherService : ServiceBase
    {
        protected FileSystemWatcher Watcher;



        // Directory must already exist unless you want to add your own code to create it.

        private string PathToFolder;
        private bool justwrotestuff;
        private System.Threading.Timer IntervalTimer;
        public CNDUploadFolderWatcherService()
        {
    
            System.Configuration.Configuration config =
           ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Configuration.AppSettingsSection appSettings =
                (System.Configuration.AppSettingsSection)config.GetSection("appSettings");
            string logfolder = appSettings.Settings["logs"].Value;
            PathToFolder = appSettings.Settings["watch"].Value;
            Job.Instance.TooEmail = appSettings.Settings["Sendto"].Value;
            Log.Instance.LogPath = @logfolder;
            Log.Instance.LogFileName = "CNDUploadFolderWatcher";
            Job.Instance.Jobsdir = appSettings.Settings["jobs"].Value;
            Job.Instance.logdir = logfolder;
            Watcher = new MyFileSystemWatcher(PathToFolder);
           
        }

        protected override void OnStart(string[] args)
        {
            IntervalTimer = new System.Threading.Timer(new System.Threading.TimerCallback(IntervalTimer_Elapsed), null, 60000, 60000);
           // Log.WriteLine("start");
            Job.Instance.CreateNewNum();
        }

        protected override void OnStop()
        {
            IntervalTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            IntervalTimer.Dispose();
            IntervalTimer = null;
        }
        private void IntervalTimer_Elapsed(object state)
        {   // Do the thing that needs doing every few minutes...
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
