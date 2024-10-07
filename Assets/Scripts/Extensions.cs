using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ext
{
    public static Vector3 Average(IEnumerable<Vector3> vectors)
    {
        uint count = 0;
        Vector3 res = Vector3.zero;
        foreach (Vector3 v in vectors)
        {
            res += v;
            count++;
        }
        return res / count;
    }
    public static float Delta(this Vector3 a, Vector3 b) => (b - a).magnitude;
    public static float Dot(this Vector3 a, Vector3 b) => Vector3.Dot(a, b);
    public static Vector3 Abs(this Vector3 a) => new(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    public static Vector3 Mul(this Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vector3 Cross(this Vector3 a, Vector3 b) => Vector3.Cross(a, b);
    public static Vector3 Div(this Vector3 a, Vector3 b) => new(b.x != 0 ? a.x / b.x : a.x / 1, b.y != 0 ? a.y / b.y : a.y / 1, b.z != 0 ? a.z / b.z : a.z / 1);
    public static Vector3 Mod(this Vector3 a, float f) => new(a.x % f, a.y % f, a.z % f);
    public static Vector3 Mod(this Vector3 a, Vector3 b) => new(a.x % b.x, a.y % b.y, a.z % b.z);
    public static Vector3 Pow(this Vector3 a, int p) => new(Mathf.Pow(a.x, p), Mathf.Pow(a.y, p), Mathf.Pow(a.z, p));
    public static Vector3 Pow(this Vector3 a, Vector3 b) => new(Mathf.Pow(a.x, b.x), Mathf.Pow(a.y, b.y), Mathf.Pow(a.z, b.z));
    public static Vector3 Min(this Vector3 a, Vector3 b) => new(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
    public static Vector3 Max(this Vector3 a, Vector3 b) => new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
    public static Vector2 XY(this Vector3 a) => new(a.x, a.y);
    public static Vector2 XZ(this Vector3 a) => new(a.x, a.z);
    public static Vector2 YZ(this Vector3 a) => new(a.y, a.z);
    public static Vector3 XYI(this Vector3 a) => new(a.x, a.y, 1);
    public static Vector3 XIZ(this Vector3 a) => new(a.x, 1, a.z);
    public static Vector3 IYZ(this Vector3 a) => new(1, a.y, a.z);
    public static Vector3 XYO(this Vector3 a) => new(a.x, a.y, 0);
    public static Vector3 XOZ(this Vector3 a) => new(a.x, 0, a.z);
    public static Vector3 OYZ(this Vector3 a) => new(0, a.y, a.z);
    public static Vector3 XOO(this Vector3 a) => new(a.x, 0, 0);
    public static Vector3 OYO(this Vector3 a) => new(0, a.y, 0);
    public static Vector3 OOZ(this Vector3 a) => new(0, 0, a.z);
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
}