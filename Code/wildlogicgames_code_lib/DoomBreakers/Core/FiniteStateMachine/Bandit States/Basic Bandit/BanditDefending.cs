﻿
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class BanditDefending : BasicEnemyBaseState
	{

		public BanditDefending(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = wildlogicgames.Utilities.GetRandomNumberInt(1, 4);
			_behaviourTimer = new Timer();
			//print("\nFall State.");
		}

		public override void IsDefending(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite)
		{
			animator.Play("Defend");
			_velocity.x = 0f;

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _enemyID));
			}
			//base.UpdateBehaviour();
		}
	}
}

