using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{


	public class Mace : ItemBase
	{
		[Header("Mace ID")]
		[Tooltip("Set its base item type to appropriate.")]
		public PlayerItem _maceID;

		[Header("Mace Type")]
		[Tooltip("Set Item to any kind of sword.")]
		public EquipmentWeaponType _weaponType;

		[Header("Material Type")]
		[Tooltip("Set Item to any type of material.")]
		public EquipmentMaterialType _materialType;

		private ItemAnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _maceSprite;
		private double _damageValue;

		public EquipmentWeaponType GetMaceType() => _weaponType;
		public EquipmentMaterialType GetMaterialType() => _materialType;
		public double Damage() => _damageValue;

		private void SetupSword()
		{
			//Ensure a sword type has been applied within the inspector.
			if (_weaponType == EquipmentWeaponType.None)
				_weaponType = EquipmentWeaponType.MorningstarMace;

			switch (_weaponType)
			{
				case EquipmentWeaponType.MorningstarMace:
					_maceID = PlayerItem.IsMace;
					_animState = ItemAnimationState.IdleMace;
					_damageValue = 0.05;
					break;
			}
			if (_materialType == EquipmentMaterialType.Bronze) _damageValue += 0.01;
			if (_materialType == EquipmentMaterialType.Iron) _damageValue += 0.02;
			if (_materialType == EquipmentMaterialType.Steel) _damageValue += 0.03;
			if (_materialType == EquipmentMaterialType.Ebony) _damageValue += 0.04;


		}

		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(),
				PlayerAnimatorController.Weapon_equipment_to_pickup, ItemAnimationState.IdleMace, _maceID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, PlayerAnimatorController animController,
									   ItemAnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupSword();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Equipment", "Weapon", _animState);
			_maceSprite = this.gameObject.AddComponent<SwordSprite>();
			_maceSprite.Setup(ref spriteRenderer, _itemID, itemType, _materialType);
		}
		public Mace(EquipmentWeaponType weaponType, EquipmentMaterialType materialType) //Constructor for equipment setup within code, not scene.
		{
			_weaponType = weaponType;
			_materialType = materialType;
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

