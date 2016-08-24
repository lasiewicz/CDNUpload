using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.FtpClient;
using System.Security.Authentication;
using System.IO;
using DirectoryMonitoring;
using System.Net;
using System.Threading;

namespace Uploader
{
    class Ftpstuff
    {
        public static Ftpstuff Instance = new Ftpstuff();
        public string Login { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }
        public string jobnumber;
        public string TooEmail;
        public string fc;
        public string subject;
        public BMail MailObject = new BMail();
        //public FtpClient conn;

        /*
    public void Connect()
    {
        using (Ftpstuff.Instance.conn = new FtpClient())
        {
            Ftpstuff.Instance.conn.Host = Ftpstuff.Instance.Host;
            Ftpstuff.Instance.conn.Credentials = new NetworkCredential(Ftpstuff.Instance.Login, Ftpstuff.Instance.Password);
            Ftpstuff.Instance.conn.Connect();

        }
    }
    */
        private Ftpstuff()
        {
           

        }
       public bool checkDirectory(string dir)
        {
            //conn.BeginCreateDirectory(dir, true,
                //    new AsyncCallback(CreateDirectoryCallback), Ftpstuff.Instance.conn);

            return true;
        }
        public bool CopyFile(string f1,string f2)
        {
            //Connect();
            using (FtpClient conn = new FtpClient())
            {
                conn.Host = Ftpstuff.Instance.Host;
                conn.Credentials = new NetworkCredential(Ftpstuff.Instance.Login, Ftpstuff.Instance.Password);
                conn.Connect();

            
            f2 = "//ht//deluxdigi";
            var remoteFileName = Path.GetFileName(f1);
            var fullRemotePath = string.Format("{0}/{1}", f2, remoteFileName);

            using (var ftpStream =conn.OpenWrite(fullRemotePath))
            using (var inputStream = new FileStream(f1, FileMode.Open))
            {
                inputStream.CopyTo(ftpStream);
                Thread.Sleep(2000);  
            }
            }

            return true;

        }
        public void getjob(bool compare)
        {

            string fc = "";
            bool iret;
            string jobroot = "c:\\jobs\\";
            string searchroot = "\\\\172.18.89.75\\nmg01\\Upload";
            string jobtextplace = jobroot + jobnumber;
            Jobfiles.Instance.fnames = System.IO.File.ReadAllLines(@jobtextplace);
            int unsuccfilecompare = 0;
            for (int x = 0; x < Jobfiles.Instance.fnames.Length; x++)
            {
                try
                {
                    string apple = Jobfiles.Instance.fnames[x].Substring(searchroot.Length + 1, Jobfiles.Instance.fnames[x].Length - searchroot.Length - 1);
                    apple = apple.Replace("\\", @"/");
                    string cppath = "/" + apple;
                    int t = 0;

                    FileCompare crc = new FileCompare();
                    for (int y = cppath.Length - 1; y > 1; y--)
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
                        // Ftpstuff.Instance.Connect();
                        iret = Ftpstuff.Instance.checkDirectory(containdir);
                        //Directory.CreateDirectory(containdir);
                    }
                    if (compare == false)
                    {
                        //File.Copy(Jobfiles.Instance.fnames[x], cppath);
                        iret = Ftpstuff.Instance.CopyFile(Jobfiles.Instance.fnames[x], cppath);
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

                subject = "Upload for job" + jobnumber;
                string Mes = "";
                if (unsuccfilecompare > 0)
                {
                    subject = "Upload for job " + jobnumber + " completed with errors";
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows " + (char)13;
                    Mes = Mes + fc;
                }
                else
                {
                    subject = "Upload for job" + jobnumber + " completed";
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows " + (char)13;
                    Mes = Mes + fc;
                }
                // Mes = "Hi";
                //  subject = "happycat";
                string test = MailObject.SendEmail(TooEmail, subject, Mes);
            }


          


        }

        static void CreateDirectoryCallback(IAsyncResult ar)
        {
           /*

            try
            {
                if (Ftpstuff.Instance.conn == null)
                    throw new InvalidOperationException("The FtpControlConnection object is null!");

                Ftpstuff.Instance.conn.EndCreateDirectory(ar);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLineToLog(ex.ToString());
            }
            */
           
        }

    }
}
