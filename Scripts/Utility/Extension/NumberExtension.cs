using System.Globalization;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public static class NumberExtension
{
    public static string ToCurrencyFormat(this int value)
    {
        return value.ToString("#,0", CultureInfo.InvariantCulture).Replace(',', '.')
               + " " + RegionInfo.CurrentRegion.ISOCurrencySymbol;
    }

    public static int ToMillisecond(this float second)
    {
        return Mathf.FloorToInt(second * 1000);
    }

    public static int Round(this int a, int b)
    {
        return Mathf.Round(1f * a / b).ToInt();
    }

    public static int ToInt(this float x)
    {
        return Mathf.RoundToInt(x);
    }

    public static int ToInt(this TMP_Text text)
    {
        return float.Parse(text.text).ToInt();
    }

    public static int ToInt(this TextMeshPro text)
    {
        return float.Parse(text.text).ToInt();
    }

    public static int ToInt(this TextMeshProUGUI text)
    {
        return float.Parse(text.text).ToInt();
    }

    public static int ToPercent(this float x)
    {
        return (100 * x).ToInt();
    }

    public static int Random(this Vector2Int range)
    {
        return UnityEngine.Random.Range(range.x, range.y + 1);
    }

    public static bool InRange(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }

    public static bool InRange(this int value, Vector2Int range)
    {
        return value >= range.x && value <= range.y;
    }

    public static bool InRange(this float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    public static bool InRange(this float value, Vector2 range)
    {
        return value >= range.x && value <= range.y;
    }
}