using UnityEngine;

namespace DoomBreakers
{
	interface IBanditCollision //: MonoBehaviour
	{
		//void TestMethod();
		//void SetOnEnabled();
		//void SetOnDisabled();
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(IEnemyStateMachine banditStateMachine);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(IEnemyStateMachine banditStateMachine);
		void EnableAttackCollisions();
		IEnemyStateMachine RegisterHitByAttack(IPlayerStateMachine playerStateMachine);
	}
}

