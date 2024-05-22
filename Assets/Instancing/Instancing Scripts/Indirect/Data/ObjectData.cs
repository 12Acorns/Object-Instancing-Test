using UnityEngine;

namespace NEG.Plugins.Instancing.Indirect.Data
{
    internal class ObjectData : MonoBehaviour
    {
        [SerializeField] private Transform objectTransform;
        [SerializeField] private Material objectMaterialInstance;

		private void Awake()
		{
            objectTransform = transform;
            //objectMaterialInstance = gameObject.GetComponent<Renderer>().material;
		}

		public Transform GetTransform() => objectTransform;
        public Material GetMaterial() => objectMaterialInstance;
    }
}
