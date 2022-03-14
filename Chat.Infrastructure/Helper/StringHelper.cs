namespace Chat.Infrastructure.Helper;

public class StringHelper
{
    public static string Key { get
        {
            return "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
        }
    }
    public static string GetString(int count)
    {
        string str = "";
        for (int i = 0; i < count; i++)
        {
            str+= Key[new Random(Key.Length).Next()-1];
        }
        return str;
    }
}
