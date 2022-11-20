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
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _shieldSprite;

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
				_shieldID = PlayerItem.IsShield;
				_animState = AnimationState.IdleShield;
				return;
			}


			//Set the appropriate animation for the shield.
			if (_playerEquip == PlayerEquipType.Shield_Bronze ||
				_playerEquip == PlayerEquipType.Shield_Iron ||
				_playerEquip == PlayerEquipType.Shield_Steel ||
				_playerEquip == PlayerEquipType.Shield_Ebony)
			{
				if (_shieldID != PlayerItem.IsShield)
					_shieldID = PlayerItem.IsShield;
				_animState = AnimationState.IdleShield;
				return;
			}

		}
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleShield, _shieldID, _playerEquip);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									    AnimationState animationState, PlayerItem itemType, PlayerEquipType playerEquipType)
		{
			SetupShield();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);
			_shieldSprite = this.gameObject.AddComponent<ShieldSprite>();
			_shieldSprite.Setup(ref spriteRenderer, _itemID, itemType, playerEquipType);
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

