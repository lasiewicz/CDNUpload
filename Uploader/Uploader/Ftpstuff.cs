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
        public string searchroot;
        public string jobroot;
        public BMail MailObject = new BMail();

        private Ftpstuff()
        {


        }


        public void getjob(bool compare)
        {
            string fc = "";
            bool iret;
            string jobtextplace = jobroot + jobnumber;
            Jobfiles.Instance.fnames = System.IO.File.ReadAllLines(@jobtextplace);
            int unsuccfilecompare = 0;
            using (FtpClient conn = new FtpClient())
            {
                conn.Host = Ftpstuff.Instance.Host;
                conn.Credentials = new NetworkCredential(Ftpstuff.Instance.Login, Ftpstuff.Instance.Password);
                conn.Connect();
                for (int x = 0; x < Jobfiles.Instance.fnames.Length; x++)
                {
                    if (Jobfiles.Instance.fnames[x].Length > 2)
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

                                if (compare == false)
                                {
                                    if (!conn.DirectoryExists(cppath))
                                    {
                                        conn.CreateDirectory(cppath);
                                    }
                                    using (var ftpStream = conn.OpenWrite(cppath))
                                    using (var inputStream = new FileStream(Jobfiles.Instance.fnames[x], FileMode.Open))
                                    {
                                        inputStream.CopyTo(ftpStream);
                                        Thread.Sleep(2000);
                                    }
                                    Log.Instance.WriteLineToLog("copied " + cppath);
                                }
                                if (compare == true)
                                {
                                    long ftpfile = conn.GetFileSize(cppath);
                                    FileInfo fInfo = new FileInfo(Jobfiles.Instance.fnames[x]);
                                    long size = fInfo.Length;
                                    if (size == ftpfile)
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
                string test = MailObject.SendEmail(TooEmail, subject, Mes);
            }

        }
    }
}