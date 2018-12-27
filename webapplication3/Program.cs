using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

namespace WebApplication3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filelog = @"c:\logerif.txt";
            try
            {
                var isService = !(Debugger.IsAttached || args.Contains("--console"));
                var pathToContentRoot = Directory.GetCurrentDirectory();
                var webHostArgs = args.Where(arg => arg != "--console").ToArray();

                if (isService)
                {
                    
                    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                    pathToContentRoot = Path.GetDirectoryName(pathToExe);

                    File.AppendAllText(filelog, "is service: path gto exe: " + pathToContentRoot);
                }

                var config = new ConfigurationBuilder()
                       .SetBasePath(pathToContentRoot)
                       .AddJsonFile("appsettings.json", optional: true)
                       .Build();

                try
                {
                    var host = new WebHostBuilder()
                    .UseConfiguration(config)
                   .UseKestrel()
                   .UseContentRoot(pathToContentRoot)
                   
                   //.UseUrls("http://*:5000")

                   .UseStartup<Startup>().Build();



                    if (isService)
                    {
                        File.AppendAllText(filelog, "start as win service");
                        Debug.WriteLine("start as win service");
                        host.RunAsService();
                    }
                    else
                    {
                        File.AppendAllText(filelog, "start as exe");
                        Debug.WriteLine("start as exe");
                        host.Run();
                    }
                }
                catch (Exception ex1)
                {

                    File.AppendAllText(filelog, ex1.Message);
                    File.AppendAllText(filelog, ex1.GetType().Name);
                    File.AppendAllText(filelog, ex1.StackTrace);
                    if (ex1.InnerException != null)
                        File.AppendAllText(filelog, ex1.InnerException.Message);
                }
               
            }
            catch (Exception ex)
            {

                File.AppendAllText(filelog, ex.Message);
            }

            
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{

        //    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
        //    var pathToContentRoot = Path.GetDirectoryName(pathToExe);

        //    var host =new WebHostBuilder()
        //        .UseKestrel()
        //        //.UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseContentRoot(pathToContentRoot)
        //        .UseUrls("http://*:5000")
        //        .UseIISIntegration()
        //        .UseStartup<Startup>();


        //    return host;
        //}
    }
}
