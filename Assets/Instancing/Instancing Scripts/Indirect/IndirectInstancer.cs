using NEG.Plugins.Instancing.Indirect.Buffer;
using NEG.Plugins.Instancing.Indirect.Update;
using UnityEngine;

namespace NEG.Plugins.Instancing.Indirect 
{
	[AddComponentMenu("NEG/Instancing/Indirect/Indirect Instancer")]
	internal sealed class IndirectInstancer : MonoBehaviour
	{
		[SerializeField] private ObjectManager objectData;
		[SerializeField] private UpdateType updateType;


		private IndirectArgumentBuffer argBuffer;

		private void Awake()
		{
			SetArgsBuffer();
		}

		private void Update()
		{
			HandleUpdateType();
		}

		private void HandleUpdateType()
		{
			switch(updateType)
			{
				case UpdateType.Continuous:
					SetArgsBuffer();


					break;
				case UpdateType.NoChange:
					Instance();
					break;
			}
		}
		private void Instance()
		{

		}
		private void SetArgsBuffer()
		{
			argBuffer.Dispose();

			var _args = GetIndirectArgsMeshData();

			argBuffer = new IndirectArgumentBuffer(_args);
		}
		private (int _count, Mesh _mesh) GetMeshData()
		{
			var _count = objectData.GetChildCount();
			var _mesh = objectData.GetMesh();

			return (_count, _mesh);
		}
		private GraphicsBuffer.IndirectDrawIndexedArgs[] GetIndirectArgsMeshData()
		{
			// Arguments for drawing mesh.
			// 0 = number of triangle indices, 1 = population, others are only relevant if drawing submeshes.
			var _argsBuffer = new GraphicsBuffer.IndirectDrawIndexedArgs[1];

			var (_count, _mesh) = GetMeshData();

			_argsBuffer[0].indexCountPerInstance = _mesh.GetIndexCount(0);
			_argsBuffer[0].instanceCount = (uint)_count;
			_argsBuffer[0].startIndex = _mesh.GetIndexStart(0);
			_argsBuffer[0].baseVertexIndex = _mesh.GetBaseVertex(0);
			_argsBuffer[0].startInstance = 0;

			return _argsBuffer;
		}

		private void Dispose()
		{
			argBuffer.Dispose();
		}

		#region Disposal
		private void OnDestroy()
		{
			Dispose();
		}
		private void OnApplicationQuit()
		{
			Dispose();
		}
		#endregion
	}
}