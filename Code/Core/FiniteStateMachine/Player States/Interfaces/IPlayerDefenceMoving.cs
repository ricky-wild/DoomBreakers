
using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerDefenceMoving
	{
		void IsDefenceMoving(ref Animator animator, ref Vector2 input);
	}
}
