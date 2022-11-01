using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class BanditBehaviours : MonoBehaviour, IBanditBehaviours
    {

        private Controller2D _controller2D;
        private Vector3 _velocity;
        private Transform _transform;
        private float _targetVelocityX, _maxJumpVelocity, _moveSpeed, _sprintSpeed, _gravity;
        private int _quickAttackIncrement; //2+ variations of this animation.

		private IPlayerStateMachine _playerStateMachineRef;

        private ITimer _behaviourTimer, _spriteColourSwapTimer;

        public void Setup(Transform t, Controller2D controller2D)
        {
			_controller2D = controller2D;
			_transform = t;
			_velocity = new Vector3();
			_moveSpeed = 3.5f;//3.75f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_maxJumpVelocity = 14.0f;//13.25f;
			_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
			_quickAttackIncrement = 0;
			//_jumpedFlag = false;

			//_behaviourTimer = new Timer();
			_behaviourTimer = this.gameObject.AddComponent<Timer>();
			_behaviourTimer.Setup();
			_spriteColourSwapTimer = this.gameObject.AddComponent<Timer>();
			_spriteColourSwapTimer.Setup();
		}
        public int GetQuickAttackIndex()
		{
			return _quickAttackIncrement;
		}
		private void Awake()
		{
		}
		void Start()
		{
		}
		public void IdleProcess(IEnemyStateMachine enemyStateMachine)
		{
			_velocity.x = 0f;
			//_velocity.y = 0f;
			//print("\n_velocity.y=" + Mathf.Abs(_velocity.y));
			if (Mathf.Abs(_velocity.y) >= 3.0f)
				enemyStateMachine.SetEnemyState(state.IsFalling);
		}
		public bool JumpProcess(IEnemyStateMachine enemyStateMachine)
		{
			return true;
		}
		public void FallProcess(IEnemyStateMachine enemyStateMachine)
		{

		}
		public void HitByQuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{

			SetBehaviourTextureDamagedFlash(0.1f, banditSprite);
			_behaviourTimer.StartTimer(0.133f);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();
				enemyStateMachine.SetEnemyState(state.IsIdle);
			}
			

		}
		void Update()
        {

        }

        public void UpdateMovement(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{
			//print("\nPlayerBehaviour.cs UpdateMovement() playerStateMachine=" + playerStateMachine.GetPlayerState());
			UpdateGravity(enemyStateMachine);

			//if (!SafeToMove(playerStateMachine))//Guard Clause
			//{
			//	//print("\nPlayerBehaviour.cs UpdateMovement()= NOT SafeToMove()");
			//	input = Vector2.zero;
			//	UpdateTransform(input);
			//	return;
			//}

			if (enemyStateMachine.GetEnemyState() == state.IsSprinting)
				_sprintSpeed = 1.75f;
			if (enemyStateMachine.GetEnemyState() != state.IsSprinting)
				_sprintSpeed = 1.0f;
			//_targetVelocityX = (input.x * (_moveSpeed * _sprintSpeed));
			//_velocity.x = _targetVelocityX;

			//print("\nx=" + _velocity.x);

			//DetectMovement(enemyStateMachine);
			DetectFaceDirection(banditSprite);

			UpdateTransform();
		}
        public void UpdateTransform()
		{
			_controller2D.Move(_velocity * Time.deltaTime, _velocity);
		}
        public void UpdateGravity(IEnemyStateMachine enemyStateMachine)
		{
			//print("\n_velocity.y=" + _velocity.y);
			if (!_controller2D.collisions.below)
			{
				_velocity.y += _gravity * Time.deltaTime;

				if (enemyStateMachine.GetEnemyState() != state.IsQuickAttack)
					return;
				if (enemyStateMachine.GetEnemyState() != state.IsJumping)
					return;

				enemyStateMachine.SetEnemyState(state.IsFalling);
			}
			if (enemyStateMachine.GetEnemyState() == state.IsJumping)
				return;
			if (enemyStateMachine.GetEnemyState() == state.IsFalling)
			{
				//_velocity.y += _gravity * Time.deltaTime;
				return;
			}
			if (_controller2D.collisions.below)
				_velocity.y = 0f;
		}

		private void DetectFaceDirection(IBanditSprite banditSprite)
		{
			if (_velocity.x < 0f)
			{
				if (banditSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
					banditSprite.FlipSprite();
				return;
			}
			if (_velocity.x > 0f)
			{
				if (banditSprite.GetSpriteDirection() == -1)
					banditSprite.FlipSprite();
				return;
			}
		}

		private void SetBehaviourTextureDamagedFlash(float time, IBanditSprite banditSprite)
		{
			_spriteColourSwapTimer.StartTimer(time);//flash sprite colour timer.
			if (_spriteColourSwapTimer.HasTimerFinished())
				banditSprite.SetTexture2DColor(Color.red);
		}
	}

}

