using UnityEngine;
using System.Collections;

namespace Hekatombe.Base
{
	public static class RandomExtension
	{
		public static Vector3 RandomDirection()
		{
			Vector3 randomDirection = new Vector3 (Random.value-0.5f, Random.value-0.5f, Random.value-0.5f);
			randomDirection.Normalize ();
			return randomDirection;
		}

		public static Vector3 RandomDirectionOverYAxis()
		{
			Vector3 randomDirection = new Vector3 (Random.value-0.5f, 0, Random.value-0.5f);
			randomDirection.Normalize ();
			return randomDirection;
		}
	}
}