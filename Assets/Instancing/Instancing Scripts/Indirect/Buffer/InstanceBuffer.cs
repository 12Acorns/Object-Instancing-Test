using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace NEG.Plugins.Instancing.Indirect.Buffer
{
    internal readonly struct InstanceBuffer
    {
        private const int size = sizeof(float) * 4 * 2;

        public readonly Matrix4x4 WorldPosition;
        public readonly Matrix4x4 InverseWorldPosition;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Size() => size;

	}
}
