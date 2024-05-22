using UnityEngine;
using System;

public class ObjectManager : MonoBehaviour
{
	[Header("Object Referencing", order = 0)]
	[SerializeField] private Transform[] objects;
	[SerializeField] private Transform parent;

	[Header("Object Data", order = 1)]
	[SerializeField] private Material targetMaterial;
	[SerializeField] private Mesh targetMesh;

	private void Awake()
	{
		objects = parent.GetComponentsInChildren<Transform>();
	}

	public Material GetMaterial()
	{
		return targetMaterial;
	}
	public Mesh GetMesh()
	{
		return targetMesh;
	}
	public ReadOnlySpan<Transform> GetChildren()
	{
		return objects;
	}
	public int GetChildCount()
	{
		return objects.Length;
	}
}
