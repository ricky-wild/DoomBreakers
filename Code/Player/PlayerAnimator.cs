using UnityEngine;

namespace DoomBreakers
{
	public enum AnimationState
	{
		IdleAnim = 0,
		MoveAnim = 1,
		SprintAnim = 2,
		JumpAnim = 3,
		AirJumpAnim = 4,
		QuickAtkAnim = 5,
		AirQuickAtkAnim = 6,
		HoldAtkAnim = 7,
		ReleaseAtkAnim = 8,
		KnockBackAtkAnim = 9,
		DodgeAnim = 10,
		DefendAnim = 11,
		DefendMoveAnim = 12,
		DefendHitAnim = 13,
		UpwardAtkAnim = 14,
		DownwardAtkAnim = 15,
		TiredAnim = 16,
		DyingAnim = 17,
		DeathAnim = 18
	};

	public class PlayerAnimator : IPlayerAnimator //MonoBehaviour, 
	{
		private Animator _animator;
		private AnimationState _animationState;
		//private string _currentStateStr;

		public PlayerAnimator(Animator animator)
		{
			_animator = animator;
			_animationState = AnimationState.IdleAnim;
			//_currentStateStr = "Idle";
			//if(this.GetComponent<Animator>() != null)
			//	_animator = this.GetComponent<Animator>();
			//else
			//	print("\nPlayerAnimator.cs= _animator Not Found/Assigned!");
		}
		public void UpdateAnimator()
		{
			switch(_animationState)
			{
				case AnimationState.IdleAnim:
					_animator.Play("Idle");
					break;
				case AnimationState.MoveAnim:
					_animator.Play("Run");
					break;
				case AnimationState.SprintAnim:
					_animator.Play("Sprint");
					break;
				case AnimationState.JumpAnim:
					_animator.Play("Jump");
					break;
				case AnimationState.AirJumpAnim:
					_animator.Play("DblJump");
					break;
				case AnimationState.QuickAtkAnim:
					_animator.Play("SmallAttack"); //SmallAttack2 - SmallAttack5
					break;
				case AnimationState.AirQuickAtkAnim:
					_animator.Play("JumpAttack"); 
					break;
				case AnimationState.HoldAtkAnim:
					_animator.Play("Attack");
					break;
				case AnimationState.ReleaseAtkAnim:
					_animator.Play("AtkRelease");
					break;
				case AnimationState.KnockBackAtkAnim:
					_animator.Play("Knock Attack");
					break;
				case AnimationState.DodgeAnim:
					_animator.Play("Dodge");
					break;
				case AnimationState.DefendAnim:
					_animator.Play("Defend");
					break;
				case AnimationState.DefendMoveAnim:
					_animator.Play("RunDefend");
					break;
				case AnimationState.DefendHitAnim:
					_animator.Play("DefHit");
					break;
				case AnimationState.UpwardAtkAnim:
					_animator.Play("SmallAttackUpward");
					break;
				case AnimationState.DownwardAtkAnim:
					
					break;
				case AnimationState.TiredAnim:
					_animator.Play("Tired");
					break;
				case AnimationState.DyingAnim:
					_animator.Play("Dying");
					break;
				case AnimationState.DeathAnim:
					_animator.Play("Dead");
					break;
			}
		}
		public void SetAnimationState(AnimationState animationState)
		{
			_animationState = animationState;
		}

		//public void SetAnimationState(string animationState)
		//{
		//	if (animationState == _currentStateStr) //Guard Clause
		//		return;

		//	_animator.Play(animationState);//int stateNameHash

		//	_currentStateStr = animationState;
		//}
	}
}

