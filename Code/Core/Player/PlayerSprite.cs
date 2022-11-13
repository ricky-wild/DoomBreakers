
using UnityEngine;

namespace DoomBreakers
{
    public enum WeaponChargeHold
	{
        None = 0,
        Minimal = 1,
        Moderate = 2,
        Maximal = 3,
	};
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
        Armor_Standard_h = 52,  //0x 343434        c f g h

        Bandit_Mask_Gloves_Boots = 78, //0x 4e3624
        Bandit_Mask_Strap_Pants = 104, //0x 683a18
        Bandit_Gloves_Boots_a = 51, //0x 332011
        Bandit_Gloves_Boots_b = 45, //0x 2d190a
        Bandit_Legs_Strap = 73, //0x 492911
    }
    public class PlayerSprite : Sprite, IPlayerSprite //
    {
		
		private SpriteColourIndex _spriteColourIndex;
		//private SpriteRenderer _spriteRenderer;
		//private Texture2D _colorSwapTexture2D;
		//private Color[] _colorSwapTextureColors;
		private int _playerID, _spriteFaceDirection;
        private ITimer _colorSwappedTimer;
        private bool _colorSwappedFlag;

        //private ITimer[] _weaponChargeTimer;// = new Timer[1];
        private ITimer _weaponChargeTimer, _weaponChargeHoldTimer;
        private const float _weaponChargeTime = 0.25f;
        private int _weaponTimerIncrement;
        private const int _weaponTimerIncrementMax = 10;
        private bool _weaponChargeTimerFlag;

        private bool _newEquipmentColorFlag;

        private WeaponChargeHold _weaponChargeHoldFlag; //Indicates how long the weapon charge went on for.
        private IPlayerEquipment _playerEquipment;


        public void Setup(SpriteRenderer spriteRenderer, int playerID)
		{
            _spriteRenderer = spriteRenderer;
            _playerID = playerID;
            _spriteFaceDirection = 1; //1 = face right, -1 = face left.

            _colorSwappedTimer = this.gameObject.AddComponent<Timer>();
            _colorSwappedTimer.Setup("_colorSwappedTimer");
            _colorSwappedFlag = false;

            _weaponChargeTimer = this.gameObject.AddComponent<Timer>();
            _weaponChargeTimer.Setup("_weaponChargeTimer");
            _weaponTimerIncrement = 0;
            _weaponChargeTimerFlag = false;

            _weaponChargeHoldTimer = this.gameObject.AddComponent<Timer>();
            _weaponChargeHoldTimer.Setup("_weaponChargeHoldTimer");

            _weaponChargeHoldFlag = WeaponChargeHold.None;

            _newEquipmentColorFlag = false;

            //SetupTexture2DColorSwap();
            ApplyCustomTexture2DColours();
        }
        public override int GetSpriteDirection()
		{
            return _spriteFaceDirection;
        }
        public WeaponChargeHold GetWeaponTexChargeFlag()
		{
            return _weaponChargeHoldFlag;
		}
        public override void FlipSprite()
		{
			//base.FlipSprite();
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
            SetupTexture2DColorSwap("_SwapTex", _playerID);

            int temp = 4;

            //switch (MenuManager._instance.GetPlayerCustomSkinId(_playerID))
            switch(temp)
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
            _colorSwapTexture2D.Apply();
        }

		public override void SetupTexture2DColorSwap(string texName, int texId)
        {
			//base.SetupTexture2DColorSwap(texName, _playerID);
			_colorSwapTexture2D = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
			_colorSwapTexture2D.filterMode = FilterMode.Point;

			int texturePixelWidth = _colorSwapTexture2D.width;
			for (int i = 0; i < texturePixelWidth; ++i)
				_colorSwapTexture2D.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

			_colorSwapTexture2D.Apply();

            //shader->ColorSwap->_SwapTex0
            //shader->ColorSwap1->_SwapTex1
            //shader->ColorSwap2->_SwapTex2
            //shader->ColorSwap3->_SwapTex3
            _spriteRenderer.material.SetTexture("_SwapTex" + _playerID, _colorSwapTexture2D);

			_colorSwapTextureColors = new Color[texturePixelWidth];

		}

        public override void SetTexture2DColor(Color color)
        {
			//base.SetTexture2DColor(color);
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

        public override void ResetTexture2DColor()
        {
			int texturePixelWidth = _colorSwapTexture2D.width;

			for (int i = 0; i < texturePixelWidth; ++i)
			{
				_colorSwapTexture2D.SetPixel(i, 0, _colorSwapTextureColors[i]);
			}

			if (_colorSwappedFlag)
				_colorSwappedFlag = false;

			_colorSwapTexture2D.Apply();
			//base.ResetTexture2DColor();
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
            UpdateNewEquipmentTextureColor();
            UpdateColorTextureResetInternally();

            if (!_weaponChargeTimerFlag) //Guard Clause.
                return;

            UpdateWeaponChargeTextureFX();

        }

        private void UpdateNewEquipmentTextureColor()
		{
            if (!_newEquipmentColorFlag)
                return;

            if (_playerEquipment == null)
                return;

            ApplyCustomTexture2DColours();
            ApplyArmorColours();
            ApplyShieldColours();
            ApplySwordColours();

            //_colorSwapTexture2D.Apply();
            _newEquipmentColorFlag = false;

        }
        private void ApplyArmorColours()
		{
            if (_playerEquipment.IsArmor())
            {
                switch (_playerEquipment.GetTorsoEquip())
                {
                    case PlayerEquipType.BreastPlate_Bronze:
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_a, ColorFromInt(0xced37b));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_b, ColorFromInt(0xc4c973));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0xbabf6e));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_d, ColorFromInt(0xafb370));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_e, ColorFromInt(0xa0a557));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x9ca060));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x61512f));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x44371c));
                        break;
                    case PlayerEquipType.BreastPlate_Iron:
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_a, ColorFromInt(0x9d9d9d));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_b, ColorFromInt(0x8b8b8b));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0x858585));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_d, ColorFromInt(0x808080));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_e, ColorFromInt(0x7e7a7a));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x747474));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x535353));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x1e1e1e));
                        break;
                    case PlayerEquipType.BreastPlate_Steel:
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_a, ColorFromInt(0xdedede));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_b, ColorFromInt(0xc6c6c6));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0xbebebe));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_d, ColorFromInt(0xb8b8b8));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_e, ColorFromInt(0xb5b0b0));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0xa8a8a8));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x7b7b7b));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x343434));
                        break;
                    case PlayerEquipType.BreastPlate_Ebony:
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_a, ColorFromInt(0x565656));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_b, ColorFromInt(0x4e4c4c));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0x494e64));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_d, ColorFromInt(0x4c4f5a));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_e, ColorFromInt(0x424141));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x383636));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x302d2d));
                        SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x201d1d));
                        break;
                }
                _colorSwapTexture2D.Apply();
            }
        }
        private void ApplyShieldColours()
		{
            if (!SafeToApplyShieldColor())
                return;

            PlayerEquipType shieldType = PlayerEquipType.Empty_None;
            if (_playerEquipment.IsShield(EquipHand.Left_Hand))
                shieldType = _playerEquipment.GetLeftHandEquip();
            if (_playerEquipment.IsShield(EquipHand.Right_Hand))
                shieldType = _playerEquipment.GetRightHandEquip();

            

            switch(shieldType)
			{
                case PlayerEquipType.Shield_Bronze:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xcbd092));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x9ca060));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x61512f));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x074f21));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x0e622c));
                    break;
                case PlayerEquipType.Shield_Iron:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xa8a8a8));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x7b7b7b));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x343434));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x4f1507));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x823c3d));
                    break;
                case PlayerEquipType.Shield_Steel:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xcfc8c8));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0xb1a7a7));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x918686));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x212454));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x3c407e));
                    break;
                case PlayerEquipType.Shield_Ebony:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0x2f3146));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x272834));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x1e1717));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x121d28));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x3f464e));
                    break;
            }

            _colorSwapTexture2D.Apply();
        }
        private bool SafeToApplyShieldColor()
		{
            if (_playerEquipment == null)
                return false;
            if (_playerEquipment.IsShield(EquipHand.Left_Hand))
                return true;
            if (_playerEquipment.IsShield(EquipHand.Right_Hand))
                return true;

            return false;
		}
        private void ApplySwordColours()
        {
            if (!SafeToApplySwordColor())
                return;

            PlayerEquipType swordType = PlayerEquipType.Empty_None;
            if (_playerEquipment.IsBroadsword(EquipHand.Left_Hand))
                swordType = _playerEquipment.GetLeftHandEquip();
            if (_playerEquipment.IsBroadsword(EquipHand.Right_Hand))
                swordType = _playerEquipment.GetRightHandEquip();
            if (_playerEquipment.IsLongsword(EquipHand.Left_Hand))
                swordType = _playerEquipment.GetLeftHandEquip();
            if (_playerEquipment.IsLongsword(EquipHand.Right_Hand))
                swordType = _playerEquipment.GetRightHandEquip();



            switch (swordType)
            {
                case PlayerEquipType.Broadsword_Bronze:
                case PlayerEquipType.Longsword_Bronze:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xe4e6c9));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xd3d6ab));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0xcbd092));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0xb5b977));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x9ca060));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x61512f));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x6c5a34));
                    break;
                case PlayerEquipType.Broadsword_Iron:
                case PlayerEquipType.Longsword_Iron:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xafafaf));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xa3a2a2));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0x939191));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0x8b8b8b));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x7c7979));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x144c18));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x1a5a1f));
                    break;
                case PlayerEquipType.Broadsword_Steel:
                case PlayerEquipType.Longsword_Steel:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xd5d5d5));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xcbc5c5));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0xb9b9b9));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0xb3afaf));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x9f9a9a));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x0f1c4e));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x182964));
                    break;
                case PlayerEquipType.Broadsword_Ebony:
                case PlayerEquipType.Longsword_Ebony:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0x565965));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0x424141));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0x383636));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0x302d2d));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x201d1d));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x4c1212));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x601c1c));
                    break;
            }

            _colorSwapTexture2D.Apply();
        }
        private bool SafeToApplySwordColor()
        {
            if (_playerEquipment == null)
                return false;
            if (_playerEquipment.IsBroadsword(EquipHand.Left_Hand))
                return true;
            if (_playerEquipment.IsBroadsword(EquipHand.Right_Hand))
                return true;
            if (_playerEquipment.IsLongsword(EquipHand.Left_Hand))
                return true;
            if (_playerEquipment.IsLongsword(EquipHand.Right_Hand))
                return true;

            return false;
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

            switch(_weaponTimerIncrement)
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

            
            if(_weaponChargeHoldTimer.HasTimerFinished())
			{
                switch(_weaponChargeHoldFlag)
				{
                    case WeaponChargeHold.None:
                        UpdateWeaponChargeIndicator(WeaponChargeHold.Minimal);
                        break;
                    case WeaponChargeHold.Minimal:
                        UpdateWeaponChargeIndicator(WeaponChargeHold.Moderate);
                        break;
                    case WeaponChargeHold.Moderate:
                        UpdateWeaponChargeIndicator(WeaponChargeHold.Maximal);
                        break;
                    case WeaponChargeHold.Maximal:
                        UpdateWeaponChargeIndicator(WeaponChargeHold.None);
                        break;
                }
                _weaponChargeHoldTimer.StartTimer(1.0f);
            }

            _colorSwapTexture2D.Apply();
        }

        private void UpdateWeaponChargeIndicator(WeaponChargeHold weaponChargeHold)
		{
            //if (_weaponChargeHoldFlag == WeaponChargeHold.Maximal)
            //    return;

            _weaponChargeHoldFlag = weaponChargeHold;

            if (weaponChargeHold == WeaponChargeHold.None)
            {
                SwapTexture2DColor(SpriteColourIndex.ChargeIndicator, ColorFromInt(0xff0000));
            }
            if (weaponChargeHold == WeaponChargeHold.Minimal)
			{
                SwapTexture2DColor(SpriteColourIndex.ChargeIndicator, ColorFromInt(0x00ff00));
			}
            if (weaponChargeHold == WeaponChargeHold.Moderate)
            {
                SwapTexture2DColor(SpriteColourIndex.ChargeIndicator, ColorFromInt(0x0000ff));
            }
            if (weaponChargeHold == WeaponChargeHold.Maximal)
            {
                SwapTexture2DColor(SpriteColourIndex.ChargeIndicator, ColorFromInt(0xffffff));
            }
            _colorSwapTexture2D.Apply();
        }

        public void SetWeaponChargeTextureFXFlag(bool b)
		{
            if (_weaponChargeTimerFlag) //Guard clause, if already true don't bother.
                return;

            _weaponChargeTimerFlag = b;
            _weaponChargeHoldFlag = WeaponChargeHold.None;

            if (!b)
                return;

            //_weaponChargeHoldFlag = WeaponChargeHold.None;
            _weaponTimerIncrement = 0;
            _weaponChargeTimer.StartTimer(_weaponChargeTime);
            _weaponChargeHoldTimer.StartTimer(1.0f);


            SwapTexture2DColor(SpriteColourIndex.ChargeFX, ColorFromInt(0xe5f7ba));
            _colorSwapTexture2D.Apply();
        }
        public void SetNewEquipmemtTextureColorFlag(bool b, IPlayerEquipment playerEquipment)
		{
            _newEquipmentColorFlag = b;
            _playerEquipment = playerEquipment;
		}

        public override void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor)
		{
            //_colorSwapTextureColors[(int)indexOfColourToSwap] = replacementColor;
            //_colorSwapTexture2D.SetPixel((int)indexOfColourToSwap, 0, replacementColor);
            base.SwapTexture2DColor(indexOfColourToSwap, replacementColor);
        }

        public override Color ColorFromInt(int c, float alpha = 1.0f)
        {
            //int r = (c >> 16) & 0x000000FF;
            //int g = (c >> 8) & 0x000000FF;
            //int b = c & 0x000000FF;

            //Color ret = ColorFromIntRGB(r, g, b);
            //ret.a = alpha;

            //return ret;
            return base.ColorFromInt(c, alpha);
        }

        public override Color ColorFromIntRGB(int r, int g, int b)
        {
            //return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, 1.0f);
            return base.ColorFromIntRGB(r, g, b);
        }

    }
}

