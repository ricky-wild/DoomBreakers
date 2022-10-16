using UnityEngine;

namespace DoomBreakers
{
	public interface IPlayerBehaviours //: MonoBehaviour
	{
		int GetQuickAttackIndex();
		void Setup(Transform t, Controller2D controller2D);
		void UpdateMovement(Vector2 input, IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);
		void UpdateTransform(Vector2 input);
		void UpdateGravity(IPlayerStateMachine playerStateMachine);
		bool SafeToJump(IPlayerStateMachine playerStateMachine);
		bool SafeToDodge(IPlayerStateMachine playerStateMachine);
		bool SafeToMove(IPlayerStateMachine playerStateMachine);
		bool SafeToSetIdle(IPlayerStateMachine playerStateMachine);
		void IdleProcess();
		bool JumpProcess(IPlayerStateMachine playerStateMachine);
		void FallProcess(IPlayerStateMachine playerStateMachine);
		void QuickAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);
		void IdleDefenceProcess(IPlayerStateMachine playerStateMachine);
		void HoldAttackProcess(IPlayerStateMachine playerStateMachine);
		void ReleaseAttackProcess(IPlayerStateMachine playerStateMachine);
		void KnockbackAttackProcess(IPlayerStateMachine playerStateMachine);
		void DodgeProcess(IPlayerStateMachine playerStateMachine, bool dodgeLeft, IPlayerSprite playerSprite);
	}
}

