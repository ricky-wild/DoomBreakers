
using UnityEngine;

namespace DoomBreakers
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected BaseState _state; //So we delegate behaviours down to the state.

        public void SetState(BaseState state)
		{
            _state = state;
            //_state.IsIdle();
		}

        public BaseState GetState()//Evil yet here we are.
		{
            return _state;
		}


    }
}

