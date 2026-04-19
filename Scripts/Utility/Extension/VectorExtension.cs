using UnityEngine;

public static class VectorExtension
{
    public static Vector3 ChangeZ(this Vector3 vector, float newZ)
    {
        return new Vector3(vector.x, vector.y, newZ);
    }
}