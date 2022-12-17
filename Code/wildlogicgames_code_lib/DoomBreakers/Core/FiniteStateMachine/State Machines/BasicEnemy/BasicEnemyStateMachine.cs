
using UnityEngine;

namespace DoomBreakers
{
    public abstract class BasicEnemyStateMachine : MonoBehaviour
    {
        protected BasicEnemyBaseState _state; //So we delegate behaviours down to the state.


        public void SetState(BasicEnemyBaseState state)
        {
            _state = state;
            //_state.IsIdle();
        }

        public BasicEnemyBaseState GetState()//Evil yet here we are.
        {
            return _state;
        }


    }
}
