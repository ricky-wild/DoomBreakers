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
		void UpdateCollision(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, int playerId);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, int playerId);
		//IPlayerStateMachine RegisterHitByAttack(IEnemyStateMachine enemyStateMachine, IPlayerStateMachine playerStateMachine,
		//										IPlayerSprite playerSprite, IBanditSprite banditSprite);
		IPlayerStateMachine RegisterHitByAttack(ICollisionData collisionData, int playerId, int banditId);
		void EnableAttackCollisions();
		//bool IsAttackCollisionsEnabled();
		void FlipAttackPoints(int dir);
	}
}

