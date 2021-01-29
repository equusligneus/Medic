using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Additions
{
    public static Vector3 To3D(this Vector2 value)
        => new Vector3(value.x, 0f, value.y);

    public static Vector2 To2D(this Vector3 value)
        => new Vector2(value.x, value.z);
}
