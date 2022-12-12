using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	interface IPlayerDodge
	{
		void IsDodging(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input, 
			bool dodgeLeft, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider);
	}
}
