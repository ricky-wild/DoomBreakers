
using UnityEngine;

namespace DoomBreakers
{
    public class CollisionData : ICollisionData //MonoBehaviour
    {
        private IPlayerStateMachine _cachedPlayerStateMachine;
        private IEnemyStateMachine _cachedEnemyStateMachine;
        private IPlayerSprite _cachedPlayerSprite;
        private IBanditSprite _cachedBanditSprite;


        public CollisionData()
		{

		}
        void Start() { }
        void Update() { }
        public void RegisterCollision(IPlayerStateMachine playerStateMachine, IEnemyStateMachine enemyStateMachine,
            IPlayerSprite playerSprite, IBanditSprite banditSprite)
		{
            _cachedPlayerStateMachine = playerStateMachine;
            _cachedEnemyStateMachine = enemyStateMachine;
            _cachedPlayerSprite = playerSprite;
            _cachedBanditSprite = banditSprite;
        }
        public void PluginPlayerState(IPlayerStateMachine playerStateMachine)
		{
            _cachedPlayerStateMachine = playerStateMachine;
        }
        public void PluginEnemyState(IEnemyStateMachine enemyStateMachine)
		{
            _cachedEnemyStateMachine = enemyStateMachine;
        }
        public void PluginPlayerSprite(IPlayerSprite playerSprite)
		{
            _cachedPlayerSprite = playerSprite;
        }
        public void PluginBanditSprite(IBanditSprite banditSprite)
		{
            _cachedBanditSprite = banditSprite;
        }



        public IPlayerStateMachine GetCachedPlayerState()
		{
            return _cachedPlayerStateMachine;
		}
        public IEnemyStateMachine GetCachedEnemyState()
		{
            return _cachedEnemyStateMachine;
		}
        public IPlayerSprite GetCachedPlayerSprite()
		{
            return _cachedPlayerSprite;//.GetSpriteDirection();
		}
        public IBanditSprite GetCachedBanditSprite()
		{
            return _cachedBanditSprite;//.GetSpriteDirection();
		}
    }
}

