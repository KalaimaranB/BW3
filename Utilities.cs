using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public static class Utilities 
{
    public static T ChangeAlpha<T>(this T g, float newAlpha)
        where T:Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }

    static public string ReplaceInsensitive(this string str, string from, string to)
    {
        str = Regex.Replace(str, from, to, RegexOptions.IgnoreCase);
        return str;
    }
}
