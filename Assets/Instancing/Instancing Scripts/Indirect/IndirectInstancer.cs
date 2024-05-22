using NEG.Plugins.Instancing.Indirect.Buffer;
using NEG.Plugins.Instancing.Indirect.Update;
using NEG.Plugins.Instancing.Indirect.Data;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

namespace NEG.Plugins.Instancing.Indirect 
{
	[AddComponentMenu("NEG/Instancing/Indirect/Indirect Instancer")]
	internal sealed class IndirectInstancer : MonoBehaviour
	{
		[SerializeField] private ObjectManager objectData;
		[SerializeField] private UpdateType updateType;

		private RenderParams renderParams;

		private readonly ComputeBuffer meshPropertiesBuffer;

		private IndirectArgumentBuffer? argBuffer = null;

		private void Awake()
		{
			Init();
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

					Instance();
					break;
				case UpdateType.NoChange:
					Instance();
					break;
			}
		}
		private void Instance()
		{
			Graphics.RenderMeshIndirect(
				renderParams, objectData.GetMesh(), 
				argBuffer?.Buffer, objectData.GetChildCount());
		}
		private void SetArgsBuffer()
		{
			argBuffer?.Dispose();

			var _args = GetIndirectArgsMeshData();

			argBuffer = new IndirectArgumentBuffer(_args);
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
		private (int _count, Mesh _mesh) GetMeshData()
		{
			var _count = objectData.GetChildCount();
			var _mesh = objectData.GetMesh();

			return (_count, _mesh);
		}
		private void SetMaterialData()
		{
			objectData.GetMaterial().SetBuffer("_PerInstanceData", meshPropertiesBuffer);
		}
		private void SetMeshPropertiesBuffer()
		{

		}
		private void SetBounds()
		{
			renderParams = new(objectData.GetMaterial())
			{
				worldBounds = new Bounds(objectData.GetParent().transform.position, Vector3.one * 1000),
				lightProbeUsage = LightProbeUsage.Off,
				receiveShadows = transform,
				reflectionProbeUsage = ReflectionProbeUsage.Off,
				shadowCastingMode = ShadowCastingMode.On,
			};
		}
		private void Init()
		{
			SetArgsBuffer();
			SetBounds();



			//SetMaterialData();
		}

		private void Dispose()
		{
			argBuffer?.Dispose();
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