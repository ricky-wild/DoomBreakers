using UnityEngine;

namespace DoomBreakers
{
	public interface ICollisionData //: MonoBehaviour
	{
		void RegisterCollision(IPlayerStateMachine playerStateMachine, IEnemyStateMachine enemyStateMachine,
			IPlayerSprite playerSprite, IBanditSprite banditSprite);
		void PluginPlayerState(IPlayerStateMachine playerStateMachine);
		void PluginEnemyState(IEnemyStateMachine enemyStateMachine);
		void PluginPlayerSprite(IPlayerSprite playerSprite);
		void PluginBanditSprite(IBanditSprite banditSprite);

		IPlayerStateMachine GetCachedPlayerState();
		IEnemyStateMachine GetCachedEnemyState();
		IPlayerSprite GetCachedPlayerSprite();
		IBanditSprite GetCachedBanditSprite();
	}
}

