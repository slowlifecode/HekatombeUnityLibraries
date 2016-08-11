using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Hekatombe.Base
{
    public static class PhysicsExtension
    {
		public static bool PointInOABB(this BoxCollider box, Vector3 point)
		{        
			point = box.transform.InverseTransformPoint( point ) - box.center;

			float halfX = (box.size.x * 0.5f);
			float halfY = (box.size.y * 0.5f);
			float halfZ = (box.size.z * 0.5f);
			if (point.x < halfX && point.x > -halfX &&
			    point.y < halfY && point.y > -halfY &&
			    point.z < halfZ && point.z > -halfZ)
			{
				return true;
			} else {
				return false;
			}
        }
			
		public static bool ContainBounds(this Bounds bounds, Bounds target)
		{
			return bounds.Contains(target.min) && bounds.Contains(target.max);
		}
    }
}

