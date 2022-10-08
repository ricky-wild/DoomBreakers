using UnityEngine;

namespace DoomBreakers
{
	interface IPlayerBehaviours //: MonoBehaviour
	{
		void UpdateMovement(Vector2 input, IPlayerStateMachine playerStateMachine);
		bool SafeToMove(IPlayerStateMachine playerStateMachine);
		bool SafeToSetIdle(IPlayerStateMachine playerStateMachine);
		void IdleProcess();
		void JumpProcess();
		void QuickAttackProcess();
	}
}

