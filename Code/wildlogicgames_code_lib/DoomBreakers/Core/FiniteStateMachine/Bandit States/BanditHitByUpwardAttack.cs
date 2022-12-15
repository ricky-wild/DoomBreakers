
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByUpwardAttack : BanditBaseState
	{

		public BanditHitByUpwardAttack(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_maxPowerStruckVelocityY = 8.0f; //10.0f for lowest impact. 14.0f for average. 16.0f for maximum impact.
											  
		}

		public override void IsHitByUpwardAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Hit");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.5f, Color.red);

			if (_velocity.y >= _maxPowerStruckVelocityY)//Near peak of jump velocity, set falling state.
			{
				banditSprite.SetBehaviourTextureFlash(0.1f, Color.red);
				//_velocity.x = 0f;
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID, false));
				return;
			}
			else
			{
				float multiplier = 1.2f;// 1.66f;
				_velocity.y += _maxPowerStruckVelocityY + multiplier / 12;
			}

			//base.UpdateBehaviour();
		}
	}
}

