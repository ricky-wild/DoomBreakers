using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Controller2D))]
    public class ItemBase : MonoBehaviour, IItem
    {
        [Header("Item ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? items.
        public int _itemID;               //Set in editor per item or else where.

        IItemBehaviour _itemBehaviour;
        IItemAnimator _itemAnimator;
        ItemBase(){}
		private void Initialize()
		{
			//_itemBehaviour = new ItemBehaviour();
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<Controller2D>());
		}
		private void Awake()
		{
			Initialize();
		}
		void Start() 
        {
        }

        void Update() 
        {

        }
    }
}

