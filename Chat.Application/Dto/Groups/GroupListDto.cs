using Chat.Application.Dto.User;

namespace Chat.Application.Dto.Groups;

public class GroupListDto
{
    public Guid Id { get; set; }

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
    public UserInfoDto? GroupOwner { get; set; }
}
