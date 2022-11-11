using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    //<summary>
    //We'll be using ItemBase for Sword, Shield, Armor as base for in game world pickup items as they
    //have all the same behaviours, bar, animations and what they represent. 
    //This will also be used for Health items that are also picked up. Such as Apples, Chicken & Fish.
    //</summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Controller2D))]
    public class ItemBase : MonoBehaviour, IItem
    {
        [Header("Item ID")]
        [Tooltip("ID ranges from 0 to ?")]   //Max ? items.
        public int _itemID;                  //Set in editor per item or else where.

        IItemBehaviour _itemBehaviour;
        IItemAnimator _itemAnimator;
        public ItemBase(){}
		public virtual void Initialize(Animator animator, AnimatorController animController, AnimationState animationState)
		{
			//_itemBehaviour = new ItemBehaviour();
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
            _itemAnimator = new ItemAnimator(animator, animController, animationState);
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
    }
}

