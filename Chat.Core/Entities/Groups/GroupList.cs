using Chat.Core.Base;

namespace Chat.Core.Entities.Groups;

public class GroupList: EntityWithAllBaseProperty
{
    /// <summary>
    /// 群聊名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid GroupOwnerId { get; set; }
    //public UserInfo? GroupOwner { get; set; }
}
