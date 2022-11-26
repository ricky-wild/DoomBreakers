
using UnityEngine;

namespace DoomBreakers
{
	public class BanditPersue : BanditBaseState
	{

		public BanditPersue(EnemyStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.
			_cachedVector3 = new Vector3();
			_behaviourTimer = new Timer();
			print("\nPersue State.");
		}

		public override void IsPersueTarget(ref Animator animator)
		{
			animator.Play("Run");//, 0, 0.0f);

			int trackingDir = 0; //Face direction either -1 left, or 1 right.
			_cachedVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_banditID, EnemyAI.Bandit).position;

			if (_cachedVector3.x > _transform.position.x)
				trackingDir = 1;
			if (_cachedVector3.x < _transform.position.x)
				trackingDir = -1;


			if (trackingDir == -1)
			{
				if (_transform.position.x > _cachedVector3.x + 1.0f)
					_targetVelocityX = -(0.5f * (_moveSpeed * _sprintSpeed));
				else
					_stateMachine.SetState(new BanditQuickAttack(_stateMachine, _velocity, _banditID));
			}
			if (trackingDir == 1)
			{
				if (_transform.position.x < _cachedVector3.x - 1.0f)
					_targetVelocityX = 0.5f * (_moveSpeed * _sprintSpeed);
				else
					_stateMachine.SetState(new BanditQuickAttack(_stateMachine, _velocity, _banditID));
			}

			_velocity.x = _targetVelocityX;

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID));

			//base.UpdateBehaviour();
		}
	}
}

