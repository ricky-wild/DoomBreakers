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
        }
        //<summary>
        //Bundle all appropriate collision assignment calls into one method call.
        //This is so fasr, AssignPlayerState(), AssignPlayerFaceDir() and TriggerEvent("ReportCollisionWithSomething").
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
        public static int GetAssignedPlayerFaceDir(int forPlayerId) => _playerFaceDir[forPlayerId];
        public static int GetRecentCollidedPlayerId() => _mostRecentCollidedPlayerId;

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
