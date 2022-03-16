using Chat.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Entities.Groups;
/// <summary>
/// 群成员列表
/// </summary>
[Index(nameof(GroupListId), nameof(UserInfoId))]
public class GroupsListUserInfos: EntityWithAllBaseProperty
{
    /// <summary>
    /// 群id
    /// </summary>
    public Guid GroupListId { get; set; }
    /// <summary>
    /// 群成员
    /// </summary>
    public Guid UserInfoId { get; set; }
}
