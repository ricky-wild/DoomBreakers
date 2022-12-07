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
        //</summary>
        private Dictionary<string, Action> _battleEventDictionary;
        private static BattleColliderManager _battleEventManager;


        private static int _mostRecentCollidedPlayerId;
        private static Dictionary<int, int> _playerFaceDir;
        private static Dictionary<int, BaseState> _playerState;
        private static Dictionary<int, ItemBase> _playerWeapon;
        private static float _playerAttackButtonHeldTime;

        //private static EnemyAI _enemyType;
        private static int _mostRecentCollidedBanditId; //Needed for Enemy Hit By Power Attack behaviour state.
        private static Dictionary<int, int> _banditFaceDir;
        private static Dictionary<int, BanditBaseState> _banditState;


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
            if (_battleEventDictionary == null)
                _battleEventDictionary = new Dictionary<string, Action>();

            if (_playerFaceDir == null)
                _playerFaceDir = new Dictionary<int, int>();
            if (_playerState == null)
                _playerState = new Dictionary<int, BaseState>();
            if (_playerWeapon == null)
                _playerWeapon = new Dictionary<int, ItemBase>();

            if (_banditFaceDir == null)
                _banditFaceDir = new Dictionary<int, int>();
            if (_banditState == null)
                _banditState = new Dictionary<int, BanditBaseState>();

            _mostRecentCollidedPlayerId = 0;
            _playerAttackButtonHeldTime = 0f;
            //_enemyType = EnemyAI.None;
        }

        //<summary>
        //Bundle all appropriate collision assignment calls into one method call.
        //This is so far, AssignPlayerState(), AssignPlayerFaceDir() and TriggerEvent("ReportCollisionWithSomething").
        //</summary>
        public static void AssignCollisionDetails(string eventName, ref BaseState playerState, int forPlayerId, IPlayerSprite playerSprite)
		{
            AssignPlayerFaceDir(forPlayerId, playerSprite);
            AssignPlayerState(ref playerState, forPlayerId);
            BaseTriggerEvent(eventName);
        }
        
        //<summary>
        //AssignPlayerState() is used to communicate player state during an attack. 
        //We will need this for enemy AI regarding the Bandit.cs->AttackedByPlayer() so
        //we can determine the enemy' next state, as appropriate.
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
        public static BaseState GetAssignedPlayerState(int playerId) => _playerState[playerId];
        
        //<summary>
        //AssignPlayerFaceDir() is used to communicate player face direction during an attack. 
        //We will need this for enemy AI regarding the HitByPowerAttack obj state. 
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
        public static int GetRecentCollidedPlayerId() => _mostRecentCollidedPlayerId;
        public static void SetPlayerHeldAttackButtonTime(float time) => _playerAttackButtonHeldTime = time;
        public static float GetPlayerHeldAttackButtonTime() => _playerAttackButtonHeldTime;




        //<summary>
        //Bundle all appropriate collision assignment calls into one method call.
        //This is so far, AssignBanditState(), AssignBanditFaceDir() and TriggerEvent("ReportCollisionWithSomething").
        //We'll overload this from Player use for Bandit use.
        //</summary>
        public static void AssignCollisionDetails(string eventName, ref BanditBaseState banditState, int forBanditId, IBanditSprite banditSprite)
        {
            AssignBanditFaceDir(forBanditId, banditSprite);
            AssignBanditState(ref banditState, forBanditId);
            BaseTriggerEvent(eventName);
        }
        public static void AssignBanditState(ref BanditBaseState banditState, int banditId)
        {
            if (!_banditState.ContainsKey(banditId))
                _banditState.Add(banditId, banditState);
            else
            {
                if (banditState.GetType() != _banditState[banditId].GetType())
                {
                    _banditState.Remove(banditId);
                    _banditState.Add(banditId, banditState);
                }
            }
        }
        public static BanditBaseState GetAssignedBanditState(int banditId) => _banditState[banditId];
        public static void AssignBanditFaceDir(int forBanditId, IBanditSprite banditSprite)
        {
            //BanditCollision.cs->UpdateDetectEnemyTargets()

            _mostRecentCollidedBanditId = forBanditId;
            if (!_banditFaceDir.ContainsKey(forBanditId))
                _banditFaceDir.Add(forBanditId, banditSprite.GetSpriteDirection());
            else
            {
                if (_banditFaceDir[forBanditId] != banditSprite.GetSpriteDirection())
                {
                    _banditFaceDir.Remove(forBanditId);
                    _banditFaceDir.Add(forBanditId, banditSprite.GetSpriteDirection());
                }
            }
        }
        public static int GetAssignedBanditFaceDir(int forBanditId) => _banditFaceDir[forBanditId];
        public static int GetRecentCollidedBanditId() => _mostRecentCollidedBanditId;



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
