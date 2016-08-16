using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.FtpClient;
using System.Security.Authentication;
using System;
using System.Net;


namespace Uploader
{
    class Ftpstuff
    {
        public static void Connect()
        {
            using (FtpClient conn = new FtpClient())
            {
                conn.Host = "deluxdigi-2.upload.llnw.net";
                conn.Credentials = new NetworkCredential("deluxdigi-ht", "g7tz8x");
                conn.Connect();
              //  MessageBox.Show("worked");
            }
        }
    }
   
}
