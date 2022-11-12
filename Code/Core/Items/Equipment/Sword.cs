using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class Sword : ItemBase
    {
		[Header("Sword ID")]
		[Tooltip("Set its base type")]
		public PlayerEquip _swordID;

		[Header("Sword Type")]
		[Tooltip("Set Item to any kind of sword.")]
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.

		public PlayerEquipType GetSwordType()
		{
			return _playerEquip;
		}

		private void SetupSword()
		{
			//Ensure a sword type has been applied within the inspector.
			if (_playerEquip == PlayerEquipType.Empty_None ||
				_playerEquip != PlayerEquipType.Broadsword_Bronze ||
				_playerEquip != PlayerEquipType.Broadsword_Iron ||
				_playerEquip != PlayerEquipType.Broadsword_Steel ||
				_playerEquip != PlayerEquipType.Broadsword_Ebony ||
				_playerEquip != PlayerEquipType.Longsword_Bronze ||
				_playerEquip != PlayerEquipType.Longsword_Iron ||
				_playerEquip != PlayerEquipType.Longsword_Steel ||
				_playerEquip != PlayerEquipType.Longsword_Ebony)
			{
				_playerEquip = PlayerEquipType.Broadsword_Bronze;
				_swordID = PlayerEquip.IsBroadsword;
			    _animState = AnimationState.IdleBroadSword;
				return;
			}


			//Set the appropriate animation for the sword.
			if (_playerEquip == PlayerEquipType.Broadsword_Bronze ||
				_playerEquip == PlayerEquipType.Broadsword_Iron ||
				_playerEquip == PlayerEquipType.Broadsword_Steel ||
				_playerEquip == PlayerEquipType.Broadsword_Ebony)
			{
				if (_swordID != PlayerEquip.IsBroadsword)
					_swordID = PlayerEquip.IsBroadsword;
				_animState = AnimationState.IdleBroadSword;
				return;
			}
			if (_playerEquip == PlayerEquipType.Longsword_Bronze ||
				_playerEquip == PlayerEquipType.Longsword_Iron ||
				_playerEquip == PlayerEquipType.Longsword_Steel ||
				_playerEquip == PlayerEquipType.Longsword_Ebony)
			{
				if(_swordID != PlayerEquip.IsLongsword)
					_swordID = PlayerEquip.IsLongsword;
				_animState = AnimationState.IdleLongsword;
				return;
			}
		}

		public override void Awake()
		{		
			Initialize(this.GetComponent<Animator>(), AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBroadSword);
		}
		public override void Initialize(Animator animator, AnimatorController animController, AnimationState animationState)
		{
			SetupSword();
			base.Initialize(animator, animController, _animState);//animationState);
		}
		public override void Start()
		{
			base.Start();

			//SetupSword();
		}
		public override void Update()
		{
			base.Update();
		}

    }
}

