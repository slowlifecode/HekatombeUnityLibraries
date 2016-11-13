using System;
using UnityEngine;

namespace Hekatombe.Base
{
	/******
	 * Intermediate REST class to allow to change between Loading Assets from Resources or from TrashMan in a clean way
	 */

	public static class DynamicAssetLoader
    {
		private static bool _loadFromResources = true;

		public static void SetLoadFromResources(bool flag)
		{
			_loadFromResources = flag;
		}

		//Use with the general behaviour set
		public static GameObject Spawn(string path, string name, Vector3 position, Quaternion quat)
		{
			return Spawn (_loadFromResources, path, name, position, quat);
		}

		//Use without the general bool _loadFromResources
		public static GameObject Spawn(bool loadFromResources, string path, string name, Vector3 position, Quaternion quat)
		{
			if (loadFromResources) {
				return GameObjectExtension.LoadAndInstantiateGameObject (path + name, position, quat);
			} else {
				return TrashMan.spawn (name, position, quat);
			}
		}

		//Use with the general behaviour set
		public static void Despawn(GameObject go)
		{
			Despawn (_loadFromResources, go);
		}
			
		//Use without the general bool _loadFromResources
		public static void Despawn(bool loadFromResources, GameObject go)
		{
			if (_loadFromResources) {
				GameObject.Destroy (go);
			} else {
				TrashMan.despawn (go);
			}
		}
    }
}