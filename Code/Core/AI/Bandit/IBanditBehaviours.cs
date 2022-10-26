using UnityEngine;

namespace DoomBreakers
{
	public interface IBanditBehaviours //: MonoBehaviour
	{
		int GetQuickAttackIndex();
		void Setup(Transform t, Controller2D controller2D);
		void UpdateMovement(Vector2 input, IEnemyStateMachine enemyStateMachine, IPlayerSprite playerSprite);
		void UpdateTransform(Vector2 input);
		void UpdateGravity(IEnemyStateMachine enemyStateMachine);


	}
}

