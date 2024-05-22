using System.Runtime.CompilerServices;
using UnityEngine;

namespace NEG.Plugins.Instancing.Indirect.Buffer
{
    internal readonly struct InstanceBuffer
    {
        public InstanceBuffer(Vector3 _position, Vector4 _colour)
        {
            Position = _position;
            Color = _colour;
		}

		private const int size = sizeof(float) * 7;

        public readonly Vector3 Position;
        public readonly Vector4 Color;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Size() => size;

	}
}
