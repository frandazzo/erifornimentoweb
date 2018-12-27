using Microsoft.Owin.Hosting;
using PassepartoutMicroservice.Passepartout;
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

namespace PassepartoutMicroservice
{
    public partial class Service1 : ServiceBase
    {
        Timer t;
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            t = new Timer();
            t.AutoReset = false;
            t.Interval = 10;
            t.Elapsed += T_Elapsed1;
            t.Start();

        }

        private void T_Elapsed1(object sender, ElapsedEventArgs e)
        {
            string filelog = @"c:\log.txt";

            PassepartoutFacade.Instance.VerificaConnessioneAttiva();
            string baseAddress = "http://127.0.0.1:" + Properties.Settings.Default.PortaServizio.ToString() + "/";
            File.AppendAllText(filelog, "starting on  " + baseAddress);
            // Start OWIN host 
            try
            {
                using (WebApp.Start<Startup>(baseAddress))
                {

                    System.Threading.Thread.Sleep(-1);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(filelog, ex.Message);
            }
           
        }

       

        protected override void OnStop()
        {
        }
       
    }
}
