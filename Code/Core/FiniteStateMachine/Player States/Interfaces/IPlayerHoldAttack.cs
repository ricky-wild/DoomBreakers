
using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerHoldAttack
	{
		void IsHoldAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input);
	}
}
