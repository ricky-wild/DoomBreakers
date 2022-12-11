using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
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
			
			_animState = AnimationState.IdleBreastplate;

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBreastplate, _armorID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									   AnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			SetupArmor();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>(), this.GetComponent<BoxCollider2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);
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

