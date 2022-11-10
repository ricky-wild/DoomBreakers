using UnityEngine;
namespace DoomBreakers
{
	public interface IPlayerSprite
	{
		void Setup(SpriteRenderer spriteRenderer, int playerID);
		int GetSpriteDirection();
		WeaponChargeHold GetWeaponTexChargeFlag();
		void FlipSprite();
		void SetupTexture2DColorSwap();
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
		void SetWeaponChargeTextureFXFlag(bool b);
	}
}

