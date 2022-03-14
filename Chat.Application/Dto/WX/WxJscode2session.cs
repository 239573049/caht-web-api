namespace Chat.Application.Dto.WX;

public class WxJscode2session
{
    public string? Openid { get; set; }
    public string? Session_key { get; set; }
    public string? Unionid { get; set; }
    public int Errcode { get; set; }
    public string? Errmsg { get; set; }
}