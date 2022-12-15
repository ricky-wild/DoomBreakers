
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerDead : BaseState//, IPlayerIdle
	{

		public PlayerDead(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
		}

		public override void IsDead(ref Animator animator, ref IPlayerSprite playerSprite)
		{
			animator.Play("Dead");//, 0, 0.0f);
			_velocity.x = 0f;


			_behaviourTimer.StartTimer(1.0f);
			if (_behaviourTimer.HasTimerFinished()) playerSprite.SetBehaviourTextureFlash(0.25f, Color.red);

			//base.UpdateBehaviour();
		}
	}
}

