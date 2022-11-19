
using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerReleaseAttack
	{
		void IsReleaseAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input);
	}
}
