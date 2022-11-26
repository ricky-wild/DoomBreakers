using UnityEngine;
namespace DoomBreakers
{
	public interface IBanditSprite
	{
		void Setup(SpriteRenderer spriteRenderer, int playerID);
		int GetSpriteDirection();
		void FlipSprite();
		void SetupTexture2DColorSwap(string texName, int texId);
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(float time, Color color);
		void SetBehaviourTextureFlash(float time, Color colour);
		void SetWeaponChargeTextureFXFlag(bool b);
	}
}

