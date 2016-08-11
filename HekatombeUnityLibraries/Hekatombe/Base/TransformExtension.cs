using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Hekatombe.Base
{
    public static class TransformExtension
    {
        public static void RotateYToDir(this Transform transform, Vector3 dir, float stepSize)
        {
            float angle = TransformExtension.GetYAngleToDir(transform, dir.normalized);

            RotateY(transform, angle * stepSize);
        }

        public static void RotateYToPoint(this Transform transform, Vector3 point, float stepSize)
        {
            Vector3 dir = point - transform.position;
            float angle = TransformExtension.GetYAngleToDir(transform, dir.normalized);
            
            RotateY(transform, angle * stepSize);
        }

        public static void RotateY(this Transform transform, float deltaAngle)
        {
            Quaternion deltaQuad = Quaternion.AngleAxis(deltaAngle, new Vector3(0, 1, 0));
            Quaternion newQuat = transform.rotation * deltaQuad;
            transform.rotation = newQuat;
        }

        public static float GetYAngleToPoint(this Transform transform, Vector3 point)
        {
            Vector3 dir = point - transform.position;
            float angle = TransformExtension.GetYAngleToDir(transform, dir.normalized);

            return angle;
        }

        public static float GetYAngleToDir(this Transform transform, Vector3 dir)
        {
            dir.Normalize();

            float projToFront = Vector3.Dot(dir, transform.forward.normalized);
            float projToRight = Vector3.Dot(dir, transform.right.normalized);
            
            float radians = Mathf.Atan2(projToRight, projToFront);
			float angle = Mathf.Rad2Deg * radians;
            
            return angle;
        }

        public static Transform FindChildByName(this Transform trans, string name)
        {
            Component[] transforms = trans.GetComponentsInChildren<Transform>(true);

            for(int k = 0; k < transforms.Count(); k++)
            {
                Transform atrans = transforms[k] as Transform;
                if(atrans.name == name)
                {
                    return atrans;
                }
            }
            return null;
		}

		public static T FindChildComponent<T>(this Transform trans, string name) where T : UnityEngine.Component
		{
			Transform t = trans.FindChildByName (name);
			if (t == null) {
				Debug.LogError ("Transform Child not found in: " + trans.name + " -- Child Name: " + name);
				return null;
			}
			return t.GetComponent<T> ().BreakIfNull ("Component not found in Child: " + name);
		}

		public static T FindComponent<T>(this Transform transform, string name) where T : Component
		{
			Transform tr = transform.Find (name).BreakIfNull("Gameobject transform not found: " + name);
			T t = tr.GetComponent<T> ();
			if (t == null) {
				Debug.LogErrorFormat ("Component of TYPE: {0} not found in GameObject: {1}", typeof(T).FullName, name);
				UnityEngine.Debug.Break(); 
			}
			return t;
		}
    }
}

