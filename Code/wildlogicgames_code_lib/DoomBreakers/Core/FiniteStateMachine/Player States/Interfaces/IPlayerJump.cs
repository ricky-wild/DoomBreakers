using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	interface IPlayerJump
	{
		void IsJumping(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input);
	}
}
