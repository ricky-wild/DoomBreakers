using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerMove
	{
		void IsMoving(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref IPlayerCollision playerCollider);
	}
}
