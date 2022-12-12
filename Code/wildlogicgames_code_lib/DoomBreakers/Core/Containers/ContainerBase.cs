using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    //<summary>
    //We'll be using ContainerBase for Barrel, Crate, Chest, Tree as base for in game world containers that can be attacked 
    //and destroyed. The idea is these Containers will loot drop items upon destruction.
    //0=nothing, 1=goldcoins, 2=apples, 3=chicken, 4=fish
    //</summary>

    public enum ContainerLootType
	{
        Nothing = 0,
        Gold_Coins = 1,
        Apples = 2,
        Chicken = 3,
        Fish = 4
	};

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
        [Header("Loot Type")]
        [Tooltip("0=nothing, 1=goldcoins, 2=apples, 3=chicken, 4=fish")]
        public ContainerLootType _containerLootType;

        [Header("Health")]
        [Tooltip("Damage absorbed before exploding open.")]
        [Range((int)10, (int)50)]
        public int _containerHealth;

        protected ITimer _timer;
        protected ContainerBehaviour _containerBehaviour;
        protected ContainerAnimator _containerAnimator;
        public ContainerBase() { }

        public virtual void Initialize(SpriteRenderer spriteRenderer, Animator animator, ContainerAnimatorController animController)
        {

            _containerBehaviour = this.gameObject.AddComponent<ContainerBehaviour>();
            _containerBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

            //Resources/ContainerAnimControllers/Barrel/Barrel.controller
            _containerAnimator = new ContainerAnimator(animator, "ContainerAnimControllers", "Barrel", "Barrel");

            //_timer = new Timer();

        }
        public virtual void Awake()
        {
            //Initialize();
        }
        public virtual void Start()
        {
        }

        public virtual void Update() //=> _itemBehaviour.UpdateMovement();
		{
            //_containerBehaviour.UpdateMovement();

        }



        public void Destroy()
        {
            _containerBehaviour.DisableCollisions(); 
            this.gameObject.SetActive(false);
        }
    }
}
