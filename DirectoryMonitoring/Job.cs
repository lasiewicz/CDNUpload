using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Windows.Forms;

namespace DirectoryMonitoring
{
    class Job
    {
        public string Jobnumber;
        public Boolean InJob;
        public string TooEmail;
        public string[] fnames = new string[1000000];
        public int fnameindex;
        public string logdir;
        public Boolean Activity;
        public static Job Instance = new Job();
        // string Jobsdir=ConfigurationManager.AppSettings["jobs"].ToString();
        public string Jobsdir;
        public Job()
        {


        }

        public void CreateNewNum()
        {
            string newfilepath = logdir +" \\" + "Jobnumber" + ".txt";
            Jobnumber = "1";
            using (System.IO.StreamReader file =
                new System.IO.StreamReader(@newfilepath))
            {
                Jobnumber = file.ReadLine();
                file.Close();
            }
            int card = Int32.Parse(Jobnumber);
            card++;
            Jobnumber = card.ToString();
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@newfilepath))
            {
                file.WriteLine(this.Jobnumber);
            }
        
        }

        public void Spitoutnewfile()
        {
            string newfilepath = Jobsdir + " \\" + this.Jobnumber + ".txt";
            string Mes = "";
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@newfilepath))
            {
                for (int x = 1; x <= this.fnameindex; x++)
                {
                    file.WriteLine(this.fnames[x]);
                    Mes = Mes + this.fnames[x] + (char) 13;
                }
                file.Close();
            }
            string subject = "CDN Upload Job " + Jobnumber + " created";
            string test = SendEmail(TooEmail,subject,Mes);
            Log.WriteLine(test);

        }

        protected string SendEmail(string toAddress, string subject, string body)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = "DeluxeMediaWrap@gmail.com";// use sender’s email id here..
            const string senderPassword = "deluxemedia"; // sender password here…
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here…
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }



    }
}

