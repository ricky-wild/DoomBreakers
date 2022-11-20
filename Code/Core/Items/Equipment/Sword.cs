using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class Sword : ItemBase
    {
		[Header("Sword ID")]
		[Tooltip("Set its base item type to appropriate.")]
		public PlayerItem _swordID;

		[Header("Sword Type")]
		[Tooltip("Set Item to any kind of sword.")]
		public PlayerEquipType _playerEquip;

		private AnimationState _animState; //Apply as appropriate based on _playerEquip.
		private IEquipmentSprite _swordSprite;

		public PlayerEquipType GetSwordType()
		{
			return _playerEquip;
		}
		private void SetupSword()
		{
			//Ensure a sword type has been applied within the inspector.
			if (_playerEquip == PlayerEquipType.Empty_None)
			{
				_playerEquip = PlayerEquipType.Broadsword_Bronze;
				_swordID = PlayerItem.IsBroadsword;
			    _animState = AnimationState.IdleBroadSword;
				return;
			}


			//Set the appropriate animation for the sword.
			if (_playerEquip == PlayerEquipType.Broadsword_Bronze ||
				_playerEquip == PlayerEquipType.Broadsword_Iron ||
				_playerEquip == PlayerEquipType.Broadsword_Steel ||
				_playerEquip == PlayerEquipType.Broadsword_Ebony)
			{
				if (_swordID != PlayerItem.IsBroadsword)
					_swordID = PlayerItem.IsBroadsword;
				_animState = AnimationState.IdleBroadSword;
				return;
			}
			if (_playerEquip == PlayerEquipType.Longsword_Bronze ||
				_playerEquip == PlayerEquipType.Longsword_Iron ||
				_playerEquip == PlayerEquipType.Longsword_Steel ||
				_playerEquip == PlayerEquipType.Longsword_Ebony)
			{
				if(_swordID != PlayerItem.IsLongsword)
					_swordID = PlayerItem.IsLongsword;
				_animState = AnimationState.IdleLongsword;
				return;
			}
		}

		public override void Awake()
		{		
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), 
				AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleBroadSword, _swordID, _playerEquip);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
									   AnimationState animationState, PlayerItem itemType, PlayerEquipType playerEquipType)
		{
			SetupSword();
			//base.Initialize(spriteRenderer, animator, animController, _animState, itemType, playerEquipType);

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
			_itemAnimator = new ItemAnimator(animator, animController, animationState);
			_swordSprite = this.gameObject.AddComponent<SwordSprite>();
			_swordSprite.Setup(ref spriteRenderer, _itemID, itemType, playerEquipType);
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

