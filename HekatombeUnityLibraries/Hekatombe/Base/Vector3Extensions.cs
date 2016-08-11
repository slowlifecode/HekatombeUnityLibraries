using System.Text;
using UnityEngine;

namespace Hekatombe.Base
{
    public static class Vector3Extensions
    {
		/********
		 * Create one Axis
		 */
		public static Vector3 CreateOnlyX(float newX)
		{
			return new Vector3(newX, 0, 0);
		}

		public static Vector3 CreateOnlyY(float newY)
		{
			return new Vector3(0, newY, 0);
		}

		public static Vector3 CreateOnlyZ(float newZ)
		{
			return new Vector3(0, 0, newZ);
		}

		/********
		 * Modify one axis
		 */
		public static Vector3 CopyVectorButModifyX(this Vector3 v, float newX)
		{
			return new Vector3(newX, v.y, v.z);
		}
		
		public static Vector3 CopyVectorButModifyY(this Vector3 v, float newY)
		{
			return new Vector3(v.x, newY, v.z);
		}
		
		public static Vector3 CopyVectorButModifyZ(this Vector3 v, float newZ)
		{
			return new Vector3(v.x, v.y, newZ);
		}

		/********
		 * Add to one axis
		 */
		public static Vector3 CopyVectorButAddX(this Vector3 v, float addX)
		{
			return new Vector3(v.x + addX, v.y, v.z);
		}
		
		public static Vector3 CopyVectorButAddY(this Vector3 v, float addY)
		{
			return new Vector3(v.x, v.y + addY, v.z);
		}
		
		public static Vector3 CopyVectorButAddZ(this Vector3 v, float addZ)
		{
			return new Vector3(v.x, v.y, v.z + addZ);
		}
    }
}