using Chat.Application.Dto.Chat;
using Chat.Application.Services.Groups;
using Chat.Core.Entities.Groups;
using Chat.Push;
using Chat.Push.PushService;
using Chat.WebCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.WebApi.Controller.Groups;

public class ApplicationRecordController : WebApiController
{
    private readonly IPrincipalAccessor _principalAccessor;
    private readonly IChatPushService _chatPushService;
    private readonly IApplicationRecordService _applicationRecordService;
    public ApplicationRecordController(
        IChatPushService chatPushService,
        IPrincipalAccessor principalAccessor,
        IApplicationRecordService applicationRecordService
        )
    {
        _chatPushService = chatPushService;
        _principalAccessor= principalAccessor;
        _applicationRecordService = applicationRecordService;
    }
    /// <summary>
    /// 添加好友申请
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddFriend(ApplicationRecord record)
    {
        record.ApplicantId = _principalAccessor.UserId();
        _ = await _applicationRecordService.AddFriend(record);
        var message = new PushMessage
        {
            Message = "您有新的好友申请！",
            PushStatue = Application.Dto.DtoEnum.PushStatueEnum.FriendRequest,
            PushTime = DateTime.Now,
            SenderId = _principalAccessor.UserId(),
        };
        await _chatPushService.SendMessageUserInfo(message,ChatMessageEnum.SystemMessage,record.BeAppliedId);
        return new OkObjectResult("发起成功");
    }
    /// <summary>
    /// 同意|拒绝好友申请
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isRefuse"></param>
    /// <returns></returns>
    public async Task<IActionResult> ApplyForDealWith(Guid id, bool isRefuse)
    {
        var (data, connectId) =await _applicationRecordService.ApplyForDealWith(id,isRefuse);
        var message = new PushMessage
        {
            Message = "您的有新的好友同意了您的申请",
            PushStatue = Application.Dto.DtoEnum.PushStatueEnum.FriendConsent,
            PushTime = DateTime.Now,
            SenderId = _principalAccessor.UserId(),
        };
        await _chatPushService.SendMessageUserInfo(message, ChatMessageEnum.SystemMessage, data.ApplicantId);
        await _chatPushService.AddToGroupAsync(connectId.ToString(), data.ApplicantId, data.BeAppliedId);
        return new OkObjectResult("同意成功");
    }

}
