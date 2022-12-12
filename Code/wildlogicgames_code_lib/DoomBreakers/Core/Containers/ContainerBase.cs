using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    //<summary>
    //We'll be using ContainerBase for Barrel, Crate, Chest, Tree as base for in game world containers that can be attacked 
    //and destroyed. The idea is these Containers will loot drop items upon destruction.
    //
    //</summary>

    public enum ContainerAnimatorController
    {
        Container_Barrel = 0,
        Container_Crate = 1,
        Container_Chest = 2,
        Container_Tree = 3

    };

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterController2D))]
    public class ContainerBase : MonoBehaviour//, Character2DBaseAnimator
    {
        protected ItemBehaviour _itemBehaviour;
        protected ContainerAnimator _containerAnimator;
        public ContainerBase() { }

        public virtual void Initialize(SpriteRenderer spriteRenderer, Animator animator, ContainerAnimatorController animController)
        {

            _itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
            _itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

            //Resources/ContainerAnimControllers/Barrel/Barrel.controller
            _containerAnimator = new ContainerAnimator(animator, "ContainerAnimControllers", "Barrel", "Barrel");

        }
        public virtual void Awake()
        {
            //Initialize();
        }
        public virtual void Start()
        {
        }

        public virtual void Update() => _itemBehaviour.UpdateMovement();



        public void Destroy()
        {
            _itemBehaviour.DisableCollisions(); //Especially important for player Equip Indicator usage.
            this.gameObject.SetActive(false);
        }
    }
}
