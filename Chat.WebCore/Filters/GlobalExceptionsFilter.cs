using Chat.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;

namespace Chat.WebCore.Filters;
/// <summary>
/// 全局异常拦截器
/// </summary>
public class GlobalExceptionsFilter:ExceptionFilterAttribute
{
    private readonly ILogger _loggerHelper;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerHelper"></param>
    public GlobalExceptionsFilter(ILogger loggerHelper)
    {
        _loggerHelper = loggerHelper;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    //[DebuggerStepThrough]
    public override void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled == false)
        {
            var ex = context.Exception as BusinessLogicException;
            var response = new 
            {
                Code = ex?.Code??500,
                message = ex?.Message??context.Exception.Message
            };
            _loggerHelper.Error(context.HttpContext.Request.Path, context.Exception, context.HttpContext.Request.Body);
            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(response),
                StatusCode = ex?.Code ?? 500,
                ContentType = "application/json;charset=utf-8"
            };
        }
        context.ExceptionHandled = true;
    }
}