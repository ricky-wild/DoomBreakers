

using UnityEngine;

namespace DoomBreakers
{
    public abstract class BasicNPCStateMachine : MonoBehaviour
    {
        protected BasicNPCBaseState _state; //So we delegate behaviours down to the state.
        public void SetState(BasicNPCBaseState state) => _state = state;

    }
}
