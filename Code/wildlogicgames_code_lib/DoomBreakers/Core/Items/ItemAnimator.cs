using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public enum ItemAnimationState
	{
		Empty = -1,
		IdleBroadSword = 0,
		IdleLongsword = 1,
		IdleShield = 2,
		IdleBreastplate = 3
	}
    public class ItemAnimator : Character2DBaseAnimator
	{

		private ItemAnimationState _animationState;
		private PlayerAnimatorController _animatorController;
		private string _animControllerFilepath;

		public ItemAnimator(Animator animator, string str1, string str2, string str3, PlayerAnimatorController animController, ItemAnimationState animState) 
			: base(animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{

			_animationState = animState;// AnimationState.IdleSword;
			_animatorController = animController;// AnimatorController.Weapon_equipment_to_pickup;
			//ItemAnimControllers/Equipment/Weapon.controller

			SetAnimatorController(animController);
		}

		public void UpdateAnimator()
		{
			switch (_animationState)
			{
				case ItemAnimationState.IdleBroadSword:
					_animator.Play("Sword");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleLongsword:
					_animator.Play("Longsword");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleShield:
					_animator.Play("Shield");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleBreastplate:
					_animator.Play("Armor");//, 0, 0.0f);
					break;
			}
		}
		public void SetAnimationState(ItemAnimationState animationState) => _animationState = animationState;
		public void SetAnimatorController(PlayerAnimatorController animatorController)
		{
			_animatorController = animatorController;
			

			switch (_animatorController)
			{
				case PlayerAnimatorController.Weapon_equipment_to_pickup:
					_animControllerFilepath = "ItemAnimControllers/Equipment/";
					_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Weapon") as RuntimeAnimatorController;
					break;

			}
		}
	}
}

