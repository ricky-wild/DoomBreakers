

namespace DoomBreakers
{
    interface ITimer 
    {
        void Setup();
        void StartTimer(float waitingTime);
        bool HasTimerFinished();
    }
}

