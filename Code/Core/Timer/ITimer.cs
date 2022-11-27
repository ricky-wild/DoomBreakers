

namespace DoomBreakers
{
    public interface ITimer 
    {
        void Setup(string timerTitle);
        void StartTimer(float waitingTime);
        bool HasTimerFinished();
        float GetTheWaitTime();
        void BeginTimeRecord();
        void FinishTimeRecord();
        float GetTimeRecord();
    }
}

