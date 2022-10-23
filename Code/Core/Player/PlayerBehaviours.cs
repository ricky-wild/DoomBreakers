using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	//PlayerBehaviours.cs obj also requires Controller2D.cs & RaycastController.cs & BoxCollder2D
	//[RequireComponent(typeof(Controller2D))]
	public class PlayerBehaviours : MonoBehaviour, IPlayerBehaviours
	{
		private Controller2D _controller2D; 
		private Vector3 _velocity;
		private Transform _transform;
		private float _targetVelocityX, _maxJumpVelocity, _moveSpeed, _sprintSpeed, _gravity;
		private int _quickAttackIncrement; //4+ variations of this animation.
		private bool _dodgedLeftFlag;//, _jumpedFlag;

		private ITimer _behaviourTimer, _dodgedTimer, _spriteColourSwapTimer;

		public PlayerBehaviours(Transform t, Controller2D controller2D)
		{
			_controller2D = controller2D;
			_transform = t;
			_velocity = new Vector3();
			_moveSpeed = 3.75f;//3.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect

			_behaviourTimer = new Timer();
		}

		public void Setup(Transform t, Controller2D controller2D)
		{
			_controller2D = controller2D;
			_transform = t;
			_velocity = new Vector3();
			_moveSpeed = 3.75f;//3.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_maxJumpVelocity = 14.0f;//13.25f;
			_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
			_quickAttackIncrement = 0;
			_dodgedLeftFlag = false;
			//_jumpedFlag = false;

			//_behaviourTimer = new Timer();
			_behaviourTimer = this.gameObject.AddComponent<Timer>();
			_behaviourTimer.Setup();
			_dodgedTimer = this.gameObject.AddComponent<Timer>();
			_dodgedTimer.Setup();
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
		public void IdleProcess(IPlayerStateMachine playerStateMachine)
		{

			_velocity.x = 0f;
			//_velocity.y = 0f;
			//print("\n_velocity.y=" + Mathf.Abs(_velocity.y));
			if (Mathf.Abs(_velocity.y) >= 3.0f)
				playerStateMachine.SetPlayerState(state.IsFalling);

		}
		public bool JumpProcess(IPlayerStateMachine playerStateMachine)
		{
			if (!SafeToJump(playerStateMachine))
				return false;


			_behaviourTimer.StartTimer(0.001f);
			
			if (_behaviourTimer.HasTimerFinished())
			{
				//_jumpedFlag = true;
				//print("\n_behaviourTimer.HasTimerFinished()=true");
				_velocity.y += _maxJumpVelocity;
				
				//playerStateMachine.SetPlayerState(state.IsFalling);
			}
			//else
			//	print("\n_behaviourTimer.HasTimerFinished()=false");

			if (_velocity.y >= (_maxJumpVelocity / 1.25f)) //Near peak of jump velocity, set falling state.
			{
				playerStateMachine.SetPlayerState(state.IsFalling);
			}

			return true;
		}
		public void FallProcess(IPlayerStateMachine playerStateMachine)
		{
			if (_controller2D.collisions.below) //Means we're finished jumping/falling.
			{
				//_jumpedFlag = false;
				_velocity.x = 0f;
				_velocity.y = 0f;
				playerStateMachine.SetPlayerState(state.IsIdle);
				DetectMovement(playerStateMachine);
			}

		}
		public void QuickAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite)
		{


			//_spriteColourSwapTimer.StartTimer(0.1f);//flash sprite colour timer.
			//if (_spriteColourSwapTimer.HasTimerFinished())
			//	playerSprite.SetTexture2DColor(Color.white);

			SetBehaviourTextureFlash(0.1f, playerSprite);

			_behaviourTimer.StartTimer(0.133f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();
				//Everything here is only processed once. This exists in Player.Update();
				if (_quickAttackIncrement >= 0 && _quickAttackIncrement < 4)
					_quickAttackIncrement++;
				else
					_quickAttackIncrement = 0;
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
		}
		public void UpwardAttackProcess(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite)
		{
			//_spriteColourSwapTimer.StartTimer(0.1f);//flash sprite colour timer.
			//if (_spriteColourSwapTimer.HasTimerFinished())
			//	playerSprite.SetTexture2DColor(Color.white);

			SetBehaviourTextureFlash(0.1f, playerSprite);

			_behaviourTimer.StartTimer(0.917f/2);//anim length
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
		}
		public void IdleDefenceProcess(IPlayerStateMachine playerStateMachine)
		{
			if(_velocity.x != 0.0f)
			{
				playerStateMachine.SetPlayerState(state.IsDefenceMoving);
				return;
			}
			if (_velocity.x == 0.0f)
				playerStateMachine.SetPlayerState(state.IsDefencePrepare);
		}
		public void HoldAttackProcess(IPlayerStateMachine playerStateMachine)
		{
			_velocity.x = 0f;
			_velocity.y = 0f;
		}
		public void ReleaseAttackProcess(IPlayerStateMachine playerStateMachine)
		{
			_velocity.x = 0f;
			_velocity.y = 0f;
			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
		}
		public void KnockbackAttackProcess(IPlayerStateMachine playerStateMachine)
		{
			_behaviourTimer.StartTimer(0.75f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
		}
		public void DodgeInitiatedProcess(IPlayerStateMachine playerStateMachine, bool dodgeLeft, IPlayerSprite playerSprite)
		{
			if (!SafeToDodge(playerStateMachine))//Guard clause.
			{
				//print("\nPlayerBehaviour.cs DodgeProcess()= NOT safe to dodge.");
				return;
			}
			int faceDir = playerSprite.GetSpriteDirection();
			if (dodgeLeft)
			{
				//_velocity.x -= 1.0f;
				if (faceDir == -1)
					playerSprite.FlipSprite();
			}
			if (!dodgeLeft)
			{
				//_velocity.x += 1.0f;
				if (faceDir == 1)
					playerSprite.FlipSprite();
			}
			//_spriteColourSwapTimer.StartTimer(0.05f);//flash sprite colour timer.
			//if (_spriteColourSwapTimer.HasTimerFinished())
			//	playerSprite.SetTexture2DColor(Color.white);
			SetBehaviourTextureFlash(0.05f, playerSprite);

			_behaviourTimer.StartTimer(1.4f/3);//anim time.
			if (_behaviourTimer.HasTimerFinished())
			{
				_dodgedLeftFlag = dodgeLeft;
				playerStateMachine.SetPlayerState(state.IsDodgeRelease);
				playerSprite.ResetTexture2DColor();				
			}
		}
		public void DodgeReleasedProcess(IPlayerStateMachine playerStateMachine)
		{
			if (_dodgedLeftFlag)
				_velocity.x -= 30.0f;
			else
				_velocity.x += 30.0f;
			_dodgedTimer.StartTimer(0.05f);
			if (_dodgedTimer.HasTimerFinished())
			{
				//playerSprite.ResetTexture2DColor();
				playerStateMachine.SetPlayerState(state.IsIdle);
				return;
			}
		}
		void Update()
		{
			//UpdateTransform(); Called In Player.cs to pass PlayerInput.cs through.
		}

		private void DetectFaceDirection(IPlayerSprite playerSprite)
		{
			if (_velocity.x < 0f)
			{
				if(playerSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
					playerSprite.FlipSprite();
				return;
			}
			if (_velocity.x > 0f)
			{
				if (playerSprite.GetSpriteDirection() == -1)
					playerSprite.FlipSprite();
				return;
			}
		}
		private void DetectMovement(IPlayerStateMachine playerStateMachine)
		{
			if (!SafeToSetIdle(playerStateMachine))//Guard Clause
				return;

			if (_targetVelocityX == 0f && _controller2D.collisions.below)
			{
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
			if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller2D.collisions.below)
			{
				if (playerStateMachine.GetPlayerState() != state.IsDefenceMoving || //Decided in Player.cs via UpdateInput()
					playerStateMachine.GetPlayerState() != state.IsSprinting)
				{
					playerStateMachine.SetPlayerState(state.IsMoving);
				}

			}

		}
		public void UpdateMovement(Vector2 input, IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite)
		{
			//print("\nPlayerBehaviour.cs UpdateMovement() playerStateMachine=" + playerStateMachine.GetPlayerState());
			UpdateGravity(playerStateMachine);

			if (!SafeToMove(playerStateMachine))//Guard Clause
			{
				//print("\nPlayerBehaviour.cs UpdateMovement()= NOT SafeToMove()");
				input = Vector2.zero;
				UpdateTransform(input);
				return;
			}

			if (playerStateMachine.GetPlayerState() == state.IsSprinting)
				_sprintSpeed = 1.75f;
			if (playerStateMachine.GetPlayerState() != state.IsSprinting)
				_sprintSpeed = 1.0f;
			_targetVelocityX = (input.x * (_moveSpeed * _sprintSpeed));
			_velocity.x = _targetVelocityX;

			//print("\nx=" + _velocity.x);
			
			DetectMovement(playerStateMachine);
			DetectFaceDirection(playerSprite);

			UpdateTransform(input);
		}
		public void UpdateTransform(Vector2 input)
		{
			_controller2D.Move(_velocity * Time.deltaTime, input);
		}
		public void UpdateGravity(IPlayerStateMachine playerStateMachine)
		{
			//print("\n_velocity.y=" + _velocity.y);
			if (!_controller2D.collisions.below)
			{
				_velocity.y += _gravity * Time.deltaTime;

				if (playerStateMachine.GetPlayerState() != state.IsQuickAttack)
					return;
				if (playerStateMachine.GetPlayerState() != state.IsJumping)
					return;

				playerStateMachine.SetPlayerState(state.IsFalling);
			}
			if (playerStateMachine.GetPlayerState() == state.IsJumping)
				return;
			if (playerStateMachine.GetPlayerState() == state.IsFalling)
			{
				//_velocity.y += _gravity * Time.deltaTime;
				return;
			}
			if (_controller2D.collisions.below)
				_velocity.y = 0f;
		}


		private void SetBehaviourTextureFlash(float time, IPlayerSprite playerSprite)
		{
			_spriteColourSwapTimer.StartTimer(time);//flash sprite colour timer.
			if (_spriteColourSwapTimer.HasTimerFinished())
				playerSprite.SetTexture2DColor(Color.white);
		}


		public bool SafeToJump(IPlayerStateMachine playerStateMachine)
		{
			//print("\nSafeToJump() _controller2D.collisions.below=" + _controller2D.collisions.below);
			//print("\n_velocity.y=" + Mathf.Abs(_velocity.y));

			if (_controller2D.collisions.below) //if (Mathf.Abs(_velocity.y) <= 1.9f)
				return true;
			//print("\n_jumpedFlag=" + _jumpedFlag);
			//if (!_jumpedFlag)
			//	return true;

			//If else check ensures spam jump input during mid air doesn't activate another jump upon landing.
			//if (_controller2D.collisions.below)
			//if(_controller2D.grounded)
			//if (_controller2D.collisions.below) //if (_velocity.y == 0f)
			//{
			//	playerStateMachine.SetPlayerState(state.IsIdle);
			//}
			//else
			//	playerStateMachine.SetPlayerState(state.IsFalling);

			//if (!_controller2D.collisions.below)
			//if(!_controller2D.grounded)
			//if (_velocity.y != 0f)
			//return false;


			return false;
		}
		public bool SafeToDodge(IPlayerStateMachine playerStateMachine)
		{
			if (playerStateMachine.GetPlayerState() == state.IsDodgeRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefencePrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefenceRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefenceMoving)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitWhileDefending)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsQuickHitWhileDefending)
				return false;

			if (playerStateMachine.GetPlayerState() == state.IsDying)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDead)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsGainedEquipment)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsArmorBroken)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackPrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGround)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGroundByEnemy)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsLockedComboAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRespawning)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsExhausted)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDialogue)
				return false;
			return true;
		}
		public bool SafeToMove(IPlayerStateMachine playerStateMachine)
		{
			//print("\nPlayerBehaviour.cs SafeToMove() playerStateMachine=" + playerStateMachine.GetPlayerState());

			if (playerStateMachine.GetPlayerState() == state.IsDying)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDead)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsGainedEquipment)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsArmorBroken)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackPrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDodgeLPrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDodgeRPrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDodgeRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGround)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGroundByEnemy)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsLockedComboAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRespawning)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsExhausted)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDialogue)
				return false;

			return true;
		}
		public bool SafeToSetIdle(IPlayerStateMachine playerStateMachine)
		{
			if (!_behaviourTimer.HasTimerFinished())
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRunningQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsQuickHitWhileDefending)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitWhileDefending)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsMidAirAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsFalling)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsJumping)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsGainedEquipment)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsArmorBroken)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByReleaseAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByEnemyReleaseAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsExhausted)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRespawning)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefencePrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefenceMoving)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsLockedComboAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDialogue)
				return false;

			return true;
		}
	}
}

