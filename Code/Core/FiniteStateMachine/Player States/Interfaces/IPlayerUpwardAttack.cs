

using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerUpwardAttack
	{
		void IsUpwardAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input);
	}
}
