using Rewired;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerInput : MyPlayerStateMachine// ,IPlayerInput//: MonoBehaviour
	{

		public enum inputState
		{
			Empty = 0,
			Jump = 1,
			Attack = 2,
			Move = 3,
			Defend = 4,
			DefenceReleased = 5,
			HoldAttack = 6,
			ReleaseAttack = 7,
			KnockBackAttack = 8,
			DodgeL = 9,
			DodgeR = 10,
			UpwardAttack = 11,
			Sprint = 12
		};
		private inputState _inputState;
		private Rewired.Player _rewirdInputPlayer;
		private Vector2 _inputVector2;
		//private MyPlayerStateMachine _playerStateMachine;


		public PlayerInput(int playerID, Transform t, Controller2D controller2D)//, MyPlayerStateMachine playerStateMachine)
		{
			_rewirdInputPlayer = ReInput.players.GetPlayer(playerID);
			_inputVector2 = new Vector2();
			//_playerStateMachine = playerStateMachine;
		}
		public bool SafeToReset()
		{
			if (_inputState == inputState.Sprint)
				return false;

			return true;
		}
		public void ResetInput()
		{
			if (!SafeToReset())
				return;

			_inputState = inputState.Empty;
		}
		public void UpdateInput()
		{
			if (_rewirdInputPlayer == null)
				return;



			_inputVector2.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");
			_inputVector2.y = _rewirdInputPlayer.GetAxis("MoveVertical");

			//if (_inputVector2.x > 0f || _inputVector2.x < 0f)
			//	SetState(new PlayerMove(this));
			//else
			//	SetState(new PlayerIdle(this));//SetState(new PlayerIdle(this));

			//if (_rewirdInputPlayer.GetButtonDown("Jump"))
			//	SetState(new PlayerJump(this));//_playerStateMachine.SetState(new PlayerJump(_playerStateMachine));//_inputState = inputState.Jump; //_inputStates[inputState.Jump] = true;
			if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
			{
				if(_inputVector2.y > 0.44f)
					_inputState = inputState.UpwardAttack;
				else
					_inputState = inputState.Attack;
			}
			if (_rewirdInputPlayer.GetButtonDown("Defend"))
				_inputState = inputState.Defend;
			if (_rewirdInputPlayer.GetButtonUp("Defend"))
				_inputState = inputState.DefenceReleased;
			if (_rewirdInputPlayer.GetButtonTimedPressDown("Attack", 0.25f))//Power Attack
				_inputState = inputState.HoldAttack;
			if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.25f))
				_inputState = inputState.ReleaseAttack;
			if (_rewirdInputPlayer.GetButtonTimedPressUp("KnockBack", 0.01f))
				_inputState = inputState.KnockBackAttack;
			if (_rewirdInputPlayer.GetButtonDown("DodgeL"))
				_inputState = inputState.DodgeL;
			if (_rewirdInputPlayer.GetButtonDown("DodgeR"))
				_inputState = inputState.DodgeR;
			if (_rewirdInputPlayer.GetButtonDown("Sprint"))
				_inputState = inputState.Sprint;
			if (_rewirdInputPlayer.GetButtonUp("Sprint"))
				_inputState = inputState.Empty;
			//if (_rewirdInputPlayer.GetAnyButtonUp())
			//	_inputVector2.x = 0.0f;
		}

		public inputState GetInputState()
		{
			return _inputState;
		}

		public Vector2 GetInputVector2()
		{
			return _inputVector2;
		}


	}
}

