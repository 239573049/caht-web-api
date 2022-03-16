using Chat.Application.Dto.DtoEnum;

namespace Chat.Application.Dto.Chat;

public class PushMessage
{
    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime PushTime { get; set; }
    /// <summary>
    /// 发送人
    /// </summary>
    public Guid SenderId { get; set; }
    /// <summary>
    /// 发送状态
    /// </summary>
    public PushStatueEnum PushStatue { get; set; }
    /// <summary>
    /// 发送内容
    /// </summary>
    public string? Message { get; set; }
    
}
