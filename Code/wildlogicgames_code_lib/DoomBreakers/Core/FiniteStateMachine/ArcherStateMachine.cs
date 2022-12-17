
using System;
using UnityEngine;

namespace DoomBreakers
{
    public class ArcherStateMachine : BasicEnemyStateMachine
    { 
        protected Vector3 _velocity;

        protected bool SafeToSetDying()
        {
            if (_state.GetType() != typeof(BanditDying))
                return true;

            return false;
        }

        protected bool IsDying()
        {
            if (_state.GetType() == typeof(BanditDying))
                return true;
            if (_state.GetType() == typeof(BanditDead))
                return true;
            return false;
        }
    }
}
