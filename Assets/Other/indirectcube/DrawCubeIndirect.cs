using UnityEngine;
using System.Collections;

public class DrawCubeIndirect : MonoBehaviour
{
    private struct InstanceData
    {
        public Vector3 Position;
        public Color Color;
    }
    public int instanceCount = 100000;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    public int subMeshIndex = 0;

    private int cachedInstanceCount = -1;
    private int cachedSubMeshIndex = -1;
    private InstanceData[] managedDatas;
    private ComputeBuffer instanceDataBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    void Start()
    {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();
    }

    void Update()
    {
        // Update starting position buffer
        if (cachedInstanceCount != instanceCount || cachedSubMeshIndex != subMeshIndex)
            UpdateBuffers();

        // Pad input
        if (Input.GetAxisRaw("Horizontal") != 0.0f)
            instanceCount = (int)Mathf.Clamp(instanceCount + Input.GetAxis("Horizontal") * 40000, 1.0f, 5000000.0f);

        // Render
        Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer, 0, null, UnityEngine.Rendering.ShadowCastingMode.On, true);
    }

    void UpdateBuffers()
    {
        // Ensure submesh index is in range
        if (instanceMesh != null)
            subMeshIndex = Mathf.Clamp(subMeshIndex, 0, instanceMesh.subMeshCount - 1);

        // Positions
        if (instanceDataBuffer != null)
            instanceDataBuffer.Release();
        instanceDataBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 3 + sizeof(float) * 4);
        managedDatas = new InstanceData[instanceCount];
        for (int i = 0; i < instanceCount; i++)
        {
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float distance = Random.Range(20.0f, 20.0f);
            float height = Random.Range(-2.0f, 2.0f);
            float size = Random.Range(0.05f, 0.25f);
            managedDatas[i].Position = new Vector3(Random.insideUnitSphere.x * 20f, Random.insideUnitSphere.y * 20f, Random.insideUnitSphere.z * 20f);
            managedDatas[i].Color = Random.ColorHSV();
        }
        instanceDataBuffer.SetData(managedDatas);
        instanceMaterial.SetBuffer("_PerInstanceData", instanceDataBuffer);

        // Indirect args
        if (instanceMesh != null)
        {
            args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
            args[1] = (uint)instanceCount;
            args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
            args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);
        }
        else
        {
            args[0] = args[1] = args[2] = args[3] = 0;
        }
        argsBuffer.SetData(args);

        cachedInstanceCount = instanceCount;
        cachedSubMeshIndex = subMeshIndex;
    }

    void OnDisable()
    {
        if (instanceDataBuffer != null)
            instanceDataBuffer.Release();
        instanceDataBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }
}