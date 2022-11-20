
using UnityEngine;

namespace DoomBreakers
{
    public interface IEnemy //: MonoBehaviour
    {
        void UpdatePlayerPathFinding();
        void UpdateStateBehaviours();
        void UpdateAnimator();
        void UpdateCollisions();
    }
}
