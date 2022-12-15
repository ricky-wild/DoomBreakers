
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerDying : BaseState//, IPlayerIdle
	{

		public PlayerDying(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			Time.timeScale = 0.5f;
		}

		public override void IsDying(ref Animator animator, ref IPlayerSprite playerSprite)
		{
			animator.Play("Dying");//, 0, 0.0f);
			_velocity.x = 0f;

			playerSprite.SetBehaviourTextureFlash(0.25f, Color.red);
			_behaviourTimer.StartTimer(1.0f);
			if (_behaviourTimer.HasTimerFinished())
			{
				Time.timeScale = 1.0f;
				_stateMachine.SetState(new PlayerDead(_stateMachine, _velocity));
			}

			//base.UpdateBehaviour();
		}
	}
}

