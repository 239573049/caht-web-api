namespace Chat.Core.Base;
public class EntityCreateDate : Entity, IHaveCreatedTime
{
    public DateTime? CreatedTime { get; set; }
}