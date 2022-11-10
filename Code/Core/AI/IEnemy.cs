
using UnityEngine;

namespace DoomBreakers
{
    public interface IEnemy //: MonoBehaviour
    {
        void UpdatePlayerPathFinding();
        void UpdateStateBehaviours();
        void UpdateAnimator();
        void UpdateCollisions();
        void ReportCollisionWithPlayer(ICollisionData collisionData, int playerId);//IPlayerStateMachine playerStateMachine);
    }
}
