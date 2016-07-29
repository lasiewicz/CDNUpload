using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryMonitoring
{
     class Job
     {
        public string Jobnumber;
        public Boolean InJob;
        public Boolean Activity;
        public static Job Instance = new Job();
        public Job()
         {
         

         }

         public void CreateNewNum()
         {

            Random rnd = new Random();
            int card = rnd.Next(9900000);
            Jobnumber = card.ToString();

           }
        




     }
}
