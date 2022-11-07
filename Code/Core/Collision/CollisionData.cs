
using UnityEngine;

namespace DoomBreakers
{
    public class CollisionData : ICollisionData //MonoBehaviour
    {
        private IPlayer[] _cachedPlayer;
        private IPlayerStateMachine _cachedPlayerStateMachine;
        private IEnemyStateMachine _cachedEnemyStateMachine;
        private IPlayerSprite _cachedPlayerSprite;
        private IBanditSprite _cachedBanditSprite;


        public CollisionData()
		{
            _cachedPlayer = new IPlayer[4];
		}
        void Start() { }
        void Update() { }

        public void PluginPlayer(IPlayer player, int playerId)
		{
            _cachedPlayer[playerId] = player;
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


        public IPlayer GetCachedPlayer(int playerId)
		{
            return _cachedPlayer[playerId];
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

