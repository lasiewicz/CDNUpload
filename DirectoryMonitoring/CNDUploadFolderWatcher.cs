using System.IO;
using System.ServiceProcess;

namespace DirectoryMonitoring
{
    public partial class DirectoryMonitoringService : ServiceBase
    {
        protected FileSystemWatcher Watcher;

        // Directory must already exist unless you want to add your own code to create it.
        string PathToFolder = @"C:\f2";

        public DirectoryMonitoringService()
        {
            Log.Instance.LogPath = @"C:\Logs";
            Log.Instance.LogFileName = "DirectoryMonitoring";
            Watcher = new MyFileSystemWatcher(PathToFolder);
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            //if there has been no actvity in the last minute
            
        }
    }
}
