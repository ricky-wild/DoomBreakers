
using UnityEngine;

namespace DoomBreakers
{
    public class CollisionData : ICollisionData //MonoBehaviour
    {
        private int _lastCollidedPlayerId;
        private IPlayer[] _cachedPlayer;
        private IPlayerStateMachine[] _cachedPlayerStateMachine;
        private IEnemyStateMachine[] _cachedEnemyStateMachine;
        private IPlayerSprite[] _cachedPlayerSprite;
        private IBanditSprite[] _cachedBanditSprite;


        public CollisionData()
		{
            _cachedPlayer = new IPlayer[4];

            _cachedPlayerStateMachine = new IPlayerStateMachine[4];
            _cachedPlayerSprite = new IPlayerSprite[4];

            int maxBanditCollisionsSupportedInOneStrike = 8;

            _cachedEnemyStateMachine = new IEnemyStateMachine[maxBanditCollisionsSupportedInOneStrike];
            _cachedBanditSprite = new IBanditSprite[maxBanditCollisionsSupportedInOneStrike];

        }
        void Start() { }
        void Update() { }

        public void PluginPlayer(IPlayer player, int playerId)
		{
            _cachedPlayer[playerId] = player;
            _lastCollidedPlayerId = playerId;

        }
        public void PluginPlayerState(IPlayerStateMachine playerStateMachine, int playerId)
		{
            _cachedPlayerStateMachine[playerId] = playerStateMachine;
        }
        public void PluginEnemyState(IEnemyStateMachine enemyStateMachine, int enemyId)
		{
            _cachedEnemyStateMachine[enemyId] = enemyStateMachine;
        }
        public void PluginPlayerSprite(IPlayerSprite playerSprite, int playerId)
		{
            _cachedPlayerSprite[playerId] = playerSprite;
        }
        public void PluginBanditSprite(IBanditSprite banditSprite, int enemyId)
		{
            _cachedBanditSprite[enemyId] = banditSprite;
        }


        public IPlayer GetCachedPlayer(int playerId)
		{
            return _cachedPlayer[playerId];
		}
        public int GetLastCollidedPlayerID()
		{
            return _lastCollidedPlayerId;
		}
        public IPlayerStateMachine GetCachedPlayerState(int playerId)
		{
            return _cachedPlayerStateMachine[playerId];
		}
        public IEnemyStateMachine GetCachedEnemyState(int enemyId)
		{
            return _cachedEnemyStateMachine[enemyId];
		}
        public IPlayerSprite GetCachedPlayerSprite(int playerId)
		{
            return _cachedPlayerSprite[playerId];//.GetSpriteDirection();
		}
        public IBanditSprite GetCachedBanditSprite(int enemyId)
		{
            return _cachedBanditSprite[enemyId];//.GetSpriteDirection();
		}


        public bool SomeDataIsNull()
		{
            if (_cachedBanditSprite == null)
                return true;
            if (_cachedPlayerSprite == null)
                return true;
            if (_cachedEnemyStateMachine == null)
                return true;
            if (_cachedPlayerStateMachine == null)
                return true;


            return false;
		}
    }
}

