using UnityEngine;

namespace DoomBreakers
{
	public interface IPlayerBehaviours //: MonoBehaviour
	{
		int GetQuickAttackIndex();
		void Setup(Transform t, Controller2D controller2D);
		void UpdateMovement(Vector2 input, IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, IPlayerCollision playerCollider);
		void UpdateTransform(Vector2 input);
		void UpdateGravity(IPlayerStateMachine playerStateMachine);
		bool SafeToJump(IPlayerStateMachine playerStateMachine);
		bool SafeToDodge(IPlayerStateMachine playerStateMachine);
		bool SafeToMove(IPlayerStateMachine playerStateMachine);
		bool SafeToSetIdle(IPlayerStateMachine playerStateMachine);
		void IdleProcess(IPlayerStateMachine playerStateMachine);
		bool JumpProcess(IPlayerStateMachine playerStateMachine);
		void FallProcess(IPlayerStateMachine playerStateMachine);
		void QuickAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);//, IPlayerCollision playerCollider);
		void UpwardAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);
		void IdleDefenceProcess(IPlayerStateMachine playerStateMachine);
		void HoldAttackProcess(IPlayerStateMachine playerStateMachine);
		void ReleaseAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);
		void KnockbackAttackProcess(IPlayerStateMachine playerStateMachine);
		void DodgeInitiatedProcess(IPlayerStateMachine playerStateMachine, bool dodgeLeft, IPlayerSprite playerSprite, IPlayerCollision playerCollider);
		void DodgeReleasedProcess(IPlayerStateMachine playerStateMachine);
		void HitByQuickAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite);
		bool EquipmentGainedProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, IPlayerEquipment playerEquipment);
	}
}

