

using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerQuickAttack
	{
		void IsQuickAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input);
	}
}
