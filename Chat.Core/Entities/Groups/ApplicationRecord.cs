using Chat.Core.Base;
using Chat.Core.DbEnum;

namespace Chat.Core.Entities.Groups;

public class ApplicationRecord: EntityWithAllBaseProperty
{
    /// <summary>
    /// 申请人
    /// </summary>
    public Guid ApplicantId { get; set; }
    /// <summary>
    /// 被申请人
    /// </summary>
    public Guid BeAppliedId { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 申请状态
    /// </summary>
    public FriendStatueEnum FriendStatue { get; set; }
}
