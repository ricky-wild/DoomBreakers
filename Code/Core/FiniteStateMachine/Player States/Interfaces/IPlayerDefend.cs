

using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerDefend
	{
		void IsDefending(ref Animator animator, ref Vector2 input);
	}
}
