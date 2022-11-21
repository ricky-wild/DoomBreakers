using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{


	public class EmptyHand : ItemBase
	{

		private PlayerItem _emptyHandID;
		private EquipmentWeaponType _weaponType;
		private EquipmentMaterialType _materialType;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _emptySprite;

		public EquipmentWeaponType GetSwordType()
		{
			return _weaponType;
		}
		public EquipmentMaterialType GetMaterialType() //=> return _materialType;
		{
			return _materialType;
		}

		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(),
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.Empty, _emptyHandID, _materialType);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									   AnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{
			_weaponType = EquipmentWeaponType.None;
			_materialType = EquipmentMaterialType.None;
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);

		}
		public EmptyHand(EquipmentWeaponType weaponType, EquipmentMaterialType materialType) //Constructor for equipment setup within code, not scene.
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

