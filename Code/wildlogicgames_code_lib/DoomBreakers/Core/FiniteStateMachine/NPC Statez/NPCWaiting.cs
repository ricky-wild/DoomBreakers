using UnityEngine;

namespace DoomBreakers
{
	public class NPCWaiting : BasicNPCBaseState
	{
		public NPCWaiting(NPCStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, npcId: id)//=> _stateMachine = s; 
		{
			_npcID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 6.66f;
			_behaviourTimer = new Timer();
			//print("\nIdle State.");
		}

		public override void IsWaiting(ref NPCAnimator animator, ref NPCSprite npcSprite)
		{
			//animator.Play("Idle");//, 0, 0.0f);
			animator.PlayAnimation((int)NPCAnimID.Wait);
			_velocity.x = 0f;


			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				npcSprite.SetBehaviourTextureFlash(0.25f, Color.green);
			}
		}
	}
}
