
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class ArmorObtainedFX : BaseFX
	{

		private Animator _animator;

		//public ArmorObtainedFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityY = 10.5f;
			_animator = this.GetComponent<Animator>();
			_animator.Play("armorGainedFX"); //FXController.controller
			_alphaFreq = 0.025f;
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
			_animator.Play("armorGainedFX");
			_velocity.y = (_targetVelocityY * Time.deltaTime);

		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();
	}
}
