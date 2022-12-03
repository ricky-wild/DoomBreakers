using UnityEngine;

namespace DoomBreakers
{
	public interface IPlayerCollision //: MonoBehaviour
	{
		void Setup(Collider2D collider2D, ref Transform[] arrayAtkPoints, int playerId);
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(ref BaseState playerState, int playerId, ref IPlayerEquipment playerEquipment, ref IPlayerSprite playerSprite);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(ref BaseState playerState, int playerId, ref IPlayerSprite playerSprite);
		void UpdateDetectItemTargets(ref IPlayerEquipment playerEquipment);
		void EnableAttackCollisions();
		//bool IsAttackCollisionsEnabled();
		void FlipAttackPoints(int dir);
	}
}

