using System;
using UnityEngine;

namespace Systems.Inventory {
    /// <summary>
    /// Represents a globally unique identifier (GUID) that is serializable with Unity and usable in game scripts.
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid> {
        [SerializeField, HideInInspector] public uint Part1;
        [SerializeField, HideInInspector] public uint Part2;
        [SerializeField, HideInInspector] public uint Part3;
        [SerializeField, HideInInspector] public uint Part4;

        public static SerializableGuid Empty => new(0, 0, 0, 0);

        public SerializableGuid(uint val0, uint val1, uint val2, uint val3) {
            Part1 = val0;
            Part2 = val1;
            Part3 = val2;
            Part4 = val3;
        }

        public static SerializableGuid NewGuid() => Guid.NewGuid().ToSerializableGuid();

        public static SerializableGuid FromHexString(string hexString) {
            if (hexString.Length != 32) {
                return Empty;
            }

            return new SerializableGuid
            (
                Convert.ToUInt32(hexString.Substring(0, 8), 16),
                Convert.ToUInt32(hexString.Substring(8, 8), 16),
                Convert.ToUInt32(hexString.Substring(16, 8), 16),
                Convert.ToUInt32(hexString.Substring(24, 8), 16)
            );
        }

        public string ToHexString() {
            return $"{Part1:X8}{Part2:X8}{Part3:X8}{Part4:X8}";
        }

        public override bool Equals(object obj) {
            return obj is SerializableGuid guid && this.Equals(guid);
        }

        public bool Equals(SerializableGuid other) {
            return Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3 && Part4 == other.Part4;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Part1, Part2, Part3, Part4);
        }

        public static bool operator ==(SerializableGuid left, SerializableGuid right) => left.Equals(right);
        public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right); 
    }
}