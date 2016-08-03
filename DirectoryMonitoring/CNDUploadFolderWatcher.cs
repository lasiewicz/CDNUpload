using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceProcess;

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
            Log.WriteLine("start");
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
                    string wtext = "job" + Job.Instance.Jobnumber;
                    Log.WriteLine(wtext);
                    justwrotestuff = false;
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
