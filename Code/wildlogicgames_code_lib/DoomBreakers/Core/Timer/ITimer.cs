

namespace DoomBreakers
{
    public interface ITimer 
    {
        void Setup(string timerTitle);
        void StartTimer(float waitingTime);
        void Reset();
        bool HasTimerFinished();
        bool HasTimerFinished(bool safeCheck);
        float GetTheWaitTime();
        void BeginTimeRecord();
        void FinishTimeRecord();
        float GetTimeRecord();
    }
}

