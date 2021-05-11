using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {             
            Console.WriteLine("Starting the API...");
            CreateHostBuilder(args).Run();
            
    }
        public static string getConString()
        {
            string conString = System.IO.File.ReadAllText(@"Properties\conString.txt");
            return conString;
    }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:5000")
            .Build();
    }
}
