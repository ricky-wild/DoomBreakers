using UnityEngine;

namespace DoomBreakers
{
	public interface ICollisionData //: MonoBehaviour
	{
		void PluginPlayer(IPlayer player, int playerId);
		void PluginPlayerState(IPlayerStateMachine playerStateMachine, int playerId);
		void PluginEnemyState(IEnemyStateMachine enemyStateMachine, int banditId);
		void PluginPlayerSprite(IPlayerSprite playerSprite, int playerId);
		void PluginBanditSprite(IBanditSprite banditSprite, int banditId);
		void PluginTargetTransform(Transform transform);

		IPlayer GetCachedPlayer(int playerId);
		int GetLastCollidedPlayerID();
		IPlayerStateMachine GetCachedPlayerState(int playerId);
		IEnemyStateMachine GetCachedEnemyState(int banditId);
		IPlayerSprite GetCachedPlayerSprite(int playerId);
		IBanditSprite GetCachedBanditSprite(int banditId);
		Transform GetCachedTargetTransform();


		bool SomeDataIsNull();
	}
}

