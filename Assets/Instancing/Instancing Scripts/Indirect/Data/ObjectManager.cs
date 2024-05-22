using System.Collections.Generic;
using UnityEngine;
using System;
using Codice.Client.BaseCommands;

namespace NEG.Plugins.Instancing.Indirect.Data
{
	internal sealed class ObjectManager : MonoBehaviour
	{
		[Header("Object Referencing", order = 0)]
		[SerializeField] private ObjectData[] objects;
		[SerializeField] private GameObject parent;

		[Header("Object Data", order = 1)]
		[SerializeField] private Material targetMaterial;
		[SerializeField] private Mesh targetMesh;

		private void Awake()
		{
			objects = parent.GetComponentsInChildren<ObjectData>();
		}

		public GameObject GetParent() => parent;
		public Material GetMaterial() => targetMaterial;
		public Mesh GetMesh() => targetMesh;

		public ReadOnlySpan<ObjectData> GetChildrenSpan() => objects;
		public IEnumerable<ObjectData> GetChildren() => objects;

		public int GetChildCount() => objects.Length;
	}
}