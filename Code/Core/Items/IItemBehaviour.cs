using System.Collections;
using UnityEngine;

namespace DoomBreakers
{
    public interface IItemBehaviour //: MonoBehaviour
    {

        void Setup(Transform t, Controller2D controller2D);
        void UpdateTransform();
        void UpdateMovement();
        void UpdateGravity();

        //void Start() { }

        //void Update() { }
    }
}

