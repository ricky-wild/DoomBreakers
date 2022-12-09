using System;
using System.Collections.Generic;

using UnityEngine;

namespace DoomBreakers
{
	public class PlayerExhausted : BaseState, IPlayerIdle
	{
		ITimer _waitTimer;
		public PlayerExhausted(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			_waitTimer = new Timer();
		}

		public override void IsExhausted(ref Animator animator, ref IPlayerSprite playerSprite)
		{
			animator.Play("Tired");//, 0, 0.0f);
			_velocity.x = 0f;

			_waitTimer.StartTimer(3.0f);
			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
				playerSprite.SetBehaviourTextureFlash(0.25f, Color.white);

			if(_waitTimer.HasTimerFinished())
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}

