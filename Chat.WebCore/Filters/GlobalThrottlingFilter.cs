//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Chat.WebCore.Filters;
///// <summary>
///// 全局限流
///// </summary>
//public class GlobalThrottlingFilter : ActionFilterAttribute
//{
//    public override async void OnActionExecuted(ActionExecutedContext context)
//    {
//        //var targetInfo = $"{context.HttpContext.Connection.RemoteIpAddress!.MapToIPv4()}:{context.HttpContext.Request.Path}";//获取客户的ip地址然后进行缓存
//        //var time = Convert.ToInt32(AppSettings.App("Throttling:Time"));
//        //var limit = Convert.ToInt32(AppSettings.App("Throttling:Limit"));
//        //var throttling=RedisHelper.Get<ThrottlingVM>(targetInfo);
//        //if (throttling == null)
//        //{
//        //    var now=DateTime.Now.AddSeconds(time);
//        //    await RedisHelper.SetAsync(targetInfo, new ThrottlingVM { Limit = 1, Time = now });
//        //    await RedisHelper.PExpireAtAsync(targetInfo,now);
//        //}
//        //else
//        //{
//        //    if (throttling.Limit >= limit)
//        //    {
//        //        context.Result = new ObjectResult(new ModelStateResult("访问过于频繁，请稍后访问！",403));
//        //    }
//        //    else
//        //    {
//        //        throttling.Limit++;
//        //        await RedisHelper.SetAsync(targetInfo,throttling);
//        //        await RedisHelper.PExpireAtAsync(targetInfo, throttling.Time);
//        //    }
//        //}
//        base.OnActionExecuted(context);
//    }

//}
