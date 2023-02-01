

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum EnemyAI
	{
        None = -1,
        Bandit = 0,
        BanditArcher = 1,
        Skeleton = 2
	};
    public class AITargetTrackingManager : MonoBehaviour
    {
        //<summary>
        //The AI target tracking manager purpose is to retrieve and supply targets to persue for
        //individual enemy AI. The targets will be binded by each enemy ID assigned and used
        //as appropriate via their ID' and type (see EnemyAI enum above).
        //Relevant data will be plugged in via BanditCollision.cs trigger and pulled out-used from
        //BanditPersue : BasicEnemyBaseState class.
        //</summary>

        private static AITargetTrackingManager _targetTrackingEventManager;


        private static Dictionary<int, Transform> _banditTargetTransforms = new Dictionary<int, Transform>();
        private static Dictionary<int, Transform> _banditArcherTargetTransforms = new Dictionary<int, Transform>();
        //private static Dictionary<int, bool> _banditArcherShooting = new Dictionary<int, bool>();
        
        private Dictionary<string, Action> _trackingEventDictionary = new Dictionary<string, Action>();
        public static AITargetTrackingManager _instance
        {
            get //When we access our instance from another place, we'll setup as appropriate if required.
            {
                if (!_targetTrackingEventManager)
                {
                    //FindObjectOfType isn't a cheap call but we only do this once, if not at all.
                    _targetTrackingEventManager = FindObjectOfType(typeof(AITargetTrackingManager)) as AITargetTrackingManager;

                    if (!_targetTrackingEventManager)
                        print("\nAITargetTrackingManager= You need an active AITargetTrackingManager script attached to a GameObject within the scene!");
                    else
                        _targetTrackingEventManager.Setup();
                }

                return _targetTrackingEventManager;
            }
        }
        private void Setup()
        {
            if (_banditTargetTransforms == null) _banditTargetTransforms = new Dictionary<int, Transform>();
            if (_banditArcherTargetTransforms == null) _banditArcherTargetTransforms = new Dictionary<int, Transform>();
            if (_trackingEventDictionary == null) _trackingEventDictionary = new Dictionary<string, Action>();
            //if (_banditArcherShooting == null) _banditArcherShooting = new Dictionary<int, bool>();
        }

        //public static void SetArcher
        public static void AssignTargetTransform(string eventName, Transform targetTransform, int forEnemyId, EnemyAI forEnemyType)
        {
            switch(forEnemyType)
			{
                case EnemyAI.Bandit:
                    if(!_banditTargetTransforms.ContainsKey(forEnemyId))
					{
                        _banditTargetTransforms.Add(forEnemyId, targetTransform);
                        //TriggerEvent(eventName);
                    }
                    else
                    {
                        if(_banditTargetTransforms[forEnemyId] != targetTransform)
						{
                            _banditTargetTransforms.Remove(forEnemyId);
                            _banditTargetTransforms.Add(forEnemyId, targetTransform);
                            //TriggerEvent(eventName);
                        }
                    }
                    TriggerEvent(eventName);
                    break;
                case EnemyAI.BanditArcher:
                    //if (!_banditArcherTargetTransforms.ContainsKey(forEnemyId))
                    //{
                    //    _banditArcherTargetTransforms.Add(forEnemyId, targetTransform);
                    //    //TriggerEvent(eventName);
                    //}
                    //else
                    //{
                    //    if (_banditArcherTargetTransforms[forEnemyId] != targetTransform)
                    //    {
                            _banditArcherTargetTransforms.Remove(forEnemyId);
                            _banditArcherTargetTransforms.Add(forEnemyId, targetTransform);
                            //TriggerEvent(eventName);
                    //    }
                    //}
                    TriggerEvent(eventName);
                    break;
                case EnemyAI.Skeleton:
                    break;
			}
            
        }
        public static Transform GetAssignedTargetTransform(int forEnemyId, EnemyAI forEnemyType)
		{
            if (_banditTargetTransforms == null) return null;
            if (forEnemyType == EnemyAI.Bandit)
			{
                //if (_banditTargetTransforms.ContainsKey(forEnemyId)) //TryGetValue(key, out value)
                //    return _banditTargetTransforms[forEnemyId];
                Transform transformValue;
                if (_banditTargetTransforms.TryGetValue(forEnemyId, out transformValue))
                    return transformValue;
            }
            if (forEnemyType == EnemyAI.BanditArcher)
            {
                //if (_banditTargetTransforms.ContainsKey(forEnemyId)) //TryGetValue(key, out value)
                //    return _banditTargetTransforms[forEnemyId];
                Transform transformValue;
                if (_banditArcherTargetTransforms.TryGetValue(forEnemyId, out transformValue))
                    return transformValue;
            }
            if (forEnemyType == EnemyAI.Skeleton)
            {
                return null;
            }

            return null;
		}

        public static void Subscribe(string eventName, Action listener)
        {
            Action thisEvent;

            //out: differs from the ref keyword in that it does not require parameter variables to be
            //initialized before they are passed to a method. Must be explicitly declared in the method
            //definition​ as well as in the calling method.
            if (_instance._trackingEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add another event to the existing ones.
                thisEvent += listener;

                //Update the dictionary.
                _instance._trackingEventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add the event to the dictionary for the first time.
                thisEvent += listener;
                _instance._trackingEventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void Unsubscribe(string eventName, Action listener)
        {
            if (_targetTrackingEventManager == null) //Guard Clause.
                return;

            Action thisEvent;
            if (_instance._trackingEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove the event from the existing ones.
                thisEvent -= listener;

                //Now update the dictionary.
                _instance._trackingEventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName)
        {
            if (_instance._trackingEventDictionary == null) _instance.Setup();
            Action thisEvent = null;
            if (_instance._trackingEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                _instance._trackingEventDictionary[eventName]();
                //thisEvent.Invoke();
            }
        }

    }
}
