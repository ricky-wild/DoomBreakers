using System;
using UnityEngine;

namespace wildlogicgames
{
	public static class DoomBreakers
	{
		public static float GetGravity()
		{
			//float gravity = -(2 * 0.8f) / UnityEngine.Mathf.Pow(0.25f, 2);
			float gravity = -(2 * 0.9f) / UnityEngine.Mathf.Pow(0.25f, 2);
			return gravity;
		}
		public static float GetMoonGravity()
		{
			float gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect.
			return gravity;
		}
	}
}