using System.Collections.Generic;
using UnityEngine;

namespace NEG.Plugins.Utility.ObjectCollection
{
    public static class ObjectCollectionUtility
    {
		public static IEnumerable<T> GetAllChildObjectsType<T>(this GameObject _parent) where T : MonoBehaviour
		{
			return _parent.GetComponentsInChildren<T>(true);
		}
		public static IEnumerable<T> GetAllChildObjectsType<T>(this Transform _parent) where T : MonoBehaviour
        {
            return _parent.GetComponentsInChildren<T>(true);
		}
    }
}