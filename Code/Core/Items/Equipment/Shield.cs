using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class Shield : ItemBase
	{
		[Header("Shield ID")]
		[Tooltip("Set its base type")]
		public PlayerEquip _shieldID;

		[Header("Shield Type")]
		[Tooltip("Set Item to any kind of shield.")]
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.

		public PlayerEquipType GetShieldType()
		{
			return _playerEquip;
		}
		private void SetupShield()
		{
			//Ensure a shield type has been applied within the inspector.
			if (_playerEquip == PlayerEquipType.Empty_None ||
				_playerEquip != PlayerEquipType.Shield_Bronze ||
				_playerEquip != PlayerEquipType.Shield_Iron ||
				_playerEquip != PlayerEquipType.Shield_Steel ||
				_playerEquip != PlayerEquipType.Shield_Ebony)
			{
				_playerEquip = PlayerEquipType.Shield_Bronze;
				_shieldID = PlayerEquip.IsShield;
				_animState = AnimationState.IdleShield;
				return;
			}


			//Set the appropriate animation for the shield.
			if (_playerEquip == PlayerEquipType.Shield_Bronze ||
				_playerEquip == PlayerEquipType.Shield_Iron ||
				_playerEquip == PlayerEquipType.Shield_Steel ||
				_playerEquip == PlayerEquipType.Shield_Ebony)
			{
				if (_shieldID != PlayerEquip.IsShield)
					_shieldID = PlayerEquip.IsShield;
				_animState = AnimationState.IdleShield;
				return;
			}

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<Animator>(), AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleShield);
		}
		public override void Initialize(Animator animator, AnimatorController animController, AnimationState animationState)
		{
			SetupShield();
			base.Initialize(animator, animController, _animState);
		}
		public override void Start()
		{
			base.Start();
		}
		public override void Update()
		{
			base.Update();
		}

	}
}

