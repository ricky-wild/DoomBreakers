
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerHitByPowerAttack : BaseState//, IPlayerJump
	{

		private int _banditFaceDir;
		public PlayerHitByPowerAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			_banditFaceDir = BattleColliderManager.GetAssignedBanditFaceDir(BattleColliderManager.GetRecentCollidedBanditId());
			_maxPowerStruckVelocityY = 11.25f;
			_maxPowerStruckVelocityX = 1.15f;
		}
		public override void IsHitByReleaseAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Hit");//, -1, 0.0f);


			playerSprite.SetBehaviourTextureFlash(0.1f, Color.red);

			float multiplier = 1.2f;// 1.66f;

			if (_velocity.y >= _maxPowerStruckVelocityY)//Near peak of jump velocity, set falling state.
			{
				playerSprite.SetBehaviourTextureFlash(0.25f, Color.red);
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY + multiplier / 6;
				_targetVelocityX = _maxPowerStruckVelocityX * (1.25f + multiplier); //Carries on over to FallProcess() where appropriate.

				int playerFaceDir = playerSprite.GetSpriteDirection();

				if (_banditFaceDir == 1 && playerFaceDir == -1) //Enemy facing right & player facing left, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
				if (_banditFaceDir == -1 && playerFaceDir == 1) //Enemy facing left & player facing right, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (_banditFaceDir == 1 && playerFaceDir == 1) //Enemy facing right & player facing right behind enemy, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (_banditFaceDir == -1 && playerFaceDir == -1) //Enemy facing left & player facing left behind enemy, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
			}


			//base.UpdateBehaviour();
		}
	}
}