
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public class ContainerBehaviour : ItemBehaviour
    {
        private bool _ignoreGravity;
        private float _targetVelocityX, _targetVelocityY;
        private ITimer _timer;
        public override void Setup(Transform t, CharacterController2D controller2D, BoxCollider2D boxCollider2D) 
            //: base (Transform: t, CharacterController2D: controller2D, BoxCollider2D: boxCollider2D)
        {
            _boxCollider2D = boxCollider2D;
            _controller2D = controller2D;
            _transform = t;
            _velocity = new Vector3();
            _timer = this.gameObject.AddComponent<Timer>();
            _timer.Setup("Container Hit Behaviour Timer");
            _ignoreGravity = false;
            _targetVelocityX = 0f;
            _targetVelocityY = 0f;
            _gravity = wildlogicgames.DoomBreakers.GetGravity();
        }
        public void IsHit()
		{
            _targetVelocityX = 4f;// 10f;
            _targetVelocityY = 4f;//10f;
            //_ignoreGravity = true;
            //_timer.StartTimer(1.5f);
            _velocity.y -= (_gravity * Time.deltaTime) * _targetVelocityY;
            _velocity.x = Time.deltaTime *_targetVelocityY;
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

