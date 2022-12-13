using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public class ItemBehaviour : MonoBehaviour//, IItemBehaviour
    {
        protected BoxCollider2D _boxCollider2D;
        protected CharacterController2D _controller2D;
        protected Vector3 _velocity;
        protected Transform _transform;
        protected float _gravity;
        private float _targetVelocityY;
        protected int _bounceMax = 5;
        protected int _bounceCount;
        public virtual void Setup(Transform t, CharacterController2D controller2D, BoxCollider2D boxCollider2D)
		{
            _boxCollider2D = boxCollider2D;
            _controller2D = controller2D;
            _transform = t;
            _velocity = new Vector3();
            _gravity = wildlogicgames.DoomBreakers.GetGravity();
            _bounceCount = 3;
        }
        private void IsBounceOffGround()
        {
            if (_bounceCount <= 0) return;

            _targetVelocityY = _bounceMax + _bounceCount;
            _velocity.y -= ((_gravity/2) * Time.deltaTime) * _targetVelocityY;
            _bounceCount--;
        }
        public virtual void UpdateTransform()
		{
            _controller2D.UpdateMovement(_velocity * Time.deltaTime, Vector2.zero, false);
        }
        public virtual void UpdateGravity()
		{
            bool collisionBelow = _controller2D._collisionDetail._collidedDirection[0];

            if (!collisionBelow)
                _velocity.y += _gravity * Time.deltaTime;
            if (collisionBelow)
			{
                _velocity.y = 0f;
                _velocity.x = 0f;
                IsBounceOffGround();
            }
        }
        public virtual void UpdateMovement()
		{

            UpdateGravity();
            UpdateTransform();
        }
        public virtual void DisableCollisions() => _boxCollider2D.enabled = false;
        void Start() { }

        void Update() { }
    }
}

