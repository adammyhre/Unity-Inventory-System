using System;

public static class GuidExtensions {
    public static SerializableGuid ToSerializableGuid(this Guid systemGuid) {
        byte[] bytes = systemGuid.ToByteArray();
        return new SerializableGuid(
            BitConverter.ToUInt32(bytes, 0),
            BitConverter.ToUInt32(bytes, 4),
            BitConverter.ToUInt32(bytes, 8),
            BitConverter.ToUInt32(bytes, 12)
        );
    }

    public static Guid ToSystemGuid(this SerializableGuid serializableGuid) {
        byte[] bytes = new byte[16];
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part1), 0, bytes, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part2), 0, bytes, 4, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part3), 0, bytes, 8, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part4), 0, bytes, 12, 4);
        return new Guid(bytes);
    }
}