using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{


    public class Sword : ItemBase, ISword
    {
		[Header("Sword ID")]
		[Tooltip("Set its base item type to appropriate.")]
		public PlayerItem _swordID;

		[Header("Sword Type")]
		[Tooltip("Set Item to any kind of sword.")]
		public EquipmentWeaponType _weaponType;

		[Header("Material Type")]
		[Tooltip("Set Item to any type of material.")]
		public EquipmentMaterialType _materialType;

		private ItemAnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _swordSprite;
		private double _damageValue;

		public EquipmentWeaponType GetSwordType() => _weaponType;	
		public EquipmentMaterialType GetMaterialType() => _materialType;
		public double Damage() => _damageValue;

		private void SetupSword()
		{
			//Ensure a sword type has been applied within the inspector.
			if (_weaponType == EquipmentWeaponType.None)
				_weaponType = EquipmentWeaponType.Broadsword;

			switch(_weaponType)
			{
				case EquipmentWeaponType.Broadsword:
					_swordID = PlayerItem.IsBroadsword;
					_animState = ItemAnimationState.IdleBroadSword;
					_damageValue = 0.065;
					break;
				case EquipmentWeaponType.Longsword:
					_swordID = PlayerItem.IsLongsword;
					_animState = ItemAnimationState.IdleLongsword;
					_damageValue = 0.07;
					break;
			}
			if (_materialType == EquipmentMaterialType.Bronze) _damageValue += 0.01;
			if (_materialType == EquipmentMaterialType.Iron) _damageValue += 0.02;
			if (_materialType == EquipmentMaterialType.Steel) _damageValue += 0.0275;
			if (_materialType == EquipmentMaterialType.Ebony) _damageValue += 0.0325;


		}

		public override void Awake()
		{		
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				PlayerAnimatorController.Weapon_equipment_to_pickup, ItemAnimationState.IdleBroadSword, _swordID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, PlayerAnimatorController animController,
									   ItemAnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupSword();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Equipment", "Weapon", _animState);
			_swordSprite = this.gameObject.AddComponent<SwordSprite>();
			_swordSprite.Setup(ref spriteRenderer, _itemID, itemType, _materialType);
		}
		public Sword(EquipmentWeaponType weaponType, EquipmentMaterialType materialType) //Constructor for equipment setup within code, not scene.
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

