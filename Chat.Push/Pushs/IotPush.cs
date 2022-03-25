using Microsoft.AspNetCore.SignalR;

namespace Chat.Push.Pushs
{
    public class IotPush:Hub
    {
        public override Task OnConnectedAsync()
        {
            var deviceId = Context.GetHttpContext().Request.Headers["deviceId"].ToString();
            Console.WriteLine("连接设备id：" + deviceId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var deviceId = Context.GetHttpContext().Request.Headers["deviceId"].ToString();
            Console.WriteLine("断开设备id：" + deviceId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
