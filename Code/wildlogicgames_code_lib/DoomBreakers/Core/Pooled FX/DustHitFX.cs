

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class DustHitFX : BaseFX
	{

		private Animator _animator;

		//public DustHitFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityX = 15.5f;
			_targetVelocityY = 15.5f;
			_animator = this.GetComponent<Animator>();
			_animator.Play("hitCloudFX"); //FXController.controller
			_alphaFreq = 0.05f;
		}
		public override void SetDirection(int dir) //=> base.SetDirection(dir);
		{
			base.SetDirection(dir);
			this.Setup(dir);
		}
		void Awake() { }
		void Start() { }


		public override void Update()
		{
			base.Update();
			_animator.Play("hitCloudFX");
			if (_direction == 1) _velocity.x = -(_targetVelocityX * Time.deltaTime);
			if (_direction == -1) _velocity.x = (_targetVelocityX * Time.deltaTime);
			_velocity.y = (_targetVelocityY * Time.deltaTime);

		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();
	}
}
