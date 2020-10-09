using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinPos.Server.Common
{
   public static class CommonFunctions
    {
        public static void SaveErrorLog(string errorMessage)
        {
            try
            {
                //string filepath = HttpContent.Current.Server.MapPath("~/Exception/");  //Text File Path

                //if (!Directory.Exists(filepath))
                //{
                //    Directory.CreateDirectory(filepath);
                //}
                //filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                //if (!File.Exists(filepath))
                //{
                //    File.Create(filepath).Dispose();
                //}
                //using (StreamWriter sw = File.AppendText(filepath))
                //{
                //    string error = "Log Written Date:" + " " + DateTime.Now.ToString()  + "Error Message:" + " " + errorMessage ;
                //    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                //    sw.WriteLine("-------------------------------------------------------------------------------------");                   
                //    sw.WriteLine(error);
                //    sw.WriteLine("--------------------------------*End*------------------------------------------");                   
                //    sw.Flush();
                //    sw.Close();

                //}

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }
    }
}
