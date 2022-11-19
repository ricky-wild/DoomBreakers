
using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerSmallHit
	{
		void IsHitBySmallAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input);
	}
}
