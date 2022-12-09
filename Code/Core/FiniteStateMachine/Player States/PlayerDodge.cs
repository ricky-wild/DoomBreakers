using UnityEngine;

namespace DoomBreakers
{
	public class PlayerDodge : BaseState, IPlayerDodge
	{
		private bool _initialDirFlag;
		public PlayerDodge(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			_initialDirFlag = false;
			//print("\nDodge State.");
		}

		public override void IsDodging(ref Animator animator, ref Controller2D controller2D, ref Vector2 input,
										 bool dodgeLeft, ref IPlayerSprite playerSprite, ref IPlayerCollision playerCollider)
		{
			animator.Play("Dodge");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));

			_behaviourTimer.StartTimer(1.4f / 3);//animation time.

			int faceDir = playerSprite.GetSpriteDirection();

			if(!_initialDirFlag)
			{
				_dodgedLeftFlag = dodgeLeft;

				if (dodgeLeft)
				{
					if (faceDir == -1)
					{
						playerSprite.FlipSprite();
						playerCollider.FlipAttackPoints(1);
					}
				}
				if (!dodgeLeft)
				{
					if (faceDir == 1)
					{
						playerSprite.FlipSprite();
						playerCollider.FlipAttackPoints(-1);
					}
				}
				_initialDirFlag = true;
			}


			playerSprite.SetBehaviourTextureFlash(0.5f, Color.white);

			if (_behaviourTimer.HasTimerFinished())
			{
				
				_stateMachine.SetState(new PlayerDodged(_stateMachine, _velocity, _dodgedLeftFlag));
			}
			//base.UpdateBehaviour();
		}
	}
}