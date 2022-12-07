using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class Shield : ItemBase
	{
		[Header("Shield ID")]
		[Tooltip("Set its base item type to appropriate.")]
		public PlayerItem _shieldID;

		[Header("Shield Type")]
		[Tooltip("Set Item to any kind of shield.")]
		public EquipmentArmorType _shieldType;

		[Header("Material Type")]
		[Tooltip("Set Item to any type of material.")]
		public EquipmentMaterialType _materialType;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _shieldSprite;

		public EquipmentArmorType GetShieldType()
		{
			return _shieldType;
		}
		public EquipmentMaterialType GetMaterialType() //=> return _materialType;
		{
			return _materialType;
		}
		private void SetupShield()
		{
			//Ensure a shield type has been applied within the inspector.
			if (_shieldType != EquipmentArmorType.Shield)
				_shieldType = EquipmentArmorType.Shield;

			if (_shieldID != PlayerItem.IsShield)
				_shieldID = PlayerItem.IsShield;

			_animState = AnimationState.IdleShield;

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleShield, _shieldID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									    AnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupShield();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);
			_shieldSprite = this.gameObject.AddComponent<ShieldSprite>();
			_shieldSprite.Setup(ref spriteRenderer, _itemID, itemType, equipMaterialType);
		}
		public Shield(EquipmentArmorType shieldType, EquipmentMaterialType materialType) //Constructor for equipment setup within code, not scene.
		{
			_shieldType = shieldType;
			_materialType = materialType;
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

