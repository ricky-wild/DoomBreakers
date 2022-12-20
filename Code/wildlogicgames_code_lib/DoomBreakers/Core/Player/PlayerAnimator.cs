
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
		Player_with_morningstarMace_controller = 7,
		Player_with_morningstarMace_with_shield_controller = 8,

		Weapon_equipment_to_pickup = 9

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
				case PlayerAnimatorController.Player_with_morningstarMace_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_mace_controller") as RuntimeAnimatorController;
					break;
				case PlayerAnimatorController.Player_with_morningstarMace_with_shield_controller:
					_animator.runtimeAnimatorController = Resources.Load(GetBaseAnimFilePath() + "/" + GetSpecificAnimFilePath() + "/Player_with_mace&shield_controller") as RuntimeAnimatorController;
					break;
			}
		}

		private void SetupAnimControllerBasedOnEquip(IPlayerEquipment playerEquipment)
		{
			SetBaseAnimFilePath("HumanAnimControllers");
			if (playerEquipment.IsArmor()) SetSpecificAnimFilePath("Armored");
			else 
				SetSpecificAnimFilePath("Unarmored");


			if (AnimControllerEmptyHanded(playerEquipment)) return;

			if (AnimControllerBroadswordOnly(playerEquipment)) return;
			if (AnimControllerLongswordOnly(playerEquipment)) return;
			if (AnimControllerMorningstarMaceOnly(playerEquipment)) return;
			if (AnimControllerShieldOnly(playerEquipment)) return;

			if (AnimControllerBroadswordANDShield(playerEquipment)) return;
			if (AnimControllerLongswordANDShield(playerEquipment)) return;
			if (AnimControllerMorningstarMaceANDShield(playerEquipment)) return;
		}

		private bool AnimControllerEmptyHanded(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand))                                //EMPTY HANDED BOTH LEFT & RIGHT
			{
				_animatorController = PlayerAnimatorController.Player_with_nothing_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerBroadswordOnly(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&                                //BROADSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsBroadsword(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_broadsword_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerLongswordOnly(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsLongsword(EquipHand.Left_Hand) &&                                 //LONGSWORD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsLongsword(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_longsword_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerMorningstarMaceOnly(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsMorningstarMace(EquipHand.Left_Hand) &&                                 //MACE MORNINGSTAR ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsMorningstarMace(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_morningstarMace_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerMorningstarMaceANDShield(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //MACE MORNINGSTAR & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsMorningstarMace(EquipHand.Right_Hand) ||
				playerEquipment.IsShield(EquipHand.Right_Hand) &&
				playerEquipment.IsMorningstarMace(EquipHand.Left_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_morningstarMace_with_shield_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerShieldOnly(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //SHIELD ONLY IN LEFT OR RIGHT HAND
				playerEquipment.IsEmptyHanded(EquipHand.Right_Hand) ||
				playerEquipment.IsEmptyHanded(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_shield_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerBroadswordANDShield(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //BROADSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsBroadsword(EquipHand.Right_Hand) ||
				playerEquipment.IsBroadsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_broadsword_with_shield_controller;
				return true;
			}
			return false;
		}
		private bool AnimControllerLongswordANDShield(IPlayerEquipment playerEquipment)
		{
			if (playerEquipment.IsShield(EquipHand.Left_Hand) &&                                    //LONGSWORD & SHIELD, LEFT OR RIGHT VICE VERSA COMBINATION
				playerEquipment.IsLongsword(EquipHand.Right_Hand) ||
				playerEquipment.IsLongsword(EquipHand.Left_Hand) &&
				playerEquipment.IsShield(EquipHand.Right_Hand))
			{
				_animatorController = PlayerAnimatorController.Player_with_longsword_with_shield_controller;
				return true;
			}
			return false;
		}


	}
}

