using System;


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
        public BMail MailObject = new BMail();
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
            Job.Instance.fnameindex = 0;
            Job.Instance.fnames[0] = "";

        }

        public void Spitoutnewfile()
        {
            string newfilepath = Jobsdir + " \\" + this.Jobnumber ;
            string Mes = "";
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@newfilepath))
            {
                for (int x = 0; x <= this.fnameindex; x++)
                {
                    file.WriteLine(this.fnames[x]);
                    
                    Mes = Mes + this.fnames[x] + (char) 13;
                  
                }
                file.Close();
            }
            string subject = "CDN Upload Job " + Jobnumber + " created";
            string test = MailObject.SendEmail(TooEmail,subject,Mes);
            Log.WriteLine(test);
            Job.Instance.fnameindex = 0;

        }

      


    }
}

