using System.ComponentModel;

namespace Chat.Core.DbEnum;

public enum SexEnum
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    None = 0,
    /// <summary>
    /// 男性
    /// </summary>
    [Description("男性")]
    Male,
    /// <summary>
    /// 女性
    /// </summary>
    [Description("女性")]
    Female
}
/// <summary>
/// 用户状态
/// </summary>
public enum StatueEnum
{
    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable,
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Forbidden
}
/// <summary>
/// 好友申请状态
/// </summary>
public enum FriendStatueEnum
{
    /// <summary>
    /// 申请中
    /// </summary>
    Applying,
    /// <summary>
    /// 成功
    /// </summary>
    Succeed,
    /// <summary>
    /// 拒绝
    /// </summary>
    Refuse
}