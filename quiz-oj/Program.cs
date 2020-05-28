using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CLRClassLib;
using System;
using System.Runtime.InteropServices;
namespace quiz_oj
{
    public class Program
    {
        [DllImport("TestATL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MyAdd(int a, int b);
        public static void Main(string[] args)
        {
            Console.WriteLine(MyAdd(1, 2));
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("https://*:7070");
                });
    }
}
