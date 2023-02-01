using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class BattleColliderManager : MonoBehaviour
	{
        //<summary>
        //The battle collider manager' purpose is too communicate collisions between enemy AI & player, vice versa.
        //We will need to communicate some details about each object involved with the collision.
        //
        //</summary>
        private Dictionary<string, Action> _battleEventDictionary;
        private static BattleColliderManager _battleEventManager;


        private static int _mostRecentCollidedPlayerId;
        private static Dictionary<int, int> _playerFaceDir;
        private static Dictionary<int, BaseState> _playerState;
        private static Dictionary<int, ItemBase> _playerWeapon;
        private static Dictionary<int, PlayerStats> _playerStats;
        private static float _playerAttackButtonHeldTime;


        private static int _mostRecentCollidedBasicEnemyId; //Needed for Enemy Hit By Power Attack behaviour state.
        private static Dictionary<int, int> _enemyFaceDir;
        private static Dictionary<int, BasicEnemyBaseState> _basicEnemyState;
        private static Dictionary<int, BanditStats> _basicEnemyStats;


        public static BattleColliderManager _instance
        {
            get //When we access our instance from another place, we'll setup as appropriate if required.
            {
                if (!_battleEventManager)
                {
                    //FindObjectOfType isn't a cheap call but we only do this once, if not at all.
                    _battleEventManager = FindObjectOfType(typeof(BattleColliderManager)) as BattleColliderManager;

                    if (!_battleEventManager)
                        print("\nBattleColliderManager= You need an active BattleColliderManager script attached to a GameObject within the scene!");
                    else
                        _battleEventManager.Setup();
                }

                return _battleEventManager;
            }
        }
        private void Setup()
        {
            if (_battleEventDictionary == null) _battleEventDictionary = new Dictionary<string, Action>();

            if (_playerFaceDir == null) _playerFaceDir = new Dictionary<int, int>();
            if (_playerState == null) _playerState = new Dictionary<int, BaseState>();
            if (_playerWeapon == null) _playerWeapon = new Dictionary<int, ItemBase>();
            if (_playerStats == null) _playerStats = new Dictionary<int, PlayerStats>();

            if (_enemyFaceDir == null) _enemyFaceDir = new Dictionary<int, int>();
            if (_basicEnemyState == null) _basicEnemyState = new Dictionary<int, BasicEnemyBaseState>();
            if (_basicEnemyStats == null) _basicEnemyStats = new Dictionary<int, BanditStats>();

            _mostRecentCollidedPlayerId = 0;
            _playerAttackButtonHeldTime = 0f;
            //_enemyType = EnemyAI.None;
        }

        //<summary>
        //Bundle all appropriate collision assignment calls into one method call.
        //This is so far, AssignPlayerState(), AssignPlayerFaceDir() and TriggerEvent("ReportCollisionWithSomething").
        //
        //</summary>
        public static void AssignCollisionDetails(string eventName, ref BaseState playerState, int forPlayerId, IPlayerSprite playerSprite, 
            ItemBase weapon, ref PlayerStats characterStat)
		{
            AssignPlayerFaceDir(forPlayerId, playerSprite);
            AssignPlayerState(ref playerState, forPlayerId);
            AssignPlayerWeapon(forPlayerId, weapon);
            AssignPlayerStats(forPlayerId, ref characterStat);
            BaseTriggerEvent(eventName);
        }
        
        //<summary>
        //AssignPlayerState() is used to communicate player state during an attack. 
        //We will need this for enemy AI regarding the Bandit.cs->AttackedByPlayer() so
        //we can determine the enemy' next state, as appropriate.
        //
        //</summary>
        public static void AssignPlayerState(ref BaseState playerState, int playerId)
		{
            if (!_playerState.ContainsKey(playerId))
                _playerState.Add(playerId, playerState);
            else
			{
                if(playerState.GetType() != _playerState[playerId].GetType())
				{
                    _playerState.Remove(playerId);
                    _playerState.Add(playerId, playerState);
                }
            }
		}
        public static BaseState GetAssignedPlayerState(int playerId) //=> _playerState[playerId];
		{
            if (!_playerState.ContainsKey(playerId)) return null;

            return _playerState[playerId];

        }
        
        //<summary>
        //AssignPlayerFaceDir() is used to communicate player face direction during an attack. 
        //We will need this for enemy AI regarding the HitByPowerAttack obj state. 
        //
        //</summary>
        public static void AssignPlayerFaceDir(int forPlayerId, IPlayerSprite playerSprite)
		{
            //PlayerCollision.cs->UpdateDetectEnemyTargets()
            _mostRecentCollidedPlayerId = forPlayerId;
            if (!_playerFaceDir.ContainsKey(forPlayerId))
                _playerFaceDir.Add(forPlayerId, playerSprite.GetSpriteDirection());
            else
            {
                if (_playerFaceDir[forPlayerId] != playerSprite.GetSpriteDirection())
                {
                    _playerFaceDir.Remove(forPlayerId);
                    _playerFaceDir.Add(forPlayerId, playerSprite.GetSpriteDirection());//There we go dumbass.
                }
            }
        }
        public static int GetAssignedPlayerFaceDir(int forPlayerId)
		{
            if (_playerFaceDir.ContainsKey(forPlayerId))
                return _playerFaceDir[forPlayerId];
            else
                return 1;
        }

        //<summary>
        //AssignPlayerWeapon() is used to communicate player damage output during an attack. 
        //We will need this for enemy AI regarding the Bandit.cs->AttackedByPlayer() Method.
        //But will use this Method at PlayerCollision.cs->UpdateDetectEnemyTargets()
        //</summary>
        public static void AssignPlayerWeapon(int forPlayerId, ItemBase weapon)
		{
            if (weapon == null) return;

            if(!_playerWeapon.ContainsKey(forPlayerId))
                _playerWeapon.Add(forPlayerId, weapon);
            else
			{
               if (_playerWeapon[forPlayerId] != weapon)
				{
                    _playerWeapon.Remove(forPlayerId);
                    _playerWeapon.Add(forPlayerId, weapon);
				}
			}
		}
        public static ItemBase GetAssignedPlayerWeapon(int forPlayerId)
		{
            if (_playerWeapon == null) return null;

            if (_playerWeapon.ContainsKey(forPlayerId))
                return _playerWeapon[forPlayerId];
            else
                return null;
		}
        public static void AssignPlayerStats(int forPlayerId, ref PlayerStats characterStat)
        {
            _mostRecentCollidedBasicEnemyId = forPlayerId;
            if (!_playerStats.ContainsKey(forPlayerId))
                _playerStats.Add(forPlayerId, characterStat);
            else
            {
                if (_playerStats[forPlayerId] != characterStat)
                {
                    _playerStats.Remove(forPlayerId);
                    _playerStats.Add(forPlayerId, characterStat);
                }
            }
        }

        public static int GetRecentCollidedPlayerId() => _mostRecentCollidedPlayerId;
        public static PlayerStats GetAssignedPlayerStatus(int forPlayerId)// => _playerStats[forPlayerId];
		{
            if (!_playerStats.ContainsKey(forPlayerId)) return null;

            return _playerStats[forPlayerId];
        }
        public static void SetPlayerHeldAttackButtonTime(float time) => _playerAttackButtonHeldTime = time;
        public static float GetPlayerHeldAttackButtonTime() => _playerAttackButtonHeldTime;




        //<summary>
        //Bundle all appropriate collision assignment calls into one method call.
        //This is so far, AssignBanditState(), AssignBanditFaceDir() and TriggerEvent("ReportCollisionWithSomething").
        //We'll overload this from Player use for Bandit use.
        //</summary>
        public static void AssignCollisionDetails(string eventName, ref BasicEnemyBaseState banditState, int forBanditId, int faceDir, ref BanditStats characterStat)
        {
            AssignBanditFaceDir(forBanditId, faceDir);
            AssignBanditState(ref banditState, forBanditId);
            AssignBanditStats(forBanditId, ref characterStat);
            BaseTriggerEvent(eventName);
        }
        public static void AssignBanditState(ref BasicEnemyBaseState banditState, int enemyId)
        {
            if (!_basicEnemyState.ContainsKey(enemyId))
                _basicEnemyState.Add(enemyId, banditState);
            else
            {
                if (banditState.GetType() != _basicEnemyState[enemyId].GetType())
                {
                    _basicEnemyState.Remove(enemyId);
                    _basicEnemyState.Add(enemyId, banditState);
                }
            }
        }
        public static BasicEnemyBaseState GetAssignedBanditState(int enemyId) //=> _basicEnemyState[enemyId];
		{
            if (!_basicEnemyState.ContainsKey(enemyId)) return null;

            return _basicEnemyState[enemyId];
        }
        public static void AssignBanditFaceDir(int forBanditId, int faceDir)
        {
            //BanditCollision.cs->UpdateDetectEnemyTargets()

            _mostRecentCollidedBasicEnemyId = forBanditId;
            if (!_enemyFaceDir.ContainsKey(forBanditId))
                _enemyFaceDir.Add(forBanditId, faceDir);
            else
            {
                if (_enemyFaceDir[forBanditId] != faceDir)
                {
                    _enemyFaceDir.Remove(forBanditId);
                    _enemyFaceDir.Add(forBanditId, faceDir);
                }
            }
        }
        public static void AssignBanditStats(int forBanditId, ref BanditStats characterStat)
		{
            _mostRecentCollidedBasicEnemyId = forBanditId;
            if (!_basicEnemyStats.ContainsKey(forBanditId))
                _basicEnemyStats.Add(forBanditId, characterStat);
            else
            {
                if (_basicEnemyStats[forBanditId] != characterStat)
                {
                    _basicEnemyStats.Remove(forBanditId);
                    _basicEnemyStats.Add(forBanditId, characterStat);
                }
            }
        }
        public static int GetAssignedBanditFaceDir(int forBanditId) => _enemyFaceDir[forBanditId];
        public static BanditStats GetAssignedBanditStatus(int forBanditId) //=> _basicEnemyStats[forBanditId];
		{
            if (!_basicEnemyStats.ContainsKey(forBanditId)) return null;

            return _basicEnemyStats[forBanditId];
        }
        public static int GetRecentCollidedBanditId() => _mostRecentCollidedBasicEnemyId;



        public static void Subscribe(string eventName, Action listener)
        {
            Action thisEvent;

            //out: differs from the ref keyword in that it does not require parameter variables to be
            //initialized before they are passed to a method. Must be explicitly declared in the method
            //definition​ as well as in the calling method.
            if (_instance._battleEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add another event to the existing ones.
                thisEvent += listener;

                //Update the dictionary.
                _instance._battleEventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add the event to the dictionary for the first time.
                thisEvent += listener;
                _instance._battleEventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void Unsubscribe(string eventName, Action listener)
        {
            if (_battleEventManager == null) //Guard Clause.
                return;

            Action thisEvent;
            if (_instance._battleEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove the event from the existing ones.
                thisEvent -= listener;

                //Now update the dictionary.
                _instance._battleEventDictionary[eventName] = thisEvent;
            }
        }
        private static void BaseTriggerEvent(string eventName)
		{
            Action thisEvent = null;
            if (_instance._battleEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                _instance._battleEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }
        public static void TriggerEvent(string eventName)
        {
            BaseTriggerEvent(eventName);
        }



    }
}
