using Chat.Core.DbEnum;

namespace Chat.Application.Dto.Groups;

public class ApplicationRecordDto
{
    public Guid Id { get; set; }
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
