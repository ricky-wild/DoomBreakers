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
		BrokenArmorAnim = 17,
		JumpUpwardAtkAnim = 18,
		FallenAnim = 19,
		DyingAnim = 20,
		DeathAnim = 21,
		SmallHitAnim = 22
	};

	public enum AnimatorController
	{
		Player_with_nothing_controller = 0,
		Player_with_broadsword_controller = 1,
		Player_with_shield_controller = 2,
		Player_with_broadsword_with_shield_controller = 3,
		Player_with_broadsword_with_broadsword_controller = 4,
		Player_with_longsword_controller = 5,
		Player_with_longsword_with_shield_controller = 6

	};

	public class PlayerAnimator : IPlayerAnimator //MonoBehaviour, 
	{
		private Animator _animator;
		private RuntimeAnimatorController _runtimeAnimatorController;
		private AnimationState _animationState;
		private AnimatorController _animatorController;
		private string _animControllerFilepath; 

		public PlayerAnimator(Animator animator)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_animationState = AnimationState.IdleAnim;
			_animatorController = AnimatorController.Player_with_nothing_controller;
			_animControllerFilepath = "HumanAnimControllers/Unarmored/";
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
				case AnimationState.BrokenArmorAnim:
					_animator.Play("BrokenArmor");
					break;
				case AnimationState.JumpUpwardAtkAnim:
					_animator.Play("JumpSmallAttackUpward");
					break;
				case AnimationState.SmallHitAnim:
					_animator.Play("Jabbed");
					break;
				case AnimationState.FallenAnim:
					_animator.Play("Fallen");
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
		public void SetAnimatorController(AnimatorController animatorController, bool withArmor)
		{
			_animatorController = animatorController;

			if(withArmor)
				_animControllerFilepath = "HumanAnimControllers/Armored/";
			else
				_animControllerFilepath = "HumanAnimControllers/Unarmored/";

			switch (_animatorController)
			{
				case AnimatorController.Player_with_nothing_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_nothing_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_shield_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_broadsword_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword&shield_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_broadsword_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword&sword_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_longsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_longsword_controller") as RuntimeAnimatorController;
					break;
				case AnimatorController.Player_with_longsword_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_longsword&shield_controller") as RuntimeAnimatorController;
					break;
			}
		}

	}
}

