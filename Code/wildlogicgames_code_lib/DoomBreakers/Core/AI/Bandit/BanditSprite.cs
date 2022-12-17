
using UnityEngine;

namespace DoomBreakers
{

    public class BanditSprite : Sprite, IBanditSprite //
    {

        private SpriteColourIndex _spriteColourIndex;
        private int _banditID, _spriteFaceDirection;
        private ITimer _colorSwappedTimer;
        private bool _colorSwappedFlag;

        //private ITimer[] _weaponChargeTimer;// = new Timer[1];
        private ITimer _behaviourTimer, _weaponChargeTimer;
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

            SetupTexture2DColorSwap("_SwapTexBandit",banditID);
        }
        public void Setup(SpriteRenderer spriteRenderer, int banditID)
        {
            _spriteRenderer = spriteRenderer;
            _banditID = banditID;
            _spriteFaceDirection = 1; //1 = face right, -1 = face left.

            _behaviourTimer = this.gameObject.AddComponent<Timer>();
            _behaviourTimer.Setup("_behaviourTimer");

            _colorSwappedTimer = this.gameObject.AddComponent<Timer>();
            _colorSwappedTimer.Setup("_colorSwappedTimer");
            _colorSwappedFlag = false;

            _weaponChargeTimer = this.gameObject.AddComponent<Timer>();
            _weaponChargeTimer.Setup("_weaponChargeTimer");
            _weaponTimerIncrement = 0;
            _weaponChargeTimerFlag = false;

            //SetupTexture2DColorSwap();
            ApplyCustomTexture2DColours();
        }
        public override int GetSpriteDirection()
        {
            return _spriteFaceDirection;
        }
        public override void FlipSprite()
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
            SetupTexture2DColorSwap("_SwapTexBandit", _banditID);

            int skinOption = wildlogicgames.Utilities.GetRandomNumberInt(0, 6);
            //switch (MenuManager._instance.GetPlayerCustomSkinId(_playerID))
            switch (skinOption)
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

            skinOption = wildlogicgames.Utilities.GetRandomNumberInt(0, 4);

            switch (skinOption)
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
            _colorSwapTexture2D.Apply();
        }

        public override void SetupTexture2DColorSwap(string texName, int texId)
        {
            _colorSwapTexture2D = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
            _colorSwapTexture2D.filterMode = FilterMode.Point;

            int texturePixelWidth = _colorSwapTexture2D.width;
            for (int i = 0; i < texturePixelWidth; ++i)
                _colorSwapTexture2D.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            _colorSwapTexture2D.Apply();

            //texName = "_SwapTexBandit";
            _spriteRenderer.material.SetTexture(texName, _colorSwapTexture2D);

            _colorSwapTextureColors = new Color[texturePixelWidth];

        }

        public override void SetTexture2DColor(float time, Color color)
        {
            int texturePixelWidth = _colorSwapTexture2D.width;
            for (int i = 0; i < texturePixelWidth; ++i)
            {
                _colorSwapTexture2D.SetPixel(i, 0, color);
            }

			if (!_colorSwappedFlag) //Ensure we reset internally upon failure to do so externally (ie a state change)
			{
                _colorSwappedTimer.Reset();
                _colorSwappedTimer.StartTimer(time + 0.01f);
                _colorSwappedFlag = true;
			}


            _colorSwapTexture2D.Apply();
        }

        public override void ResetTexture2DColor()
        {
            int texturePixelWidth = _colorSwapTexture2D.width;

            for (int i = 0; i < texturePixelWidth; ++i)
            {
                _colorSwapTexture2D.SetPixel(i, 0, _colorSwapTextureColors[i]);
            }

            if (_colorSwappedFlag) _colorSwappedFlag = false;

            _colorSwapTexture2D.Apply();

        }
        public override void SetBehaviourTextureFlash(float time, Color colour)
        {
            //behaviourTimer.StartTimer(time);//flash sprite colour timer.
            //if (_behaviourTimer.HasTimerFinished())
            SetTexture2DColor(time, colour);
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

        public override void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor)
        {
            base.SwapTexture2DColor(indexOfColourToSwap, replacementColor);
        }

        public override Color ColorFromInt(int c, float alpha = 1.0f)
        {
            return base.ColorFromInt(c, alpha);
        }

        public override Color ColorFromIntRGB(int r, int g, int b)
        {
            return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, 1.0f);
        }

    }
}

