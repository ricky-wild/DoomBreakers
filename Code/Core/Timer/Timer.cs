
using UnityEngine;

namespace DoomBreakers
{
    public class Timer : MonoBehaviour, ITimer
    {
        private float _timeStamp;
        private float _waitTime;
        private bool _timeStamped, _timerCompleted;

        public Timer()
		{
            _timeStamp = 0f;
            _waitTime = 0f;
            _timeStamped = false;
            _timerCompleted = false;
        }

        public void Setup()
		{
            _timeStamp = 0f;
            _waitTime = 0f;
            _timeStamped = false;
            _timerCompleted = false;
        }

        public void StartTimer(float waitingTime)
		{
            if (_timeStamped) //Guard Clause
                return;

            _timerCompleted = false;
            _waitTime = waitingTime;
            _timeStamp = Time.time;
            _timeStamped = true;
        }

        void Update()
		{
            //if (!_timeStamped) //Guard Clause
            //    return;

            //if (DetectTimePassed())
            //    _timerCompleted = true;

        }

        public bool HasTimerFinished()
		{
            return DetectTimePassed();//_timerCompleted;
        }

        private bool DetectTimePassed()
		{
            //if (!_timeStamped)
            //    return false;

            if (Time.time > _timeStamp + _waitTime)
            {
                //Reset variables to allow more time stamps detections.
                _timeStamp = 0f;
                _timeStamped = false;
                return true;
            }

            return false;
		}



    }
}


