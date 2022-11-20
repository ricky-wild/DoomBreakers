using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{

	public enum AnimationState
	{
		IdleBroadSword = 0,
		IdleLongsword = 1,
		IdleShield = 2,
		IdleBreastplate = 3
	}
    public class ItemAnimator : IItemAnimator
    {
		private Animator _animator;
		private RuntimeAnimatorController _runtimeAnimatorController;
		private AnimationState _animationState;
		private AnimatorController _animatorController;
		private string _animControllerFilepath;

		public ItemAnimator(Animator animator, AnimatorController animController, AnimationState animState)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_animationState = animState;// AnimationState.IdleSword;
			_animatorController = animController;// AnimatorController.Weapon_equipment_to_pickup;
			_animControllerFilepath = "ItemAnimControllers/"; //Equipment/Weapon.controller

			SetAnimatorController(animController);
		}

		public void UpdateAnimator()
		{
			switch (_animationState)
			{
				case AnimationState.IdleBroadSword:
					_animator.Play("Sword");//, 0, 0.0f);
					break;
				case AnimationState.IdleLongsword:
					_animator.Play("Longsword");//, 0, 0.0f);
					break;
				case AnimationState.IdleShield:
					_animator.Play("Shield");//, 0, 0.0f);
					break;
				case AnimationState.IdleBreastplate:
					_animator.Play("Armor");//, 0, 0.0f);
					break;
			}
		}
		public void SetAnimationState(AnimationState animationState)
		{
			_animationState = animationState;
		}
		public void SetAnimatorController(AnimatorController animatorController)
		{
			_animatorController = animatorController;
			

			switch (_animatorController)
			{
				case AnimatorController.Weapon_equipment_to_pickup:
					_animControllerFilepath = "ItemAnimControllers/Equipment/";
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Weapon") as RuntimeAnimatorController;
					break;

			}
		}
	}
}

