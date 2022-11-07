using UnityEngine;

namespace DoomBreakers
{
	public interface ICollisionData //: MonoBehaviour
	{
		void PluginPlayer(IPlayer player, int playerId);
		void PluginPlayerState(IPlayerStateMachine playerStateMachine);
		void PluginEnemyState(IEnemyStateMachine enemyStateMachine);
		void PluginPlayerSprite(IPlayerSprite playerSprite);
		void PluginBanditSprite(IBanditSprite banditSprite);

		IPlayer GetCachedPlayer(int playerId);
		IPlayerStateMachine GetCachedPlayerState();
		IEnemyStateMachine GetCachedEnemyState();
		IPlayerSprite GetCachedPlayerSprite();
		IBanditSprite GetCachedBanditSprite();
	}
}

