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
    [RequireComponent(typeof(Controller2D))]
    public class ContainerBase : MonoBehaviour//, Character2DBaseAnimator
    {
        private ContainerAnimator _containerAnimator;
        public ContainerBase() { }

        public virtual void Initialize(SpriteRenderer spriteRenderer, Animator animator, ContainerAnimatorController animController, AnimationState animationState)
        {

            //_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
            //_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

            _containerAnimator = new ContainerAnimator(animator, "", "", "");

        }
    }
}
