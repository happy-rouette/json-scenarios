using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Extensions
{
    public static Vector2 ToWorldPos(this Vector2 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }

    public static Vector3 ToWorldPos(this Vector3 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }

    public static Vector3 SetAxisValue(this Vector3 v, int axis, float newValue) 
    {
        float[] tabV = { v.x, v.y, v.z };
        tabV[axis] = newValue;
        return new Vector3(tabV[0], tabV[1], tabV[2]);
    }

    public static void AddOrUpdate<TKey,TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] = value;
        else dictionary.Add(key, value);
    }

    public static string SplitCamelCaseToLower(this string str)
    {
        return string.Join(" ", Regex.Split(str, @"(?<!^)(?=[A-Z](?![A-Z]|$))")).ToLower();
    }
}
