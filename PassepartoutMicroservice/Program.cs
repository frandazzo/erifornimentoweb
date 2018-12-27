using AutoMapper;
using Microsoft.Owin.Hosting;
using PassepartoutMicroservice.AutomapperProfiles;
using PassepartoutMicroservice.Passepartout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PassepartoutMicroservice
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<BasicProfile>();
            });

           

#if DEBUG
            Service1 s = new Service1();
            s.OnDebug();
            System.Threading.Thread.Sleep(-1);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
#endif

           

        }
    }
}
