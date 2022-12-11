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

	public class BanditAnimator //: IBanditAnimator
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
