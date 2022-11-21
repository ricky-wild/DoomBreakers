using System;
using UnityEngine;

namespace DoomBreakers
{
	public class MyPlayerStateMachine : StateMachine
	{
		protected bool _inputDodgedLeft;
        protected int _quickAttackIncrement;
        protected Vector3 _velocity;

        public bool SafeToSetIdle() //Still require these checks to have player behave as desired.
        {
            //We don't want to set to idle each frame if already Idle AND is Jumping AND is Falling ect.
            if (_state.GetType() != typeof(PlayerIdle) && 
                _state.GetType() != typeof(PlayerFall) && 
                _state.GetType() != typeof(PlayerJump) && 
                _state.GetType() != typeof(PlayerDodge) && 
                _state.GetType() != typeof(PlayerQuickAttack) &&
                _state.GetType() != typeof(PlayerUpwardAttack) &&
                _state.GetType() != typeof(PlayerHoldAttack) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerDefend) &&
                _state.GetType() != typeof(PlayerGainedEquipment)) 
                return true;

            return false;
        }

        public bool SafeToSetMove()
        {
            if (_state.GetType() != typeof(PlayerMove) && 
                _state.GetType() != typeof(PlayerFall) && 
                _state.GetType() != typeof(PlayerJump) && 
                _state.GetType() != typeof(PlayerDodge) && 
                _state.GetType() != typeof(PlayerQuickAttack) &&
                _state.GetType() != typeof(PlayerUpwardAttack) &&
                _state.GetType() != typeof(PlayerHoldAttack) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerDefend) &&
                _state.GetType() != typeof(PlayerGainedEquipment))
                return true;
            return false;
        }

        public bool SafeToSetJump()
        {
            if (_state.GetType() != typeof(PlayerJump) &&
                _state.GetType() != typeof(PlayerGainedEquipment))
                return true;

            return false;

        }

        public bool SafeToSetHoldAttack()
        {
            if (_state.GetType() != typeof(PlayerJump) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerFall))
                return true;

            return false;

        }
    }
}
