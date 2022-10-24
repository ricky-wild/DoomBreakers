using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerCollision //: MonoBehaviour
	{
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(IPlayerStateMachine playerStateMachine);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(IPlayerStateMachine playerStateMachine);
		void EnableAttackCollisions();
	}
}

