
using UnityEngine;

namespace DoomBreakers
{

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
	public enum IndicatorAnimID //_playerIndicatorAnimator.Play(animName);//Animator state names= ID1, ID1Pickup, ID1Tired, ID1Death
	{
		Idle = 0,//_indicatorAnimStr[0]
		Pickup = 1,//_indicatorAnimStr[1] and so on..
		Tired = 2,
		Dead = 3
	}; //Must corrispond to string[] _indicatorAnimStr assignment order.

	public class PlayerAnimator : IPlayerAnimator //MonoBehaviour, 
	{

		private int _playerID;

		private Animator _animator;
		private RuntimeAnimatorController _runtimeAnimatorController;
		private AnimationState _animationState;
		private AnimatorController _animatorController;
		private string _animControllerFilepath;

		private Animator _playerIndicatorAnimator;
		private string[] _indicatorAnimStr = new string[4];

		public PlayerAnimator(Animator animator, ref Animator playerIndicatorAnimator, int playerId)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_animatorController = AnimatorController.Player_with_nothing_controller;
			_animControllerFilepath = "HumanAnimControllers/Unarmored/";

			_playerIndicatorAnimator = playerIndicatorAnimator;
			_playerID = playerId;

			_indicatorAnimStr[0] = "ID" + (_playerID + 1).ToString() + "";
			_indicatorAnimStr[1] = "ID" + (_playerID + 1).ToString() + "Pickup";
			_indicatorAnimStr[2] = "ID" + (_playerID + 1).ToString() + "Tired";
			_indicatorAnimStr[3] = "ID" + (_playerID + 1).ToString() + "Death";

			//_playerIndicatorAnimator.Play(_indicatorAnimStr[0]); 
			PlayIndicatorAnimation(IndicatorAnimID.Idle);
		}

		public void PlayIndicatorAnimation(IndicatorAnimID indicatorAnim) => _playerIndicatorAnimator.Play(_indicatorAnimStr[(int)indicatorAnim]);

		public void SetAnimatorController(ref IPlayerEquipment playerEquipment)//AnimatorController animatorController, bool withArmor)
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

