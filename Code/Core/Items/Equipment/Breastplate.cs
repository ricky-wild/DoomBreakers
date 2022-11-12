using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class Breastplate : ItemBase
	{
		[Header("Armor ID")]
		[Tooltip("Set its base type")]
		public PlayerEquip _armorID;

		[Header("Armor Type")]
		[Tooltip("Set Item to any kind of armor.")]
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.

		public PlayerEquipType GetArmorType()
		{
			return _playerEquip;
		}
		private void SetupArmor()
		{
			//Ensure a armor type has been applied within the inspector.
			if (_playerEquip == PlayerEquipType.Empty_None ||
				_playerEquip != PlayerEquipType.BreastPlate_Bronze ||
				_playerEquip != PlayerEquipType.BreastPlate_Iron ||
				_playerEquip != PlayerEquipType.BreastPlate_Steel ||
				_playerEquip != PlayerEquipType.BreastPlate_Ebony)
			{
				_playerEquip = PlayerEquipType.BreastPlate_Bronze;
				_armorID = PlayerEquip.IsBreastPlate;
				_animState = AnimationState.IdleBreastplate;
				return;
			}


			//Set the appropriate animation for the armor.
			if (_playerEquip == PlayerEquipType.BreastPlate_Bronze ||
				_playerEquip == PlayerEquipType.BreastPlate_Iron ||
				_playerEquip == PlayerEquipType.BreastPlate_Steel ||
				_playerEquip == PlayerEquipType.BreastPlate_Ebony)
			{
				if (_armorID != PlayerEquip.IsBreastPlate)
					_armorID = PlayerEquip.IsBreastPlate;
				_animState = AnimationState.IdleBreastplate;
				return;
			}

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<Animator>(), AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBreastplate);
		}
		public override void Initialize(Animator animator, AnimatorController animController, AnimationState animationState)
		{
			SetupArmor();
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

