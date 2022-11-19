using System;
using UnityEngine;

namespace DoomBreakers
{
	public class MyPlayerStateMachine : StateMachine
	{
		protected bool _inputDodgedLeft;

        public bool SafeToSetIdle() //Look into handling more than one state at a time.
        {
            //We don't want to set to idle each frame if already Idle AND is Jumping AND is Falling.
            if (_state.GetType() != typeof(PlayerIdle) && _state.GetType() != typeof(PlayerFall) && _state.GetType() != typeof(PlayerJump)
                && _state.GetType() != typeof(PlayerDodge) && _state.GetType() != typeof(PlayerQuickAttack))
                return true;

            return false;
        }

        public bool SafeToSetMove()
        {
            if (_state.GetType() != typeof(PlayerMove) && _state.GetType() != typeof(PlayerFall) && _state.GetType() != typeof(PlayerJump)
                && _state.GetType() != typeof(PlayerDodge) && _state.GetType() != typeof(PlayerQuickAttack))
                return true;
            return false;
        }

        public bool SafeToSetJump()
        {
            if (_state.GetType() != typeof(PlayerJump))
                return true;

            return false;

        }
    }
}
