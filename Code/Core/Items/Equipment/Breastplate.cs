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
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _breastplateSprite;

		public PlayerEquipType GetArmorType()
		{
			return _playerEquip;
		}
		private void SetupArmor()
		{
			//Ensure a armor type has been applied within the inspector.
			if (_playerEquip == PlayerEquipType.Empty_None)
			{
				_playerEquip = PlayerEquipType.BreastPlate_Bronze;
				_armorID = PlayerItem.IsBreastPlate;
				_animState = AnimationState.IdleBreastplate;
				return;
			}


			//Set the appropriate animation for the armor.
			if (_playerEquip == PlayerEquipType.BreastPlate_Bronze ||
				_playerEquip == PlayerEquipType.BreastPlate_Iron ||
				_playerEquip == PlayerEquipType.BreastPlate_Steel ||
				_playerEquip == PlayerEquipType.BreastPlate_Ebony)
			{
				if (_armorID != PlayerItem.IsBreastPlate)
					_armorID = PlayerItem.IsBreastPlate;
				_animState = AnimationState.IdleBreastplate;
				return;
			}

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBreastplate, _armorID, _playerEquip);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									   AnimationState animationState, PlayerItem itemType, PlayerEquipType playerEquipType)
		{
			SetupArmor();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);
			_breastplateSprite = this.gameObject.AddComponent<BreastplateSprite>();
			_breastplateSprite.Setup(ref spriteRenderer, _itemID, itemType, playerEquipType);
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

