using System.Security.Cryptography;
using System.Text;

namespace Chat.Infrastructure.Extension;

public static class DESExtension
{
    private static string Key
    {
        get
        {

            return "TOKENKEY";
        }
    }
    /// <summary>
    /// DES加密
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string DESEncrypt(this string value)
    {
        DESCryptoServiceProvider des = new();
        byte[] inputByteArray = Encoding.Default.GetBytes(value);
        des.Key = Encoding.ASCII.GetBytes(Key);
        des.IV = Encoding.ASCII.GetBytes(Key);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        ret.ToString();
        return ret.ToString();

    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string DESDecrypt(this string value)
    {
        DESCryptoServiceProvider des = new();
        byte[] inputByteArray = new byte[value.Length / 2];
        for (int x = 0; x < value.Length / 2; x++)
        {
            int i = (Convert.ToInt32(value.Substring(x * 2, 2), 16));
            inputByteArray[x] = (byte)i;
        }
        des.Key = Encoding.ASCII.GetBytes(Key);
        des.IV = Encoding.ASCII.GetBytes(Key);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Encoding.Default.GetString(ms.ToArray());
    }
}