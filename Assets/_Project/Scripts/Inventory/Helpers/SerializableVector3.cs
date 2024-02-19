using UnityEngine;

/// <summary>
/// Represents a serializable version of the Unity Vector3 struct.
/// </summary>
public struct SerializableVector3 {
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString() => $"[{x}, {y}, {z}]";

    public static implicit operator Vector3(SerializableVector3 vector) {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static implicit operator SerializableVector3(Vector3 vector) {
        return new SerializableVector3(vector.x, vector.y, vector.z);
    }
}
