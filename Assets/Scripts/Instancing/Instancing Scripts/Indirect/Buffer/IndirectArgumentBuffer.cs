using UnityEngine;
using System;

namespace NEG.Plugins.Instancing.Indirect.Buffer
{
	public readonly struct IndirectArgumentBuffer : IDisposable
	{
		public IndirectArgumentBuffer(GraphicsBuffer.IndirectDrawIndexedArgs[] _args, int _bufferLength)
		{
			Buffer = new(GraphicsBuffer.Target.IndirectArguments, _bufferLength, GraphicsBuffer.IndirectDrawIndexedArgs.size);

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