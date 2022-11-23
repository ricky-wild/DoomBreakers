
using UnityEngine;

namespace DoomBreakers
{
    public abstract class EnemyStateMachine : MonoBehaviour
    {
        protected BanditBaseState _state; //So we delegate behaviours down to the state.


        public void SetState(BanditBaseState state)
        {
            _state = state;
            //_state.IsIdle();
        }

        public BanditBaseState GetState()//Evil yet here we are.
        {
            return _state;
        }


    }
}
