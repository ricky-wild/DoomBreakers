using UnityEngine;

namespace DoomBreakers
{
	public interface IBanditCollision //: MonoBehaviour
	{
		//void TestMethod();
		//void SetOnEnabled();
		//void SetOnDisabled();
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(IEnemyStateMachine banditStateMachine, IBanditSprite banditSprite);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(IEnemyStateMachine banditStateMachine, IBanditSprite banditSprite);
		void EnableTargetCollisionDetection();
		IEnemyStateMachine RegisterHitByAttack(ICollisionData collisionData);//IPlayerStateMachine playerStateMachine);
		Transform GetCollidedTargetTransform();
		void FlipAttackPoints(int dir);
		ICollisionData GetRecentCollision();
	}
}

