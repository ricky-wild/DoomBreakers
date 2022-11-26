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

            //if (_state.GetType() != typeof(PlayerIdle) &&
            //    _state.GetType() != typeof(PlayerFall) &&
            //    _state.GetType() != typeof(PlayerJump) &&
            //    _state.GetType() != typeof(PlayerDodge) &&
            //    _state.GetType() != typeof(PlayerQuickAttack) &&
            //    _state.GetType() != typeof(PlayerUpwardAttack) &&
            //    _state.GetType() != typeof(PlayerHoldAttack) &&
            //    _state.GetType() != typeof(PlayerReleaseAttack) &&
            //    _state.GetType() != typeof(PlayerDefend) &&
            //    _state.GetType() != typeof(PlayerGainedEquipment))
            //    return true;

            //if (enemyStateMachine.IsQuickAttackHit())
            //    return false;
            //if (enemyStateMachine.IsQuickAttack())
            //    return false;
            //if (enemyStateMachine.IsPowerAttackHit())
            //    return false;
            //if (enemyStateMachine.IsQuickHitWhenDefending())
            //    return false;
            //if (enemyStateMachine.IsPowerHitWhenDefending())
            //    return false;
            //if (enemyStateMachine.IsImpactHit())
            //    return false;


            //if (enemyStateMachine.IsFalling())
            //    return false;

            return false;
        }
    }
}
