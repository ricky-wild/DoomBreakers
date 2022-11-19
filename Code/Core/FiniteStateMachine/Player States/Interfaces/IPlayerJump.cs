using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerJump
	{
		void IsJumping(ref Animator animator, ref Controller2D controller2D, ref Vector2 input);
	}
}
