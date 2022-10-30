using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private Dictionary<string, Action> _eventDictionary;

    private static EventManager _eventManager;

    public static EventManager _instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                    print("\nEvenetManager=There needs to be one active EventManger script on a GameObject in your scene!");
                else
                    _eventManager.Initialize();
            }

            return _eventManager;
        }
    }

    void Initialize()
    {
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<string, Action>();
    }

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;

        if (_instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            _instance._eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            _instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (_eventManager == null) //Guard Clause.
            return;

        Action thisEvent;
        if (_instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            _instance._eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent = null;
        if (_instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
            // OR USE instance.eventDictionary[eventName]();
        }
    }
}