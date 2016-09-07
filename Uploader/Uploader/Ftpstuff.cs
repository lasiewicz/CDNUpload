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
        public string ftptype;
        public BMail MailObject = new BMail();
        public string PrivateKeyPath;

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
            string jobtextplace = jobroot + jobnumber;
            Jobfiles.Instance.fnames = System.IO.File.ReadAllLines(@jobtextplace);
            int unsuccfilecompare = 0;
            int Fnum = 0;
            for (int x = 0; x < Jobfiles.Instance.fnames.Length; x++)
            {
                if (Jobfiles.Instance.fnames[x].Length > 2)
                {
                    Fnum++;
                }
            }
            for (int x = 0; x < Fnum; x++)
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
                        Log.Instance.WriteLineToLog("Starting file " + Cppath);
                        string containdir = Cppath.Substring(0, t);
                        SessionOptions sessionOptions;
                        bool areEqual = String.Equals(Ftpstuff.Instance.ftptype, "SCP", StringComparison.Ordinal);
                        if (areEqual)
                        {
                            sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = Ftpstuff.Instance.Host,
                                UserName = Ftpstuff.Instance.Login,
                                Password = Ftpstuff.Instance.Password,
                                SshHostKeyFingerprint = "ssh-dss 1024 ee:33:bd:ac:7b:6e:bd:0b:60:6e:49:20:56:cb:00:d3",
                                SshPrivateKeyPath = Ftpstuff.Instance.PrivateKeyPath
                            };
                        }
                        else
                        {
                            sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Ftp,
                                HostName = Ftpstuff.Instance.Host,
                                UserName = Ftpstuff.Instance.Login,
                                Password = Ftpstuff.Instance.Password


                            };
                        }


                        using (Session session = new Session())
                        {
                            if (compare == false)
                            {
                                try
                                {
                                    try
                                    {
                                        session.Open(sessionOptions);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.WriteLine(ex.ToString());
                                        Log.WriteLine("Could not open sesson");
                                       
                                    }


                                    // Upload files

                                    TransferOptions transferOptions = new TransferOptions();
                                    transferOptions.TransferMode = TransferMode.Binary;

                                    TransferOperationResult transferResult;
                                    transferResult = session.PutFiles(Jobfiles.Instance.fnames[x], Cppath, false, transferOptions);

                                    // Throw on any error
                                    transferResult.Check();

                                    // Print results
                                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                                    {
                                        Log.Instance.WriteLineToLog("copied " + Cppath);
                                    }


                                }
                                catch
                                {
                                    Log.Instance.WriteLineToLog("took a while for " + Cppath);
                                    bool folderexists = false;
                                    try
                                    {
                                        folderexists = session.FileExists(containdir);
                                    }
                                    catch
                                    {

                                    }
                                    if (folderexists == false)
                                    {

                                        Log.Instance.WriteLineToLog("created directory " + containdir);
                                        session.CreateDirectory(containdir);
                                    }

                                    bool fileexists = session.FileExists(Cppath);
                                    if (fileexists == false)
                                    {

                                        Log.Instance.WriteLineToLog("removed file" + Cppath);
                                        session.RemoveFiles(Cppath);

                                    }

                                    TransferOptions transferOptions = new TransferOptions();
                                    transferOptions.TransferMode = TransferMode.Binary;

                                    TransferOperationResult transferResult;
                                    transferResult = session.PutFiles(Jobfiles.Instance.fnames[x], Cppath, false, transferOptions);

                                    // Throw on any error
                                    transferResult.Check();

                                    // Print results
                                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                                    {
                                        Log.Instance.WriteLineToLog("copied " + Cppath);
                                    }

                                }

                            }
                      
                            if (compare == true)
                            {
                                session.Open(sessionOptions);
                                long ftpfile = session.GetFileInfo(Cppath).Length;
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

                    catch (Exception ex)
                    {
                        Log.WriteLine(ex.ToString());
                        Log.Instance.WriteLineToLog("error with " + Jobfiles.Instance.fnames[x]);
                    }


                }
                if (compare == false)
                {
                    Log.Instance.WriteLineToLog("Done");
                }

       


            }
            if (compare == true)
            {
                subject = "Upload for job" + jobnumber;
                string Mes = "";
                if (unsuccfilecompare > 0)
                {
                    subject = "Error with job" + jobnumber;
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows " + (char)13;
                    Mes = Mes + fc;
                }
                else
                {
                    subject = jobnumber;
                    Mes = " job " + jobnumber + +(char)13;
                    Mes = Mes + "file compare follows " + (char)13;
                    Mes = Mes + fc;
                }
                string test = MailObject.SendEmail(TooEmail, subject, Mes);
            }

        }
    }
}