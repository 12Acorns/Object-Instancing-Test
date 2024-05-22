using NEG.Plugins.Instancing.Indirect.Buffer;
using NEG.Plugins.Instancing.Indirect.Update;
using NEG.Plugins.Instancing.Indirect.Data;
using UnityEngine.Rendering;
using UnityEngine;
using NEG.Plugins.Utility.ObjectCollection;
using Unity.Collections;

namespace NEG.Plugins.Instancing.Indirect 
{
	[AddComponentMenu("NEG/Instancing/Indirect/Indirect Instancer")]
	internal sealed class IndirectInstancer : MonoBehaviour
	{
		[Header("Object Collection")]
		[SerializeField] private ObjectCollectionType collectionMethod;

		[Header("Object Data")]
		[SerializeField] private ObjectManager objectData;
		[SerializeField] private UpdateType updateType;

		private RenderParams renderParams;

		private ComputeBuffer meshPropertiesBuffer;

		private IndirectArgumentBuffer? argBuffer = null;

		private NativeArray<InstanceBuffer> instanceArray;

		private int count;
		private Mesh mesh;

		private void Start()
		{
			Init();
			mesh = objectData.GetMesh();
			count = objectData.GetChildCount();
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
				renderParams, mesh, 
				argBuffer?.Buffer, 1);
		}
		private void SetArgsBuffer()
		{
			argBuffer?.Dispose();

			var _args = GetIndirectArgsMeshData();

			argBuffer = new IndirectArgumentBuffer(_args, _args.Length);
		}
		private void SetPositionBuffers()
		{
			instanceArray = new(objectData.GetChildCount(), Allocator.Persistent, 
				NativeArrayOptions.UninitializedMemory);

			meshPropertiesBuffer = new(objectData.GetChildCount(), InstanceBuffer.Size(),
				ComputeBufferType.IndirectArguments, ComputeBufferMode.SubUpdates);

			var _children = objectData.GetChildrenSpan();

			for(int i = 0; i < instanceArray.Length; i++)
			{
				instanceArray[i] = new(_children[i].transform.position, Vector4.one);
			}

			meshPropertiesBuffer.SetData(instanceArray);

			objectData.GetMaterial().SetBuffer("_PerInstanceData", meshPropertiesBuffer);
		}
		private GraphicsBuffer.IndirectDrawIndexedArgs[] GetIndirectArgsMeshData()
		{
			// Arguments for drawing mesh.
			// 0 = number of triangle indices, 1 = population, others are only relevant if drawing submeshes.
			var _argsBuffer = new GraphicsBuffer.IndirectDrawIndexedArgs[1];

			var (_count, _mesh) = GetMeshData();

			_argsBuffer[0].indexCountPerInstance = _mesh.GetIndexCount(0);
			_argsBuffer[0].baseVertexIndex = _mesh.GetBaseVertex(0);
			_argsBuffer[0].startIndex = _mesh.GetIndexStart(0);
			_argsBuffer[0].instanceCount = (uint)_count;
			_argsBuffer[0].startInstance = 0;

			return _argsBuffer;
		}
		private (int _count, Mesh _mesh) GetMeshData()
		{
			var _count = objectData.GetChildCount();
			var _mesh = objectData.GetMesh();

			return (_count, _mesh);
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
		private void SetChildrenIfNeeded()
		{
			switch(collectionMethod)
			{
				case ObjectCollectionType.Lazy:
					var _children = objectData.GetParent().GetAllChildObjectsType<ObjectData>();
					objectData.SetChildren(_children);
					break;
			}
		}
		private void DisableChildren()
		{
			foreach(var _child in objectData.GetChildren())
			{
				_child.gameObject.SetActive(false);
			}
		}
		private void Init()
		{
			SetChildrenIfNeeded();
			SetArgsBuffer();
			SetBounds();
			SetPositionBuffers();
			DisableChildren();
		}

		private void Dispose()
		{
			argBuffer?.Dispose();
			if(instanceArray.IsCreated)
			{
				instanceArray.Dispose();
			}
			meshPropertiesBuffer.Dispose();
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