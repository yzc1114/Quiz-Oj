using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace quiz_oj.Configs.Middlewares
{
    public class ResponseFormatFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult == null)
                {
                    context.Result = new ObjectResult(new ResponseFormat(code: 0, message: null, data: null));
                }
                else
                {
                    context.Result = new ObjectResult(new ResponseFormat(code: 0, message: null, data: objectResult.Value));
                }
            } 
            else
            {
                Trace.Fail("Please debug. We don't support non ObjectResult");
            }
        }
    }
}