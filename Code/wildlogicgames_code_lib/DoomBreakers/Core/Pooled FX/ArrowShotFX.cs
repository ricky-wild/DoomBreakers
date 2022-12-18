
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

	public class ArrowShotFX : BaseFX
	{


		private int _speed;
		//public ArrowShotFX(int direction) => Setup(direction);
		public override void Setup(int direction)
		{
			base.Setup(direction);
			_targetVelocityX = 0.0f;
			_applyAlphaFadeFlag = false;
			_speed = wildlogicgames.Utilities.GetRandomNumberInt(16, 22); //20 is the ideal value.
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

			_targetVelocityX += _speed * Time.deltaTime;
			_velocity.x = _targetVelocityX;
		}
		public override void FlipSprite() => base.FlipSprite();
		public override void ApplyTransparency() => base.ApplyTransparency();

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player")) ProcessCollisionWithPlayer(0);
			if (collision.CompareTag("Player2")) ProcessCollisionWithPlayer(1);
			if (collision.CompareTag("Player3")) ProcessCollisionWithPlayer(2);
			if (collision.CompareTag("Player4")) ProcessCollisionWithPlayer(3);
		}

		private void ProcessCollisionWithPlayer(int playerId)
		{
			if (_applyAlphaFadeFlag) return;

			BattleColliderManager.TriggerEvent("ReportCollisionWithArrowForPlayer" + playerId.ToString());
			base._applyAlphaFadeFlag = true;
			_alphaFreq = 0.025f;
			_speed = 2;
			_targetVelocityX = 1;
		}
	}
}
