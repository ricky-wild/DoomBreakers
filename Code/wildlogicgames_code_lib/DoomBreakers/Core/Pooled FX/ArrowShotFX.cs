
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class ArrowShotFX : BaseFX
	{



		//public ArrowShotFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityX = 50.0f;
			_applyAlphaFadeFlag = false;
			_animator = this.GetComponent<Animator>();
			_animator.Play("arrowShotFX"); //FXController.controller
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

			_animator.Play("arrowShotFX"); //FXController.controller

			_targetVelocityX = _targetVelocityX * Time.deltaTime;
			_velocity.x = (_targetVelocityX * Time.deltaTime);
		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();
	}
}
