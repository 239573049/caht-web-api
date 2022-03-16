using Chat.Core.Base;

namespace Chat.Core.Entities.Groups;
/// <summary>
/// 好友模块
/// </summary>
public class Friend: EntityWithAllBaseProperty
{
    /// <summary>
    /// 连接id
    /// </summary>
    public Guid ConnectId { get; set; }
    /// <summary>
    /// 当前人id
    /// </summary>
    public Guid BelongId { get; set; }
    /// <summary>
    /// 好友id
    /// </summary>
    public Guid FriendsId { get; set; }
}
