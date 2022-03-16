using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto.Groups;

public class FriendDto
{
    public int Id { get; set; }

    /// <summary>
    /// 当前人id
    /// </summary>
    public Guid BelongId { get; set; }
    /// <summary>
    /// 好友id
    /// </summary>
    public Guid FriendsId { get; set; }
}
