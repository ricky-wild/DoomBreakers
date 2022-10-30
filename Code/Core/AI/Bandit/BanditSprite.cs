
using UnityEngine;

namespace DoomBreakers
{

    public class BanditSprite : MonoBehaviour, IBanditSprite //
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

            Bandit_Mask_Gloves_Boots = 78, //0x 4e3624
            Bandit_Mask_Strap_Pants = 104, //0x 683a18
            Bandit_Gloves_Boots_a = 51, //0x 332011
            Bandit_Gloves_Boots_b = 45, //0x 2d190a
            Bandit_Legs_Strap = 73, //0x 492911

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
        private int _banditID, _spriteFaceDirection;
        private ITimer _colorSwappedTimer;
        private bool _colorSwappedFlag;

        //private ITimer[] _weaponChargeTimer;// = new Timer[1];
        private ITimer _weaponChargeTimer;
        private const float _weaponChargeTime = 0.25f;
        private int _weaponTimerIncrement;
        private const int _weaponTimerIncrementMax = 10;
        private bool _weaponChargeTimerFlag;

        public BanditSprite(SpriteRenderer spriteRenderer, int banditID)
        {
            _spriteRenderer = spriteRenderer;
            _banditID = banditID;
            _spriteFaceDirection = 1; //1 = face right, -1 = face left.

            _weaponChargeTimerFlag = false;

            SetupTexture2DColorSwap();
        }
        public void Setup(SpriteRenderer spriteRenderer, int banditID)
        {
            _spriteRenderer = spriteRenderer;
            _banditID = banditID;
            _spriteFaceDirection = 1; //1 = face right, -1 = face left.

            _colorSwappedTimer = this.gameObject.AddComponent<Timer>();
            _colorSwappedTimer.Setup();
            _colorSwappedFlag = false;

            _weaponChargeTimer = this.gameObject.AddComponent<Timer>();
            _weaponChargeTimer.Setup();
            _weaponTimerIncrement = 0;
            _weaponChargeTimerFlag = false;

            //SetupTexture2DColorSwap();
            ApplyCustomTexture2DColours();
        }
        public int GetSpriteDirection()
        {
            return _spriteFaceDirection;
        }
        public void FlipSprite()
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
        public void ApplyCustomTexture2DColours()
        {
            SetupTexture2DColorSwap();

            //switch (MenuManager._instance.GetPlayerCustomSkinId(_playerID))
            switch (_banditID)
            {
                default: //STANDARD SKIN
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x3b370c));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2d87a));  //SKIN A
                    SwapTexture2DColor(SpriteColourIndex.Skin_b, ColorFromInt(0xd0c774));  //SKIN B
                    SwapTexture2DColor(SpriteColourIndex.Skin_c, ColorFromInt(0xc2ba6d));  //SKIN C
                    SwapTexture2DColor(SpriteColourIndex.Skin_d, ColorFromInt(0xb1aa64));  //SKIN D
                    SwapTexture2DColor(SpriteColourIndex.Skin_e, ColorFromInt(0x999455));  //SKIN E
                    SwapTexture2DColor(SpriteColourIndex.Cloth_a, ColorFromInt(0x0430ff)); //CLOTH A
                    SwapTexture2DColor(SpriteColourIndex.Cloth_b, ColorFromInt(0x0b279d)); //CLOTH B
                    break;
                case 0:
                    SwapTexture2DColor(SpriteColourIndex.Hair, ColorFromInt(0x3b370c));    //HAIR
                    SwapTexture2DColor(SpriteColourIndex.Skin_a, ColorFromInt(0xe2d87a));  //SKIN A
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
            }


            //Base the colour variation of the enemy from the AI difficulty applied.
            switch (_banditID)
            {
                case 0: //Default sprite colours.
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Gloves_Boots, ColorFromInt(0x4e3624));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Strap_Pants, ColorFromInt(0x683a18));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_a, ColorFromInt(0x332011));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_b, ColorFromInt(0x2d190a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Legs_Strap, ColorFromInt(0x492911));
                    break;
                case 1: //Focused&Aggressive
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Gloves_Boots, ColorFromInt(0x2f1705)); //2f1705
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Strap_Pants, ColorFromInt(0x510d0a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_a, ColorFromInt(0x912a1f));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_b, ColorFromInt(0x2d190a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Legs_Strap, ColorFromInt(0x492911));
                    break;
                case 2: //Unfocused&Aggressive
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Gloves_Boots, ColorFromInt(0x1d342f));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Strap_Pants, ColorFromInt(0x6d4924));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_a, ColorFromInt(0xaa8120));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_b, ColorFromInt(0x2d190a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Legs_Strap, ColorFromInt(0x492911));
                    break;
                case 3: //Focused&Soft
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Gloves_Boots, ColorFromInt(0x202020));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Strap_Pants, ColorFromInt(0x683a18));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_a, ColorFromInt(0x912a1f));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_b, ColorFromInt(0x2d190a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Legs_Strap, ColorFromInt(0x492911));
                    break;
                case 4: //Unfocused&Soft
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Gloves_Boots, ColorFromInt(0x202020));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Mask_Strap_Pants, ColorFromInt(0x223a18));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_a, ColorFromInt(0x332011));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Gloves_Boots_b, ColorFromInt(0x2d190a));
                    SwapTexture2DColor(SpriteColourIndex.Bandit_Legs_Strap, ColorFromInt(0x464520));
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


            _spriteRenderer.material.SetTexture("_SwapTex" + _banditID, _colorSwapTexture2D);

            _colorSwapTextureColors = new Color[texturePixelWidth];

        }

        public void SetTexture2DColor(Color color)
        {
            int texturePixelWidth = _colorSwapTexture2D.width;
            for (int i = 0; i < texturePixelWidth; ++i)
            {
                _colorSwapTexture2D.SetPixel(i, 0, color);
            }

            if (!_colorSwappedFlag) //Ensure we reset internally upon failure to do so externally (ie a state change)
            {
                _colorSwappedTimer.StartTimer(1.0f);
                _colorSwappedFlag = true;
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

            if (_colorSwappedFlag)
                _colorSwappedFlag = false;

            _colorSwapTexture2D.Apply();

        }

        private void UpdateColorTextureResetInternally()
        {
            //Ensure we reset internally upon failure to do so externally (ie a state change)
            if (!_colorSwappedFlag)
                return;

            if (_colorSwappedTimer.HasTimerFinished())
            {
                ResetTexture2DColor();
                _colorSwappedFlag = false;
            }
        }

        void Update()
        {
            UpdateColorTextureResetInternally();

            if (!_weaponChargeTimerFlag) //Guard Clause.
                return;

            UpdateWeaponChargeTextureFX();

        }

        private void UpdateWeaponChargeTextureFX()
        {
            if (_weaponChargeTimer.HasTimerFinished())
            {
                if (_weaponTimerIncrement < _weaponTimerIncrementMax)
                    _weaponTimerIncrement++;
                else
                    _weaponTimerIncrement = 0;
                _weaponChargeTimer.StartTimer(_weaponChargeTime);

            }

            switch (_weaponTimerIncrement)
            {
                case 0:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xffffff));
                    break;
                case 1:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfded91));
                    break;
                case 2:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfbce5e));
                    break;
                case 3:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfdac40));
                    break;
                case 4:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xff6a2f));
                    break;
                case 5:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfdac40));
                    break;
                case 6:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfbce5e));
                    break;
                case 7:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xfded91));
                    break;
                case 8:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xe5f7ba));
                    break;
                case 9:
                    SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xffffff));
                    break;
            }

            _colorSwapTexture2D.Apply();
        }

        public void SetWeaponChargeTextureFXFlag(bool b)
        {
            if (_weaponChargeTimerFlag) //Guard clause, if already true don't bother.
                return;

            _weaponChargeTimerFlag = b;

            if (!b)
                return;

            _weaponTimerIncrement = 0;
            _weaponChargeTimer.StartTimer(_weaponChargeTime);

            SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xe5f7ba));
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

