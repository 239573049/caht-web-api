using Chat.Push.PushService;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Push;

public static class HubExtension
{
    /// <summary>
    /// 注册工具服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHubService(this IServiceCollection services)
    {
        services.AddSignalRCore();
        services.AddSingleton<IChatPushService, ChatPushService>();
        return services;
    }
}
