using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hekatombe.Base
{
    public static class GameObjectExtension
	{
		/// <summary>
		///     Resources.Load with Log Error
		/// </summary>
		public static T ResourcesLoad<T>(string path) where T : UnityEngine.Object
		{
			T obj = Resources.Load<T>(path);
			if (obj == null)
			{
				Debug.LogError("Resource not found at path: " + path);
			}
			return obj;
		}

		/// <summary>
		///     Dulpicate a GameObject that is already on the scene
		/// 	Return the selected Component associated to the duplicated GameObject
		/// </summary>
		public static T DuplicateGameObject<T>(GameObject originalGameObject, Vector3 offsetPos) where T : UnityEngine.Component
		{
			GameObject go = Instantiate(originalGameObject, originalGameObject.transform.position, originalGameObject.transform.rotation) as GameObject;
			go.transform.SetParent(originalGameObject.transform.parent);
			go.transform.localScale = originalGameObject.transform.localScale;
			Vector3 newPos = originalGameObject.transform.localPosition;
			newPos += offsetPos;
			go.transform.localPosition = newPos;
			return go.GetComponent<T>().BreakIfNull("Component of type " + typeof(T).Name + " not found on Duplicate GameObject named: " + originalGameObject.name);
		}

		/// <summary>
		///     To save to do Resources.Load + Instantiate every time
		///     Position & Quaternion.identity as a default parameters
		/// </summary>
		public static T LoadAndInstantiate<T>(string path) where T : UnityEngine.Component
		{
			return LoadAndInstantiate<T> (path, Vector3.zero, Quaternion.identity);
		}

        /// <summary>
        ///     To save to do Resources.Load + Instantiate every time
		///     Quaternion.identity as a default parameter
		/// </summary>
		public static T LoadAndInstantiate<T>(string path, Vector3 position) where T : UnityEngine.Component
		{
			return LoadAndInstantiate<T> (path, position, Quaternion.identity);
		}

        /// <summary>
        ///     To save to do Resources.Load + Instantiate every time
		/// </summary>
		public static T LoadAndInstantiate<T>(string path, Vector3 position, Quaternion quaternion) where T : UnityEngine.Component
		{
			GameObject go = LoadAndInstantiateGameObject (path, position, quaternion);
			return go.GetComponent<T> ().BreakIfNull("This GameObject requires a component of type " + typeof(T).Name);
		}

        /// <summary>
        ///     To save to do Resources.Load + Instantiate every time, specific for GameObjects
		/// </summary>
		public static GameObject LoadAndInstantiateGameObject(string path, Vector3 position, Quaternion quaternion)
		{
			GameObject prefab = Resources.Load<GameObject>(path).BreakIfNull("Object not found at: " + path);
			GameObject go = prefab.Instantiate<GameObject>(position, quaternion);
			return go;
		}

		/// <summary>
		///     Returns a component of type T in the game object. If the component does not exists, it 
		///     logs an error and pauses execution
		/// </summary>
		public static T GetComponentBreakIfNull<T>(this GameObject datGameObject) where T : UnityEngine.Component
		{
			return datGameObject.GetComponent<T>().BreakIfNull("This GameObject requires a component of type " + typeof(T).Name);
		}

		/// <summary>
		///     Instantiate with Cast
		/// </summary>
		public static T Instantiate<T>(this T datGameObject, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
		{
			return UnityEngine.Object.Instantiate(datGameObject, position, rotation) as T;
		}

		/// <summary>
		///     Exception if an object is null
		/// </summary>
		public static T BreakIfNull<T>(this T datGameObject, string optionalMessage = null, params object[] formatArgs) where T : class
		{
			if(datGameObject == null)
			{
				UnityEngine.Debug.LogError(
					string.Format("[ERROR] [{0}] object [{1}] is null{2}", 
						Time.frameCount, 
						typeof(T),
						optionalMessage == null ? string.Empty : " --> " + string.Format(optionalMessage, formatArgs)));
				UnityEngine.Debug.Break();    
			}
			return datGameObject;
		}

		public static void RemoveCloneFromName(this UnityEngine.Object gameObject)
		{
			gameObject.name = gameObject.name.Replace ("(Clone)", "");
		}

		public static T FindComponent<T>(string name) where T : Component
		{
			GameObject tr = GameObject.Find (name).BreakIfNull("Gameobject not found: " + name);
			T t = tr.GetComponent<T> ();
			if (t == null) {
				Debug.LogErrorFormat ("Component of TYPE: {0} not found in GameObject: {1}", typeof(T).FullName, name);
				UnityEngine.Debug.Break(); 
			}
			return t;
		}

        public static void SetActiveRecursive(this GameObject obj, bool active)
        {
            obj.SetActive(active);
            for(int k = 0; k < obj.transform.childCount; k++)
            {
                Transform tr = obj.transform.GetChild(k);
				tr.gameObject.SetActiveRecursive(active);
            }
        }

		public static void SetActiveRecursiveCollider(this GameObject obj, bool active)
        {
            BoxCollider coll = obj.transform.GetComponent<BoxCollider>();
            if(coll != null)
            {
                coll.enabled = active;
            }

            for(int k = 0; k < obj.transform.childCount; k++)
            {
                Transform tr = obj.transform.GetChild(k);
				SetActiveRecursiveCollider(tr.gameObject, active);
            }
        }

        public static GameObject GetChildRecursive(this GameObject gameObject, string name)
        {
            Component[] transforms = gameObject.transform.GetComponentsInChildren(typeof(Transform), true);

            for(int k = 0; k < transforms.Count(); k++)
            {
                Transform atrans = transforms[k] as Transform;
                if(atrans.name == name)
                {
                    return atrans.gameObject;
                }
            }
            return null;
        }

        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            for(int k = 0; k < gameObject.transform.childCount; k++)
            {
                Transform child = gameObject.transform.GetChild(k);
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public static string GetFullPath(this GameObject gameObject)
        {
            string path = "/" + gameObject.name;
            while(gameObject.transform.parent != null)
            {
                path = "/" + gameObject.name + path;
                gameObject = gameObject.transform.parent.gameObject;
            }
            return path;
        }

		public static Bounds GetBoundsRecursive(this GameObject gameObject)
		{
			Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer> ();
			
			Bounds bounds = new Bounds();
			
			foreach( Renderer rend in renderers ) 
			{
				bounds.Encapsulate( rend.bounds.min );
				bounds.Encapsulate( rend.bounds.max );
			}
			return bounds; 
		}
    }
}

