using System;
using UnityEngine;

namespace Hekatombe.Utils
{
    public class MathUtils
	{
		//To obtain the position Y of a parabola given X and other init data
		//From: http://physics.stackexchange.com/questions/60855/given-angle-initial-velocity-and-acceleration-due-to-gravity-plot-parabolic-t
		public static float ParabolaYFromX(float posX, float posInitX, float posInitY, float initialSpeed, float angle, float gravity)
		{
			angle = Mathf.Deg2Rad * angle;
			float time = (posX-posInitX)/(initialSpeed*Mathf.Cos(angle));
			return posInitY + (initialSpeed * Mathf.Sin(angle) * time) + (0.5f * gravity * time * time);
		}
    }
}