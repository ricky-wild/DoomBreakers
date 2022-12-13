
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public class ContainerBehaviour : ItemBehaviour
    {

        private float _targetVelocityX, _targetVelocityY;

        public override void Setup(Transform t, CharacterController2D controller2D, BoxCollider2D boxCollider2D) 
            //: base (Transform: t, CharacterController2D: controller2D, BoxCollider2D: boxCollider2D)
        {
            _boxCollider2D = boxCollider2D;
            _controller2D = controller2D;
            _transform = t;
            _velocity = new Vector3();
            _targetVelocityX = 0f;
            _targetVelocityY = 0f;
            _gravity = wildlogicgames.DoomBreakers.GetGravity();
        }
        public void IsHit()
		{
            _bounceCount = 2;
            _bounceMax = 3;
            _targetVelocityX = _bounceMax + _bounceCount;
            _targetVelocityY = _bounceMax + _bounceCount;

            _velocity.y -= ((_gravity / 2) * Time.deltaTime) * _targetVelocityY;
            _velocity.x = ((_gravity / 4) * Time.deltaTime) * _targetVelocityY;
        }
        public void UpdateContainerMovement()
        {
            //base.UpdateMovement();

            //if(!_ignoreGravity) base.UpdateGravity();

            base.UpdateGravity();

            //if (_timer.HasTimerFinished(true))
            //    _ignoreGravity = false;

            base.UpdateTransform();
        }
        public override void DisableCollisions() => _boxCollider2D.enabled = false;
        void Start() { }

        void Update() { }
    }
}

