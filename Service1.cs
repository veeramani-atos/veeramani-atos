using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Reflection;

namespace MyFirstService
{

    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)  
       
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                System.Diagnostics.Debugger.Launch();
                WriteToFile("Service is started at " + DateTime.Now);

                string filename = "@C:\\New\\exempleWord.docx";

                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();                
                Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(filename);
                doc.Content.Text += "Word";
                doc.Save();
                app.Quit();


                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                timer.Interval = 5000; //number in milisecinds  
                timer.Enabled = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           

        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}