
using UnityEngine;

namespace DoomBreakers
{
    interface IEnemy //: MonoBehaviour
    {
        void UpdatePlayerPathFinding();
        void UpdateStateBehaviours();
        void UpdateAnimator();
        void UpdateCollisions();
        void ReportCollisionWithPlayer(IPlayerStateMachine playerStateMachine);
    }
}
