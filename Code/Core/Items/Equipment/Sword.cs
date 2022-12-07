using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _swordSprite;
		private double _damage;

		public EquipmentWeaponType GetSwordType() => _weaponType;	
		public EquipmentMaterialType GetMaterialType() => _materialType;
		public double Damage() => _damage;

		private void SetupSword()
		{
			//Ensure a sword type has been applied within the inspector.
			if (_weaponType == EquipmentWeaponType.None)
				_weaponType = EquipmentWeaponType.Broadsword;

			switch(_weaponType)
			{
				case EquipmentWeaponType.Broadsword:
					_swordID = PlayerItem.IsBroadsword;
					_animState = AnimationState.IdleBroadSword;
					_damage = 0.065;
					break;
				case EquipmentWeaponType.Longsword:
					_swordID = PlayerItem.IsLongsword;
					_animState = AnimationState.IdleLongsword;
					_damage = 0.07;
					break;
			}
			if (_materialType == EquipmentMaterialType.Bronze) _damage += 0.01;
			if (_materialType == EquipmentMaterialType.Iron) _damage += 0.02;
			if (_materialType == EquipmentMaterialType.Steel) _damage += 0.0275;
			if (_materialType == EquipmentMaterialType.Ebony) _damage += 0.0325;


		}

		public override void Awake()
		{		
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBroadSword, _swordID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									   AnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupSword();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, animController, _animState);//animationState);
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

