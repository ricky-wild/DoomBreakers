using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class ItemBehaviour : MonoBehaviour, IItemBehaviour
    {
        private Controller2D _controller2D;
        private Vector3 _velocity;
        private Transform _transform;
        private float _gravity;

        public void Setup(Transform t, Controller2D controller2D)
		{
            _controller2D = controller2D;
            _transform = t;
            _velocity = new Vector3();
            _gravity = wildlogicgames.DoomBreakers.GetGravity();
        }
        public void UpdateTransform()
		{
            _controller2D.Move(_velocity * Time.deltaTime, _velocity);
        }
        public void UpdateGravity()
		{
            if (!_controller2D.collisions.below)
                _velocity.y += _gravity * Time.deltaTime;
            if (_controller2D.collisions.below)
                _velocity.y = 0f;
        }
        public void UpdateMovement()
		{

            UpdateGravity();
            UpdateTransform();
        }
        void Start() { }

        void Update() { }
    }
}

