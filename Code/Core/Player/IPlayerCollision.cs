using UnityEngine;

namespace DoomBreakers
{
	public interface IPlayerCollision //: MonoBehaviour
	{
		void Setup(Collider2D collider2D, ref Transform[] arrayAtkPoints);
		void SetupLayerMasks();
		void SetupAttackRadius();
		void SetupCompareTags();
		string GetCompareTag(CompareTags compareTagId);
		void UpdateCollision(ref BaseState playerState, int playerId, ref IPlayerEquipment playerEquipment);
		void ProcessCollisionFlags(Collider2D collision);
		void UpdateDetectEnemyTargets(ref BaseState playerState, int playerId);
		void UpdateDetectItemTargets(ref IPlayerEquipment playerEquipment);
		void EnableAttackCollisions();
		//bool IsAttackCollisionsEnabled();
		void FlipAttackPoints(int dir);
	}
}

