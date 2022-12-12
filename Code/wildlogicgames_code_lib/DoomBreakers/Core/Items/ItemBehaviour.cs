using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public class ItemBehaviour : MonoBehaviour//, IItemBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private CharacterController2D _controller2D;
        private Vector3 _velocity;
        private Transform _transform;
        private float _gravity;

        public void Setup(Transform t, CharacterController2D controller2D, BoxCollider2D boxCollider2D)
		{
            _boxCollider2D = boxCollider2D;
            _controller2D = controller2D;
            _transform = t;
            _velocity = new Vector3();
            _gravity = wildlogicgames.DoomBreakers.GetGravity();
        }
        public void UpdateTransform()
		{
            _controller2D.UpdateMovement(_velocity * Time.deltaTime, Vector2.zero, false);
        }
        public void UpdateGravity()
		{
            bool collisionBelow = _controller2D._collisionDetail._collidedDirection[0];

            if (!collisionBelow)
                _velocity.y += _gravity * Time.deltaTime;
            if (collisionBelow)
                _velocity.y = 0f;
        }
        public void UpdateMovement()
		{

            UpdateGravity();
            UpdateTransform();
        }
        public void DisableCollisions() => _boxCollider2D.enabled = false;
        void Start() { }

        void Update() { }
    }
}

