using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerFall
	{
		void IsFalling(ref Animator animator, ref Controller2D controller2D, ref Vector2 input);
	}
}
