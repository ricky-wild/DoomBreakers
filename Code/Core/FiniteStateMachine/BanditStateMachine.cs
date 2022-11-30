using System;
using UnityEngine;

namespace DoomBreakers
{
    public class BanditStateMachine : EnemyStateMachine
    {
        protected bool _inputDodgedLeft;
        protected int _quickAttackIncrement;
        protected Vector3 _velocity;


        protected bool SafeToSetPersue() 
        {
            if (_state.GetType() == typeof(BanditDefending))
                return false;
            if (_state.GetType() == typeof(BanditHitDefending))
                return false;

            return true;
        }

        protected bool NotDefending()
		{
            if (_state.GetType() != typeof(BanditDefending) && _state.GetType() != typeof(BanditHitDefending)) //This one caught me off guard, AND required.
                return true;
            return false;
        }

        protected bool IsDefending()
        {
            if (_state.GetType() == typeof(BanditDefending))
                return true;
            return false;
        }
    }
}
