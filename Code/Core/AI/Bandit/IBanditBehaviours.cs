using UnityEngine;

namespace DoomBreakers
{
	public interface IBanditBehaviours //: MonoBehaviour
	{
		int GetQuickAttackIndex();
		void Setup(Transform t, Controller2D controller2D);
		void UpdateMovement(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
		void UpdateTransform();
		void UpdateGravity(IEnemyStateMachine enemyStateMachine);
		void HitByQuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
	}
}

