using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector2 ToWorldPos(this Vector2 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }

    public static Vector3 SetAxisValue(this Vector3 v, int axis, float newValue) 
    {
        float[] tabV = { v.x, v.y, v.z };
        tabV[axis] = newValue;
        return new Vector3(tabV[0], tabV[1], tabV[2]);
    }
}
