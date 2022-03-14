using Chat.Core.DbEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto.User;

public class UserInfoDto
{
    public Guid Id{ get; set; }     
    public string? Name { get; set; }
    /// <summary>
    /// 微信id
    /// </summary>
    public string? WXOpenId { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    public long? Mobile { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string? AccountNumber { get; set; }
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
    /// 性别
    /// </summary>
    public string? SexName { get; set; }
    /// <summary>
    /// 账号状态
    /// </summary>
    public string? StatueName { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    public string? EMail { get; set; }
}
