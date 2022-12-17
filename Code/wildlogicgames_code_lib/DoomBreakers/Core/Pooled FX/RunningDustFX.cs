
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class RunningDustFX : BaseFX 
	{



		//public RunningDustFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityX = 10.5f;
			//_animator = this.GetComponent<Animator>();
			//_animator.Play(""); //FXController.controller
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
			if (_direction == 1) _velocity.x = -(_targetVelocityX * Time.deltaTime);
			if (_direction == -1) _velocity.x = (_targetVelocityX * Time.deltaTime);

		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();
	}
}
