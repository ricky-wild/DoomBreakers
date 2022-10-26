using System;
using UnityEngine;
using System.Collections;

namespace DoomBreakers
{
	public class EnemyStateMachine : IEnemyStateMachine//: MonoBehaviour
	{

		private state _state;

		public EnemyStateMachine(state state)
		{
			_state = state;
		}
		public void SetEnemyState(state state)
		{
			//print("\nEnemyStateMachine.cs= state being set too, "+ state);
			if (!SafeToSetState(state)) //Guard clause
				return;
			_state = state;
		}
		public state GetEnemyState()
		{
			return _state;
		}

		private bool SafeToSetState(state state)
		{
			if (_state == state) //Guard clause don't assign the same state.
				return false;

			if (_state == state.IsFalling && state == state.IsJumping) //Prevents delayed jumping behaviour upon jump button spam
				return false;
			if (_state == state.IsFalling && state == state.IsMoving) //Prevents delayed jumping behaviour upon jump button spam
				return false;


			return true;
		}


	}
}


