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
		IdleFish = 6,
		IdleGoldcoin = 7,
		IdleRuby = 8,
		IdleEmerald = 9,
		IdleSapphire = 10,
		IdleDiamond = 11
	}
    public class ItemAnimator : Character2DBaseAnimator
	{

		private ItemAnimationState _animationState;
		private PlayerAnimatorController _animatorController;
		private string _animControllerFilepath;

		public ItemAnimator(Animator animator, string baseFilePath, string subFilePath, string animControllerName, ItemAnimationState animState) 
			: base(animator: ref animator, baseAnimationControllerFilepath: baseFilePath, specificAnimControllerFilePath: subFilePath, theAnimationControllerName: animControllerName)
		{

			_animationState = animState;// AnimationState.IdleSword;
			//_animatorController = animController;// AnimatorController.Weapon_equipment_to_pickup;

			SetAnimatorController();
		}
		public ItemAnimator(Animator animator, string baseFilePath, string subFilePath, string animControllerName, HealingItemType healingItemType)
						: base(animator: ref animator, baseAnimationControllerFilepath: baseFilePath, specificAnimControllerFilePath: subFilePath, theAnimationControllerName: animControllerName)
		{
			if (healingItemType == HealingItemType.None) _animationState = ItemAnimationState.Empty;
			if (healingItemType == HealingItemType.Apple) _animationState = ItemAnimationState.IdleApple;
			if (healingItemType == HealingItemType.Chicken) _animationState = ItemAnimationState.IdleChicken;
			if (healingItemType == HealingItemType.Fish) _animationState = ItemAnimationState.IdleFish;

			SetAnimatorController();
		}
		public ItemAnimator(Animator animator, string baseFilePath, string subFilePath, string animControllerName, CurrencyItemType currencyItemType)
				: base(animator: ref animator, baseAnimationControllerFilepath: baseFilePath, specificAnimControllerFilePath: subFilePath, theAnimationControllerName: animControllerName)
		{
			if (currencyItemType == CurrencyItemType.None) _animationState = ItemAnimationState.Empty;
			if (currencyItemType == CurrencyItemType.Goldcoin) _animationState = ItemAnimationState.IdleGoldcoin;
			if (currencyItemType == CurrencyItemType.Ruby) _animationState = ItemAnimationState.IdleRuby;
			if (currencyItemType == CurrencyItemType.Emerald) _animationState = ItemAnimationState.IdleEmerald;
			if (currencyItemType == CurrencyItemType.Saphhire) _animationState = ItemAnimationState.IdleSapphire;
			if (currencyItemType == CurrencyItemType.Diamond) _animationState = ItemAnimationState.IdleDiamond;

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


				case ItemAnimationState.IdleGoldcoin:
					_animator.Play("Goldcoin");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleRuby:
					_animator.Play("Ruby");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleEmerald:
					_animator.Play("Emerald");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleSapphire:
					_animator.Play("Sapphire");//, 0, 0.0f);
					break;
				case ItemAnimationState.IdleDiamond:
					_animator.Play("Diamond");//, 0, 0.0f);
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

