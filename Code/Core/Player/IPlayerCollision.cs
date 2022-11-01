using UnityEngine;

namespace DoomBreakers
{
	public interface IPlayerCollision //: MonoBehaviour
	{
		//void Setup(Collider2D collider2D, ref Transform[] arrayAtkPoints);
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(IPlayerStateMachine playerStateMachine);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(IPlayerStateMachine playerStateMachine);
		void EnableAttackCollisions();
		//bool IsAttackCollisionsEnabled();
		void FlipAttackPoints(int dir);
	}
}

