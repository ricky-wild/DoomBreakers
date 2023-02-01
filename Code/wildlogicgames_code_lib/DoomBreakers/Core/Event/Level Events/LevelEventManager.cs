using System;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace DoomBreakers
{
	public class LevelEventManager : MonoBehaviour
	{
        //[Header("UI Level Event Text")]
        private static TMPro.TextMeshProUGUI _levelEventText; //LevelEvent Text (TMP)

        //<summary>
        //The level event manager' purpose is to communicate when a check point has been reached or not
        //by Players. It should serve players the check point position upon their Respawning state.
        //It will also be responsible for other events that happen through one level to the next.
        //These will include the DoomPortal events as well as ShopKeeper & NPC events.
        //
        //</summary>
        private Dictionary<string, Action> _levelEventDictionary;
        private static LevelEventManager _levelEventManager;

        private static Dictionary<int, Campsite> _campsitesReached;

        private static ProCamera2DCinematics _cameraCinematics;
        private static Transform _prevTransform;
        private static bool _endOfLevel, _startOfLevel;
        private static float _startLevelDuration = 3.0f;
        private static float _endLevelDuration = 20.0f;
        private static ITimer _timer;

        public static LevelEventManager _instance
        {
            get
            {
                if (!_levelEventManager)
                {
                    _levelEventManager = FindObjectOfType(typeof(LevelEventManager)) as LevelEventManager;

                    if (!_levelEventManager)
                        print("\nLevelEvenetManager=There needs to be one active LevelEventManger script on a GameObject in your scene!");
                    else
                        _levelEventManager.Initialize();
                }

                return _levelEventManager;
            }
        }

		private static void Awake() => _levelEventManager.Initialize();

        private void Initialize()
        {
            if (_levelEventDictionary == null) _levelEventDictionary = new Dictionary<string, Action>();
            if (_campsitesReached == null) _campsitesReached = new Dictionary<int, Campsite>();
            if (_cameraCinematics == null) _cameraCinematics = ProCamera2D.Instance.GetComponent<ProCamera2DCinematics>();
            _endOfLevel = false;
            _startOfLevel = false;
            //_levelEventText = FindObjectOfType(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
            GameObject go = GameObject.Find("LevelEvent Text (TMP)");// as TMPro.TextMeshProUGUI;
            _levelEventText = go.GetComponent<TMPro.TextMeshProUGUI>();
            _levelEventText.text = "";
            _timer = new Timer();
        }
        public static void PluginCampsites(int campsiteId, ref Campsite campsite)
		{
            if (!_campsitesReached.ContainsKey(campsiteId)) 
                _campsitesReached.Add(campsiteId, campsite);
        }
        public static void ActivateCampsite(int campsiteId) //=> _campsitesReached[campsiteId].Activate();
		{
            if (_campsitesReached[campsiteId].GetState() == CampfireAnimID.Running) return;
            TriggerEvent("CampsiteReachedEvent");
            _campsitesReached[campsiteId].Activate();
        }
        public static void ActivateStartLevel(Transform transform)
        {
            if (!_startOfLevel) _startOfLevel = true;
            _levelEventText.text = "YOU ENTER THE FOREST";
            _timer.StartTimer(_startLevelDuration);
            PlayCinematic(transform, 3.0f, _startLevelDuration, 1.5f);
        }
        public static void ActivateEndLevel(Transform transform)
		{
            if (!_endOfLevel) _endOfLevel = true;
            _levelEventText.text = "YOU LEAVE THE FOREST";
            _timer.StartTimer(_endLevelDuration);
            PlayCinematic(transform, 3.0f, _endLevelDuration, 1.5f);
        }
        public static void PlayCinematic(Transform transform, float easeInDuration, float holdDuration, float zoomAmount)
		{
            _cameraCinematics.RemoveCinematicTarget(_prevTransform);
            _cameraCinematics.RemoveCinematicTarget(transform);
            _cameraCinematics.AddCinematicTarget(transform, easeInDuration, holdDuration, zoomAmount, EaseType.EaseIn, "", "", 0);
            _cameraCinematics.LetterboxAmount = 0.133f;
            _cameraCinematics.Play();

            _prevTransform = transform;

        }
        public static void Subscribe(string eventName, Action listener)
        {
            Action thisEvent;

            if (_instance._levelEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                _instance._levelEventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                _instance._levelEventDictionary.Add(eventName, thisEvent);
            }
        }
        public static void Unsubscribe(string eventName, Action listener)
        {
            if (_levelEventManager == null) //Guard Clause.
                return;

            Action thisEvent;
            if (_instance._levelEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                _instance._levelEventDictionary[eventName] = thisEvent;
            }
        }
        public static void TriggerEvent(string eventName)
        {
            Action thisEvent = null;
            if (_instance._levelEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
                // OR USE instance.eventDictionary[eventName]();
            }
        }


		private void Update()
		{
            UpdateStartOfLevel();
            UpdateEndOfLevel();

        }

        private static void UpdateStartOfLevel()
        {
            if (!_startOfLevel) return;

            if (_timer.HasTimerFinished())
            {
                //Level Begin Here.
                _startOfLevel = false;
                _levelEventText.text = "";
            }
        }
        private static void UpdateEndOfLevel()
		{
            if (!_endOfLevel) return;

            if(_timer.HasTimerFinished())
			{
                //Level Transition Here.
                _endOfLevel = false;
                _levelEventText.text = "";
            }
		}
        public static bool IsStartOfLevel() => _startOfLevel;
        public static bool IsEndOfLevel() => _endOfLevel;
	}
}
