
namespace DoomBreakers
{
    public interface IPlayer //: MonoBehaviour
    {
        void UpdateInput();
        void UpdateStateBehaviours();
        void UpdateAnimator();
        void UpdateCollisions();
        //void ReportCollisionWithEnemy(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
        void ReportCollisionWithEnemy(ICollisionData collisionData, int banditId);
    }
}

