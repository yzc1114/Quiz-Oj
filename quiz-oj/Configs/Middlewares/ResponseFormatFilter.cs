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
            switch (context.Result)
            {
                case RedirectResult _:
                    return;
                case ObjectResult o:
                {
                    var objectResult = o;
                    context.Result = new ObjectResult(new ResponseFormat(code: 0, message: null, data: objectResult.Value));

                    break;
                }
                default:
                    Trace.Fail("Please debug. We don't support non ObjectResult");
                    break;
            }
        }
    }
}