using System.ComponentModel;
using System.Reflection;

namespace Chat.Infrastructure.Exceptions;

public static class EnumExtension
{
    /// <summary>
    /// 获取枚举特性注释
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string? GetEnumString(this Enum enumValue)
    {
        return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? null;
    }
    /// <summary>
    /// 注释转换枚举
    /// </summary>
    /// <typeparam name="T">枚举</typeparam>
    /// <param name="enumValue">注释</param>
    /// <returns></returns>
    public static T? GetEnumVal<T>(this string enumValue)
    {
        var enumType = typeof(T);
        var fields = enumType.GetFields().ToList();
        foreach (var field in fields)
        {
            var fieldValue = field.CustomAttributes.FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value;
            if (fieldValue == null) continue;
            var enumStringValue = fieldValue.ToString();
            if (enumStringValue == enumValue)
            {
                return (T)Enum.Parse(typeof(T), field.GetValue(null)?.ToString() ?? string.Empty);
            }
        }
        return default;
    }
}
