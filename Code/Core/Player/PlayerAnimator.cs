
using UnityEngine;

namespace DoomBreakers
{
	public enum AnimationState
	{
		IdleLongsword = -4,
		IdleBreastplate = -3,
		IdleShield = -2,
		IdleBroadSword = -1,

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
		SmallHitAnim = 22,
		PowerHitAnim = 23
	};

	public enum AnimatorController
	{
		Player_with_nothing_controller = 0,
		Player_with_broadsword_controller = 1,
		Player_with_shield_controller = 2,
		Player_with_broadsword_with_shield_controller = 3,
		Player_with_broadsword_with_broadsword_controller = 4,
		Player_with_longsword_controller = 5,
		Player_with_longsword_with_shield_controller = 6,

		Weapon_equipment_to_pickup = 7

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
		public void UpdateAnimator(IPlayerBehaviours playerBehaviour)
		{

			switch (_animationState)
			{
				case AnimationState.IdleAnim:
					_animator.Play("Idle");//, 0, 0.0f);
					break;
				case AnimationState.MoveAnim:
					_animator.Play("Run");
					break;
				case AnimationState.SprintAnim:
					_animator.Play("Sprint");
					break;
				case AnimationState.JumpAnim:
					_animator.Play("Jump");//, -1, 0.0f);//, -1, 0.0f);
					break;
				case AnimationState.AirJumpAnim:
					_animator.Play("DblJump");
					break;
				case AnimationState.QuickAtkAnim:
					int quickAttackIndex = playerBehaviour.GetQuickAttackIndex();
					switch(quickAttackIndex)
					{
						case 0:
							_animator.Play("SmallAttack");//, -1, 0.0f); //SmallAttack2 - SmallAttack5
							break;
						case 1:
							_animator.Play("SmallAttack2");//, -1, 0.0f);
							break;
						case 2:
							_animator.Play("SmallAttack3");
							break;
						case 3:
							_animator.Play("SmallAttack4");
							break;
					}
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
				case AnimationState.PowerHitAnim:
					_animator.Play("Hit");
					break;
				case AnimationState.FallenAnim:
					_animator.Play("Fall");
					break;
				case AnimationState.DyingAnim:
					_animator.Play("Dying");
					break;
				case AnimationState.DeathAnim:
					_animator.Play("Dead");
					break;
			}
		}
		public AnimationState GetAnimationState()
		{
			return _animationState;
		}
		public void SetAnimationState(AnimationState animationState)
		{
			_animationState = animationState;
		}
		public void SetAnimatorController(IPlayerEquipment playerEquipment)//AnimatorController animatorController, bool withArmor)
		{
			SetupAnimControllerBasedOnEquip(playerEquipment);

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

		private void SetupAnimControllerBasedOnEquip(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsArmor())
				_animControllerFilepath = "HumanAnimControllers/Armored/";
			else
				_animControllerFilepath = "HumanAnimControllers/Unarmored/";

			if (playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) && 
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand))								//EMPTY HANDED BOTH LEFT & RIGHT
			{
				_animatorController = AnimatorController.Player_with_nothing_controller;
				return;
			}
			if (playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&								//BROADSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) && 
				playerEquipment.IsBroadsword(EquipHand.Right_Hand))
			{
				_animatorController = AnimatorController.Player_with_broadsword_controller;
				return;
			}

			if (playerEquipment.IsLongsword(EquipHand.Left_Hand) &&									//LONGSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsLongsword(EquipHand.Right_Hand))
			{
				_animatorController = AnimatorController.Player_with_longsword_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //SHIELD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = AnimatorController.Player_with_shield_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //BROADSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsBroadsword(EquipHand.Right_Hand) ||
				playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = AnimatorController.Player_with_broadsword_with_shield_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //LONGSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsLongsword(EquipHand.Right_Hand) ||
				playerEquipment.IsLongsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = AnimatorController.Player_with_longsword_with_shield_controller;
				return;
			}
		}

		

	}
}

