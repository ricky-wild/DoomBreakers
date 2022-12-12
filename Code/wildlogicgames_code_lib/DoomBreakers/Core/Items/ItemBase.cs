using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    //<summary>
    //We'll be using ItemBase for Sword, Shield, Armor as base for in game world pickup items as they
    //have all the same behaviours, bar, animations and what they represent. 
    //This will also be used for Health items that are also picked up. Such as Apples, Chicken & Fish.
    //</summary>

    public enum EquipmentMaterialType
    {
        None = -1,
        Bronze = 0,
        Iron = 1,
        Steel = 2,
        Ebony = 3
    };
    public enum EquipmentWeaponType
	{
        None = -1,
        Broadsword = 0,
        Longsword = 1
	};
    public enum EquipmentArmorType
	{
        None = -1,
        Breastplate = 0,
        Shield = 1
	}


    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterController2D))]
    public class ItemBase : MonoBehaviour//, IItem
    {
        [Header("Item ID")]
        [Tooltip("ID ranges from 0 to ?")]   //Max ? items.
        public int _itemID;                  //Set in editor per item or else where.

        protected ItemBehaviour _itemBehaviour;
        protected ItemAnimator _itemAnimator;

        public ItemBase(){}
		public virtual void Initialize(SpriteRenderer spriteRenderer, Animator animator, PlayerAnimatorController animController,
                                       ItemAnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
		{

			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

            //Resources/ItemAnimControllers/Equipment/Weapon.controller
            _itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Equipment", "Weapon", animController, animationState);


        }
        public virtual void Awake()
		{
			//Initialize();
		}
        public virtual void Start() 
        {
        }

        public virtual void Update() 
        {
            _itemAnimator.UpdateAnimator();
            _itemBehaviour.UpdateMovement();
        }

        

        public void Destroy()
		{
            _itemBehaviour.DisableCollisions(); //Especially important for player Equip Indicator usage.
            this.gameObject.SetActive(false);
		}
    }
}

