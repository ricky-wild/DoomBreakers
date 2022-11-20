using UnityEngine;

namespace DoomBreakers
{
	public interface IBanditBehaviours //: MonoBehaviour
	{
		int GetQuickAttackIndex();
		void Setup(Transform t, Controller2D controller2D);
		void UpdateMovement(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite, IBanditCollision banditCollider);
		void UpdateTransform();
		void UpdateGravity(IEnemyStateMachine enemyStateMachine);
		void IdleProcess(IEnemyStateMachine enemyStateMachine, IBanditCollision banditCollider);
		void WaitingProcess(IEnemyStateMachine enemyStateMachine);
		bool JumpProcess(IEnemyStateMachine enemyStateMachine);
		void FallProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite, int banditId);
		void PersueTarget(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite); //Transform targetTransform, IBanditSprite banditSprite);
		void QuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
		void HitByQuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
		//void HitByPowerAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
		void HitByPowerAttackProcess(int banditId);

		bool SafeToPersueTarget(IEnemyStateMachine enemyStateMachine, Transform targetTransform, IPlayerStateMachine playerStateMachine);
	}
}

