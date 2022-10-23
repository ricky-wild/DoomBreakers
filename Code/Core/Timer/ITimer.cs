

namespace DoomBreakers
{
    public interface ITimer 
    {
        void Setup();
        void StartTimer(float waitingTime);
        bool HasTimerFinished();
        float GetTheWaitTime();
    }
}

