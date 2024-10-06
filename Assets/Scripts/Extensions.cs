using UnityEngine;

public static class Vector3Ext
{
    public static float delta(this Vector3 a, Vector3 b) => (b - a).length();
    public static float length(this Vector3 a) => Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
    public static float dot(this Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;
    public static Vector3 abs(this Vector3 a) => new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    public static Vector3 mul(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vector3 cross(this Vector3 a, Vector3 b) => new Vector3(a.y * b.z - a.z * b.y, a.x * b.z - a.z * b.x, a.x * b.y - a.y * b.x);
    public static Vector3 div(this Vector3 a, Vector3 b) => new Vector3(b.x != 0 ? a.x / b.x : a.x / 1, b.y != 0 ? a.y / b.y : a.y / 1, b.z != 0 ? a.z / b.z : a.z / 1);
    public static Vector3 mod(this Vector3 a, float f) => new Vector3(a.x % f, a.y % f, a.z % f);
    public static Vector3 mod(this Vector3 a, Vector3 b) => new Vector3(a.x % b.x, a.y % b.y, a.z % b.z);
    public static Vector3 pow(this Vector3 a, int p) => new Vector3(Mathf.Pow(a.x, p), Mathf.Pow(a.y, p), Mathf.Pow(a.z, p));
    public static Vector2 xy(this Vector3 a) => new Vector2(a.x, a.y);
    public static Vector2 xz(this Vector3 a) => new Vector2(a.x, a.z);
    public static Vector2 yz(this Vector3 a) => new Vector2(a.y, a.z);
    public static Vector3 xyl(this Vector3 a) => new Vector3(a.x, a.y, 1);
    public static Vector3 xlz(this Vector3 a) => new Vector3(a.x, 1, a.z);
    public static Vector3 lyz(this Vector3 a) => new Vector3(1, a.y, a.z);
    public static Vector3 xyo(this Vector3 a) => new Vector3(a.x, a.y, 0);
    public static Vector3 xoz(this Vector3 a) => new Vector3(a.x, 0, a.z);
    public static Vector3 oyz(this Vector3 a) => new Vector3(0, a.y, a.z);
    public static Vector3 xoo(this Vector3 a) => new Vector3(a.x, 0, 0);
    public static Vector3 oyo(this Vector3 a) => new Vector3(0, a.y, 0);
    public static Vector3 ooz(this Vector3 a) => new Vector3(0, 0, a.z);
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
}