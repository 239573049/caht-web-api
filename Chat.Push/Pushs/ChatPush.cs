
using Chat.WebCore.Base;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Push.Pushs;

public class ChatPush:Hub
{
    private readonly IPrincipalAccessor _principalAccessor;
    public ChatPush(
        IPrincipalAccessor principalAccessor
        )
    {
        _principalAccessor = principalAccessor;
    }
    public override async Task OnConnectedAsync()
    {
        var connectId = Context.ConnectionId;
        var token = Context.GetHttpContext().Request.Query["Authorization"].ToString();//获取用户token
        var userId = _principalAccessor.GetUserId(token);
        await RedisHelper.SetAsync(userId+ "ChatPush", connectId);
        await RedisHelper.SAddAsync(key: "on_line", new { userId, connectId });
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectId = Context.ConnectionId;
        var token = Context.GetHttpContext().Request.Query["Authorization"].ToString();//获取用户token
        var userId = _principalAccessor.GetUserId(token);
        RedisHelper.Del(userId + "ChatPush");
        await RedisHelper.SRemAsync("on_line", new { userId, connectId });
    }

}
