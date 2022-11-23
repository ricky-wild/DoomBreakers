using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class BattleColliderManager : MonoBehaviour
	{
        //<summary>
        //The battle collider manager' purpose is too communicate collisions between enemy AI & player, vice versa.
        //We will need to communicate position data between player and enemy objects.
        //We will use a fixed array[4] holding player transform data for enemy obj to use.
        //We will use a dictionary<playerID, enemyTransform> for player obj to use.
        //</summary>
        private Dictionary<string, Action> _battleEventDictionary;
        private static BattleColliderManager _battleEventManager;

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
        }
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

        public static void TriggerEvent(string eventName)
        {
            Action thisEvent = null;
            if (_instance._battleEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                _instance._battleEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }


    }
}
