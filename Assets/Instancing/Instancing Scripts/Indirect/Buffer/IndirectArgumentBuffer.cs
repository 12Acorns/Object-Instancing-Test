using UnityEngine;
using System;

namespace NEG.Plugins.Instancing.Indirect.Buffer
{
	public readonly struct IndirectArgumentBuffer : IDisposable
	{
		private const int lengthOfBuffer = 5;

		private const int sizeOfUInt = sizeof(uint);

		public IndirectArgumentBuffer(GraphicsBuffer.IndirectDrawIndexedArgs[] _args)
		{
			Buffer = new(GraphicsBuffer.Target.IndirectArguments, lengthOfBuffer, sizeOfUInt);

			indirectArgs = _args;
			Buffer.SetData(indirectArgs);
		}

		private readonly GraphicsBuffer.IndirectDrawIndexedArgs[] indirectArgs;

		public readonly GraphicsBuffer Buffer;

		public void Dispose()
		{
			Buffer.Dispose();
		}
	}
}