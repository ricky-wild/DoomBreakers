using UnityEngine;
namespace DoomBreakers
{
	public interface IPlayerSprite
	{
		int GetSpriteDirection();
		void FlipSprite();
		void SetupTexture2DColorSwap();
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
	}
}

