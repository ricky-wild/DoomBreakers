using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public enum BanditAnimatorController
	{
		Bandit_with_nothing_controller = 0,
		Bandit_with_broadsword_controller = 1,
		Bandit_with_shield_controller = 2,
		Bandit_with_broadsword_and_shield_controller = 3,
		Bandit_with_broadsword_and_broadsword_controller = 4,
		Bandit_with_bow_and_arrows_controller = 5
	};

	public class BanditAnimator : Character2DBaseAnimator 
	{


		private BanditAnimatorController _animatorController;


		public BanditAnimator(Animator animator, string animFilePath, string animSubFilePath, string animControllerName)
			: base(animator: ref animator, baseAnimationControllerFilepath: animFilePath, 
				  specificAnimControllerFilePath: animSubFilePath, theAnimationControllerName: animControllerName)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_baseAnimControllerFilepath = animFilePath;
			_specificAnimControllerFilepath = animSubFilePath;
			_theAnimationControllerName = animControllerName;
			//_animatorController = BanditAnimatorController.Bandit_with_nothing_controller;
			//"EnemyAnimControllers/HumanoidBandit/Bandit_with_bow&arrows";
		}

		public void SetAnimatorController(BanditAnimatorController animatorController)
		{
			_animatorController = animatorController;

			if (_animatorController == BanditAnimatorController.Bandit_with_nothing_controller) SetAnimControllerName("Bandit_with_nothing_controller");
			if (_animatorController == BanditAnimatorController.Bandit_with_broadsword_controller) SetAnimControllerName("Bandit_with_sword_controller");
			if (_animatorController == BanditAnimatorController.Bandit_with_shield_controller) SetAnimControllerName("Bandit_with_shield_controller");
			if (_animatorController == BanditAnimatorController.Bandit_with_broadsword_and_shield_controller) SetAnimControllerName("Bandit_with_sword&shield_controller");
			if (_animatorController == BanditAnimatorController.Bandit_with_broadsword_and_broadsword_controller) SetAnimControllerName("Bandit_with_sword&sword_controller");
			if (_animatorController == BanditAnimatorController.Bandit_with_bow_and_arrows_controller) SetAnimControllerName("Bandit_with_bow&arrows_controller");

			_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/" + GetAnimControllerName())
				as RuntimeAnimatorController;


		}
	}

}
