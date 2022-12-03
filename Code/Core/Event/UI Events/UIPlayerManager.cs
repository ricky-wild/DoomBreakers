using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{

    public class UIPlayerManager : MonoBehaviour
    {
        //<summary>
        //The UIPlayerManager purpose is to retrieve and supply appropriate information
        //between each Player.cs class that relates to each PlayerUI.cs panel.
        //</summary>



        private static UIPlayerManager _UIPlayerEventManager;

        private Dictionary<string, Action> _UIEventDictionary;

        private static Dictionary<int, PlayerStats> _playerStatsEventDictionary;
        private static UIAnimationFlag[] _equipmentGainedEvent;// = new EquipmentGainedFlag[4]; //Max 4 players
        public static UIPlayerManager _instance
        {
            get //When we access our instance from another place, we'll setup as appropriate if required.
            {
                if (!_UIPlayerEventManager)
                {
                    //FindObjectOfType isn't a cheap call but we only do this once, if not at all.
                    _UIPlayerEventManager = FindObjectOfType(typeof(UIPlayerManager)) as UIPlayerManager;

                    if (!_UIPlayerEventManager)
                        print("\nUIPlayerManager= You need an active UIPlayerManager script attached to a GameObject within the scene!");
                    else
                        _UIPlayerEventManager.Setup();
                }

                return _UIPlayerEventManager;
            }
        }
        private void Setup()
        {
            if (_UIEventDictionary == null) _UIEventDictionary = new Dictionary<string, Action>();
            if (_playerStatsEventDictionary == null) _playerStatsEventDictionary = new Dictionary<int, PlayerStats>();
            _equipmentGainedEvent = new UIAnimationFlag[4]; //Max 4 players
        }

        public static void Subscribe(string eventName, Action listener)
        {
            Action thisEvent;

            //out: differs from the ref keyword in that it does not require parameter variables to be
            //initialized before they are passed to a method. Must be explicitly declared in the method
            //definition​ as well as in the calling method.
            if (_instance._UIEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add another event to the existing ones.
                thisEvent += listener;

                //Update the dictionary.
                _instance._UIEventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add the event to the dictionary for the first time.
                thisEvent += listener;
                _instance._UIEventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void Unsubscribe(string eventName, Action listener)
        {
            if (_UIPlayerEventManager == null) //Guard Clause.
                return;

            Action thisEvent;
            if (_instance._UIEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove the event from the existing ones.
                thisEvent -= listener;

                //Now update the dictionary.
                _instance._UIEventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, ref PlayerStats playerStats, int playerId)
        {
            Action thisEvent = null;
            if (_instance._UIEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                if(!_playerStatsEventDictionary.ContainsKey(playerId)) _playerStatsEventDictionary.Add(playerId, playerStats);
                else
				{
                    if(_playerStatsEventDictionary[playerId] != playerStats)
					{
                        _playerStatsEventDictionary.Remove(playerId);
                        _playerStatsEventDictionary.Add(playerId, playerStats);
					}
				}
                _instance._UIEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }

        public static PlayerStats GetPlayerStats(int playerId) => _playerStatsEventDictionary[playerId];
        public static void SetPlayerStats(ref PlayerStats playerStats, int playerId) => _playerStatsEventDictionary[playerId] = playerStats;

        public static void TriggerEvent(string eventName, UIAnimationFlag equipmentGainedFlag, int playerId)
        {
            Action thisEvent = null;
            if (_instance._UIEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                if (_equipmentGainedEvent[playerId] != equipmentGainedFlag)
                    _equipmentGainedEvent[playerId] = equipmentGainedFlag;
                _instance._UIEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }
        public static UIAnimationFlag GetEquipmentGainedFlag(int playerId)
		{
            UIAnimationFlag temp = _equipmentGainedEvent[playerId];
            _equipmentGainedEvent[playerId] = UIAnimationFlag.None;
            return temp;
        }
    }
}