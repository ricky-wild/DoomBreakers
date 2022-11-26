
using UnityEngine;

namespace DoomBreakers
{
	public class BanditFall : BanditBaseState
	{

		public BanditFall(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			print("\nFall State.");
		}

		public override void IsFalling(ref Animator animator, ref Controller2D controller2D)
		{
			if (_velocity.y <= _maxJumpVelocity)
				animator.Play("Fall");
			_velocity.x = (_moveSpeed * _sprintSpeed);
			if (controller2D.collisions.below) //Means we're finished jumping/falling.
			{
				_velocity.x = 0f;
				_velocity.y = 0f;
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity,_banditID));
			}
			//base.UpdateBehaviour();
		}
	}
}

