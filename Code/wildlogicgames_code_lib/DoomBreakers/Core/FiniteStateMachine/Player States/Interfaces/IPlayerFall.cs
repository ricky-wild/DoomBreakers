using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	interface IPlayerFall
	{
		void IsFalling(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input);
	}
}
