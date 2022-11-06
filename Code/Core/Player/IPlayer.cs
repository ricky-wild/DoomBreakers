
namespace DoomBreakers
{
    interface IPlayer //: MonoBehaviour
    {
        void UpdateInput();
        void UpdateStateBehaviours();
        void UpdateAnimator();
        void UpdateCollisions();
        void ReportCollisionWithEnemy(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite);
    }
}

