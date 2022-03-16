namespace Chat.Application.Dto.Groups;

public class GroupsListUserInfosDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 群id
    /// </summary>
    public Guid GroupListId { get; set; }
    /// <summary>
    /// 群成员
    /// </summary>
    public Guid UserInfoId { get; set; }
}
