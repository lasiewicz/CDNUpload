using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryMonitoring
{
    public class MyFileSystemWatcher : FileSystemWatcher
    {
        private string fname;
        public MyFileSystemWatcher()
        {
            Init();
        }

        public MyFileSystemWatcher(String inDirectoryPath)
            : base(inDirectoryPath)
        {
            Init();
        }

        public MyFileSystemWatcher(String inDirectoryPath, string inFilter)
            : base(inDirectoryPath, inFilter)
        {
            Init();
        }

        private void Init()
        {
            IncludeSubdirectories = true;
            // Eliminate duplicates when timestamp doesn't change
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size; // The default also has NotifyFilters.LastWrite
            EnableRaisingEvents = true;
            Created += Watcher_Created;
            Changed += Watcher_Changed;
            Deleted += Watcher_Deleted;
            Renamed += Watcher_Renamed;
        }

        public void Watcher_Created(object source, FileSystemEventArgs inArgs)
        {
            if (fname != inArgs.FullPath)
            {
                Log.WriteLine(inArgs.FullPath);
                fname = inArgs.FullPath;
            }


        }

        public void Watcher_Changed(object sender, FileSystemEventArgs inArgs)
        {
            if (fname != inArgs.FullPath)
            {
                Log.WriteLine(inArgs.FullPath);
                fname = inArgs.FullPath;
            }
        }

        public void Watcher_Deleted(object sender, FileSystemEventArgs inArgs)
        {
           // Log.WriteLine("File deleted: " + inArgs.FullPath);
        }

        public void Watcher_Renamed(object sender, RenamedEventArgs inArgs)
        {
          //  Log.WriteLine("File renamed: " + inArgs.OldFullPath + ", New name: " + inArgs.FullPath);
        }
    }
}
