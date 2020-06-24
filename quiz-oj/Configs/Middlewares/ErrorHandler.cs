using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using quiz_oj.Configs.Exceptions;
using MyExceptionLibrary.ExceptionLibrary;

namespace quiz_oj.Configs.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
    
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
    
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ResponseFormat data;
                if (ex is CodableException)
                {
                    var e = ex as CodableException;
                    data = new ResponseFormat(code: e.Code, message: e.Message, data: null);
                }
                else
                {
                    data = new ResponseFormat(code: -1, message: ex.Message, data: null);
                }
                await HandleExceptionAsync(context, data);
            }
        }
    
        private static Task HandleExceptionAsync(HttpContext context, object data)
        {
            return context.Response.WriteAsync(JsonSerializer.Serialize(data));
        }
    }
    
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}