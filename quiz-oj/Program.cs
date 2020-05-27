using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using quiz_oj.Dao;
using quiz_oj.DynamicCodeExecutor;
using quiz_oj.Entities;

namespace quiz_oj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
//             var task = new Compiler().Compile("", @"
// using System;
// using System.Collections.Generic;
//
// public class TestC
// {
//     public int Me(int[] a, int b)
//     {
//         var head = new ListNode(-1);
//         head.Next = new ListNode(b);
//         return a[head.Next.Val];
//     }
// }
//
// ");
//             task.Wait();
//             var a = task.Result;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:7070");
                });
    }
}
