﻿using System;
using UnityEngine;

namespace DoomBreakers
{

	public class NPCStateMachine : BasicNPCStateMachine
	{
		protected Vector3 _velocity;

        //protected float ProcessPowerAttackFromPlayer(ref BaseState playerAttackState, int enemyId)
        //{
        //    if (IsDying()) return 0f;
        //    if (playerAttackState.GetType() == typeof(PlayerReleaseAttack))
        //    {
        //        float playerAttackedButtonTime = BattleColliderManager.GetPlayerHeldAttackButtonTime();
        //        SetState(new BanditHitByPowerAttack(this, _velocity, enemyId));
        //        return playerAttackedButtonTime;
        //    }
        //    return 0f;
        //}
        //protected bool ProcessUpwardAttackFromPlayer(ref BaseState playerAttackState, int enemyId)
        //{
        //    if (IsDying()) return false;
        //    if (playerAttackState.GetType() == typeof(PlayerUpwardAttack))
        //    {
        //        SetState(new BanditHitByUpwardAttack(this, _velocity, enemyId));
        //        return true;
        //    }
        //    return false;
        //}
        //protected bool ProcessKnockAttackFromPlayer(ref BaseState playerAttackState, int playerId, int playerFaceDir, int enemyId, int banditFaceDir)
        //{
        //    if (IsDying()) return false;
        //    if (playerAttackState.GetType() == typeof(PlayerKnockAttack))
        //    {
        //        if (NotDefending())
        //        {
        //            SetState(new BanditHitByKnockAttack(this, _velocity, enemyId));
        //            return true;
        //        }
        //        else
        //        {
        //            if (IsDefendingCorrectDirection(playerFaceDir, banditFaceDir))
        //            {
        //                SetState(new BanditHitDefending(this, _velocity, enemyId));
        //                return false;
        //            }
        //            else
        //            {
        //                SetState(new BanditHitByKnockAttack(this, _velocity, enemyId));
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //protected bool ProcessQuickAttackFromPlayer(ref BaseState playerAttackState, int playerId, int playerFaceDir, int enemyId, int banditFaceDir)
        //{
        //    if (IsDying()) return false;
        //    if (playerAttackState.GetType() == typeof(PlayerQuickAttack))
        //    {
        //        if (NotDefending())
        //        {
        //            SetState(new BanditHitByQuickAttack(this, _velocity, enemyId));
        //            return true;
        //        }
        //        else
        //        {
        //            if (IsDefendingCorrectDirection(playerFaceDir, banditFaceDir))
        //            {

        //                SetState(new BanditHitDefending(this, _velocity, enemyId));
        //                return false;
        //            }
        //            else
        //            {
        //                SetState(new BanditHitByQuickAttack(this, _velocity, enemyId));
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //protected bool IsDefendingCorrectDirection(int playerFaceDir, int banditFaceDir)
        //{
        //    //int banditFaceDir = _banditSprite.GetSpriteDirection();

        //    if (banditFaceDir == 1 && playerFaceDir == -1 ||
        //        banditFaceDir == -1 && playerFaceDir == 1)
        //        return true;

        //    return false;
        //}
        //protected bool SafeToSetPersue() //_detectPlatformEdge = true in constructor() BanditPersue()
        //{
        //    if (_state.GetType() == typeof(BanditJump))
        //        return false;
        //    if (_state.GetType() == typeof(BanditFall))
        //        return false;
        //    if (_state.GetType() == typeof(BanditHitByPowerAttack))//_detectPlatformEdge = false in constructor()
        //        return false;
        //    if (_state.GetType() == typeof(BanditHitByKnockAttack))//_detectPlatformEdge = false in constructor()
        //        return false;
        //    if (_state.GetType() == typeof(BanditDefending))
        //        return false;
        //    if (_state.GetType() == typeof(BanditHitDefending))
        //        return false;
        //    if (_state.GetType() == typeof(BanditReleaseAttack))
        //        return false;
        //    if (_state.GetType() == typeof(BanditDying))
        //        return false;
        //    if (_state.GetType() == typeof(BanditDead))
        //        return false;

        //    return true;
        //}
        //protected bool SafeToSetDying()
        //{
        //    if (_state.GetType() != typeof(BanditDying))
        //        return true;

        //    return false;
        //}

        //protected bool NotDefending()
        //{
        //    if (_state.GetType() != typeof(BanditDefending) && _state.GetType() != typeof(BanditHitDefending)) //This one caught me off guard, AND required.
        //        return true;
        //    return false;
        //}

        //protected bool IsDefending()
        //{
        //    if (_state.GetType() == typeof(BanditDefending))
        //        return true;
        //    return false;
        //}
        //protected bool IsDying()
        //{
        //    if (_state.GetType() == typeof(BanditDying))
        //        return true;
        //    if (_state.GetType() == typeof(BanditDead))
        //        return true;
        //    return false;
        //}
    }
}
