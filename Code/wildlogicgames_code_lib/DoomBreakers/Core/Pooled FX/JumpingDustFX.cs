
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class JumpingDustFX : BaseFX
	{

		private Animator _animator;

		//public JumpingDustFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityY = 10.5f;
			_animator = this.GetComponent<Animator>();
			_animator.Play("dustCloudJumpFX");
			_alphaFreq = 0.05f;
		}
		public override void SetDirection(int dir) => base.SetDirection(dir);
		void Awake() { }
		void Start() { }


		public override void Update()
		{
			_velocity.y = (_targetVelocityY * Time.deltaTime);
			base.Update();
		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();
	}
}
