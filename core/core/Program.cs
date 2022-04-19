using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace core
{
    internal class Program
    {
        private static void Main(string[] args)
        {


            ProxyCache<JCDecauxItem> cache = new ProxyCache<JCDecauxItem>();
            cache.Get("Lyon_2010");
            cache.Get("Lyon_2010");
            Console.ReadLine();


        }

    }
    public class MyReflectionClass
    {
        public string MyMethod(String param1, String param2)
        {
            string responseString = $"<HTML><BODY> Hello {param1} et {param2}</BODY></HTML>";
            return responseString;
        }

        public int incr(string a)
        {
            int x = Int32.Parse(a);
            x++;
            return x;
        }

        public string callExe()
        {
            //
            // Set up the process with the ProcessStartInfo class.
            // https://www.dotnetperls.com/process
            //
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\ngnva\source\repos\td2\TD2\ExecTest\bin\Debug\ExecTest.exe"; // Specify exe name.
            start.Arguments = "Utilisation executable"; // Specify arguments.
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            //
            // Start the process.
            //
            using (Process process = Process.Start(start))
            {
                //
                // Read in all the text from the process with the StreamReader.
                //
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();

                    Console.WriteLine(result);
                    return result;
                }
            }
        }

       
    }
}
