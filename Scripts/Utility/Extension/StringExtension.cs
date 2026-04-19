public static class StringExtension
{
    public static string Color(this string content, string color)
    {
        return $"<color={color}>{content}</color>";
    }

    public static string Size(this string str, int size)
    {
        return $"<size={size}>{str}</size>";
    }

    public static string Bold(this string str)
    {
        return $"<b>{str}</b>";
    }

    public static string Italic(this string str)
    {
        return $"<i>{str}</i>";
    }

    public static string Format(this string content, params object[] args)
    {
        return string.Format(content, args);
    }
}