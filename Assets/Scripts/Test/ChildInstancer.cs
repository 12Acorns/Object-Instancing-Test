using NEG.Plugins.Instancing.Indirect.Data;
using System.Collections.Generic;
using UnityEngine;

namespace NEG.Plugins.Test
{
    internal sealed class ChildInstancer : MonoBehaviour
    {
        [SerializeField] private int instanceCount;

		private int squareRatio;

		private void Awake()
		{
			squareRatio = (int)Mathf.Sqrt(instanceCount);

			var _objects = new List<GameObject>(instanceCount);

			for(int _xPos = 0; _xPos < squareRatio; _xPos++)
			{
				for(int _yPos = 0; _yPos < squareRatio; _yPos++)
				{
					var _object = new GameObject($"{_xPos * 2}, {_yPos * 2}");

					_object.AddComponent<ObjectData>();
					_object.transform.SetParent(transform);

					_object.transform.position = new Vector3(_xPos * 2, 0, _yPos * 2);

					_objects.Add(_object);
				}
			}
		}
	}
}