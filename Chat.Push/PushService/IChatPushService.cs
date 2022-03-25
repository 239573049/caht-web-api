using Chat.Push.Pushs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Push.PushService;

public interface IChatPushService
{
    /// <summary>
    /// 发送信息至指定人
    /// </summary>
    /// <returns></returns>
    Task SendMessageUserInfo<TEntity>(TEntity entity, ChatMessageEnum method, Guid receiveId);

    /// <summary>
    /// 发送信息多个人
    /// </summary>
    /// <returns></returns>
    Task SendMessageUserInfo<TEntity>(TEntity entity, ChatMessageEnum method, List<Guid> receiveId);
    /// <summary>
    /// 添加用户至群组
    /// </summary>
    /// <param name="connectId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task AddToGroupAsync(string connectId, params Guid[] ids);
    /// <summary>
    /// 将用户从群组删除
    /// </summary>
    /// <param name="connectId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task RemoveFromGroupAsync(string connectId,params Guid[] ids);
}
public class ChatPushService:IChatPushService
{
    private readonly IHubContext<ChatPush> _hubContext;
    public  ChatPushService(
        IHubContext<ChatPush> hubContext
        )
    {
        _hubContext = hubContext;
    }
    public async Task SendMessageUserInfo<TEntity>(TEntity entity, ChatMessageEnum method, Guid receiveId)
    {
        var connectId = RedisHelper.Get(receiveId.ToString() + "ChatPush");
        await _hubContext.Clients.Client(connectId).SendAsync(method.ToString(), entity);
    }
    public async Task SendMessageUserInfo<TEntity>(TEntity entity, ChatMessageEnum method, List<Guid> receiveId)
    {
        var connectId = RedisHelper.MGet(receiveId.Select(a=>a+ "ChatPush").ToArray());
        await _hubContext.Clients.Clients(connectId).SendAsync(method.ToString(), entity);
    }
    public async Task AddToGroupAsync(string connectId, Guid[] ids)
    {
        var connectIds = RedisHelper.MGet(ids.Select(a => a + "ChatPush").ToArray());
        foreach (var d in connectIds)
        {
            await _hubContext.Groups.AddToGroupAsync(d, connectId);
        }
    }
    public async Task RemoveFromGroupAsync(string connectId, Guid[] ids)
    {
        var connectIds = RedisHelper.MGet(ids.Select(a => a + "ChatPush").ToArray());
        foreach (var d in connectIds)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(d, connectId);
        }
    }
}
