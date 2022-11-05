
namespace DoomBreakers
{
	public interface IEnemyStateMachine //: MonoBehaviour
	{

		void SetEnemyState(state setTheState);
		state GetEnemyState();

		//<summary>
		//We make use of these bool returns for cleaner code & readibility throughout project. Otherwise we have the
		//following to look at,
		//if (enemyStateMachine.GetEnemyState() == state.IsQuickAttack)
		//		playerStateMachine.SetPlayerState(state.IsQuickHitWhileDefending);
		//</summary>
		//<return bool>The Method name relation to state returns true if that's the case</return>

		bool IsIdle();
		bool IsMoving();
		bool IsSprinting();
		bool IsJumping();
		bool IsFalling();
		bool IsQuickAttack();
		bool IsPowerAttackPrepare();
		bool IsPowerAttackRelease();
		bool IsKnockbackAttackPrepare();
		bool IsKnockbackAttackRelease();
		bool IsUpwardAttack();
		bool IsJumpingAttack();
		bool IsMovingAttack();
		bool IsDefendingPrepare();
		bool IsDefendingRelease();
		bool IsDefendingMoving();
		bool IsDodgeLeftPrepare();
		bool IsDodgeRightPrepare();
		bool IsDodgeRelease();
		bool IsQuickHitWhenDefending();
		bool IsPowerHitWhenDefending();
		bool IsQuickAttackHit();
		bool IsPowerAttackHit();
		bool IsImpactHit();
		bool IsGainedEquipment();
		bool IsArmorDestroyed();
		bool IsRespawning();
		bool IsWaiting();
		bool IsExhausted();
		bool IsDying();
		bool IsDead();
	}
}

