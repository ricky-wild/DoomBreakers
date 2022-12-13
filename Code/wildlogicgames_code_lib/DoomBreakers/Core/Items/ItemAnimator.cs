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
		IdleBreastplate = 3,
		IdleApple = 4,
		IdleChicken = 5,
		IdleFish = 6
	}
    public class ItemAnimator : Character2DBaseAnimator
	{

		private ItemAnimationState _animationState;
		private PlayerAnimatorController _animatorController;
		private string _animControllerFilepath;

		public ItemAnimator(Animator animator, string str1, string str2, string str3, ItemAnimationState animState) 
			: base(animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{

			_animationState = animState;// AnimationState.IdleSword;
			//_animatorController = animController;// AnimatorController.Weapon_equipment_to_pickup;

			SetAnimatorController();
		}
		public ItemAnimator(Animator animator, string str1, string str2, string str3, HealingItemType healingItemType)
						: base(animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{
			if (healingItemType == HealingItemType.None) _animationState = ItemAnimationState.Empty;
			if (healingItemType == HealingItemType.Apple) _animationState = ItemAnimationState.IdleApple;
			if (healingItemType == HealingItemType.Chicken) _animationState = ItemAnimationState.IdleChicken;
			if (healingItemType == HealingItemType.Fish) _animationState = ItemAnimationState.IdleFish;

			SetAnimatorController();
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


				case ItemAnimationState.IdleApple:
					_animator.Play("appleUp");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleChicken:
					_animator.Play("chickenUp");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleFish:
					_animator.Play("fishUp");//, 0, 0.0f);
					break;
			}
		}
		public void SetAnimationState(ItemAnimationState animationState) => _animationState = animationState;
		public void SetAnimatorController()
		{
			//"ItemAnimControllers/Equipment/Weapon";
			//"ItemAnimControllers/HealthItems/Health";

			_animControllerFilepath = "" + GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/" + GetAnimControllerName() + "";
			_animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath) as RuntimeAnimatorController;
		}
	}
}

