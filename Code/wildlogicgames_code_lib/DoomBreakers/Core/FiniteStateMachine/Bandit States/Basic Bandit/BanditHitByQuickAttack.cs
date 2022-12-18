

using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByQuickAttack : BasicEnemyBaseState
	{
		private int _randomStateDir;
		public BanditHitByQuickAttack(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_randomStateDir = 0;
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			//print("\nHitByQuickAttack State.");
		}

		public override void IsHitByQuickAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Jabbed");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.5f, Color.red);

			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();
				_targetVelocityX = 0f;
				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if(_randomStateDir < 60)
					_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _enemyID));
				else
					_stateMachine.SetState(new BanditDefending(_stateMachine, _velocity, _enemyID));
			}

			//base.UpdateBehaviour();
		}
	}
}

