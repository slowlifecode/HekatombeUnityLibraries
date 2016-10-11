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

		public static Vector3 RandomVector3Range(float range)
		{
			return new Vector3 (Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
		}

		public static int RangeButDifferentThan(int min, int max, int different)
		{
			int rand = Random.Range (min, max);
			//If it's the same value, get the next
			if (rand == different) {
				rand = (rand + 1) % max;
			}
			return rand;
		}
	}
}