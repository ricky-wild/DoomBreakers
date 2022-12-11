using System.Collections;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public interface IItemBehaviour //: MonoBehaviour
    {

        void Setup(Transform t, CharacterController2D controller2D, BoxCollider2D boxCollider2D);
        void UpdateTransform();
        void UpdateMovement();
        void UpdateGravity();
        void DisableCollisions();

        //void Start() { }

        //void Update() { }
    }
}

