using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Breastplate : ItemBase
	{
		[Header("Armor ID")]
		[Tooltip("Set its base item type to appropriate.")]
		public PlayerItem _armorID;

		[Header("Armor Type")]
		[Tooltip("Set Item to any kind of armor.")]
		public EquipmentArmorType _armorType;

		[Header("Material Type")]
		[Tooltip("Set Item to any type of material.")]
		public EquipmentMaterialType _materialType;

		private ItemAnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _breastplateSprite;

		public EquipmentArmorType GetArmorType()
		{
			return _armorType;
		}
		public EquipmentMaterialType GetMaterialType() //=> return _materialType;
		{
			return _materialType;
		}

		private void SetupArmor()
		{
			//Ensure a armor type has been applied within the inspector.
			if (_armorType != EquipmentArmorType.Breastplate)
				_armorType = EquipmentArmorType.Breastplate;

			if (_armorID != PlayerItem.IsBreastPlate)
				_armorID = PlayerItem.IsBreastPlate;
			
			_animState = ItemAnimationState.IdleBreastplate;

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				PlayerAnimatorController.Weapon_equipment_to_pickup, ItemAnimationState.IdleBreastplate, _armorID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, PlayerAnimatorController animController,
									   ItemAnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupArmor();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Equipment", "Weapon", animationState);
			_breastplateSprite = this.gameObject.AddComponent<BreastplateSprite>();
			_breastplateSprite.Setup(ref spriteRenderer, _itemID, itemType, equipMaterialType);
		}
		public Breastplate(EquipmentArmorType armorType, EquipmentMaterialType materialType) //Constructor for equipment setup within code, not scene.
		{
			_armorType = armorType;
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

