using UnityEngine;

namespace NEG.Plugins.Instancing.Indirect.Data
{
    public sealed class ObjectData : MonoBehaviour
    {
        [SerializeField] private Transform objectTransform;
        [SerializeField] private Material objectMaterialInstance;

		private void Awake()
		{
            objectTransform = transform;
            if(!gameObject.TryGetComponent<Renderer>(out var _renderer))
            {
                return;
            }
            objectMaterialInstance = _renderer.material;
        }

		public Transform GetTransform() => objectTransform;
        public Material GetMaterial() => objectMaterialInstance;
    }
}
