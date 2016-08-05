using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows.Forms;

namespace DirectoryMonitoring
{
    public partial class CNDUploadFolderWatcherService : ServiceBase
    {
        protected FileSystemWatcher Watcher;
        
      
        
        // Directory must already exist unless you want to add your own code to create it.
        string PathToFolder = @"C:\f2";
        private bool justwrotestuff;
        private System.Threading.Timer IntervalTimer;
        public CNDUploadFolderWatcherService()
        {
           
            Log.Instance.LogPath = @"C:\Logs";
            Log.Instance.LogFileName = "CNDUploadFolderWatcher";
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
                    Job.Instance.CreateNewNum();
                    string wtext = "job" + Job.Instance.Jobnumber;
                   // Log.WriteLine(wtext);
                    string newfilepath = "c:\\jobs\\" + Job.Instance.Jobnumber + ".txt";
                    string blankfilepath = Log.Instance.LogPath + "\\blanklog" + ".txt";
                    try
                    {
                        File.Copy(Log.Instance.LogFullPath, newfilepath);
                        // WRITE METHOD TO WRITE ARRAY TO LOG
                    }
                    catch (Exception ex)
                    {
                        
                        
                        Log.WriteLine(ex.ToString());
                    }
                    justwrotestuff = false;
                    Job.Instance.InJob = false;
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
