using System;
using System.IO;

namespace DirectoryMonitoring
{
    public class Log
    {
        public static Log Instance = new Log();

        private Log()
        {
            //if ()
            LogFileName = "Start";
            LogFileExtension = ".log";
        }

        public StreamWriter Writer { get; set; }

        public string LogPath { get; set; }

        public string LogFileName { get; set; }

        public string LogFileExtension { get; set; }

        public string LogFile { get { return LogFileName + LogFileExtension; } }

        public string LogFullPath { get { return Path.Combine(LogPath, LogFile); } }

        public bool LogExists { get { return File.Exists(LogFullPath); } }

        public void WriteLineToLog(String inLogMessage)
        {
            WriteToLog(inLogMessage + Environment.NewLine);
        }

        public void WriteToLog(String inLogMessage)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if (Writer == null)
            {
                Writer = new StreamWriter(LogFullPath, true);
            }

            Writer.Write(inLogMessage);
            Writer.Flush();
        }
       

        public static void WriteLine(String inLogMessage)
        {
      
            Log.Instance.WriteLineToLog(inLogMessage);
        }

        public static void Write(String inLogMessage)
        {
            Log.Instance.WriteToLog(inLogMessage);
        }
    }
}
