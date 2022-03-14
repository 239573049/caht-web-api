using Chat.Core.Base;
using Chat.Core.DbEnum;
using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities.User;

public class UserInfo: EntityWithAllBaseProperty
{
    public string? Name { get; set; }
    /// <summary>
    /// 微信id
    /// </summary>
    [MaxLength(50)]
    public string? WXOpenId { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength( 11)]
    public long? Mobile { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string? AccountNumber{ get; set; }
    /// <summary>
    /// 头像
    /// </summary>
    public string? ChatHead { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public SexEnum Sex { get; set; }
    /// <summary>
    /// 账号状态
    /// </summary>
    public StatueEnum Statue { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    public string? EMail { get; set; }

}
