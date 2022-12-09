using System;
using UnityEngine;

namespace DoomBreakers
{
	public class MyPlayerStateMachine : StateMachine
	{
		protected bool _inputDodgedLeft;
        protected int _quickAttackIncrement;
        protected Vector3 _velocity;

        protected bool SafeToSetIdle() //Still require these checks to have player behave as desired.
        {
            //We don't want to set to idle each frame if already Idle AND is Jumping AND is Falling ect.
            if (_state.GetType() != typeof(PlayerIdle) && 
                _state.GetType() != typeof(PlayerFall) && 
                _state.GetType() != typeof(PlayerJump) && 
                _state.GetType() != typeof(PlayerDodge) && 
                _state.GetType() != typeof(PlayerQuickAttack) &&
                _state.GetType() != typeof(PlayerUpwardAttack) &&
                _state.GetType() != typeof(PlayerKnockAttack) &&
                _state.GetType() != typeof(PlayerHoldAttack) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerDefend) &&
                _state.GetType() != typeof(PlayerGainedEquipment) &&
                _state.GetType() != typeof(PlayerBrokenEquipment) &&
                _state.GetType() != typeof(PlayerHitByQuickAttack) &&
                _state.GetType() != typeof(PlayerHitByPowerAttack) &&
                _state.GetType() != typeof(PlayerHitDefending) &&
                _state.GetType() != typeof(PlayerExhausted) &&
                _state.GetType() != typeof(PlayerDying) &&
                _state.GetType() != typeof(PlayerDead))
                return true;

            return false;
        }

        protected bool SafeToSetMove()
        {
            if (_state.GetType() != typeof(PlayerMove) &&
                _state.GetType() != typeof(PlayerSprint) &&
                _state.GetType() != typeof(PlayerFall) && 
                _state.GetType() != typeof(PlayerJump) && 
                _state.GetType() != typeof(PlayerDodge) && 
                _state.GetType() != typeof(PlayerQuickAttack) &&
                _state.GetType() != typeof(PlayerUpwardAttack) &&
                _state.GetType() != typeof(PlayerKnockAttack) &&
                _state.GetType() != typeof(PlayerHoldAttack) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerDefend) &&
                _state.GetType() != typeof(PlayerGainedEquipment) &&
                _state.GetType() != typeof(PlayerBrokenEquipment) &&
                _state.GetType() != typeof(PlayerHitByQuickAttack) &&
                _state.GetType() != typeof(PlayerHitByPowerAttack) &&
                _state.GetType() != typeof(PlayerHitDefending) &&
                _state.GetType() != typeof(PlayerExhausted) &&
                _state.GetType() != typeof(PlayerDying) &&
                _state.GetType() != typeof(PlayerDead))
                return true;
            return false;
        }

        protected bool SafeToSetSprint()
        {
            if (
                _state.GetType() != typeof(PlayerSprint) &&
                _state.GetType() != typeof(PlayerFall) &&
                _state.GetType() != typeof(PlayerJump) &&
                _state.GetType() != typeof(PlayerDodge) &&
                _state.GetType() != typeof(PlayerQuickAttack) &&
                _state.GetType() != typeof(PlayerUpwardAttack) &&
                _state.GetType() != typeof(PlayerKnockAttack) &&
                _state.GetType() != typeof(PlayerHoldAttack) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerDefend) &&
                _state.GetType() != typeof(PlayerGainedEquipment) &&
                _state.GetType() != typeof(PlayerBrokenEquipment) &&
                _state.GetType() != typeof(PlayerHitByQuickAttack) &&
                _state.GetType() != typeof(PlayerHitByPowerAttack) &&
                _state.GetType() != typeof(PlayerHitDefending) &&
                _state.GetType() != typeof(PlayerExhausted) &&
                _state.GetType() != typeof(PlayerDying) &&
                _state.GetType() != typeof(PlayerDead))
                return true;
            return false;
        }

        protected bool SafeToSetJump()
        {
            if (_state.GetType() != typeof(PlayerJump) &&
                _state.GetType() != typeof(PlayerGainedEquipment) &&
                _state.GetType() != typeof(PlayerExhausted))
                return true;

            return false;

        }

        protected bool SafeToSetHoldAttack()
        {
            if (_state.GetType() != typeof(PlayerJump) &&
                _state.GetType() != typeof(PlayerReleaseAttack) &&
                _state.GetType() != typeof(PlayerGainedEquipment) &&
                _state.GetType() != typeof(PlayerBrokenEquipment) &&
                _state.GetType() != typeof(PlayerFall) &&
                _state.GetType() != typeof(PlayerExhausted))
                return true;

            return false;

        }

        protected bool ProcessQuickAttackFromBandit(ref BanditBaseState attackingBanditState, int banditFaceDir, int playerFaceDir)
		{
            if (attackingBanditState.GetType() == typeof(BanditQuickAttack))
            {
                if (!IsDefendingSelf())
				{
                    SetState(new PlayerHitByQuickAttack(this, _velocity));
                    return true;
                }
                else//if (IsDefendingSelf())
                {
                    if (IsDefendingCorrectDirection(banditFaceDir, playerFaceDir))
					{
                        SetState(new PlayerHitDefending(this, _velocity));
                        return false;
                    }
                    else
					{
                        SetState(new PlayerHitByQuickAttack(this, _velocity));
                        return true;
                    }
                }
            }
            return false;
        }
        protected bool ProcessPowerAttackFromBandit(ref BanditBaseState attackingBanditState, int banditFaceDir, int playerFaceDir)
        {
            if (attackingBanditState.GetType() == typeof(BanditReleaseAttack))
            {
                if (!IsDefendingSelf())
                {
                    SetState(new PlayerHitByPowerAttack(this, _velocity));
                    return true;
                }
                else//if (IsDefendingSelf())
                {
                    if (IsDefendingCorrectDirection(banditFaceDir, playerFaceDir))
                    {
                        SetState(new PlayerHitDefending(this, _velocity));
                        return false;
                    }
                    else
                    {
                        SetState(new PlayerHitByPowerAttack(this, _velocity));
                        return true;
                    }
                }
            }
            return false;
        }
        protected bool IsDefendingCorrectDirection(int enemyFaceDir, int playerFaceDir)
        {
            //Detrmine which way the player is facing whilst defending & the enemy bandit is attacking.
            //Why? Player doesn't successfully defend against enemy attack defending the wrong face direction.

            //Enemy would only ever be attacking if directly in front of player.
            //So if player face direction is 1 (right) then enemy would have to be -1 (left) 
            //case sceneria true for successful defence.
            if (playerFaceDir == 1 && enemyFaceDir == -1 ||
                playerFaceDir == -1 && enemyFaceDir == 1)
                return true;

            return false;
        }
        protected bool IsDefendingSelf()
        {
            if (_state.GetType() == typeof(PlayerDefend))
                return true;
            if (_state.GetType() == typeof(PlayerHitDefending))
                return true;

            return false;
        }
        protected bool IsIgnoreDamage()
        {
            if (_state.GetType() == typeof(PlayerDodge))
                return true;
            if (_state.GetType() == typeof(PlayerDodged))
                return true;
            if (_state.GetType() == typeof(PlayerGainedEquipment))
                return true;
            if (_state.GetType() == typeof(PlayerBrokenEquipment))
                return true;
            if (_state.GetType() == typeof(PlayerExhausted))
                return true;
            if (_state.GetType() == typeof(PlayerDying))
                return true;
            if (_state.GetType() == typeof(PlayerDead))
                return true;


            return false;
        }
        protected bool SafeToSetDying()
        {
            if (_state.GetType() != typeof(PlayerDying))
                return true;

            return false;
        }
        protected bool IsDying()
        {
            if (_state.GetType() == typeof(PlayerDying))
                return true;
            if (_state.GetType() == typeof(PlayerDead))
                return true;
            return false;
        }
    }
}
