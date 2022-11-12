using UnityEngine;

namespace DoomBreakers
{
    public class Sprite : MonoBehaviour, ISprite
    {
		private SpriteRenderer _spriteRenderer;
		private Texture2D _colorSwapTexture2D;
		private Color[] _colorSwapTextureColors;
		private int _spriteFaceDirection;
		private ITimer _colorSwappedTimer;
		private bool _colorSwappedFlag;

		public virtual void Setup(SpriteRenderer spriteRenderer)
		{
			_spriteRenderer = spriteRenderer;
			_spriteFaceDirection = 1; //1 = face right, -1 = face left.

			_colorSwappedTimer = this.gameObject.AddComponent<Timer>();
			_colorSwappedTimer.Setup("_colorSwappedTimer");
			_colorSwappedFlag = false;

			//SetupTexture2DColorSwap();
		}
		public virtual int GetSpriteDirection()
		{
			return _spriteFaceDirection;
		}
		public virtual void FlipSprite()
		{
			if (_spriteFaceDirection == 1) //Facing Right
			{
				_spriteFaceDirection = -1; //Then set to face left.
				_spriteRenderer.flipX = true;
				return;
			}
			if (_spriteFaceDirection == -1) //Facing Left
			{
				_spriteFaceDirection = 1; //Then set to face right.
				_spriteRenderer.flipX = false;
				return;
			}
		}
		public virtual void SetupTexture2DColorSwap(int texId)
		{
			_colorSwapTexture2D = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
			_colorSwapTexture2D.filterMode = FilterMode.Point;

			int texturePixelWidth = _colorSwapTexture2D.width;
			for (int i = 0; i < texturePixelWidth; ++i)
				_colorSwapTexture2D.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

			_colorSwapTexture2D.Apply();


			_spriteRenderer.material.SetTexture("_SwapTex" + texId, _colorSwapTexture2D);

			_colorSwapTextureColors = new Color[texturePixelWidth];
		}
		public virtual void ResetTexture2DColor()
		{
			int texturePixelWidth = _colorSwapTexture2D.width;

			for (int i = 0; i < texturePixelWidth; ++i)
			{
				_colorSwapTexture2D.SetPixel(i, 0, _colorSwapTextureColors[i]);
			}

			if (_colorSwappedFlag)
				_colorSwappedFlag = false;

			_colorSwapTexture2D.Apply();
		}
		public virtual void SetTexture2DColor(Color color)
		{
			int texturePixelWidth = _colorSwapTexture2D.width;
			for (int i = 0; i < texturePixelWidth; ++i)
			{
				_colorSwapTexture2D.SetPixel(i, 0, color);
			}

			if (!_colorSwappedFlag) //Ensure we reset internally upon failure to do so externally (ie a state change)
			{
				_colorSwappedTimer.StartTimer(0.5f);
				_colorSwappedFlag = true;
			}

			_colorSwapTexture2D.Apply();
		}
		
		public virtual void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor)
		{
			_colorSwapTextureColors[(int)indexOfColourToSwap] = replacementColor;
			_colorSwapTexture2D.SetPixel((int)indexOfColourToSwap, 0, replacementColor);
		}
		public virtual Color ColorFromInt(int c, float alpha = 1.0f)
		{
			int r = (c >> 16) & 0x000000FF;
			int g = (c >> 8) & 0x000000FF;
			int b = c & 0x000000FF;

			Color ret = ColorFromIntRGB(r, g, b);
			ret.a = alpha;

			return ret;
		}

		public virtual Color ColorFromIntRGB(int r, int g, int b)
		{
			return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, 1.0f);
		}

		void Start() { }

        void Update() { }
    }
}

