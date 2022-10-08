using Rewired;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerInput : IPlayerInput//: MonoBehaviour
	{

		public enum inputState
		{
			Empty = 0,
			Jump = 1,
			Attack = 2,
			Move = 3
		};
		private inputState _inputState;
		private Rewired.Player _rewirdInputPlayer;
		private Vector2 _inputVector2;
		//private Dictionary<inputState, bool> _inputStates = new Dictionary<inputState, bool>();


		public PlayerInput(int playerID)
		{
			_rewirdInputPlayer = ReInput.players.GetPlayer(playerID);
			_inputVector2 = new Vector2();

			//Initialize input states
			//_inputStates.Add(inputState.Empty, false);
			//_inputStates.Add(inputState.Jump, true);
		}
		public void ResetInput()
		{
			_inputState = inputState.Empty;
			//foreach (var key in _inputStates.Keys.ToList())
			//{
			//	_inputStates[key] = false;
			//}
		}
		public void UpdateInput()
		{
			if (_rewirdInputPlayer == null)
				return;

			if (_rewirdInputPlayer.GetAnyButtonUp())
				_inputVector2.x = 0.0f;

			_inputVector2.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");
			//_input.x = _rewirdInputPlayer.GetAxis(0);
			_inputVector2.y = _rewirdInputPlayer.GetAxis("MoveVertical");

			if (_rewirdInputPlayer.GetButtonDown("Jump"))
				_inputState = inputState.Jump; //_inputStates[inputState.Jump] = true;
			if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
				_inputState = inputState.Attack;
		}

		public inputState GetInputState()
		{
			return _inputState;
		}

		public Vector2 GetInputVector2()
		{
			return _inputVector2;
		}

		//public Dictionary<inputState, bool> GetInputStates()
		//{
		//	return _inputStates;
		//}

		//public bool IsJumpPressed()
		//{
		//	if (_inputState == inputState.Jump)
		//		return true;
		//	return false;
		//}
	}
}

