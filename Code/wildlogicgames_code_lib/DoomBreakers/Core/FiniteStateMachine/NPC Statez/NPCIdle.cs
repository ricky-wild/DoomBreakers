using UnityEngine;

namespace DoomBreakers
{
	public class NPCIdle : BasicNPCBaseState
	{
		public NPCIdle(NPCStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, npcId: id)//=> _stateMachine = s; 
		{
			_npcID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 3.33f;
			_behaviourTimer = new Timer();
			//print("\nIdle State.");
		}

		public override void IsIdle(ref NPCAnimator animator)
		{
			//animator.Play("Idle");//, 0, 0.0f);
			animator.PlayAnimation((int)NPCAnimID.Idle);
			_velocity.x = 0f;


			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished()) _stateMachine.SetState(new NPCTravelling(_stateMachine, _velocity, _transform, _npcID));
		}


	}
}
