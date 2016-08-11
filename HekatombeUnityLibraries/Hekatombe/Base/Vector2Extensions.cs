using System.Text;
using UnityEngine;

namespace Hekatombe.Base
{
    public static class Vector2Extensions
    {
		/********
		 * Create one Axis
		 */
		public static Vector2 CreateOnlyX(float newX)
		{
			return new Vector2(newX, 0);
		}

		public static Vector2 CreateOnlyY(float newY)
		{
			return new Vector2(0, newY);
		}

		/********
		 * Modify one axis
		 */
		public static Vector2 CopyVectorButModifyX(this Vector2 v, float newX)
		{
			return new Vector2(newX, v.y);
		}
		
		public static Vector2 CopyVectorButModifyY(this Vector2 v, float newY)
		{
			return new Vector2(v.x, newY);
		}

		/********
		 * Add to one axis
		 */
		public static Vector2 CopyVectorButAddX(this Vector2 v, float addX)
		{
			return new Vector2(v.x + addX, v.y);
		}
		
		public static Vector2 CopyVectorButAddY(this Vector2 v, float addY)
		{
			return new Vector2(v.x, v.y + addY);
		}
    }
}