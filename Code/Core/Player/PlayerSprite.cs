
using UnityEngine;

namespace DoomBreakers
{

	public class PlayerSprite : IPlayerSprite //MonoBehaviour
    {
		public enum SpriteColourIndex
		{
			//Supplying red colour value of each pixel we want to swap out for another.

			Black = 0,

			Hair = 59,    //0x 3b370c
			Skin_a = 226, //0x e2d87a  lightest
			Skin_b = 208, //0x d0c774
			Skin_c = 194, //0x c2ba6d
			Skin_d = 177, //0x b1aa64 !!PART OF THE ARMOR!!  181 0Xb5b0b0
			Skin_e = 153, //0x 999455  darkest
			Cloth_a = 4,  //0x 0430ff 0030ff
			Cloth_b = 11, //0x 0b279d ALSO Cape

			ChargeFX = 5, //0X 00ff49
			ChargeIndicator = 3, //0x 039029

			Sword_Standard_a = 228, //0x e4e6c9
			Sword_Standard_b = 211, //0x d3d6ab
			Sword_Standard_c = 181, //0x b5b977
			Sword_Standard_d = 156, //0x 9ca060
			Sword_Standard_e = 203, //0x cbd092
			Sword_Standard_handle_a = 97, //0x 61512f
			Sword_Standard_handle_b = 108, //0x 6c5a34

			Sword_2_Standard_a = 220, //0x dce6c9
			Sword_2_Standard_b = 197, //0x c5d6ab
			Sword_2_Standard_c = 200, //0x c8d092
			Sword_2_Standard_d = 179, //0x b3b977
			Sword_2_Standard_e = 159, //0x 9fa060
			Sword_2_Standard_handle_a = 94, //0x 5e512f
			Sword_2_Standard_handle_b = 106, //0x 6a5a34

			Shield_Standard_a = 169, //0x a9a8a8
			Shield_Standard_b = 124, //0x 7c7b7b
			Shield_Standard_c = 53, //0x 353434
			Shield_Standard_d = 79, //0x 4f1507
			Shield_Standard_e = 130, //0x 823c3d

			Armor_Standard_a = 222, //0x dedede
			Armor_Standard_b = 198, //0x c6c6c6
			Armor_Standard_c = 190, //0x bebebe
			Armor_Standard_d = 184, //0x b8b8b8
			Armor_Standard_e = 181, //0x b5b0b0
			Armor_Standard_f = 168, //0x a8a8a8
			Armor_Standard_g = 123, //0x 7b7b7b
			Armor_Standard_h = 52  //0x 343434        c f g h
		}
		
		private SpriteColourIndex _spriteColourIndex;
		private SpriteRenderer _spriteRenderer;
		private Texture2D _colorSwapTexture2D;
		private Color[] _colorSwapTextureColors;
		private int _playerID, _spriteFaceDirection;

		public PlayerSprite(SpriteRenderer spriteRenderer, int playerID)
		{
            _spriteRenderer = spriteRenderer;
			_playerID = playerID;
            _spriteFaceDirection = 1; //1 = face right, -1 = face left.
            SetupTexture2DColorSwap();
        }
        public int GetSpriteDirection()
		{
            return _spriteFaceDirection;
        }
        public void FlipSprite()
		{
            if(_spriteFaceDirection == 1) //Facing Right
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
		public void ApplyCustomTexture2DColours()
		{
            SetupTexture2DColorSwap();

            //switch (MenuManager._instance.GetPlayerCustomSkinId(_playerID))
            switch(_playerID)
            {
                default: //STANDARD SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x3b370c));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2d87a));  //SKIN A 841e00
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd0c774));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc2ba6d));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb1aa64));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x999455));  //SKIN E
                    break;
                case 1: //DARK SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x230e01));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0x68643d));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0x5a5635));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0x4b492e));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0x353320));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x252316));  //SKIN E
                    break;
                case 2: //WHITER SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x99940d));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xeee698));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xdfd893));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xd4cd8d));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xc6c086));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0xb2ae7a));  //SKIN E
                    break;
                case 3: //ORANGE SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x230e01));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2ba5b));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd0a955));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc29c4e));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb18c45));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x997536));  //SKIN E
                    break;
                case 4: //BLACK SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x855334));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0x27261b));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0x212017));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0x1b1a13));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0x14140f));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x0e0e08));  //SKIN E
                    break;
                case 5: //PALE SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x83550b));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xfbf4b3));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xede6ad));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xe2ddab));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xd2cea3));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0xc6c2a0));  //SKIN E
                    break;
                case 6: //STANDARD ENHANCED SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x83550b));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xffd874));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xffc768));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xffba5a));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xe2aa48));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0xb2942a));  //SKIN E
                    break;
                case 7: //R lobsterish SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x641f14));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe27c7a));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd07274));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc26a6d));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb16064));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x995255));  //SKIN E
                    break;
                case 8: //G zombish SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x858382));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0x8dd84f));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0x82c74c));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0x7aba48));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0x70aa42));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x629439));  //SKIN E
                    break;
                case 9: //B snowwalkerish SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0xe0e0e0));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2d8fc));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd0c7f9));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc2baf5));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb1aaf0));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x9994e8));  //SKIN E
                    break;
                case 10: //GOLDEN SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0xf7bd60));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2d824));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd0c720));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc2ba1c));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb1aa16));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x99940d));  //SKIN E
                    break;
            }

            //Cloth colours are preset accordingly to player Ids. P1 blue P2 red P3 green P4 yellow
            switch (_playerID)
            {
                case 0:
                    SwapTexture2DColor(SpriteColourIndex.Cloth_a, ColorFromInt(0x0430ff)); //CLOTH A
                    SwapTexture2DColor(SpriteColourIndex.Cloth_b, ColorFromInt(0x0b279d)); //CLOTH B
                    break;
                case 1:
                    SwapTexture2DColor(SpriteColourIndex.Cloth_a, ColorFromInt(0xfd5e5e)); //CLOTH A
                    SwapTexture2DColor(SpriteColourIndex.Cloth_b, ColorFromInt(0xff0000)); //CLOTH B
                    break;
                case 2:
                    SwapTexture2DColor(SpriteColourIndex.Cloth_a, ColorFromInt(0x56e25b)); //CLOTH A
                    SwapTexture2DColor(SpriteColourIndex.Cloth_b, ColorFromInt(0x56e25b)); //CLOTH B
                    break;
                case 3:
                    SwapTexture2DColor(SpriteColourIndex.Cloth_a, ColorFromInt(0xd6f85d)); //CLOTH A
                    SwapTexture2DColor(SpriteColourIndex.Cloth_b, ColorFromInt(0xcff806)); //CLOTH B
                    break;
            }
        }

		public void SetupTexture2DColorSwap()
		{
			_colorSwapTexture2D = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
			_colorSwapTexture2D.filterMode = FilterMode.Point;

			int texturePixelWidth = _colorSwapTexture2D.width;
			for (int i = 0; i < texturePixelWidth; ++i)
				_colorSwapTexture2D.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

			_colorSwapTexture2D.Apply();


			_spriteRenderer.material.SetTexture("_SwapTex" + _playerID, _colorSwapTexture2D);

			_colorSwapTextureColors = new Color[texturePixelWidth];

		}

        public void SetTexture2DColor(Color color)
        {
            int texturePixelWidth = _colorSwapTexture2D.width;
            for (int i = 0; i < texturePixelWidth; ++i)
            {
                _colorSwapTexture2D.SetPixel(i, 0, color);
            }
            _colorSwapTexture2D.Apply();
        }

        public void ResetTexture2DColor()
        {
            int texturePixelWidth = _colorSwapTexture2D.width;

            for (int i = 0; i < texturePixelWidth; ++i)
            {
                _colorSwapTexture2D.SetPixel(i, 0, _colorSwapTextureColors[i]);
            }
            _colorSwapTexture2D.Apply();

        }

        private void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor)
		{
			_colorSwapTextureColors[(int)indexOfColourToSwap] = replacementColor;
			_colorSwapTexture2D.SetPixel((int)indexOfColourToSwap, 0, replacementColor);
		}

        private static Color ColorFromInt(int c, float alpha = 1.0f)
        {
            int r = (c >> 16) & 0x000000FF;
            int g = (c >> 8) & 0x000000FF;
            int b = c & 0x000000FF;

            Color ret = ColorFromIntRGB(r, g, b);
            ret.a = alpha;

            return ret;
        }

        private static Color ColorFromIntRGB(int r, int g, int b)
        {
            return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, 1.0f);
        }

    }
}

