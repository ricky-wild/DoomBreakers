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
		void UpdateCollision(ref BasicEnemyBaseState banditState, IBanditSprite banditSprite);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(ref BasicEnemyBaseState banditState, IBanditSprite banditSprite);
		void EnableTargetCollisionDetection();
		
		//Transform GetCollidedTargetTransform();
		void FlipAttackPoints(int dir);

	}
}

