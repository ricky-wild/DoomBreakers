using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{

	public enum BanditAnimatorController
	{
		Bandit_with_nothing_controller = 0,
		Bandit_with_broadsword_controller = 1,
		Bandit_with_shield_controller = 2,
		Bandit_with_broadsword_with_shield_controller = 3,
		Bandit_with_broadsword_with_broadsword_controller = 4
	};

	public class BanditAnimator : IBanditAnimator
	{
		private Animator _animator;
		private RuntimeAnimatorController _runtimeAnimatorController;
		private AnimationState _animationState;
		private BanditAnimatorController _animatorController;
		private string _animControllerFilepath;

		public BanditAnimator(Animator animator)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_animatorController = BanditAnimatorController.Bandit_with_nothing_controller;
			_animControllerFilepath = "EnemyAnimControllers/HumanoidBandit/";
		}
		public void UpdateAnimator(IBanditBehaviours banditBehaviour)
		{
			
			//switch (_animationState)
			//{
			//	case AnimationState.IdleAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Idle");//, 0, 0.0f);
			//		break;
			//	case AnimationState.MoveAnim:
			//		//_animator.speed = 9.0f;
			//		_animator.Play("Run");
			//		break;
			//	case AnimationState.JumpAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Jump");//, -1, 0.0f);//, -1, 0.0f);
			//		break;
			//	case AnimationState.AirJumpAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("DblJump");
			//		break;
			//	case AnimationState.QuickAtkAnim:
			//		//_animator.speed = 1.2f;
			//		int quickAttackIndex = banditBehaviour.GetQuickAttackIndex();
			//		switch (quickAttackIndex)
			//		{
			//			case 0:
			//				_animator.Play("SmallAttack");//, -1, 0.0f); //SmallAttack2 - SmallAttack5
			//				break;
			//			case 1:
			//				_animator.Play("SmallAttack2");//, -1, 0.0f);
			//				break;
			//		}
			//		break;
			//	case AnimationState.AirQuickAtkAnim:
			//		//_animator.speed = 2.0f;
			//		_animator.Play("JumpAttack");
			//		break;
			//	case AnimationState.HoldAtkAnim:
			//		//_animator.speed = 1.5f;
			//		_animator.Play("Attack");
			//		break;
			//	case AnimationState.ReleaseAtkAnim:
			//		//_animator.speed = 1.5f;
			//		_animator.Play("AtkRelease");
			//		break;
			//	case AnimationState.DefendAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Defend");
			//		break;
			//	case AnimationState.DefendHitAnim:
			//		//_animator.speed = 2.0f;
			//		_animator.Play("DefHit");
			//		break;
			//	case AnimationState.SmallHitAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Jabbed");
			//		break;
			//	case AnimationState.PowerHitAnim:
			//		//_animator.speed = 5.0f;
			//		_animator.Play("Hit");
			//		break;
			//	case AnimationState.FallenAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Fall");
			//		break;
			//	case AnimationState.DyingAnim:
			//		//_animator.speed = 1.2f;
			//		_animator.Play("Dying");
			//		break;
			//	case AnimationState.DeathAnim:
			//		//_animator.speed = 1.0f;
			//		_animator.Play("Dead");
			//		break;
			//}
		}
		public AnimationState GetAnimationState()
		{
			return _animationState;
		}
		public void SetAnimationState(AnimationState animationState)
		{
			//if (_animationState == animationState)//Guard Clause
			//	return;

			//UpdateAnimator(); //.Play() once.

			_animationState = animationState;
		}
		public void SetAnimatorController(BanditAnimatorController animatorController)
		{
			_animatorController = animatorController;

			_animControllerFilepath = "EnemyAnimControllers/HumanoidBandit/";

			switch (_animatorController)
			{
				case BanditAnimatorController.Bandit_with_nothing_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Bandit_with_nothing_controller") as RuntimeAnimatorController;
					break;
				case BanditAnimatorController.Bandit_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Bandit_with_sword_controller") as RuntimeAnimatorController;
					break;
				case BanditAnimatorController.Bandit_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Bandit_with_shield_controller") as RuntimeAnimatorController;
					break;
				case BanditAnimatorController.Bandit_with_broadsword_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Bandit_with_sword&shield_controller") as RuntimeAnimatorController;
					break;
				case BanditAnimatorController.Bandit_with_broadsword_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Bandit_with_sword&sword_controller") as RuntimeAnimatorController;
					break;

			}
		}
	}

}
