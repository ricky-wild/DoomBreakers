
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public enum PlayerAnimatorController
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
	public enum IndicatorAnimID //_playerIndicatorAnimator.Play(animName);//Animator state names= ID1, ID1Pickup, ID1Tired, ID1Death
	{
		Idle = 0,//_indicatorAnimStr[0]
		Pickup = 1,//_indicatorAnimStr[1] and so on..
		Tired = 2,
		Dead = 3
	}; //Must corrispond to string[] _indicatorAnimStr assignment order.

	public class PlayerAnimator : Character2DBaseAnimator
	{

		private int _playerID;

		private AnimationState _animationState;
		private PlayerAnimatorController _animatorController;

		private Animator _playerIndicatorAnimator;
		private string[] _indicatorAnimStr = new string[4];

		public PlayerAnimator(ref Animator animator, string str1, string str2, string str3, ref Animator playerIndicatorAnimator, int playerId)
			: base (animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{

			_animatorController = PlayerAnimatorController.Player_with_nothing_controller;
			//_animControllerFilepath = "HumanAnimControllers/Unarmored/";

			_playerIndicatorAnimator = playerIndicatorAnimator;
			_playerID = playerId;

			_indicatorAnimStr[0] = "ID" + (_playerID + 1).ToString() + "";
			_indicatorAnimStr[1] = "ID" + (_playerID + 1).ToString() + "Pickup";
			_indicatorAnimStr[2] = "ID" + (_playerID + 1).ToString() + "Tired";
			_indicatorAnimStr[3] = "ID" + (_playerID + 1).ToString() + "Death";

			PlayIndicatorAnimation(IndicatorAnimID.Idle);
		}

		public void PlayIndicatorAnimation(IndicatorAnimID indicatorAnim) => _playerIndicatorAnimator.Play(_indicatorAnimStr[(int)indicatorAnim]);

		public void SetAnimatorController(ref IPlayerEquipment playerEquipment)//AnimatorController animatorController, bool withArmor)
		{
			SetupAnimControllerBasedOnEquip(playerEquipment);

			switch (_animatorController)//"HumanAnimControllers/Armored/";
			{
				case PlayerAnimatorController.Player_with_nothing_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_nothing_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_sword_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_shield_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_broadsword_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_sword&shield_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_broadsword_with_broadsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_sword&sword_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_longsword_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_longsword_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_longsword_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_longsword&shield_controller") as RuntimeAnimatorController;
					break;
			}
		}

		private void SetupAnimControllerBasedOnEquip(IPlayerEquipment playerEquipment)
		{
			SetBaseAnimFilePath("HumanAnimControllers");
			if (playerEquipment.IsArmor()) SetSpecificAnimFilePath("Armored");
			else 
				SetSpecificAnimFilePath("Unarmored");

			if (playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) && 
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand))								//EMPTY HANDED BOTH LEFT & RIGHT
			{
				_animatorController = PlayerAnimatorController.Player_with_nothing_controller;
				return;
			}
			if (playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&								//BROADSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) && 
				playerEquipment.IsBroadsword(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_broadsword_controller;
				return;
			}

			if (playerEquipment.IsLongsword(EquipHand.Left_Hand) &&									//LONGSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsLongsword(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_longsword_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //SHIELD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_shield_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //BROADSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsBroadsword(EquipHand.Right_Hand) ||
				playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_broadsword_with_shield_controller;
				return;
			}

			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //LONGSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsLongsword(EquipHand.Right_Hand) ||
				playerEquipment.IsLongsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_longsword_with_shield_controller;
				return;
			}
		}

		

	}
}

