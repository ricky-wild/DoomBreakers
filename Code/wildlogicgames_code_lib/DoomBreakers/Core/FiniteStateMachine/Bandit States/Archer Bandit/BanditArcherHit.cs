
using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherHit : BasicEnemyBaseState
	{
		private int _randomStateDir;
		public BanditArcherHit(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_randomStateDir = 0;
			_behaviourTimer = new Timer();

			//print("\nHitByQuickAttack State.");
		}

		public override void IsHit(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Hit");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.5f, Color.red);

			_behaviourTimer.StartTimer(_quickAtkWaitTime*2);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();
				_targetVelocityX = 0f;
				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 70)
					_stateMachine.SetState(new BanditArcherAim(_stateMachine, _velocity, _enemyID));
				else
					_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));
			}

			//base.UpdateBehaviour();
		}
	}
}

