using System;
using System.Net.FtpClient;
using System.IO;
using DirectoryMonitoring;
using System.Net;
using System.Threading;
using WinSCP;

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
              
        public void test()
        {
      
            try
            {
                // Setup session options
                /*
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = "ushens.upload.akamai.com",
                    UserName = "sshacs",
                    Password = "",
                    SshHostKeyFingerprint = "ssh-dss 1024 ee:33:bd:ac:7b:6e:bd:0b:60:6e:49:20:56:cb:00:d3",
                    SshPrivateKeyPath = "C:\\Users\\lasiewiw\\Desktop\\deluxe_xfer_putty_priv.ppk"
                };
                */
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = "deluxdigi-2.upload.llnw.net",
                    UserName = "deluxdigi-ht",
                    Password = "g7tz8x"
                  //  SshHostKeyFingerprint = "ssh-dss 1024 ee:33:bd:ac:7b:6e:bd:0b:60:6e:49:20:56:cb:00:d3",
                   // SshPrivateKeyPath = "C:\\Users\\lasiewiw\\Desktop\\deluxe_xfer_putty_priv.ppk"
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(@"d:\toupload\*", "/home/user/", false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    }
                }

               // return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                Log.Instance.WriteLineToLog(e.ToString());
              //  return 1;
            }
        }
    
        public void getjob(bool compare)
        {
            string fc = "";
            bool iret;
            string jobtextplace = jobroot + jobnumber;
            Jobfiles.Instance.fnames = System.IO.File.ReadAllLines(@jobtextplace);
            int unsuccfilecompare = 0;
            for (int x = 0; x < Jobfiles.Instance.fnames.Length; x++)
            {
                if (Jobfiles.Instance.fnames[x].Length > 2)
                {
                    try
                    {
                        string Tempath = Jobfiles.Instance.fnames[x].Substring(searchroot.Length - 3, Jobfiles.Instance.fnames[x].Length - searchroot.Length + 3);
                        Tempath = Tempath.Replace("\\", @"/");
                        string Cppath = "/" + Tempath;
                        int t = 0;

                        FileCompare crc = new FileCompare();
                        for (int y = Cppath.Length - 1; y > 1; y--)
                        {
                            if (Cppath.Substring(y, 1) == "/")
                            {
                                t = y + 1;
                                break;
                            }
                        }
                        string containdir = Cppath.Substring(0, t);


                        using (FtpClient conn = new FtpClient())
                        { 
                            conn.Host = Ftpstuff.Instance.Host;
                            conn.Credentials = new NetworkCredential(Ftpstuff.Instance.Login, Ftpstuff.Instance.Password);
                            conn.Connect();
                            if (compare == false)
                            {
                                try
                                {
                                    using (var ftpStream = conn.OpenWrite(Cppath))
                                    {
                                        using (var inputStream = new FileStream(Jobfiles.Instance.fnames[x], FileMode.Open))
                                        {
                                            inputStream.CopyTo(ftpStream);
                                            Thread.Sleep(2000);
                                        }
                                        Log.Instance.WriteLineToLog("copied " + Cppath);
                                    }
                                }
                                catch
                                {
                                    Log.Instance.WriteLineToLog("took a while for " + Cppath);
                                    conn.Connect();
                                    bool folderexists = conn.DirectoryExists(containdir);
                                    if (folderexists == false)
                                    {
                                        conn.Connect();
                                        Log.Instance.WriteLineToLog("created directory " + containdir);
                                        conn.CreateDirectory(containdir);
                                    }
                                    conn.Connect();
                                    bool fileexists = conn.FileExists(Cppath);
                                    if (fileexists == false)
                                    {
                                        conn.Connect();
                                        Log.Instance.WriteLineToLog("removed file" + Cppath);
                                        conn.DeleteFile(Cppath);
                                    }
                                    conn.Connect();
                                    using (var ftpStream = conn.OpenWrite(Cppath))
                                    {
                                        using (var inputStream = new FileStream(Jobfiles.Instance.fnames[x], FileMode.Open))
                                        {
                                            inputStream.CopyTo(ftpStream);
                                            Thread.Sleep(2000);
                                        }
                                        Log.Instance.WriteLineToLog("copied " + Cppath);
                                    }
                                }
                                if (compare == true)
                                {
                                    long ftpfile = conn.GetFileSize(Cppath);
                                    FileInfo fInfo = new FileInfo(Jobfiles.Instance.fnames[x]);
                                    long size = fInfo.Length;
                                    if (size == ftpfile)
                                    {
                                        Log.Instance.WriteLineToLog("file compare successful for " + Cppath);
                                        fc = fc + "successful for " + Cppath + (char)13;
                                    }
                                    else
                                    {
                                        Log.Instance.WriteLineToLog("file compare NOT successful for " + Cppath);
                                        fc = fc + "NOT successful file compare for " + Cppath + (char)13;
                                        unsuccfilecompare++;
                                    }
                                }
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