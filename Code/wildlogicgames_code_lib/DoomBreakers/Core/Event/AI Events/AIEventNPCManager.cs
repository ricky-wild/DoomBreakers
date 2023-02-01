using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class AIEventNPCManager : MonoBehaviour
	{
        //<summary>
        //The AI events NPC manager purpose is to assign the correct behaviour to an NPC
        //based on their NPC Collision trigger. They will either need to Jump due to platform collision,
        //Follow a Player they've collided with, Rest at a campsite checkpoint they've collided with and
        //finally, Flee from an Enemy Ai collided with.
        //</summary>

        private static AIEventNPCManager _npcEventManager;

        private Dictionary<string, Action> _npcEventDictionary;

        public static AIEventNPCManager _instance
        {
            get //When we access our instance from another place, we'll setup as appropriate if required.
            {
                if (!_npcEventManager)
                {
                    //FindObjectOfType isn't a cheap call but we only do this once, if not at all.
                    _npcEventManager = FindObjectOfType(typeof(AIEventNPCManager)) as AIEventNPCManager;

                    if (!_npcEventManager)
                        print("\nAIEventNPCManager= You need an active AIEventNPCManager script attached to a GameObject within the scene!");
                    else
                        _npcEventManager.Setup();
                }

                return _npcEventManager;
            }
        }

        private void Setup()
        {
            if (_npcEventDictionary == null) _npcEventDictionary = new Dictionary<string, Action>();
        }

        public static void Subscribe(string eventName, Action listener)
        {
            Action thisEvent;

            //out: differs from the ref keyword in that it does not require parameter variables to be
            //initialized before they are passed to a method. Must be explicitly declared in the method
            //definition​ as well as in the calling method.
            if (_instance._npcEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add another event to the existing ones.
                thisEvent += listener;

                //Update the dictionary.
                _instance._npcEventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add the event to the dictionary for the first time.
                thisEvent += listener;
                _instance._npcEventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void Unsubscribe(string eventName, Action listener)
        {
            if (_instance == null) //Guard Clause.
                return;
            if (_instance._npcEventDictionary == null) //Guard Clause.
                return;

            Action thisEvent;
            if (_instance._npcEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove the event from the existing ones.
                thisEvent -= listener;

                //Now update the dictionary.
                _instance._npcEventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName)
        {
            Action thisEvent = null;
            if (_instance._npcEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                _instance._npcEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }
    }
}
