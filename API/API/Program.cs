using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting the API...");
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:5000")
            .Build();
    }
}
