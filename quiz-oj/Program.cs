using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CLRClassLib;
using System;
using System.Runtime.InteropServices;
namespace quiz_oj
{
    public class Program
    {
        [DllImport("TreeNodeStrChecker.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool isValidTreeNodeString(string s);

        public static void Main(string[] args)
        {
            //Console.WriteLine(isValidTreeNodeString("[1,2,4,null]"));
            //Console.WriteLine(add(1));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://*:7070");
                });
    }
}
