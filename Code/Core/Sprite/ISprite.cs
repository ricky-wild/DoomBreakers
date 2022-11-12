using UnityEngine;
namespace DoomBreakers
{
	public interface ISprite
	{
		void Setup(SpriteRenderer spriteRenderer);
		int GetSpriteDirection();	
		void FlipSprite();
		void SetupTexture2DColorSwap(int texId);
		void ResetTexture2DColor();
		void SetTexture2DColor(Color color);

		void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor);
		//static Color ColorFromInt(int c, float alpha = 1.0f);
		//static Color ColorFromIntRGB(int r, int g, int b)
	}
}