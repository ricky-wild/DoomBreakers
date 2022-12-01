using System;
using UnityEngine;

namespace DoomBreakers
{
    public class BanditStateMachine : EnemyStateMachine
    {
        protected bool _inputDodgedLeft;
        protected int _quickAttackIncrement;
        protected Vector3 _velocity;

        protected float ProcessPowerAttackFromPlayer(ref BaseState playerAttackState, int banditId)
        {
            if (playerAttackState.GetType() == typeof(PlayerReleaseAttack))
            {
                float playerAttackedButtonTime = BattleColliderManager.GetPlayerHeldAttackButtonTime();
                SetState(new BanditHitByPowerAttack(this, _velocity, banditId));
                return playerAttackedButtonTime;
            }
            return 0f;
        }
        protected void ProcessUpwardAttackFromPlayer(ref BaseState playerAttackState, int banditId)
		{
            if (playerAttackState.GetType() == typeof(PlayerUpwardAttack)) SetState(new BanditHitByUpwardAttack(this, _velocity, banditId));
        }
        protected void ProcessKnockAttackFromPlayer(ref BaseState playerAttackState, int playerId, int playerFaceDir,  int banditId, int banditFaceDir)
        {
            if (playerAttackState.GetType() == typeof(PlayerKnockAttack))
            {
                if (NotDefending()) SetState(new BanditHitByKnockAttack(this, _velocity, banditId));
                else
                {
                    if (IsDefendingCorrectDirection(playerFaceDir, banditFaceDir))
                        SetState(new BanditHitDefending(this, _velocity, banditId));
                    else
                        SetState(new BanditHitByKnockAttack(this, _velocity, banditId));
                }
            }
        }
        protected void ProcessQuickAttackFromPlayer(ref BaseState playerAttackState, int playerId, int playerFaceDir, int banditId, int banditFaceDir)
		{
            if (playerAttackState.GetType() == typeof(PlayerQuickAttack))
            {
                if (NotDefending()) SetState(new BanditHitByQuickAttack(this, _velocity, banditId));
                else
                {
                    if (IsDefendingCorrectDirection(playerFaceDir, banditFaceDir))
                        SetState(new BanditHitDefending(this, _velocity, banditId));
                    else
                        SetState(new BanditHitByQuickAttack(this, _velocity, banditId));
                }
            }
        }
        protected bool IsDefendingCorrectDirection(int playerFaceDir, int banditFaceDir)
        {
            //int banditFaceDir = _banditSprite.GetSpriteDirection();

            if (banditFaceDir == 1 && playerFaceDir == -1 ||
                banditFaceDir == -1 && playerFaceDir == 1)
                return true;

            return false;
        }
        protected bool SafeToSetPersue() 
        {
            if (_state.GetType() == typeof(BanditDefending))
                return false;
            if (_state.GetType() == typeof(BanditHitDefending))
                return false;
            if (_state.GetType() == typeof(BanditReleaseAttack))
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
