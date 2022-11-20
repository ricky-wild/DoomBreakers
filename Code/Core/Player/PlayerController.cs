using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;
using Rewired;
//using TNet;

//CTRL + M, O close all functions.

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerController : MonoBehaviour
{

    [Header("Player ID")]
    public int _playerID;

    //private DataNode _saveDataNode;

    private PlayerStatData _stats;
    private int _life;
    private int _faceDir;
    private int _dodgeDir;


    //[Header("FX Controller")]
    //public PlayerFXController _childFXController;

    [Header("ID Controller")]
    public IDController _IDController;

    [Header("Target Controller")]
    public TargetController _targetController;

    [Header("UI DMG Counter Text")]
    public PlayerDmgCounterUIText _dmgCounterUIText;

    [Header("2D Gravity")]
    public Controller2D _controller;

    //private SpriteTrail.SpriteTrail _spriteTrailRef;

    [Header("Attack Points")]
    public Transform _smallAttackPoint;
    public Transform _powerAttackPoint;
    public Transform _aboveAttackPoint;
    public LayerMask[] _enemyLayers;

    [Header("Attack Ranges")]
    public float _smallAttackRange;
    public float _powerAttackRange;
    private double _dmgA, _dmgB, _dmgSum;

    private Vector2 _input;
    private Transform _transform;
    private Vector3 _velocity, _startPos;
    private Vector2 _hitImpact;
    private float _gravity;
    private float _moveSpeed;
    private float _sprintSpeed;
    private float _maxJumpVelocity;
    private float _potionSpeed;
    private float _jumpBoost;
    //private float _hitImpact;
    private float _hitDamage;
    private float _targetVelocityX;
    private float _targetVelocityY;
    private bool _jumped;
    private bool _dblJumped;
    private bool _jumpDuringHit;
    private bool _falling;
    private bool _fallen;
    private bool _respawned;
    private bool _dying;
    private bool _attacked;
    private bool _released;
    //private bool _comboAttack;
    private int _comboAttack;
    private int _comboInputFlag;

    private int _knockAttack;
    private int _smlAttack;
    private int _smlAtkIncrement;
    private bool _defended;
    private bool _dodge;
    private bool _armorBroken;
    private bool _isHit;
    private bool _isQuickHit;
    private bool _isHitDefending;
    private bool _isQuickHitDefending;
    private bool _isTired;
    private bool _isSprint;
    private bool _isUpwardAtk;
    private bool _applyHitFlag;
    private bool _isSlamOnGround;
    private bool _dustCloudFlag;
    private bool _jumpCloudFlag;
    private bool _attackCloudFlag;
    private bool _hitCloudFlag;
    private bool _hitTexFlag;
    private bool _processTexFxFlag;
    private bool _weapChrgeTexFlag;
    private float _pwrAtkDurStamp;
    private bool _hitShakeFlag;
    private bool _fallDmgFlag;
    private bool _applyFallDmg;

    private bool _isInWater;
    private bool _weaponCollidedFlag;
    private bool _isEnemyCollidedFlag;
    private bool _enableAtkUpdate;
    private bool _enableTargUpdate;
    private bool _applyTargUpdate;
    private int[] _collidedPlayerId = new int[4];
    private int[] _collidedEnemyId = new int[12];//8
    private int _animatorControllerIndex;
    private bool _equipAquiredId;

    private bool _processUpdate;
    private bool _offsceen;
    private bool _lifeUp;

    private Collider2D _collider2d;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Texture2D _colorSwapTex, _colorSwapTexOriginal;
    private Color[] _spriteColors;

    private Player _rewirdInputPlayer;

    public ProCamera2DCinematics _cameraCinematics;
    public ShakePreset _shakePreset;
    private ProCamera2DShake _proCamera2DShake;

    //private TimerComponent _hitByReleaseAtkTimer;
    //private TimerComponent _exitDemoTimer;
    private TimerComponent _comboAttackTimer;
    private TimerComponent _behaviourTimer;
    private TimerComponent _fxTimer;
    private TimerComponent _hitBehaviourTimer;
    private TimerComponent _hitCloudsTimer;
    private TimerComponent _hitTextureTimer;
    private TimerComponent _weapChrgeTextureTimer;
    private TimerComponent _equipGainedAnimTimer;
    private TimerComponent _fallTimer;
    private TimerComponent[] _powerAtkCloudTimer = new TimerComponent[3]; //3 states in power attack.

    private string _animControllerFilepath;



    void Awake()
    {
        _rewirdInputPlayer = ReInput.players.GetPlayer(_playerID);

        //_rewirdInputPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);

        //// Load joysticks maps in each joystick in the "UI" category and "Default" layout and set it to be enabled on start
        //foreach (Joystick joystick in _rewirdInputPlayer.controllers.Joysticks)
        //{
        //    _rewirdInputPlayer.controllers.maps.LoadMap(ControllerType.Joystick, joystick.id, "UI", "Default", true);
        //}

        //_hitByReleaseAtkTimer = new TimerComponent();
        //_exitDemoTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _comboAttackTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _behaviourTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _fxTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _hitBehaviourTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _hitCloudsTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _hitTextureTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _weapChrgeTextureTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _equipGainedAnimTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        _fallTimer = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
        for (int i = 0; i < 3; i++)
        {
            _powerAtkCloudTimer[i] = new TimerComponent();//gameObject.AddComponent<TimerComponent>();
            _powerAtkCloudTimer[i].SetTimerDuration(1.0f, _playerID);
        }

        //For midair attack spam prevention
        _behaviourTimer.SetTimestampedFlag(false, _playerID + 4);
        _behaviourTimer.SetTimerDuration(1.0f, _playerID + 4);//0.4f
        _behaviourTimer.StartTimer(_playerID + 4);
        _stats = gameObject.AddComponent<PlayerStatData>();
    }

    void Start()
    {

		if (!MenuManager._instance.HasDetectedPlayerFlag(_playerID))
		{
			RemovePlayerFromCamera();
			this.gameObject.SetActive(false);
			return;
		}


        if (GameManager._instance.GetScene() == GameManager.Level.TrainingGroundScene)//GameManager.Level.TutorialScene)
        {
            _stats.Health -= 1.0;
            GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 0);
            if (_playerID != 0)
                RemovePlayerFromCamera();
        }
        else
		{
            if (GameManager._instance.GetScene() == GameManager.Level.TheKingsCallScene)
            {
                if (_playerID != 0)
                    RemovePlayerFromCamera();
            }
            else
			{
                if (_playerID != 0)
                {
                    _stats.Health = 0f;
                    GameManager.SetPlayerStatFlag(_playerID, true);
                    GameManager.SetPlayerStats(_playerID, _stats);
                    GameManager.UpdatePlayerStats(_playerID, _stats, 0);
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Dying);
                    RemovePlayerFromCamera();
                }
            }
        }


        _transform = this.transform;
        _input = Vector2.zero;
        _velocity = Vector3.zero;
        _startPos = _transform.position;
        //_spriteTrailRef = this.GetComponent<SpriteTrail.SpriteTrail>();
        //_spriteTrailRef.DisableTrail();

        //_gravity = -(2 * 0.8f) / Mathf.Pow(0.45f, 2);
        _gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
        _moveSpeed = 3.75f;//3.5f;
        _sprintSpeed = 1.0f;
        _maxJumpVelocity = 11.25f;// 10.0f;//6.6f;//4.6f;
        _potionSpeed = 1.0f;// _moveSpeed * 2;
        _jumpBoost = 1.0f;
        //_hitImpact = 1.0f;
        _hitImpact = new Vector2();
        _hitImpact.x = 1.0f;
        _hitImpact.y = 1.0f;
        _hitDamage = 1.0f;
        _dmgA = 0;
        _dmgB = 0;
        _dmgSum = 0;
        _targetVelocityX = 0f;
        _targetVelocityX = 0f;

        _life = 0;
        _faceDir = 1; //-1 = facing left, 1 = facing right.
        _dodgeDir = 0;

        _isInWater = false;
        _jumped = false;
        _dblJumped = false;
        _jumpDuringHit = false;
        _falling = false;
        _fallen = false;
        _respawned = false;
        _dying = false;
        _dustCloudFlag = false;
        _jumpCloudFlag = false;
        _attackCloudFlag = false;
        _hitCloudFlag = false;
        _attacked = false;
        _released = false;
        _comboAttack = 0;
        _comboInputFlag = 0;

        _knockAttack = 0;
        _smlAttack = 0;
        _smlAtkIncrement = 0;
        _defended = false;
        _dodge = false;
        _armorBroken = false;
        _isHit = false;
        _isQuickHit = false;
        _isHitDefending = false;
        _isQuickHitDefending = false;
        _isSlamOnGround = false;
        _isTired = false;
        _isSprint = false;
        _isUpwardAtk = false;
        _applyHitFlag = false;
        _hitTexFlag = false;
        _processTexFxFlag = false;
        _weapChrgeTexFlag = false;
        _pwrAtkDurStamp = 0f;
        _hitShakeFlag = false;
        _fallDmgFlag = false;
        _applyFallDmg = false;

        _processUpdate = true;
        _offsceen = false;
        _lifeUp = false;

        _isEnemyCollidedFlag = false;
        _weaponCollidedFlag = false;
        _enableAtkUpdate = false;
        _enableTargUpdate = false;
        _applyTargUpdate = false;

        for (int i = 0; i < _collidedPlayerId.Length; i++)
        {
            _collidedPlayerId[i] = -1;
            _collidedEnemyId[i] = -1;

        }
        _animatorControllerIndex = 0;
        _equipAquiredId = false;

        _controller = GetComponent<Controller2D>();
        _collider2d = GetComponent<Collider2D>();
        _collider2d.enabled = true;
        _animator = this.GetComponent<Animator>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        //GameManager.ResetPlayerEquip(_playerID);
        //GameManager.Save(_playerID);
        GameManager.Load(_playerID);

        PlayerMonitor.PluginPlayerController(this, _playerID);
        SetupPreviousSceneEquip();
        SetupCustomSkin();


        _cameraCinematics = ProCamera2D.Instance.GetComponent<ProCamera2DCinematics>();
        _proCamera2DShake = ProCamera2D.Instance.GetComponent<ProCamera2DShake>();
        //_proCamera2DShake.Shake(_shakePreset);
        //_proCamera2DShake.enabled = false;

        DialogueManager._instance.SetCurrentActEndFlag(false);
        DialogueManager._instance.SetCurrentActStartFlag(true);



        //PlayerMonitor.PluginPlayerController(this, _playerID);

        GameManager.SaveConfig(true, true);//Set save flags to prevent prologue storys playing again on next time play through!

        //_saveDataNode = new DataNode("player" + _playerID.ToString(), this);

        //_exitDemoTimer.SetTimerDuration(60.25f, 0);//250ms

    }

    private void SetupPreviousSceneEquip()
	{
        _animControllerFilepath = "HumanAnimControllers/Unarmored/";
        //GameManager.SetPreviousScenePlayerEquip(_playerID);
        _equipAquiredId = true; //for animator
        switch (GameManager.GetCurrentPlayerEquipA(_playerID))
        {
            case ItemData.ItemType.None:
                _animatorControllerIndex = 0;
                break;
            case ItemData.ItemType.Sword:
                switch (GameManager.GetCurrentPlayerSubEquipA(_playerID))
                {
                    case ItemData.ItemSubType.sword_bronze:
                    case ItemData.ItemSubType.sword_iron:
                    case ItemData.ItemSubType.sword_steel:
                    case ItemData.ItemSubType.sword_ebony:
                        _animatorControllerIndex = 1;
                        //FXObjPooler._curInstance.Instantiate(_transform.position, 24, null);//24 = broadsword gained fx
                        break;
                    case ItemData.ItemSubType.longsword_bronze:
                    case ItemData.ItemSubType.longsword_iron:
                    case ItemData.ItemSubType.longsword_steel:
                    case ItemData.ItemSubType.longsword_ebony:
                        _animatorControllerIndex = 5;

                        break;
                    case ItemData.ItemSubType.longsword_fire:
                        _animatorControllerIndex = 5;
                        FXObjPooler._curInstance.Instantiate(_transform.position, 32, PlayerMonitor.GetPlayerTransform(_playerID), _playerID);//32 = fire fx for longsword
                        break;
                }
                break;
            case ItemData.ItemType.Shield:
                _animatorControllerIndex = 2;
                
                break;

        }
        AnimControllerSwapout();

        switch (GameManager.GetCurrentPlayerEquipB(_playerID))
        {

            case ItemData.ItemType.Sword:
                switch (GameManager.GetCurrentPlayerSubEquipB(_playerID))
                {
                    case ItemData.ItemSubType.sword_bronze:
                    case ItemData.ItemSubType.sword_iron:
                    case ItemData.ItemSubType.sword_steel:
                    case ItemData.ItemSubType.sword_ebony:
                        _animatorControllerIndex = 3;
                        //FXObjPooler._curInstance.Instantiate(_transform.position, 24, null);//24 = broadsword gained fx
                        break;
                    case ItemData.ItemSubType.longsword_bronze:
                    case ItemData.ItemSubType.longsword_iron:
                    case ItemData.ItemSubType.longsword_steel:
                    case ItemData.ItemSubType.longsword_ebony:
                        _animatorControllerIndex = 5;
                        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                            _animatorControllerIndex = 6;

                        break;
                    case ItemData.ItemSubType.longsword_fire:
                        _animatorControllerIndex = 5;
                        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                            _animatorControllerIndex = 6;
                        FXObjPooler._curInstance.Instantiate(_transform.position, 32, PlayerMonitor.GetPlayerTransform(_playerID), _playerID);//32 = fire fx for longsword
                        break;
                }
                break;
            case ItemData.ItemType.Shield:
                _animatorControllerIndex = 3;
                
                break;
        }
        AnimControllerSwapout();

        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword &&
               GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            _animatorControllerIndex = 4;

        if(GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
		{
            
            _animControllerFilepath = "HumanAnimControllers/Armored/";
        }


        AnimControllerSwapout();
        _equipAquiredId = false; //for animator
        _animatorControllerIndex = 0;
        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

        //Update the UI as appropriate.
        //Weapons & armor UI
        //GameManager.ItemPickupFlag(true);
        //Health & currency, kill count UI
        //GameManager.SetPlayerStatFlag(_playerID, true);
        GameManager.SetPlayerUILoadUpdateFlag(true);

        return;
    }

    private void ApplyOriginalSkinSetup()
	{
        //DESC: Function exists to assist with buy merchant item mechanic. Usual method of applying
        //      correct equipment skins fails when the player is already equipped and decides to
        //      buy another piece. That's because the usual method to apply correct skin works on
        //      top of the fact the sprite texture has original start colours in place to reference.

        //Setup animation to hold all colours need for recording.
        _animator.runtimeAnimatorController = Resources.Load("HumanAnimControllers/Armored/Player_with_sword&shield_controller") as RuntimeAnimatorController;

        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;

        for (int i = 0; i < colorSwapTex.width; ++i)
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        colorSwapTex.Apply();


        _spriteRenderer.material.SetTexture("_SwapTex" + _playerID, colorSwapTex);

        _spriteColors = new Color[colorSwapTex.width];
        _colorSwapTexOriginal = colorSwapTex;


        //Revert back now original colours have been recorded.
        _animator.runtimeAnimatorController = Resources.Load("HumanAnimControllers/Unarmored/Player_with_nothing_controller") as RuntimeAnimatorController;

    }

    private void SetupCustomSkin()
    {
        InitColorSwapTex();

        switch (MenuManager._instance.GetPlayerCustomSkinId(_playerID))
        {
            default: //STANDARD SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x3b370c));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xe2d87a));  //SKIN A 841e00
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xd0c774));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xc2ba6d));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xb1aa64));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x999455));  //SKIN E
                break;
            case 1: //DARK SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x230e01));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0x68643d));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0x5a5635));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0x4b492e));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0x353320));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x252316));  //SKIN E
                break;
            case 2: //WHITER SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x99940d));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xeee698));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xdfd893));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xd4cd8d));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xc6c086));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0xb2ae7a));  //SKIN E
                break;
            case 3: //ORANGE SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x230e01));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xe2ba5b));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xd0a955));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xc29c4e));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xb18c45));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x997536));  //SKIN E
                break;
            case 4: //BLACK SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x855334));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0x27261b));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0x212017));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0x1b1a13));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0x14140f));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x0e0e08));  //SKIN E
                break;
            case 5: //PALE SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x83550b));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xfbf4b3));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xede6ad));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xe2ddab));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xd2cea3));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0xc6c2a0));  //SKIN E
                break;
            case 6: //STANDARD ENHANCED SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x83550b));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xffd874));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xffc768));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xffba5a));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xe2aa48));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0xb2942a));  //SKIN E
                break;
            case 7: //R lobsterish SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x641f14));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xe27c7a));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xd07274));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xc26a6d));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xb16064));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x995255));  //SKIN E
                break;
            case 8: //G zombish SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0x858382));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0x8dd84f));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0x82c74c));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0x7aba48));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0x70aa42));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x629439));  //SKIN E
                break;
            case 9: //B snowwalkerish SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0xe0e0e0));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xe2d8fc));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xd0c7f9));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xc2baf5));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xb1aaf0));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x9994e8));  //SKIN E
                break;
            case 10: //GOLDEN SKIN
                SwapColor(TexIndexData.Hair, ColorFromInt(0xf7bd60));    //HAIR
                SwapColor(TexIndexData.Skin_a, ColorFromInt(0xe2d824));  //SKIN A
                SwapColor(TexIndexData.Skin_b, ColorFromInt(0xd0c720));  //SKIN B
                SwapColor(TexIndexData.Skin_c, ColorFromInt(0xc2ba1c));  //SKIN C
                SwapColor(TexIndexData.Skin_d, ColorFromInt(0xb1aa16));  //SKIN D
                SwapColor(TexIndexData.Skin_e, ColorFromInt(0x99940d));  //SKIN E
                break;
        }
        
        //Cloth colours are preset accordingly to player Ids. P1 blue P2 red P3 green P4 yellow
        switch (_playerID)
        {
            case 0:
                SwapColor(TexIndexData.Cloth_a, ColorFromInt(0x0430ff)); //CLOTH A
                SwapColor(TexIndexData.Cloth_b, ColorFromInt(0x0b279d)); //CLOTH B
                break;
            case 1:
                SwapColor(TexIndexData.Cloth_a, ColorFromInt(0xfd5e5e)); //CLOTH A
                SwapColor(TexIndexData.Cloth_b, ColorFromInt(0xff0000)); //CLOTH B
                break;
            case 2:
                SwapColor(TexIndexData.Cloth_a, ColorFromInt(0x56e25b)); //CLOTH A
                SwapColor(TexIndexData.Cloth_b, ColorFromInt(0x56e25b)); //CLOTH B
                break;
            case 3:
                SwapColor(TexIndexData.Cloth_a, ColorFromInt(0xd6f85d)); //CLOTH A
                SwapColor(TexIndexData.Cloth_b, ColorFromInt(0xcff806)); //CLOTH B
                break;
        }

        switch (GameManager.GetCurrentPlayerSubEquipC(_playerID))
        {
            case ItemData.ItemSubType.armor_bronze:
                SwapColor(TexIndexData.Armor_Standard_a, ColorFromInt(0xced37b));
                SwapColor(TexIndexData.Armor_Standard_b, ColorFromInt(0xc4c973));
                SwapColor(TexIndexData.Armor_Standard_c, ColorFromInt(0xbabf6e));
                SwapColor(TexIndexData.Armor_Standard_d, ColorFromInt(0xafb370));
                SwapColor(TexIndexData.Armor_Standard_e, ColorFromInt(0xa0a557));
                SwapColor(TexIndexData.Armor_Standard_f, ColorFromInt(0x9ca060));
                SwapColor(TexIndexData.Armor_Standard_g, ColorFromInt(0x61512f));
                SwapColor(TexIndexData.Armor_Standard_h, ColorFromInt(0x44371c));
                break;
            case ItemData.ItemSubType.armor_iron:
                SwapColor(TexIndexData.Armor_Standard_a, ColorFromInt(0x9d9d9d));
                SwapColor(TexIndexData.Armor_Standard_b, ColorFromInt(0x8b8b8b));
                SwapColor(TexIndexData.Armor_Standard_c, ColorFromInt(0x858585));
                SwapColor(TexIndexData.Armor_Standard_d, ColorFromInt(0x808080));
                SwapColor(TexIndexData.Armor_Standard_e, ColorFromInt(0x7e7a7a));
                SwapColor(TexIndexData.Armor_Standard_f, ColorFromInt(0x747474));
                SwapColor(TexIndexData.Armor_Standard_g, ColorFromInt(0x535353));
                SwapColor(TexIndexData.Armor_Standard_h, ColorFromInt(0x1e1e1e));
                break;
            case ItemData.ItemSubType.armor_steel:
                SwapColor(TexIndexData.Armor_Standard_a, ColorFromInt(0xdedede));
                SwapColor(TexIndexData.Armor_Standard_b, ColorFromInt(0xc6c6c6));
                SwapColor(TexIndexData.Armor_Standard_c, ColorFromInt(0xbebebe));
                SwapColor(TexIndexData.Armor_Standard_d, ColorFromInt(0xb8b8b8));
                SwapColor(TexIndexData.Armor_Standard_e, ColorFromInt(0xb5b0b0));
                SwapColor(TexIndexData.Armor_Standard_f, ColorFromInt(0xa8a8a8));
                SwapColor(TexIndexData.Armor_Standard_g, ColorFromInt(0x7b7b7b));
                SwapColor(TexIndexData.Armor_Standard_h, ColorFromInt(0x343434));
                break;
            case ItemData.ItemSubType.armor_ebony:
                SwapColor(TexIndexData.Armor_Standard_a, ColorFromInt(0x565656));
                SwapColor(TexIndexData.Armor_Standard_b, ColorFromInt(0x4e4c4c));
                SwapColor(TexIndexData.Armor_Standard_c, ColorFromInt(0x494e64));
                SwapColor(TexIndexData.Armor_Standard_d, ColorFromInt(0x4c4f5a));
                SwapColor(TexIndexData.Armor_Standard_e, ColorFromInt(0x424141));
                SwapColor(TexIndexData.Armor_Standard_f, ColorFromInt(0x383636));
                SwapColor(TexIndexData.Armor_Standard_g, ColorFromInt(0x302d2d));
                SwapColor(TexIndexData.Armor_Standard_h, ColorFromInt(0x201d1d));
                break;
        }

        switch (GameManager.GetCurrentPlayerSubEquipA(_playerID))
        {
            case ItemData.ItemSubType.longsword_fire:
                SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xfdec66)); //e4e6c9 fdec66
                SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xc5ab27)); //d3d6ab c5ab27
                SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0xcbd092));
                SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0xc56527)); //b5b977 c56527
                SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x9ca060));
                SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x91321a)); //61512f 91321a
                SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0xa9361a)); //6c5a34 a9361a
                break;
            case ItemData.ItemSubType.sword_bronze:
            case ItemData.ItemSubType.longsword_bronze:
                SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xe4e6c9));
                SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xd3d6ab));
                SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0xcbd092));
                SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0xb5b977)); //b5b977
                SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x9ca060));
                SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x61512f));
                SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x6c5a34));
                break;
            case ItemData.ItemSubType.sword_iron:
            case ItemData.ItemSubType.longsword_iron:
                SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xafafaf));
                SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xa3a2a2));
                SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0x939191));
                SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0x8b8b8b));
                SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x7c7979));
                SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x144c18));
                SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x1a5a1f));
                break;
            case ItemData.ItemSubType.sword_steel:
            case ItemData.ItemSubType.longsword_steel:
                SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xd5d5d5));
                SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xcbc5c5));
                SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0xb9b9b9));
                SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0xb3afaf));
                SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x9f9a9a));
                SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x0f1c4e));
                SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x182964));
                break;
            case ItemData.ItemSubType.sword_ebony:
            case ItemData.ItemSubType.longsword_ebony:
                SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0x565965));
                SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0x424141));
                SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0x383636));
                SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0x302d2d));
                SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x201d1d));
                SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x4c1212));
                SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x601c1c));
                break;


            case ItemData.ItemSubType.shield_bronze:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xcbd092));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x9ca060));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x61512f));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x074f21));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x0e622c));
                break;
            case ItemData.ItemSubType.shield_iron:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xa8a8a8));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x7b7b7b));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x343434));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x4f1507));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x823c3d));
                break;
            case ItemData.ItemSubType.shield_steel:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xcfc8c8));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0xb1a7a7));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x918686));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x212454));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x3c407e));
                break;
            case ItemData.ItemSubType.shield_ebony:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0x2f3146));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x272834));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x1e1717));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x121d28));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x3f464e));
                break;

        }

        switch (GameManager.GetCurrentPlayerSubEquipB(_playerID))
        {
            case ItemData.ItemSubType.sword_bronze:
            case ItemData.ItemSubType.longsword_bronze:
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
				{
                    SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xe4e6c9));
                    SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xd3d6ab));
                    SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0xcbd092));
                    SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0xb5b977));
                    SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x9ca060));
                    SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x61512f));
                    SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x6c5a34));
                }
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword) //Use offset pixels for 2nd sword in anims
                {
                    SwapColor(TexIndexData.Sword_2_Standard_a, ColorFromInt(0xe4e6c9));
                    SwapColor(TexIndexData.Sword_2_Standard_b, ColorFromInt(0xd3d6ab));
                    SwapColor(TexIndexData.Sword_2_Standard_c, ColorFromInt(0xcbd092));
                    SwapColor(TexIndexData.Sword_2_Standard_d, ColorFromInt(0xb5b977));
                    SwapColor(TexIndexData.Sword_2_Standard_e, ColorFromInt(0x9ca060));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_a, ColorFromInt(0x61512f));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_b, ColorFromInt(0x6c5a34));
                }

                break;
            case ItemData.ItemSubType.sword_iron:
            case ItemData.ItemSubType.longsword_iron:
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                {
                    SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xafafaf));
                    SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xa3a2a2));
                    SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0x939191));
                    SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0x8b8b8b));
                    SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x7c7979));
                    SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x144c18));
                    SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x1a5a1f));
                }
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword) //Use offset pixels for 2nd sword in anims
                {
                    SwapColor(TexIndexData.Sword_2_Standard_a, ColorFromInt(0xafafaf));
                    SwapColor(TexIndexData.Sword_2_Standard_b, ColorFromInt(0xa3a2a2));
                    SwapColor(TexIndexData.Sword_2_Standard_c, ColorFromInt(0x939191));
                    SwapColor(TexIndexData.Sword_2_Standard_d, ColorFromInt(0x8b8b8b));
                    SwapColor(TexIndexData.Sword_2_Standard_e, ColorFromInt(0x7c7979));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_a, ColorFromInt(0x144c18));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_b, ColorFromInt(0x1a5a1f));
                }
                break;
            case ItemData.ItemSubType.sword_steel:
            case ItemData.ItemSubType.longsword_steel:
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                {
                    SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0xd5d5d5));
                    SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0xcbc5c5));
                    SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0xb9b9b9));
                    SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0xb3afaf));
                    SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x9f9a9a));
                    SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x0f1c4e));
                    SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x182964));
                }
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword) //Use offset pixels for 2nd sword in anims
                {
                    SwapColor(TexIndexData.Sword_2_Standard_a, ColorFromInt(0xd5d5d5));
                    SwapColor(TexIndexData.Sword_2_Standard_b, ColorFromInt(0xcbc5c5));
                    SwapColor(TexIndexData.Sword_2_Standard_c, ColorFromInt(0xb9b9b9));
                    SwapColor(TexIndexData.Sword_2_Standard_d, ColorFromInt(0xb3afaf));
                    SwapColor(TexIndexData.Sword_2_Standard_e, ColorFromInt(0x9f9a9a));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_a, ColorFromInt(0x0f1c4e));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_b, ColorFromInt(0x182964));
                }
                break;
            case ItemData.ItemSubType.sword_ebony:
            case ItemData.ItemSubType.longsword_ebony:
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                {
                    SwapColor(TexIndexData.Sword_Standard_a, ColorFromInt(0x565965));
                    SwapColor(TexIndexData.Sword_Standard_b, ColorFromInt(0x424141));
                    SwapColor(TexIndexData.Sword_Standard_c, ColorFromInt(0x383636));
                    SwapColor(TexIndexData.Sword_Standard_d, ColorFromInt(0x302d2d));
                    SwapColor(TexIndexData.Sword_Standard_e, ColorFromInt(0x201d1d));
                    SwapColor(TexIndexData.Sword_Standard_handle_a, ColorFromInt(0x4c1212));
                    SwapColor(TexIndexData.Sword_Standard_handle_b, ColorFromInt(0x601c1c));
                }
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword) //Use offset pixels for 2nd sword in anims
                {
                    SwapColor(TexIndexData.Sword_2_Standard_a, ColorFromInt(0x565965));
                    SwapColor(TexIndexData.Sword_2_Standard_b, ColorFromInt(0x424141));
                    SwapColor(TexIndexData.Sword_2_Standard_c, ColorFromInt(0x383636));
                    SwapColor(TexIndexData.Sword_2_Standard_d, ColorFromInt(0x302d2d));
                    SwapColor(TexIndexData.Sword_2_Standard_e, ColorFromInt(0x201d1d));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_a, ColorFromInt(0x4c1212));
                    SwapColor(TexIndexData.Sword_2_Standard_handle_b, ColorFromInt(0x601c1c));
                }
                break;


            case ItemData.ItemSubType.shield_bronze:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xcbd092));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x9ca060));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x61512f));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x074f21));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x0e622c));
                break;
            case ItemData.ItemSubType.shield_iron:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xa8a8a8));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x7b7b7b));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x343434));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x4f1507));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x823c3d));
                break;
            case ItemData.ItemSubType.shield_steel:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0xcfc8c8));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0xb1a7a7));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x918686));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x212454));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x3c407e));
                break;
            case ItemData.ItemSubType.shield_ebony:
                SwapColor(TexIndexData.Shield_Standard_a, ColorFromInt(0x2f3146));
                SwapColor(TexIndexData.Shield_Standard_b, ColorFromInt(0x272834));
                SwapColor(TexIndexData.Shield_Standard_c, ColorFromInt(0x1e1717));
                SwapColor(TexIndexData.Shield_Standard_d, ColorFromInt(0x121d28));
                SwapColor(TexIndexData.Shield_Standard_e, ColorFromInt(0x3f464e));
                break;
        }



        _colorSwapTex.Apply();
    }


    private void OnEnabled()
    {
    }

    private void OnDisabled()
    {
    }


    void FixedUpdate()
    {
        //CollisionsFixedUpdate();
        //DetectCollisionsFixedUpdate();


    }

    void Update()
    {
        //if (_exitDemoTimer.HasTimerFinished(0))
        //{
        //    //MenuManager._instance.Setup();
        //    GameManager.SetScene(1);
        //    MenuManager._instance.LoadScene(1);

        //}

        //print("\n_processUpdate=" + _processUpdate);
        if (_processUpdate)
        {
            //print("\n_falling=" + _falling);
            UpdateDeathFlag();
            UpdateTargetFlag();
            UpdateWeaponChargeTexFX();
            UpdateTextureFlashFX();
            UpdateStaminaPool();
            UpdateInput();
            UpdateAnimator();
            UpdateMovement();
            UpdateStateBehaviours();
            UpdateGravity();
            UpdateTransform();
        }

		if (DialogueManager._instance.GetIsCurrentActEndFlag())
		{
			UpdateTextureFlashFX();
			UpdateStateBehaviours();
			UpdateGravity();
			UpdateTransform();
		}
		if (DialogueManager._instance.GetIsStoryTellingFlag())
		{
            if (GameManager._instance.GetScene() == GameManager.Level.TrainingGroundScene)
                return;
            if (GameManager._instance.GetScene() == GameManager.Level.TutorialScene)
                return;
            if (GameManager._instance.GetScene() == GameManager.Level.TheKingsCallScene)
                return;
            //UpdateStateBehaviours();
            UpdateGravity();
			UpdateTransform();
		}



	}//0300 2003410 REF 623 95332 29951 A

    private void BeginWeaponChargeTexFX()
    {
        if (!_weapChrgeTexFlag)
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 0);
            _weapChrgeTextureTimer.StartTimer(0);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xffffff));
            _colorSwapTex.Apply();
            _weapChrgeTexFlag = true;
        }
    }

    private void FinishWeaponChargeTexFX()
    {
        if (_weapChrgeTexFlag)
        {
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xffffff));
            _colorSwapTex.Apply();
            for (int i = 0; i < 9; i++)
            {
                _weapChrgeTextureTimer.ForceTimerEnd(i);
            }
            _weapChrgeTexFlag = false;
        }
    }

    private void UpdateWeaponChargeTexFX()
    {
        if (!_weapChrgeTexFlag)
            return;

        //Moved too UpdateStateAtkBehaviours() for optimization reasons.
        //switch (StateManager.GetAttackState(_playerID))
        //{
        //    case AttackStateData.CurrentState.ChargeStart:
        //        SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xd5e400));
        //        break;
        //    case AttackStateData.CurrentState.ChargeMid:
        //        SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xc9783e));
        //        break;
        //    case AttackStateData.CurrentState.ChargeEnd:
        //        SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xa12c1d));
        //        break;
        //    case AttackStateData.CurrentState.None:

        //        break;
        //}

        if (_weapChrgeTextureTimer.HasTimerFinished(0))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 1);
            _weapChrgeTextureTimer.StartTimer(1);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xe5f7ba));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(1))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 2);
            _weapChrgeTextureTimer.StartTimer(2);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfded91));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(2))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 3);
            _weapChrgeTextureTimer.StartTimer(3);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfbce5e));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(3))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 4);
            _weapChrgeTextureTimer.StartTimer(4);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfdac40));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(4))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 5);
            _weapChrgeTextureTimer.StartTimer(5);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xff6a2f));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(5))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 6);
            _weapChrgeTextureTimer.StartTimer(6);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfdac40));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(6))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 7);
            _weapChrgeTextureTimer.StartTimer(7);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfbce5e));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(7))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 8);
            _weapChrgeTextureTimer.StartTimer(8);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xfded91));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(8))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 9);
            _weapChrgeTextureTimer.StartTimer(9);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xe5f7ba));
            _colorSwapTex.Apply();
        }
        if (_weapChrgeTextureTimer.HasTimerFinished(9))
        {
            _weapChrgeTextureTimer.SetTimerDuration(0.1f, 0);
            _weapChrgeTextureTimer.StartTimer(0);
            SwapColor(TexIndexData.ChargeFX, ColorFromInt(0xffffff));
            _colorSwapTex.Apply();
        }
    }

    private void UpdateTextureFlashFX()
    {
        if (!_processTexFxFlag)
            return;

        if (_hitTextureTimer.HasTimerFinished(0))
        {
            //Assign original texture data.
            //SetupCustomSkin(); //Nope, black outline isn't accounted for.
            ResetSetSpriteColor();
            _processTexFxFlag = false;
            _hitTexFlag = false;
        }

    }

    private void UpdateStaminaPool()
    {
        //Guard clauses to allow for health stat updates upon attack collisions.
        if (StateManager.IsHitByQuickAttack(_playerID))
            return;
        if (StateManager.IsHitByReleaseAttack(_playerID))
            return;
        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
            return;
        if (StateManager.IsQuickHitWhileDefending(_playerID))
            return;
        if (StateManager.IsHitWhileDefending(_playerID))
            return;
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.Dying)
            return;
        //Guard clause.
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.Dead)
            return;

        if(GameManager.GetPlayerStatUseFlag(_playerID) != 3)
		{
            GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(false);
            GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
        }

    }

    private void UpdateDeathFlag()
    {
        //Guard clause.
        if (!_processUpdate) //Flagged upon death, after dying anim.
            return;

        //Guard clause.
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.Dying)
            return;

        //Guard clause.
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.Dead)
            return;

        //if (_stats.Health <= 0f)
        if (GameManager.GetPlayerStatData(_playerID).Health <= 0f)
        {
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Dying);
            _IDController.SetDeathFlag(true);
        }
        if (GameManager.GetPlayerStatData(_playerID).Stamina <= 0.1f)
        {
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Exhausted);
            _IDController.SetExhaustedFlag(true);
            _behaviourTimer.SetTimestampedFlag(false, _playerID);
            _behaviourTimer.SetTimerDuration(2.0f, _playerID);//0.4f
            _behaviourTimer.StartTimer(_playerID);
            SetTextureFlashFX(Color.white, 0.10f);
        }
    }

    public void SetAutoCurTarget(Transform t, int enemyId)
	{
        //_lastCurTargEnemyId = enemyId;
        //_targetController.DeselectClosestTarget(false, true);
        //_targetController.SetCurTargetAttacked(t, enemyId);
    }

    private void UpdateTargetFlag()
    {
        if (_applyTargUpdate)
        {
            //_targetController

			_applyTargUpdate = false;
        }
    }

    private void InitialJumpLogic()
    {
        JumpInteruptionsLogic();
        if (!_dblJumped && !_jumped) //Initial Jump
        {


            _jumpBoost = 1.0f;
            _behaviourTimer.SetTimerDuration(0.5f, _playerID);
            if (!_behaviourTimer.HasTimerBeenSet(_playerID))
            {
                _behaviourTimer.StartTimer(_playerID);
                _targetVelocityY = (_maxJumpVelocity * _jumpBoost);// * 2.0f;
                _velocity.y = _targetVelocityY; //Gravity unaffected by this, works here.
                _jumped = true;//For animator.
                _dblJumped = false;
                if(!_controller.collisions.platformDrop)
                    _falling = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                //StateManager.AddToPlayerStateQueue(_playerID, PlayerStateData.CurrentState.Jump);

                AudioManager.PlaySfx(0, _playerID);
            }

            if (!_jumpCloudFlag)
            {
                if (_isInWater)
                    return;
                if (_dustCloudFlag)
                {
                    _dustCloudFlag = false;
                }
                _fxTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                _fxTimer.StartTimer(_playerID);
                StateManager.SetPlayerDir(_playerID, _faceDir);
                FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx
                _jumpCloudFlag = true;
            }
        }

    }

    private void SecondaryJumpLogic()
    {
        //_jumpBoost = 1.25f;
        if (!_dblJumped && _jumped) //Secondary Jump
        {
            _jumpBoost = 1.25f;
            _behaviourTimer.SetTimerDuration(0.05f, _playerID);
            if (!_behaviourTimer.HasTimerBeenSet(_playerID))
            {
                _behaviourTimer.StartTimer(_playerID);
                _targetVelocityY = (_maxJumpVelocity * _jumpBoost);// * 2.0f;
                _velocity.y = _targetVelocityY; //Gravity unaffected by this, works here.
                _jumped = false;
                _dblJumped = true;//For animator.
                if(!_controller.collisions.platformDrop)
                    _falling = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);

                AudioManager.PlaySfx(5, _playerID);
            }

            if (!_jumpCloudFlag)
            {
                _fxTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                _fxTimer.StartTimer(_playerID);
                StateManager.SetPlayerDir(_playerID, _faceDir);
                FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx
                _jumpCloudFlag = true;
            }
        }
    }

    private void JumpInteruptionsLogic()
    {
        //Any jump states that are interupted by other states, such as an attack,being hit, ensure
        //we complete the jump state after these interuptions by setting a flag.
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByReleaseAtk)
        {
            _jumpDuringHit = true;
            _hitBehaviourTimer.ForceTimerEnd(_playerID);
            return;
        }
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByQuickAtk)
        {
            _jumpDuringHit = true;
            _hitBehaviourTimer.ForceTimerEnd(_playerID);
            //_isQuickHit = false;
            //if (_hitBehaviourTimer.HasTimerFinished(_playerID))//calls DetectTimePassed() to fully complete ForceTimerEnd().. i think.
            //    return;
            return;
        }
    }

    private bool SafeToComboAttack()
	{
        //PREVENT LONGSWORD COMBO ATTACK ANIMATIONS
        switch(GameManager.GetCurrentPlayerSubEquipA(_playerID))
		{
            case ItemData.ItemSubType.longsword_bronze:
            case ItemData.ItemSubType.longsword_iron:
            case ItemData.ItemSubType.longsword_steel:
            case ItemData.ItemSubType.longsword_ebony:
                return false;
        }
            

        //Shield & sword
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield && GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            return true;

        //Sword & shield
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword && GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Shield)
            return true;

        //Sword & sword
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword && GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            return true;

        //Just a sword
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword && GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.None)
            return true;

        if (StateManager.IsExhausted(_playerID))
            return false;

        return false;
	}
    
    private void UpdateInput()
    {
        if (StateManager.IsDying(_playerID))
            return;
        if (StateManager.IsDead(_playerID))
            return;


        //if (MenuManager._instance.GetPlayerInputType(_playerID) == InputData.InputType.KEYBOARD)
        //{
        //    if (_rewirdInputPlayer.GetAnyButtonUp())
        //        _input.x = 0.0f;
        //}


        #region PauseInput
        if (_rewirdInputPlayer.GetButtonTimedPressUp("Pause", 0.05f))
        {
            if (!_processUpdate)
                return;
            //if (DialogueManager._instance._dialogueFlag == DialogueManager.DialogueFlag.merchant)
            //    return;
            //if (DialogueManager._instance._dialogueFlag == DialogueManager.DialogueFlag.storyteller)
            //    return;
            //if (DialogueManager._instance._dialogueFlag == DialogueManager.DialogueFlag.message)
            //    return;

            if (!GameManager.GetPausedFlag())
            {
                GameManager.SetPausedFlag(true);
                //_processUpdate = false; //moved to PlayerMonitor.cs so all players are effected.
                return;
            }
        }
        #endregion

        #region MovementInput
        if (_rewirdInputPlayer.GetButtonDown("Sprint"))
		{
            _sprintSpeed = 1.75f;
            _isSprint = true; //Was this held down upon run attack? initialize FX
            _isUpwardAtk = false;
            _moveSpeed = 3.75f;//3.5f;
        }
        if (_rewirdInputPlayer.GetButtonUp("Sprint"))
		{
            //_isSprint = false;
            _sprintSpeed = 1.0f;
        }
        if (MenuManager._instance.GetPlayerInputType(_playerID) == InputData.InputType.KEYBOARD)
        {
            //Works when only 1 axis allowed per player in rewired editor.
            _input.x = _rewirdInputPlayer.GetAxis(0);
            _input.y = _rewirdInputPlayer.GetAxis("MoveVertical");
            //print("\ninput.y=" + _input.y);

            if (_input.x < 0.1f)
                _input.x = _rewirdInputPlayer.GetAxis(1);
            if (_input.y < -0.01f)
			{
                if(_controller.collisions.platformDrop)
				{
                    _falling = true;
                    UpdateAnimator();
                    //print("\nreached!");
				}
			}

            //if (_rewirdInputPlayer.GetButtonDown("KeyboardMoveLeft"))
            //	_input.x = -1.0f;
            //if (_rewirdInputPlayer.GetButtonDown("KeyboardMoveRight"))
            //	_input.x = 1.0f;

            if (_rewirdInputPlayer.GetAnyButtonUp())
                _input.x = 0.0f;

            //_input.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");
            //print("\ninput.x=" + _input.x);


        }
        if (MenuManager._instance.GetPlayerInputType(_playerID) != InputData.InputType.XBOXONE)
        {

			_input.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");

			//Works when only 1 axis allowed per player in rewired editor.
			//_input.x = _rewirdInputPlayer.GetAxis(0);

			//if (_input.x < 0.1f)
			//    _input.x = _rewirdInputPlayer.GetAxis(1);

			//if (_rewirdInputPlayer.GetAnyButtonUp())
			//    _input.x = 0.0f;

		}
        //https://forum.unity.com/threads/eventsystem-button-script-and-using-joystick-gamepad.548419/

        _input.y = _rewirdInputPlayer.GetAxis("MoveVertical");
        ////print("\nVerticalAxis=" + _input.y);
        //("\n_isUpwardAtk=" + _isUpwardAtk);
        if (_input.y >= 0.5f)
		{
            //print("\nTarget");
            
            _isUpwardAtk = true;
            _isSprint = false;
        }
        else
		{
            if (StateManager.IsUpwardAttack(_playerID))
                return;

            if (_isUpwardAtk)
            {
                _isUpwardAtk = false;
                _smlAttack = 0;
                _smlAtkIncrement = 0;
            }

        }
        #endregion

        #region JumpInput
        if (_rewirdInputPlayer.GetButtonDown("Jump"))
        {
            JumpInputLogic();
            return;
        }
        #endregion

        #region PowerAttackInput
        if (_rewirdInputPlayer.GetButtonTimedPressDown("Attack", 0.25f))//Power Attack
        {
            HoldPowerAttackInputLogic();

            return;
        }
        if (_rewirdInputPlayer.GetButtonDown("Attack"))
        {
            ////_velocity.x = 0f; //doesn't work.

            ////_velocity.x = 0f;
            ////_targetVelocityX = 0f;

            ////         if (StateManager.IsHitByQuickAttack(_playerID))
            ////{
            ////             _attacked = false;//For animator.
            ////             _animator.SetBool("attacked", _attacked);
            ////         }

            //if (!SafeToAttack())
            //    return;

            //if (StateManager.IsAttackPrepare(_playerID))
            //{
            //    _sprintSpeed = 1.0f;

               

            //    //if (!SafeToAttack())
            //    //    return;
            //    BeginWeaponChargeTexFX();
            //    if (!_attacked)
            //        _attacked = true;//For animator.

            //    //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldAttack);
            //    //GameManager.SetReleaseAtkEnemyData(AttackStateData.CurrentState.ChargeStart);
            //    //_childFXController.SetPlayerDXId(1);
            //    //AudioManager.PlaySfx(1, _playerID);
            //}
        }
        if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.25f))
        {

            ReleasePowerAttackInputLogic();
            return;
        }
        if (_rewirdInputPlayer.GetButtonUp("Attack")) //QA above may be missed, so catch this case sceneria
        {
            if (StateManager.IsAttackPrepare(_playerID))
            {
                _sprintSpeed = 1.0f;
                //_behaviourTimer.ForceTimerEnd(_playerID);
                //_behaviourTimer.SetTimestampedFlag(false, _playerID);
                _behaviourTimer.SetTimerDuration(0.5f, _playerID);//(0.33f, _playerID);
                _attacked = false;//For animator.
                _released = true;
                _weaponCollidedFlag = false;
                _enableAtkUpdate = true;
                FinishWeaponChargeTexFX();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ReleaseAttack);
                _behaviourTimer.StartTimer(_playerID);
                AudioManager.StopAttackSfxFlag(_playerID);
                GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost();
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
            }
        }
        #endregion

        #region ComboAttackInput
        //if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
        if (_rewirdInputPlayer.GetButtonDoublePressUp("Attack"))
        {
            if (!SafeToComboAttack())
                return;

            if (StateManager.IsLockedComboAttack(_playerID))
                return;

            if (GameManager.GetTankEnemyBattleFlag())
                return;

            if (GameManager._instance.GetScene() == GameManager.Level.TutorialScene)
                return;

            if (_comboInputFlag < 100)//28
            {
                _comboInputFlag++;
            }
            else
            {
                if (_comboInputFlag == 100)
                {
                    //if (StateManager.IsExhausted(_playerID))
                    //    return;
                    if (!SafeToComboAttack())
                        return;

                    _smlAttack = 0;
                    _velocity.x = 0f;
                    _targetVelocityX = 0f;
                    //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                    _comboInputFlag = 0;

                    int targEnemyId = _targetController.GetActiveTargetEnemyId();
                    if (targEnemyId == -1)
                        return;

                    if (EnemyStatesManager.IsDead(targEnemyId))
                        return;
                    if (EnemyStatesManager.IsDying(targEnemyId))
                        return;

                    _comboAttack = 1;
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.LockedComboAttack);
                    EnemyStatesManager.SetEnemyState(_targetController.GetActiveTargetEnemyId(), EnemyStateData.CurrentState.LockedComboHit);
                    SetTextureFlashFX(Color.white, 0.25f);
                    _comboAttackTimer.SetTimestampedFlag(false, _comboAttack);
                    _comboAttackTimer.SetTimerDuration(2.0f, _comboAttack);//0.4f
                    _comboAttackTimer.StartTimer(_comboAttack);
                    //_spriteTrailRef.EnableTrail();
                    FXObjPooler._curInstance.Instantiate(_transform.position, 19, null, 0);
                    AudioManager.PlaySfx(23, _playerID);
                    NullifyAnimatorFlags();
                    //Time.timeScale = 0.75f;
                }
            }

        }
        #endregion

        #region QuickAttackInput
        //if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.05f)) //GetButtonDown
        //if (_rewirdInputPlayer.GetButtonDown("Attack"))
        if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
        {
            QuickAttackInputLogic();
        }
        if (_rewirdInputPlayer.GetButtonUp("Attack"))
        {

        }
        #endregion

        #region KnockBackInput
        if (_rewirdInputPlayer.GetButtonTimedPressUp("KnockBack", 0.01f))
        {
            //if (GameManager._instance._level == GameManager.Level.TutorialScene)
            //    return;
            //print("\nKnockBackAttack button pressed!");

            //UNARMORED ANIMS
            //Broadsword & shield done
            //Broadsword done
            KnockBackInputLogic();
        }
        #endregion



        #region DefenceInput
            if (_rewirdInputPlayer.GetButtonDown("Defend"))
        {
            HoldDefenseInputLogic();

        }
        if (_rewirdInputPlayer.GetButtonUp("Defend"))
        {
            ReleaseDefenseInputLogic();

        }
        #endregion

        #region DodgeInput
        if (_rewirdInputPlayer.GetButtonDown("DodgeL"))
        {
            DodgeLeftInputLogic();
        }
        if (_rewirdInputPlayer.GetButtonDown("DodgeR"))
        {
            DodgeRightInputLogic();
        }
        #endregion

        #region TargetInput
        if (_rewirdInputPlayer.GetButtonDown("Target"))
        {
            //         _sprintSpeed = 1.0f;
            //         if (!_enableTargUpdate)
            //{
            //             //_targetController.FlushCachedTargets();
            //             AudioManager.PlaySfx(16, _playerID);
            //             _enableTargUpdate = true;
            //         }
            if (StateManager.IsRunningQuickAttack(_playerID))
                return;

            _sprintSpeed = 1.0f;
            if (_enableTargUpdate)
                _enableTargUpdate = false;
            if (_applyTargUpdate)
                _applyTargUpdate = false;
            _targetController.DeselectClosestTarget();
        }
        if (_rewirdInputPlayer.GetButtonTimedPressDown("Target", 0.5f))
        {
            if (StateManager.IsRunningQuickAttack(_playerID))
                return;

            _sprintSpeed = 1.0f;
            if (_enableTargUpdate)
                _enableTargUpdate = false;
            if (_applyTargUpdate)
                _applyTargUpdate = false;
            _targetController.DeselectClosestTarget();
            
        }
        #endregion


    }

    public void JumpInputLogic()
	{
        //Guard clause.
        if (!SafeToJump())
            return;

        //AIPathFinder.RecordPlayerJumpPoint(_playerID, _transform.position, _faceDir);

        _sprintSpeed = 1.0f;

        //JumpInteruptionsLogic();
        InitialJumpLogic();
        SecondaryJumpLogic();
    }

    public void HoldPowerAttackInputLogic()
	{

        //Guard clause.
        if (!SafeToAttack())
            return;

        _sprintSpeed = 1.0f;

        if (GameManager.GetPlayerStatData(_playerID).Stamina <= 0.1f)
        {
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Exhausted);
            _IDController.SetExhaustedFlag(true);
            _behaviourTimer.SetTimestampedFlag(false, _playerID);
            _behaviourTimer.SetTimerDuration(2.0f, _playerID);//0.4f
            _behaviourTimer.StartTimer(_playerID);
            SetTextureFlashFX(Color.white, 0.10f);
            _attacked = false;
            _released = false;
            UpdateAnimator();
            return;
        }
        //if (StateManager.IsExhausted(_playerID))
        //    return;

        _isQuickHit = false;
        _animator.SetBool("jab", _isQuickHit);
        //print("\nAttack button is being held.");
        BeginWeaponChargeTexFX();
        _attacked = true;//For animator.
                         //SetAtkAnim(true, false);//For animator.


        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldAttack);
        GameManager.SetReleaseAtkEnemyData(AttackStateData.CurrentState.ChargeStart, _playerID);
        //_childFXController.SetPlayerDXId(1);
        AudioManager.PlaySfx(1, _playerID);

    }

    public void ReleasePowerAttackInputLogic()
	{
        //Guard clause.
        if (!SafeToAttack())
            return;

        //print("\nAttack button released.");
        //_behaviourTimer.ForceTimerEnd(_playerID);
        //_behaviourTimer.SetTimestampedFlag(false, _playerID);
        //_behaviourTimer.SetTimerDuration(0.5f, _playerID);//(0.33f, _playerID);
        _behaviourTimer.SetTimerDuration(0.5f, _playerID);//(0.33f, _playerID);
        _attacked = false;//For animator.
        _released = true;

        _weaponCollidedFlag = false;
        _enableAtkUpdate = true;

        FinishWeaponChargeTexFX();

        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ReleaseAttack);
        //if (StateManager.IsAttackPrepare(_playerID))
        //{
        //    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ReleaseAttack);
        //}
        //if (StateManager.IsMidAirAttackPrepare(_playerID))
        //{
        //    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.MidAirReleaseAttack);
        //}

        _behaviourTimer.StartTimer(_playerID);


        AudioManager.StopAttackSfxFlag(_playerID);



        GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost();
        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
    }

    public void QuickAttackInputLogic()
	{
        
        //Prevent spamming.
        if (_behaviourTimer.HasTimerBeenSet(_playerID))
        {
            if (!_behaviourTimer.HasTimerFinished(_playerID))
                return;
        }

        //Guard clause. This means we're doing a strong attack and don't want a quick attack to execute.
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
            return;

        //Guard clause.
        if (!SafeToQuickAttack())
            return;

        _sprintSpeed = 1.0f;






        //Following 2 lines are to clear the jump timer and allow for the quick attack midair timer.
        //_behaviourTimer.ForceTimerEnd(_playerID);
        _behaviourTimer.SetTimestampedFlag(false, _playerID);
        _behaviourTimer.SetTimerDuration(0.1f, _playerID);//0.4f
        if (_controller.collisions.below)
        {
            if (_isUpwardAtk) //(_input.y >= 0.4f) //UPWARD ATTACK
            {
                _enableAtkUpdate = true;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.AttackUpward);
                _behaviourTimer.SetTimerDuration(0.65f, _playerID);
                _smlAttack = 6;
                _smlAtkIncrement = 0;
            }
            else
			{
                if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller.collisions.below)
                {
                    if (_isSprint)
                    {
                        if (IsWeaponEquipped())//We have sword equiped.
                        {
                            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.RunningQuickAttack);
                            _behaviourTimer.SetTimerDuration(0.65f, _playerID);

                            _enableAtkUpdate = false;
                            _behaviourTimer.SetTimestampedFlag(false, 10);
                            _behaviourTimer.SetTimerDuration(0.325f, 10);
                            _behaviourTimer.StartTimer(10);
                            _behaviourTimer.SetTimestampedFlag(false, 11);
                            _behaviourTimer.SetTimerDuration(0.475f, 11);
                            _behaviourTimer.StartTimer(11);
                        }
                        else
                        {
                            _enableAtkUpdate = true;
                            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.QuickAttack);
                        }
                    }
                    else
                    {
                        _enableAtkUpdate = true;
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.QuickAttack);
                    }


                }
                else
                {
                    _enableAtkUpdate = true;
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.QuickAttack);
                }
            }

        }
        else
        {
            if (_isUpwardAtk) //(_input.y >= 0.4f) //UPWARD ATTACK
			{
                _enableAtkUpdate = true;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.AttackUpward);
                _behaviourTimer.SetTimerDuration(0.35f, _playerID);
                _smlAttack = 6;
                _smlAtkIncrement = 0;
            }
            else
			{
                if (!_behaviourTimer.HasTimerFinished(_playerID + 4))
                    return;
                _behaviourTimer.SetTimestampedFlag(false, _playerID + 4);
                _behaviourTimer.SetTimerDuration(1.0f, _playerID + 4);//0.4f
                _behaviourTimer.StartTimer(_playerID + 4);
                //Above timer code used to prevent spam air attacks
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.MidAirAttack);
                _enableAtkUpdate = true;
            }

        }
        _behaviourTimer.StartTimer(_playerID);


        _smlAttack = 1 + _smlAtkIncrement; //increment should be zero initially.
                                           //_animator.SetInteger("smlAttack", _smlAttack);
        _weaponCollidedFlag = false;
        //_enableAtkUpdate = true;

        _isQuickHit = false;
        _animator.SetBool("jab", _isQuickHit);

        SetTextureFlashFX(Color.white, 0.025f);

        AudioManager.PlaySfx(19, _playerID);
        GameManager.GetPlayerStatData(_playerID).Action = _stats.GetQuickAttackActionCost();
        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
        GameManager.SetStaminaUIFlag(_playerID, true);
    }

    public void KnockBackInputLogic()
	{
        
        //Prevent spamming.
        if (_behaviourTimer.HasTimerBeenSet(_playerID))
        {
            if (!_behaviourTimer.HasTimerFinished(_playerID))
                return;
        }

        //Guard clause.
        if (!SafeToKnockAttack())
            return;

        _sprintSpeed = 1.0f;

        BeginWeaponChargeTexFX();

        if (_controller.collisions.below)
        {
            SetTextureFlashFX(Color.white, 0.035f);
            //_velocity.x = 0f;
            _behaviourTimer.SetTimestampedFlag(false, _playerID);
            _behaviourTimer.SetTimerDuration(0.1f, _playerID);//0.4f
            _behaviourTimer.StartTimer(_playerID);
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.KnockBackAtkPrepare);
            _knockAttack = 1;
            AudioManager.PlaySfx(24, _playerID);
            
        }
        else
        {
            _behaviourTimer.SetTimestampedFlag(false, _playerID);
            _behaviourTimer.SetTimerDuration(0.1f, _playerID);//0.4f
            _behaviourTimer.StartTimer(_playerID);
            //_knockAttack = 1;
            AudioManager.PlaySfx(24, _playerID);
            FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.MidAirHoldAttack);
            FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx
            

        }
        //else
        //{
        //    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.MidAirAttack);
        //}



    }

    public void HoldDefenseInputLogic()
	{
        _moveSpeed = _moveSpeed / 2;

        //Guard clause.
        if (!SafeToDefend())
            return;

        //print("\nDefend button pressed.");
        _defended = true;//For animator.
        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);
        
    }

    public void ReleaseDefenseInputLogic()
	{
        _moveSpeed = 3.75f;//3.5f;
        //Guard clause.
        if (!SafeToDefend())
            return;

        //print("\nDefend button released.");

        //_behaviourTimer.SetTimerDuration(0.25f, _playerID);
        _defended = false;//For animator.

        _isHitDefending = false;
        _isQuickHitDefending = false;
        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
       
        //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ReleaseDefence);
        //_behaviourTimer.StartTimer(_playerID);
        //print("\nDefend release timer begun.");
    }

    public void DodgeLeftInputLogic()
	{
        
        //Guard clause.
        if (!SafeToDodge())
            return;

        _sprintSpeed = 1.0f;


        _isQuickHit = false;

        //print("\nLeft Dodge button pressed.");
        _dodge = true;//For animator.
        _velocity.x = 0f;
        _dodgeDir = -1;

        if (_faceDir == -1)
        {
            if (_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
                _faceDir = 1;
                SetAtkCollisionFaceDir(_faceDir);
            }
        }


        _behaviourTimer.SetTimestampedFlag(false, _playerID);
        _behaviourTimer.SetTimerDuration(0.25f, _playerID);//0.15f
        _behaviourTimer.StartTimer(_playerID);
        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.DodgePrepare);

        UpdateAnimator();

        GameManager.GetPlayerStatData(_playerID).Action = _stats.GetDodgeActionCost();
        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
        GameManager.SetStaminaUIFlag(_playerID, true);

        SetTextureFlashFX(Color.white, 0.10f);

        AudioManager.PlaySfx(18, _playerID);
    }

    public void DodgeRightInputLogic()
	{
        
        //Guard clause.
        if (!SafeToDodge())
            return;

        _sprintSpeed = 1.0f;

        _isQuickHit = false;

        //print("\nRight Dodge button pressed.");
        _dodge = true;//For animator.
        _velocity.x = 0f;
        _dodgeDir = 1;

        if (_faceDir == 1)
        {
            if (!_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
                _faceDir = -1;
                SetAtkCollisionFaceDir(_faceDir);
            }
        }


        _behaviourTimer.SetTimestampedFlag(false, _playerID);
        _behaviourTimer.SetTimerDuration(0.25f, _playerID);//0.15f
        _behaviourTimer.StartTimer(_playerID);
        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.DodgePrepare);

        UpdateAnimator();

        GameManager.GetPlayerStatData(_playerID).Action = _stats.GetDodgeActionCost();
        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
        GameManager.SetStaminaUIFlag(_playerID, true);

        SetTextureFlashFX(Color.white, 0.10f);

        AudioManager.PlaySfx(18, _playerID);
    }

    private void ApplyAnimController()
    {
        if (GameManager.ItemPickupFlagged(_playerID))
        {


            SetupCustomSkin(); //Call here too. Ensures the correct skin is applied during equipment gain anims.
            _equipAquiredId = true; //for animator

            //DONT FORGET TO UPDATE HERE TOO! SetupPreviousSceneEquip()

            switch (GameManager.GetCurrentPlayerEquipA(_playerID))
            {
                case ItemData.ItemType.None:
                    _animatorControllerIndex = 0;
                    break;
                case ItemData.ItemType.Sword:
                    switch (GameManager.GetCurrentPlayerSubEquipA(_playerID))
                    {
                        case ItemData.ItemSubType.sword_bronze:
                        case ItemData.ItemSubType.sword_iron:
                        case ItemData.ItemSubType.sword_steel:
                        case ItemData.ItemSubType.sword_ebony:
                            _animatorControllerIndex = 1;
                            //FXObjPooler._curInstance.Instantiate(_transform.position, 24, null);//24 = broadsword gained fx
                            break;
                        case ItemData.ItemSubType.longsword_bronze:
                        case ItemData.ItemSubType.longsword_iron:
                        case ItemData.ItemSubType.longsword_steel:
                        case ItemData.ItemSubType.longsword_ebony:
                        case ItemData.ItemSubType.longsword_fire:
                            _animatorControllerIndex = 5;

                            break;
                    }
                    break;
                case ItemData.ItemType.Shield:
                    _animatorControllerIndex = 2;
                    //FXObjPooler._curInstance.Instantiate(_transform.position, 25, null);//25 = shield gained fx
                    break;
            }
            switch (GameManager.GetCurrentPlayerEquipB(_playerID))
            {

                case ItemData.ItemType.Sword:
                    switch (GameManager.GetCurrentPlayerSubEquipB(_playerID))
                    {
                        case ItemData.ItemSubType.sword_bronze:
                        case ItemData.ItemSubType.sword_iron:
                        case ItemData.ItemSubType.sword_steel:
                        case ItemData.ItemSubType.sword_ebony:
                            _animatorControllerIndex = 3;
                            //FXObjPooler._curInstance.Instantiate(_transform.position, 24, null);//24 = broadsword gained fx
                            break;
                        case ItemData.ItemSubType.longsword_bronze:
                        case ItemData.ItemSubType.longsword_iron:
                        case ItemData.ItemSubType.longsword_steel:
                        case ItemData.ItemSubType.longsword_ebony:
                        case ItemData.ItemSubType.longsword_fire:
                            _animatorControllerIndex = 5;
                            if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                                _animatorControllerIndex = 6;

                            break;
                    }

                    break;
                case ItemData.ItemType.Shield:
                    _animatorControllerIndex = 3;
                    //FXObjPooler._curInstance.Instantiate(_transform.position, 25, null);//25 = shield gained fx
                    switch (GameManager.GetCurrentPlayerSubEquipA(_playerID))
                    {
                        case ItemData.ItemSubType.sword_bronze:
                        case ItemData.ItemSubType.sword_iron:
                        case ItemData.ItemSubType.sword_steel:
                        case ItemData.ItemSubType.sword_ebony:
                            _animatorControllerIndex = 3;

                            break;
                        case ItemData.ItemSubType.longsword_bronze:
                        case ItemData.ItemSubType.longsword_iron:
                        case ItemData.ItemSubType.longsword_steel:
                        case ItemData.ItemSubType.longsword_ebony:
                        case ItemData.ItemSubType.longsword_fire:
                            _animatorControllerIndex = 6;

                            break;
                    }
                    break;
            }

            if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword &&
                GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
			{


                _animatorControllerIndex = 4;
            }



            if (GameManager.GetArmoredUpFlag())
            {
                //FXObjPooler._curInstance.Instantiate(_transform.position, 26, null);//26 = armor gained fx
                _equipAquiredId = true; //for animator
                _animControllerFilepath = "HumanAnimControllers/Armored/";
                //FXObjPooler._curInstance.Instantiate(_transform.position, 6); //arg2= 6 for armorGainfx 
                //AudioManager.PlaySfx(12, _playerID);
                GameManager.SetArmoredUpFlag(false);
            }


			if (GameManager.IsItemBuyFlag())
			{
				//GameManager.SetItemBuyEquipId(0); //Reset to 0 as EquipC isn't included here. DON'T RESET increment never gets passed 1 this way!
				GameManager.SetItemBuyType(ItemData.ItemType.None);
				GameManager.SetItemBuyFlag(false);
			}


			FXObjPooler._curInstance.Instantiate(_transform.position, 6, null, 0); //arg2= 6 for armorGainfx 
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.GainedEquipment);
            _equipGainedAnimTimer.SetTimerDuration(1.25f, 0);
            _equipGainedAnimTimer.StartTimer(0);
            //AnimControllerSwapout(); //Moved to _equipGainedAnimTimer finish. This is so the anim is applied during current anim controller.

            //ApplyCustomItemSkin();
            GameManager.ItemPickupFlag(false);

        }
    }

    private void AnimControllerSwapout()
    {
        
        switch (_animatorControllerIndex)
        {
            case 0:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_nothing_controller") as RuntimeAnimatorController;
                break;
            case 1:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword_controller") as RuntimeAnimatorController;
                break;
            case 2:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_shield_controller") as RuntimeAnimatorController;
                break;
            case 3:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword&shield_controller") as RuntimeAnimatorController;
                break;
            case 4:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_sword&sword_controller") as RuntimeAnimatorController;
                break;
            case 5:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_longsword_controller") as RuntimeAnimatorController;
                break;
            case 6:
                _animator.runtimeAnimatorController = Resources.Load(_animControllerFilepath + "Player_with_longsword&shield_controller") as RuntimeAnimatorController;
                break;

        }


        SetupCustomSkin();
    }

    private void UpdateAnimator()
    {
        //WARNINGS - DUE to animator set calls based on anim controller plugged in some of these
        //calls do not exist. TODO: Set an enum flag to identify which animcontroller is set and based
        //on that flag use a switch statement to call the appropriate ones. Then no more warnings from here :)
        //Warnings could slow down the game?


        //Mathf.Abs() so we get negative&positive values for left&right movement in animation.
        _animator.SetFloat("x_speed", Mathf.Abs(_velocity.x));
        //print("\nx_speed=" + _velocity.x);

        _animator.SetBool("sprint", _isSprint);

        FlipLeftRightHandItems(false);

        //Set jump animation when appropriate.
        _animator.SetBool("jumped", _jumped);
        _animator.SetBool("dblJump", _dblJumped);//dblJump

        //Set fall animation when appropriate.
        //if(!_jumped && !_dblJumped)
        _animator.SetBool("falling", _falling);

        //Set fallen to death animation when appropriate.
        _animator.SetBool("fallen", _fallen);

        //Set respawned waiting animation when appropriate.
        _animator.SetBool("respawned", _respawned);

        //Set dying animation when appropriate.
        _animator.SetBool("dying", _dying);


        //Set attack animation when appropriate.
        _animator.SetBool("attacked", _attacked);
        _animator.SetBool("released", _released);
        _animator.SetInteger("smlAttack", _smlAttack);
        _animator.SetInteger("comboAttack", _comboAttack);
        _animator.SetInteger("knockAttack", _knockAttack);

        //Set hit animation when appropriate.
        _animator.SetBool("hit", _isHit);
        _animator.SetBool("slamOnGround", _isSlamOnGround);

        //Set exhaustion animation when appropriate.
        _animator.SetBool("tired", _isTired);

        //Set quick hit animation when appropriate.
        _animator.SetBool("jab", _isQuickHit);

        //Set def hit animation when appropriate.
        _animator.SetBool("hitDef", _isHitDefending);

        //Set def quick hit animation when appropriate.
        _animator.SetBool("jabDef", _isQuickHitDefending);

        //Set defence animation when appropriate.
        _animator.SetBool("defended", _defended);

        //Set dodge animation when appropriate.
        //if (GameManager.GetCurrentPlayerEquipC(_playerID) != ItemData.ItemType.Armor)
        _animator.SetBool("dodge", _dodge);

        //Set aquired equipment animation when appropriate.
        _animator.SetBool("equipAquired", _equipAquiredId);

        //Set broken equipment animation when appropriate.
        _animator.SetBool("armorBroken", _armorBroken);




        ApplyAnimController();

        if (_lifeUp)
        {
            if (_life < 4)
                _life++;
            _lifeUp = false;
            _animator.SetInteger("life", _life);
        }
    }

    private void SetAtkAnim(bool b, bool isUponRelease)
    {
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword)
        {
            _attacked = b;
            _smlAttack = 0; //!b
            return;
        }

    }

    private void SetFallAnim()
    {

        if (!_falling)
        {
            if (_velocity.y < 0)
            {
                _falling = true;

            }
        }
    }

    private void UpdateMovement()
    {
        //Guard clause.
        if (!SafeToMove())
            return;

        _targetVelocityX = (_input.x * (_moveSpeed * _potionSpeed)) * _sprintSpeed; // * _hitImpact;

        if (_applyHitFlag)
        {
            _targetVelocityY = 1.0f;

            //_hitImpact will be negative sign if attacker is facing left, meaning
            //the value will be negative, causing player to not be lifted off ground. 

            _targetVelocityY = _targetVelocityY * _hitImpact.y;

            _velocity.y = _targetVelocityY;

            _applyHitFlag = false; //Don't apply continously, just once.
        }
        if (_hitImpact.x != 1.0f)
        {
            //Then player has been hit and the targ vel could possibily be zero.
            _targetVelocityX = 1.0f;
            _targetVelocityX = _targetVelocityX * _hitImpact.x;
        }


        _velocity.x = _targetVelocityX;
        //_velocity.y = _targetVelocityY; //Gravity is affected by this, no where near as strong when uncommented.

        //Guard clause.
        if (StateManager.IsMidAirAttack(_playerID))
            return;
        if (StateManager.IsJumping(_playerID))
            return; //Avoid changing player state in this case, whilst allowing mid air movement.
        if (StateManager.IsMidAirAttackPrepare(_playerID))
            return; //Avoid changing player state in this case, whilst allowing mid air movement.
        if (StateManager.IsMidAirAttackRelease(_playerID))
            return; //Avoid changing player state in this case, whilst allowing mid air movement.
        if (StateManager.IsHitWhileDefending(_playerID)) //Due to logic flow these are required to ensure their state
            return;                                      //Due to the below code, setting state to idle.
        if (StateManager.IsQuickHitWhileDefending(_playerID))
            return;
        if (StateManager.IsRunningQuickAttack(_playerID))
            return;
        if (StateManager.IsQuickAttack(_playerID))
            return;
        if (StateManager.IsUpwardAttack(_playerID))
            return;

        if (_targetVelocityX == 0f && _controller.collisions.below)// && !StateManager.IsJumping(_playerID))
        {
            if (SafeToSetIdle())
            {
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
        }
        if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller.collisions.below)// && !StateManager.IsJumping(_playerID))
        {
            //_respawned = false;
            if (SafeToSetIdle())
            {
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Run);
            }
        }

        //if (_controller.collisions.left)
        //    _velocity.x += 20.0f;
        //if (_controller.collisions.right)
        //    _velocity.x -= 20.0f;

        //if (!_isHit) //Ensure player is off ground.
        //    return;
        //if(_controller.collisions.left &
        //    StateManager.IsHitByReleaseAttack(_playerID))
        //{
        //    _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);
        //    _hitBehaviourTimer.ForceTimerEnd(_playerID);
        //    GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
        //    _isHit = false;
        //    SetFallAnim();
        //    _hitImpact.x = 1.0f;
        //    _hitImpact.y = 1.0f;
        //    _targetVelocityX = 0.0f;
        //    _targetVelocityY = 0.0f;

        //    if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
        //        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
        //    else
        //        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

        //    if (_jumpDuringHit)
        //    {
        //        InitialJumpLogic();
        //        //SecondaryJumpLogic();
        //        _jumpDuringHit = false;
        //    }
        //}

    }

    private void EnableDirectionChange()
	{
        if (_input.x > 0.1f)
        {
            if (_faceDir == -1)
            {
                if (_spriteRenderer.flipX)
                {
                    _spriteRenderer.flipX = false;
                    _faceDir = 1;
                    SetAtkCollisionFaceDir(_faceDir);
                    //_childFXController.Flip(false);
                }
            }
        }
        if (_input.x < -0.1f)
        {
            if (_faceDir == 1)
            {
                if (!_spriteRenderer.flipX)
                {
                    _spriteRenderer.flipX = true;
                    _faceDir = -1;
                    SetAtkCollisionFaceDir(_faceDir);
                    //_childFXController.Flip(true);
                }
            }
        }
    }

    public void SetGameOverRespawnFlag()
	{
        //GameoverUIController.cs -> void Update() -> if (_blackoutToMenuTimer.HasTimerFinished(1))
        _processUpdate = true;
        _animator.SetBool("dead", false);
        ProCamera2D.Instance.AddCameraTarget(_transform);
        Time.timeScale = 1.0f;
        _collider2d.enabled = true;

        _dying = false;
        _velocity.x = 0;
        _velocity.y = 0;

        //Flagged here. Then flagged off over in PlayerUIStatController.cs
        _stats.Armor = 0;
        _stats.Currency = 0;
        _stats.Health = PlayerStatData._maxHealth / 4;
        _stats.Stamina = 0;
        _stats.KillCount = 0;

        GameManager.SetPlayerStats(_playerID, _stats);
        GameManager.ResetPlayerEquip(_playerID);
        AudioManager.PlaySfx(21, _playerID);
        FXObjPooler._curInstance.Instantiate(_transform.position, 14, null, 0); //arg2= for fallenPlayerRespawnfx
                                                                                //AudioManager.PlaySfx(20, 0);
        _falling = true;
        _fallen = true;
        UpdateAnimator();

        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Respawning);
        _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);
        //_hitBehaviourTimer.SetTimerDuration(1.25f, _playerID);
        _hitBehaviourTimer.SetTimerDuration(1.75f, _playerID);
        _hitBehaviourTimer.StartTimer(_playerID);

        SetTextureFlashFX(Color.red, 0.75f);

        RespawnManager.SetRespawnedCampsiteItems(true, _playerID);
        GameManager.SetRespawnFlag(true, _playerID); //Rest of work happens over in PlayerUIStatController.cs.
        GameManager.SetPlayerUILoadUpdateFlag(true);
        _IDController.SetDeathFlag(false);
        return;
    }

    private void DetectTeamPlayerRespawningUpdate()
	{
        //Here we check for the flag that indicates we're respawning fallen
        //team players. We want to ensure the player still standing cannot 
        //venture too far away from the campsite whilst we wait to respawn
        //dead players and re-add them to the camera.

        //Without this measure in place, player can venture away causing issues
        //when the respawned players are added to the camera. Not only this but
        //discouraging/preventing still standing player from leaving campsite
        //will help keep them alive as the screen fades to black during respawn process.

        //print("\nRespawnManager.GetTeamPlayersRespawnFlag()=" + RespawnManager.GetTeamPlayersRespawnFlag());

        if (RespawnManager.GetTeamPlayersRespawnFlag())
		{
            //Identify the latest/current campsite we're at and then calculate
            //if player is certain distance away. If so, prevent player going
            //further away.

            int id = RespawnManager.GetLatestCampsiteActivatedID();

            float dist = Vector3.Distance(_transform.position, RespawnManager.GetCampsiteTransform(id).position);

            if (dist > 7.0f)
			{
                _input.x = 0f;
                _targetVelocityX = 0f;
                _velocity.x = 0.0f;
            }
        }

    }

    private void UpdateStateBehaviours()
    {
        DetectCollisionsUpdate();
        UpdateStateAtkBehaviours();
        DetectTeamPlayerRespawningUpdate();

        //print("\nPlayerState" + _playerID + "=" + StateManager.GetPlayerState(_playerID));
        if (_armorBroken && _equipGainedAnimTimer.HasTimerFinished(1))
        {
            _armorBroken = false;
            UpdateAnimator();
            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
        }

        if (StateManager.IsReset(_playerID))
		{
            //Here we make all previous behaviour timer checks and reset all vars associated.
            //We also check the previous state in queue. We want to ensure if a player has been set to
            //a reset state, that has happened likely during any other state. So we don't want the player
            //getting stuck in any states because some variables were not reset or some timers didn't get
            //to end during a certain state. Here we make sure everythng is wiped a clean so we can
            //safely set the next state regardless of being mid-whatever-state beforehand.

		}
        if (StateManager.IsDead(_playerID))
        {
            if (_processUpdate)
            {
                _processUpdate = false;
                _animator.SetBool("dead", true);
                Time.timeScale = 1.0f;
                _velocity.x = 0f;
                _velocity.y = 0f;
                _targetVelocityX = 0f;
                _targetVelocityY = 0f;
                _collider2d.enabled = false;

                // Remove empty targets
                //ProCamera2D.Instance.CameraTargets.RemoveAt(_playerID+1);

                //Ensure we remove target, which includes camera target of enemy.
                _targetController.DeselectClosestTarget();

                //if (ProCamera2D.Instance.CameraTargets.Count == 1)
                //{
                //    //Last player remaining has died, do not remove this camera target.
                //    GameManager.SetGameoverFlag(true);
                //    return;
                //}
                int playerCount = PlayerMonitor.GetPlayerCount();//0;
                //if (MenuManager._instance.HasDetectedPlayerFlag(0))
                //    playerCount++;
                //if (MenuManager._instance.HasDetectedPlayerFlag(1))
                //    playerCount++;
                //if (MenuManager._instance.HasDetectedPlayerFlag(2))
                //    playerCount++;
                //if (MenuManager._instance.HasDetectedPlayerFlag(3))
                //    playerCount++;


                if(_playerID == 0)//if (playerCount == 1)
                    GameManager.SetGameoverFlag(true);
                else
				{
                    RemovePlayerFromCamera();
                }


                return;

                //RemovePlayerFromCamera();

                //GameManager.ResetPlayerEquip(_playerID);
                ////GameManager.Save(_playerID);
            }

            return;
        }
        if (StateManager.IsDying(_playerID))
        {

            //         if(GameManager._instance.GetScene() == GameManager.Level.CoverArtScene)
            //{
            //             StateManager.SetPlayerState(0, PlayerStateData.CurrentState.Idle);
            //             _stats.Health = 1.0f;
            //             GameManager.UpdatePlayerStats(0, _stats, 0);
            //}

            if (!_controller.collisions.below)
                return;

            //Play dying anim and prevent any input.
            if (!_dying)
            {

                //Kill or other players off to enable gameoever respawning.
                for(int i = 1; i < 4; i++)
				{
                    _stats.Health = 0f;
                    GameManager.SetPlayerStatFlag(i, true);
                    GameManager.SetPlayerStats(i, _stats);
                    GameManager.UpdatePlayerStats(i, _stats, 0);
                    StateManager.SetPlayerState(i, PlayerStateData.CurrentState.Dying);

                    if(PlayerMonitor.GetPlayerController(i) != null)
                        PlayerMonitor.GetPlayerController(i).RemovePlayerFromCamera();
                    else
                        RemovePlayerFromCamera();
                }



                _velocity.x = 0f;
                _velocity.y = 0f;
                _targetVelocityX = 0f;
                _targetVelocityY = 0f;
                //_gravity = 0f;

                //Set jump animation when appropriate.
                _jumped = false;
                _dblJumped = false;
                _falling = false;
                _attacked = false;

                SetAtkAnim(false, false);
                _released = false;
                _attackCloudFlag = false;
                ResetPowerAttackTimers();

                _smlAttack = 0;
                _isHit = false;
                _isQuickHit = false;
                _isHitDefending = false;
                _isQuickHitDefending = false;
                _defended = false;
                _equipAquiredId = false;
                _weapChrgeTexFlag = true;
                FinishWeaponChargeTexFX();
                AudioManager.StopAttackSfxFlag(_playerID);

                _behaviourTimer.SetTimestampedFlag(false, 0);
                //_behaviourTimer.ResetPrematureFinFlag(_playerID); //HasTimerFinished(_playerID) wouldn't execute as intended without this!
                _behaviourTimer.SetTimerDuration(1.0f, 0);
                _behaviourTimer.StartTimer(0);
                //SetTextureFlashFX(Color.white, 0.25f);
                SetTextureFlashFX(Color.white, 0.05f);

                AudioManager.PlaySfx(11, _playerID);
                Time.timeScale = 0.5f;

                _dying = true;
            }
            if (_behaviourTimer.HasTimerFinished(0))
            {
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Dead);
            }
            return;
        }
        if (StateManager.IsIdle(_playerID))
        {
            if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.MidAirReleaseAttack ||
                StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.MidAirHoldAttack)
			{
                _attacked = false;
                _released = false;
            }

            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
			{
                //_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
                _respawned = false;
			}
            if (GameManager.GetCurCombatStatus())
			{
                GameManager.SetCurCombatStatus(false);
			}

            if (_fxTimer.HasTimerFinished(0))
            {
                if (_dustCloudFlag)
                {
                    _dustCloudFlag = false;
                }
            }
            return;
        }
        if (StateManager.IsMoving(_playerID))
        {
            _isHit = false;
            _hitImpact.x = 1.0f;
            _hitImpact.y = 1.0f;


            if (!_dustCloudFlag)
            {
                if (_isInWater)
                    return;
                _fxTimer.SetTimestampedFlag(false, 0);
                _fxTimer.SetTimerDuration(0.8f, 0); //1 sec per instantiation.
                _fxTimer.StartTimer(0);
                StateManager.SetPlayerDir(_playerID, _faceDir);
                FXObjPooler._curInstance.Instantiate(_transform.position, 0, null, 0);
                _dustCloudFlag = true;
            }
            if (_fxTimer.HasTimerFinished(0))
            {
                if (_dustCloudFlag)
                {
                    _dustCloudFlag = false;
                }
            }
            return;
        }
        if (StateManager.IsJumping(_playerID))
        {

            if (_isQuickHit)
                _isQuickHit = false;
            SetFallAnim();
            if (_behaviourTimer.HadTimerBeenSet(_playerID))
            {
                if (_controller.collisions.below)// && _jump < 1f)
                {

                    //if (_jumped)
                    //{
                    _jumped = false;


                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                    //FlipLeftRightHandItems(false);
                    //}
                    //if(_dblJumped)
                    //{
                    _dblJumped = false;

                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                    //}
                }
            }
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {

                //print("\njump timer finished.");
                // _velocity.y = 0; //Not needed.
                //_targetVelocityY = 0f;
                //print("\ny=" + _velocity.y);
                //_falling = true;

            }
            if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HitByReleaseAtk ||
                StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HitByEnemyReleaseAtk)
            {


                if (_isHit)
                {
                    //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByReleaseAtk);
                    GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
                    _isHit = false;
                    _jumpDuringHit = false;
                    //SetFallAnim();
                    _hitImpact.x = 1.0f;
                    _hitImpact.y = 1.0f;
                    _targetVelocityX = 0.0f;
                    _targetVelocityY = 0.0f;
                    _velocity.x = _targetVelocityX;
                    _velocity.y = _targetVelocityY;


                    _hitBehaviourTimer.ForceTimerEnd(_playerID);
                    _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);

                    if (_hitBehaviourTimer.HasTimerFinished(_playerID))
                        return;
                }

            }

            #region FXJumpDustClouds

            if (_fxTimer.HasTimerFinished(_playerID))
            {
                if (_jumpCloudFlag)
                    _jumpCloudFlag = false;
            }
            #endregion
            return;
        }

        if (StateManager.IsKnockBackAtkPrepare(_playerID))
        {

            if (_isQuickHit)
                _isQuickHit = false;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                SetTextureFlashFX(Color.white, 0.05f);
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.KnockBackAttack);
                _knockAttack = 2;

                _behaviourTimer.SetTimestampedFlag(false, _playerID);
                _behaviourTimer.SetTimerDuration(0.4f, _playerID);//0.4f
                _behaviourTimer.StartTimer(_playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx

                _weaponCollidedFlag = false;
                _enableAtkUpdate = true;

                AudioManager.PlaySfx(19, _playerID);
                GameManager.GetPlayerStatData(_playerID).Action = _stats.GetKnockBackActionCost();
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                GameManager.SetStaminaUIFlag(_playerID, true);
            }
            return;
        }
        if (StateManager.IsKnockBackAttack(_playerID))
		{
            if (GameManager._instance._level != GameManager.Level.TutorialScene)
			{
                if(_velocity.x == 0f) //Only apply small momentum in movement if idle when using this attack.
				{
                    if (_faceDir == 1)
                        _velocity.x += 0.05f;
                    if (_faceDir == -1)
                        _velocity.x -= 0.05f;
                }

            }

            if (_isQuickHit)
                _isQuickHit = false;
            if (_behaviourTimer.HasTimerFinished(_playerID))
			{
                FinishWeaponChargeTexFX();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                _knockAttack = 0;

                //if(!_controller.collisions.below)
                //    FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, 0); //arg2= 37 for runQuickAtkfx 
                //FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, 0); //arg2= 37 for runQuickAtkfx 
                //GameManager.GetPlayerStatData(_playerID).Action = _stats.GetKnockBackActionCost();
                //GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                //GameManager.SetStaminaUIFlag(_playerID, true);
            }
            return;
        }
        
        if (StateManager.IsUpwardAttack(_playerID))
		{
            _smlAttack = 6;
            _smlAtkIncrement = 0;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                _smlAttack = 0;
                _smlAtkIncrement = 0;
                if (_isQuickHit)
                    _isQuickHit = false;
                _isUpwardAtk = false;


                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

            }
            return;
        }

        if (StateManager.IsQuickAttack(_playerID))
        {
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                _smlAttack = 0;
                if (_isQuickHit)
                    _isQuickHit = false;


                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                if (_smlAtkIncrement >= 0)
                {
                    _smlAtkIncrement++;
                    if (_smlAtkIncrement >= 4)
                        _smlAtkIncrement = 0;
                    
                    UpdateAnimator();
                    return;
                }
            }
            return;
        }
        if (StateManager.IsRunningQuickAttack(_playerID))
		{

            


            //print("_isSprint=" + _isSprint);
            //_enableAtkUpdate = true;
            if (_behaviourTimer.HasTimerFinished(10))
                _enableAtkUpdate = true;
            if (_behaviourTimer.HasTimerFinished(11))
                _enableAtkUpdate = true;

            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                _smlAttack = 0;
                if (_isQuickHit)
                    _isQuickHit = false;

                _enableAtkUpdate = true;
                //print("\nspeed upon atk=" + _velocity.x);
                //FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, _playerID); //arg2= 37 for runQuickAtkfx 
                //GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost() / 2;
                //GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                //GameManager.SetStaminaUIFlag(_playerID, true);

                //if (_smlAtkIncrement == 4)
                if (_isSprint)
				{
                    if (!_weaponCollidedFlag)
                    {
                        FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, _playerID); //arg2= 37 for runQuickAtkfx 
                        GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost();
                        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                        GameManager.SetStaminaUIFlag(_playerID, true);
                        //_isSprint = false;
                    }
                    _isSprint = false;
                }

				//_sprintSpeed = 1.0f;
				//FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, 0); //arg2= 37 for runQuickAtkfx 

				if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller.collisions.below)
				{
                    
                    if (SafeToSetIdle())
                    {
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Run);
                    }
                }
                else
				{
                    if (SafeToSetIdle())
                    {
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                    }
                }
                //if (_smlAtkIncrement >= 0)
                //{
                //    _smlAtkIncrement++;
                //    if (_smlAtkIncrement >= 4)
                //    {
                //        _smlAtkIncrement = 0;
                //        //FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, _playerID); //arg2= 37 for runQuickAtkfx 
                //        //GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost()/2;
                //        //GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                //        //GameManager.SetStaminaUIFlag(_playerID, true);
                //    }

                //    UpdateAnimator();
                //    return;
                //}
            }
            return;
        }
        
        if (StateManager.IsAttackPrepare(_playerID))
        {
            EnableDirectionChange();


            _input.x = 0f;
            _velocity.x = 0f;
            _targetVelocityX = 0f;

            GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerHoldActionCost();
            GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);




            if (IsWeaponEquipped())
            {
                if (!_attackCloudFlag)
                {
                    StateManager.SetAttackState(_playerID, AttackStateData.CurrentState.ChargeStart);
                    _powerAtkCloudTimer[0].StartTimer(_playerID);
                    GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerHoldActionCost();
                    GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                    
                    _attackCloudFlag = true;
                }
            }


            //GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerHoldActionCost();
            //GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);

            return;
        }
        if (StateManager.IsAttackRelease(_playerID))
        {
            //_enableAtkUpdate = true;
            if (_isQuickHit)
                _isQuickHit = false;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                //print("\nattack release timer finished.");
                //FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, 0); //arg2= 37 for runQuickAtkfx 
                Vector3 v = _transform.position;
                if (_faceDir == 1)
                    v.x += 1.0f;
                if (_faceDir == -1)
                    v.x -= 1.0f;

                FXObjPooler._curInstance.Instantiate(v, 37, null, _playerID); //arg2= 37 for runQuickAtkfx 
                GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(false);
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);

                if (_hitShakeFlag)
				{
                    //if(_proCamera2DShake != null)
                    // _proCamera2DShake.Shake(_shakePreset);
                    _hitShakeFlag = false;
                }


                SetAtkAnim(false, false);
                _released = false;

                //_enableAtkUpdate = true; //Needs enabling for the player HIT as well...
                _attackCloudFlag = false;
                //_hitBehaviourTimer.SetTimerDuration(1.0f, _playerID);
                //FXObjPooler._curInstance.Instantiate(_transform.position, 2); //arg2= 2 for attackfx

                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle); //moved to dmg logic
                ResetPowerAttackTimers();
                //StateManager.SetAttackState(_playerID, AttackStateData.CurrentState.None);
                //print("\nPlayerID=" + _playerID + " Attackstate=" + StateManager.GetAttackState(_playerID));
            }
            //If check because above doesn't work in unknown case scenrio
            if (_behaviourTimer.HadTimerBeenSet(_playerID))
            {
                //_proCamera2DShake.Shake(_shakePreset);
                SetAtkAnim(false, false);
                _released = false;
                _attackCloudFlag = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle); //moved to dmg logic
                ResetPowerAttackTimers();
            }
            GameManager.SetStaminaUIFlag(_playerID, true);
            return;
        }
        
        if (StateManager.IsMidAirAttack(_playerID))
		{

            //_targetVelocityY = _targetVelocityY +0;

            if(_behaviourTimer.HadTimerBeenSet(_playerID))
			{
                _smlAttack = 0;
                if (_isQuickHit)
                    _isQuickHit = false;
                //SetFallAnim();
                _falling = true;
                _velocity.y = 0f;
                _targetVelocityY = 0f;
                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                _smlAtkIncrement = 0;
                _behaviourTimer.SetTimestampedFlag(false, _playerID);
                return;
            }

            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                //print("\nMidair atk fin.");
                _smlAttack = 0;
                if (_isQuickHit)
                    _isQuickHit = false;
                //SetFallAnim();
                _falling = true;
                _velocity.y = 0f;
                _targetVelocityY = 0f;

                if (_targetController.IsTargetActive())
                {
                    //if (PlayerMonitor.GetPlayerController(GameManager.GetCurCombatPlayerIdAtk()).GetFaceDir() == 1)
                    if (_faceDir == 1)
                    {
                        _velocity.x += 2.0f;
                    }
                    //if (PlayerMonitor.GetPlayerController(GameManager.GetCurCombatPlayerIdAtk()).GetFaceDir() == -1)
                    if (_faceDir == -1)
                    {
                        _velocity.x -= 2.0f;
                    }
                }



                //_enableAtkUpdate = true; //Needs placing before timer ends.

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                _smlAtkIncrement = 0;
                return;

                //if (_smlAtkIncrement == 0)
                //{
                //    _smlAtkIncrement++;
                //    return;
                //}
                //if (_smlAtkIncrement == 1)
                //{
                //    _smlAtkIncrement++;
                //    return;
                //}
                //if (_smlAtkIncrement == 2)
                //{
                //    _smlAtkIncrement++;
                //    return;
                //}
                //if (_smlAtkIncrement == 3)
                //{
                //    _smlAtkIncrement = 0;
                //    return;
                //}
            }
            return;
        }
        if (StateManager.IsMidAirAttackPrepare(_playerID))
		{
            if (_isQuickHit)
                _isQuickHit = false;
            if (!_attacked)
                _attacked = true;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                _knockAttack = 0;
                SetTextureFlashFX(Color.white, 0.10f);
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.MidAirReleaseAttack);

                _behaviourTimer.SetTimestampedFlag(false, _playerID);
                _behaviourTimer.SetTimerDuration(0.4f, _playerID);//0.4f
                _behaviourTimer.StartTimer(_playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx

                _weaponCollidedFlag = false;
                _enableAtkUpdate = true;

                AudioManager.PlaySfx(19, _playerID);
                GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost();
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                GameManager.SetStaminaUIFlag(_playerID, true);
            }
            if(_controller.collisions.below)
			{
                _knockAttack = 0;
                _attacked = false;
                _released = false;
                _weaponCollidedFlag = false;
                _enableAtkUpdate = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
            return;
        }
        if (StateManager.IsMidAirAttackRelease(_playerID))
        {
            if (_isQuickHit)
                _isQuickHit = false;
            if (_attacked)
			{
                _attacked = false;
                _released = true;
            }
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                FinishWeaponChargeTexFX();
                _knockAttack = 0;
                _released = false;
                _velocity.y = 0f;
                _targetVelocityY = 0f;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                //FXObjPooler._curInstance.Instantiate(_transform.position, 37, null, _playerID);
            }
            if (_controller.collisions.below)
            {
                FinishWeaponChargeTexFX();
                _knockAttack = 0;
                _attacked = false;
                _released = false;
                _weaponCollidedFlag = false;
                _enableAtkUpdate = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
            return;
        }

        if (StateManager.IsLockedComboAttack(_playerID))
		{
            //Queue SFX that initiates combo attack!
            //Queue VFX that initiates combo attack!

            //Fast Animation attacks x 3
            //Move player x
            //Move player y
            //Queue player white sprite flash
            //Queue camaera shake

            //Repeat x 3 or more.

            if (_isTired)
            {
                _IDController.SetExhaustedFlag(false);
                _isTired = false;
            }

            if (_comboAttackTimer.HasTimerFinished(_comboAttack))
			{
                //Time.timeScale = 1.0f;
                SetTextureFlashFX(Color.white, 0.25f);
                _comboAttack++;

                if(_faceDir == 1)
				{
                    // _velocity.x += 2.0f;
                    _transform.Translate(new Vector3(0.5f, 0.75f, 0f));
                }
                else
				{
                    //_velocity.x -= 2.0f;
                    _transform.Translate(new Vector3(-0.5f, 0.75f, 0f));
                }
                _velocity.y = 0f;


                print("\n_comboAttack=" + _comboAttack.ToString());

                if (_comboAttack == 4)
				{
                    _comboAttack = 0;
                    print("\n_comboAttack=" + _comboAttack.ToString());
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                    _comboInputFlag = 0;
                    NullifyAnimatorFlags();
                    _animator.SetInteger("comboAttack", _comboAttack);
                    //_spriteTrailRef.DisableTrail();
                    return;
                }

                _comboAttackTimer.SetTimestampedFlag(false, _comboAttack);
                _comboAttackTimer.SetTimerDuration(2.0f, _comboAttack);//0.4f
                _comboAttackTimer.StartTimer(_comboAttack);
            }
        }

        if (StateManager.IsDefencePrepare(_playerID))
        {
            //EnableDirectionChange();
            //_input.x = 0f;
            //_velocity.x = 0f;
            //_targetVelocityX = 0f;

            //Time.timeScale = 0.5f;

            //_moveSpeed = _moveSpeed / 2;
            GameManager.GetPlayerStatData(_playerID).Action = _stats.GetDefenceActionCost();
            GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);

            if (_isQuickHit)
                _isQuickHit = false;
            _smlAttack = 0;//Incase attacking anim whilstwithin hit anim.

            return;
        }
        if (StateManager.IsDefenceRelease(_playerID))
        {

            //_isHitDefending = false;
            //_isQuickHitDefending = false;
            //_moveSpeed = _moveSpeed * 2;
            //Time.timeScale = 1.0f;
            if (GameManager.GetPlayerStatUseFlag(_playerID) != 3)
            {
                GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(true);
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
            }

            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            return;
        }
        
        if (StateManager.IsDodgePrepare(_playerID))
        {
            //if (_faceDir == -1)
            //    _velocity.x += 0.205f;
            //if (_faceDir == 1)
            //    _velocity.x -= 0.205f;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {

                //if (_dodge)
                //    _dodge = false;

                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.DodgeRelease);

                _behaviourTimer.SetTimerDuration(0.05f, _playerID);
                _behaviourTimer.StartTimer(_playerID);
                SetTextureFlashFX(Color.white, 0.1f);


				if (!_targetController.IsTargetActive())
				{
					if (_faceDir == -1)
						_velocity.x += 41.0f;
					if (_faceDir == 1)
						_velocity.x -= 41.0f;
				}
                else
				{
                    if (_dodgeDir == -1)
                        _velocity.x -= 41.0f;
                    if (_dodgeDir == 1)
                        _velocity.x += 41.0f;
                }
				//           else
				//           {

				//               if(_targetController.IsTargetActive())
				//{

				//                   int targetEnemyFaceDir = -1;
				//                   int targetCurEnemyId = _targetController.GetActiveTargetEnemyId();

				//                   targetEnemyFaceDir = EnemyStatesManager.GetSpecificEnemyDir(targetCurEnemyId);

				//                   if (targetEnemyFaceDir == -1)
				//                       _velocity.x += 41.0f;
				//                   if (targetEnemyFaceDir == 1)
				//                       _velocity.x -= 41.0f;

				//               }//GetCurAtkTargEnemyId()
				//               else
				//{
				//                   //Make player move back slowly away from target when targetting.

				//                   int targetEnemyFaceDir = -1;
				//                   int targetCurEnemyId = _targetController.GetActiveTargetEnemyId();

				//                   targetEnemyFaceDir = EnemyStatesManager.GetSpecificEnemyDir(targetCurEnemyId);

				//                   if (targetEnemyFaceDir == -1)
				//                       _velocity.x += 41.0f;
				//                   if (targetEnemyFaceDir == 1)
				//                       _velocity.x -= 41.0f;
				//               }




				//           }


			}
            return;
        }
        if (StateManager.IsDodgeRelease(_playerID))
        {
            //if (_faceDir == -1)
            //    _velocity.x += 0.005f;
            //if (_faceDir == 1)
            //    _velocity.x -= 0.005f;
            if (_behaviourTimer.HasTimerFinished(_playerID))
            {
                if (_dodge)
                    _dodge = false;
                //if (_faceDir == -1)
                //    _velocity.x += 10.00f;
                //if (_faceDir == 1)
                //    _velocity.x -= 10.00f;
                //SetTextureFlashFX(Color.white, 0.10f);
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                _velocity.x = 0f;

				if (_targetController.IsTargetActive())
				{
                    _dodgeDir = 0;

                    if (_faceDir == -1)
					{
						_faceDir = 1;
						return;
					}
					if (_faceDir == 1)
					{
						_faceDir = -1;
						return;
					}
				}

			}
            return;
        }

        if (StateManager.IsHitByReleaseAttack(_playerID))
        {
            
            if (!_isHit)
            {
                if (_isTired)
                {
                    _IDController.SetExhaustedFlag(false);
                    _isTired = false;
                }
                //Get _hitDamage value from player whom charged attack.
                _hitDamage = GetAttackingPlayerHitDamage(false);

                //print("\nPlayerID=" + _playerID + " Attackstate=" + StateManager.GetAttackState(_playerID));
                print("\n_hitDamage=" + _hitDamage);
                //    " and timer=" + GetAttackingPlayerHitDamage(true));

                //_hitImpact.x = GameManager.GetCurCombatPlayerAtkDir() * _hitDamage;
                //_hitImpact.x = _faceDir * _hitDamage;

                int dir = GameManager.GetCurCombatPlayerAtkDir();

                print("\nGetCurCombatPlayerAtkDir()=" + dir);


                _hitImpact.x = dir * _hitDamage;// * 1.25f;
                _hitImpact.y = _hitImpact.y * _hitDamage;// * 4.25f;
                _hitBehaviourTimer.SetTimerDuration(GetAttackingPlayerHitDamage(true), _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);


                SetTextureFlashFX(Color.white, 0.75f);
                _applyHitFlag = true;
                _isHit = true;

                //GameManager.SetPlayerHealth(_playerID, 0.5f);

                //Ensure these states are allowed to finish or begin. See, if(_jumpDuringHit)
                if (_jumped)
                    _jumped = false;
                if (_dblJumped)
                    _dblJumped = false;


                //AudioManager.PlaySfx(3, _playerID);
                _hitCloudsTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                _hitCloudsTimer.StartTimer(_playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 4, null, 0); //arg2= 4 for smashfx 
                FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0); //arg2= 3 for hitfx
                AudioManager.PlaySfx(6, _playerID);
                AudioManager.PlaySfx(24, _playerID);
                _hitCloudFlag = false;
            }
            else
            {
                #region FXHitClouds

                if (_hitCloudsTimer.HasTimerFinished(_playerID))
                {
                    if (_hitCloudFlag)
                        _hitCloudFlag = false;

                    if (!_hitCloudFlag)
                    {
                        _hitCloudsTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                        _hitCloudsTimer.StartTimer(_playerID);
                        //StateManager.SetPlayerDir(_playerID, _faceDir);
                        FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0); //arg2= 3 for hitfx
                        AudioManager.PlaySfx(7, _playerID);
                        _hitCloudFlag = true;
                    }
                }
                #endregion

            }
            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
            {
                //CollisionData.Participants.SwordWithPlayer needs resetting for the player that was HIT.
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
                //print("\nPlayer" + (_playerID + 1) + " been hit timer has finished.");
                //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);


                _isHit = false;
                SetFallAnim();
                //_applyHitFlag = false;

                //InitialJumpLogic();
                //SecondaryJumpLogic();

                //if(!_applyHitFlag)
                //{
                _hitImpact.x = 1.0f;
                _hitImpact.y = 1.0f;
                _targetVelocityX = 0.0f;
                _targetVelocityY = 0.0f;
                //}


                //print("\nLANDED FROM HIT: PLAYER" + (_playerID+1) + " face direction= " + _faceDir);



                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                {
                    if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HoldDefence)
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);
                    else
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                }

                if (_jumpDuringHit)
                {
                    //if (!_controller.collisions.below)
                    InitialJumpLogic();
                    //SecondaryJumpLogic();
                    _jumpDuringHit = false;
                }

                _gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2);
            }
            if (_hitBehaviourTimer.HadTimerBeenSet(_playerID))
            {
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
                _isHit = false;
                SetFallAnim();
                _hitImpact.x = 1.0f;
                _hitImpact.y = 1.0f;
                _targetVelocityX = 0.0f;
                _targetVelocityY = 0.0f;

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                {
                    if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HoldDefence)
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);
                    else
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                }

                if (_jumpDuringHit)
                {
                    InitialJumpLogic();
                    _jumpDuringHit = false;
                }

                _gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2);
            }

        }
        if (StateManager.IsHitByQuickAttack(_playerID))
        {
            if (_isQuickHitDefending)
                return;

            if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HoldAttack ||
                StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
            {
                _sprintSpeed = 1.0f;
                _behaviourTimer.SetTimerDuration(0.5f, _playerID);//(0.33f, _playerID);
                NullifyAnimatorFlags();
                _attacked = false;//For animator.
                _released = true;
                UpdateAnimator();
                _weaponCollidedFlag = false;
                _enableAtkUpdate = true;
                FinishWeaponChargeTexFX();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ReleaseAttack);
                _behaviourTimer.StartTimer(_playerID);
                AudioManager.StopAttackSfxFlag(_playerID);
                GameManager.GetPlayerStatData(_playerID).Action = _stats.GetPowerAttackActionCost();
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
                return;
            }

            if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.ArmorBroken)
			{
                _armorBroken = false;
                //StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ArmorBroken);
            }

            if (_equipGainedAnimTimer.HasTimerFinished(0))
            {
                AnimControllerSwapout();
                _equipAquiredId = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                return;
            }

            if (_isTired)
            {
                _IDController.SetExhaustedFlag(false);
                _isTired = false;
            }
            _attacked = false;//incase of hit right after release attack, hopefully fixes loop relese attack anim
            _released = false;

            UpdateArmorState();

            if (!_isQuickHit)
            {
                if (PlayerMonitor.GetPlayerCount() > 1)
                {
                    int atkID = GameManager.GetCurCombatPlayerIdAtk();

                    if (atkID != -1)
                    {
                        //if (_faceDir == 1) GameManager.GetCurCombatPlayerIdAtk()
                        if (PlayerMonitor.GetPlayerController(atkID).GetFaceDir() == 1)
                        {
                            _velocity.x += 2.0f;
                            _velocity.y += 1.5f;
                        }
                        //if (_faceDir == -1)
                        if (PlayerMonitor.GetPlayerController(atkID).GetFaceDir() == -1)
                        {
                            _velocity.x -= 2.0f;
                            _velocity.y += 1.5f;
                        }
                    }

                }


                _hitBehaviourTimer.SetTimerDuration(0.24f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                SetTextureFlashFX(Color.red, 0.1f);


                _isQuickHit = true;
                _smlAttack = 0;//Incase attacking anim whilstwithin hit anim.
                //Ensure these states are allowed to finish or begin. See, if(_jumpDuringHit)
                if (_jumped)
                    _jumped = false;
                if (_dblJumped)
                    _dblJumped = false;
                if (GameManager.GetCurrentPlayerEquipC(_playerID) != ItemData.ItemType.Armor)
                {
                    AudioManager.PlaySfx(3, _playerID);
                    FXObjPooler._curInstance.Instantiate(_transform.position, 2, null, 0); //arg2= 2 for attackfx
                }
                if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
                {
                    AudioManager.PlaySfx(9, _playerID);
                    FXObjPooler._curInstance.Instantiate(_transform.position, 5, null, 0); //arg2= 2 for attackfx
                }



            }
            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
            {
                //CollisionData.Participants.SwordWithPlayer needs resetting for the player that was HIT.
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);


                _isQuickHit = false;
                //SetFallAnim();

                //_jumpDuringHit = true;
                //_hitBehaviourTimer.ForceTimerEnd(_playerID);
                //if (_hitBehaviourTimer.HasTimerFinished(_playerID))//calls DetectTimePassed() to fully complete ForceTimerEnd().. i think.
                //    return;

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                {
                    if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.HoldDefence)
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);
                    else
                        StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                }



                if (_jumpDuringHit)
                {

                    InitialJumpLogic();
                    SecondaryJumpLogic();
                    _jumpDuringHit = false;
                }


                
            }
        }

        if (StateManager.IsHitWhileDefending(_playerID))
        {
            if (!_isHitDefending)
            {
                _hitBehaviourTimer.SetTimerDuration(0.5f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                _isHitDefending = true;

                AudioManager.PlaySfx(8, _playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0);

                GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(false) * 2;
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
            }
            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
            {
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
                _isHitDefending = false;
                _defended = true;//For animator.
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);

            }
        }
        if (StateManager.IsQuickHitWhileDefending(_playerID))
        {
            if (!_isQuickHitDefending)
            {
                _hitBehaviourTimer.SetTimerDuration(0.5f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                _isQuickHitDefending = true;

                AudioManager.PlaySfx(8, _playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0);

                GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(false)*2;
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
            }
            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
            {
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);
                _isQuickHitDefending = false;
                _defended = true;//For animator.
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HoldDefence);
            }
        }

        if (StateManager.IsSlamOnGround(_playerID))
		{
            if (!_isSlamOnGround)
            {
                if (_isTired)
                {
                    _IDController.SetExhaustedFlag(false);
                    _isTired = false;
                }
                _isSlamOnGround = true;

                //_hitBehaviourTimer.ForceTimerEnd(0); //prematurely end flag interfers with HasTimerFinished(0)
                _hitBehaviourTimer.SetTimestampedFlag(false, 0);
                _hitBehaviourTimer.SetTimerDuration(0.4f, 0);
                _hitBehaviourTimer.StartTimer(0);
                //_collidedHitByPlayerId = GameManager.GetEnemyHitByPlayerId(_enemyID);
                //_hitImpact.x = GameManager.GetEnemyHitByPlayerFaceDir(_collidedHitByPlayerId) * (_hitDamage / 2);
                AudioManager.PlayEnemySfx(4, _playerID);
            }
            if (_hitBehaviourTimer.HasTimerFinished(0))
            {
                _isSlamOnGround = false;
                NullifyAnimatorFlags();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByReleaseAtk);
                return;
            }
        }

        if (StateManager.HasGainedEquip(_playerID))
        {
            _velocity.x = 0f;
            //_velocity.y = 0f;
            //Set jump animation when appropriate.
            _jumped = false;
            _dblJumped = false;
            _falling = false;
            _attacked = false;
            _released = false;
            _smlAttack = 0;
            _isHit = false;
            _isQuickHit = false;
            _isHitDefending = false;
            _isQuickHitDefending = false;
            _defended = false;
            UpdateAnimator();

            if (_equipGainedAnimTimer.HasTimerFinished(0))
            {
                AnimControllerSwapout();
                _equipAquiredId = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
        }

        if (StateManager.HasArmorBroken(_playerID))
        {
            _velocity.x = 0f;
            //_velocity.y = 0f;
            //Set jump animation when appropriate.
            _jumped = false;
            _dblJumped = false;
            _falling = false;
            _attacked = false;
            _released = false;
            _smlAttack = 0;
            _isHit = false;
            _isQuickHit = false;
            _isHitDefending = false;
            _isQuickHitDefending = false;
            _defended = false;
            _armorBroken = true;
            UpdateAnimator();

            if (_equipGainedAnimTimer.HasTimerFinished(1))
            {
                _armorBroken = false;
                UpdateAnimator();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
        }




        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
        {
            if (!_isSlamOnGround)
            {
                if (_isTired)
                {
                    _IDController.SetExhaustedFlag(false);
                    _isTired = false;
                }

                _isSlamOnGround = true;

                //_hitBehaviourTimer.ForceTimerEnd(0); //prematurely end flag interfers with HasTimerFinished(0)
                _hitBehaviourTimer.SetTimestampedFlag(false, 0);
                _hitBehaviourTimer.SetTimerDuration(0.4f, 0);
                _hitBehaviourTimer.StartTimer(0);
                //_collidedHitByPlayerId = GameManager.GetEnemyHitByPlayerId(_enemyID);
                //_hitImpact.x = GameManager.GetEnemyHitByPlayerFaceDir(_collidedHitByPlayerId) * (_hitDamage / 2);
                AudioManager.PlayEnemySfx(4, _playerID);
            }
            if (_hitBehaviourTimer.HasTimerFinished(0))
            {
                _isSlamOnGround = false;
                NullifyAnimatorFlags();
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByEnemyReleaseAtk);
                return;
            }
        }

        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
        {
            if (!_isHit)
            {
                if (_isTired)
                {
                    _IDController.SetExhaustedFlag(false);
                    _isTired = false;
                }
                //Get _hitDamage value from player whom charged attack.
                _hitDamage = 3.0f;

                int dir = -_faceDir;

                _hitImpact.x = dir * _hitDamage;
                _hitImpact.y = 1.0f + (_hitImpact.y * _hitDamage)*6;
                _hitBehaviourTimer.SetTimerDuration(1.5f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                SetTextureFlashFX(Color.white, 0.75f);
                _applyHitFlag = true;
                _isHit = true;

                //GameManager.SetPlayerHealth(_playerID, 0.5f);

                //Ensure these states are allowed to finish or begin. See, if(_jumpDuringHit)
                if (_jumped)
                    _jumped = false;
                if (_dblJumped)
                    _dblJumped = false;


                //AudioManager.PlaySfx(3, _playerID);
                _hitCloudsTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                _hitCloudsTimer.StartTimer(_playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 4, null, 0); //arg2= 4 for smashfx 
                FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0); //arg2= 3 for hitfx
                AudioManager.PlaySfx(6, _playerID);
                _hitCloudFlag = false;
            }
            else
            {
                #region FXHitClouds

                if (_hitCloudsTimer.HasTimerFinished(_playerID))
                {
                    if (_hitCloudFlag)
                        _hitCloudFlag = false;

                    if (!_hitCloudFlag)
                    {
                        _hitCloudsTimer.SetTimerDuration(0.25f, _playerID); //1 sec per instantiation.
                        _hitCloudsTimer.StartTimer(_playerID);
                        //StateManager.SetPlayerDir(_playerID, _faceDir);
                        FXObjPooler._curInstance.Instantiate(_transform.position, 3, null, 0); //arg2= 3 for hitfx
                        AudioManager.PlaySfx(7, _playerID);
                        _hitCloudFlag = true;
                    }
                }
                #endregion

            }
            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
            {
                //CollisionData.Participants.SwordWithPlayer needs resetting for the player that was HIT.
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);

                _velocity.x = 0f;
                _smlAttack = 0;
                NullifyAnimatorFlags();

                _isHit = false;
                SetFallAnim();

                _hitImpact.x = 1.0f;
                _hitImpact.y = 1.0f;
                _targetVelocityX = 0.0f;
                _targetVelocityY = 0.0f;

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                if (_jumpDuringHit)
                {
                    //if (!_controller.collisions.below)
                    InitialJumpLogic();
                    //SecondaryJumpLogic();
                    _jumpDuringHit = false;
                }

            }
            if (_hitBehaviourTimer.HadTimerBeenSet(_playerID))
			{
                //CollisionData.Participants.SwordWithPlayer needs resetting for the player that was HIT.
                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);

                _velocity.x = 0f;
                _smlAttack = 0;
                NullifyAnimatorFlags();

                _isHit = false;
                SetFallAnim();

                _hitImpact.x = 1.0f;
                _hitImpact.y = 1.0f;
                _targetVelocityX = 0.0f;
                _targetVelocityY = 0.0f;

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.Jump)
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Jump);
                else
                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);

                if (_jumpDuringHit)
                {
                    //if (!_controller.collisions.below)
                    InitialJumpLogic();
                    //SecondaryJumpLogic();
                    _jumpDuringHit = false;
                }

            }

        }

        if (StateManager.IsRespawning(_playerID))
		{
			_input.x = 0f;
			_input.y = 0f;
            Time.timeScale = 0.5f;
            //_velocity.x = 0f;
            //_velocity.y = 0f;
            if(_isTired)
			{
                _IDController.SetExhaustedFlag(false);
                _isTired = false;
            }

            if (_hitBehaviourTimer.HasTimerFinished(_playerID))
			{

                if (_playerID != 0)
				{
                    RemovePlayerFromCamera();
				}

                _animatorControllerIndex = 0;
                _animControllerFilepath = "HumanAnimControllers/Unarmored/";
                AnimControllerSwapout();

                AudioManager.PlaySfx(20, 0);
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                Vector3 v = _startPos;

                int campsiteID = RespawnManager.GetLatestCampsiteActivatedID();

                if (RespawnManager.IsCamspiteFiredUp(campsiteID))
				{
                    v = RespawnManager.GetCampsiteTransform(campsiteID).position;
                }
                //v.y += 3.0f;
                switch (_playerID)
				{
                    case 0:
                        v.x -= 2.0f;
                        break;
                    case 1:
                        v.x -= 4.0f;
                        break;
                    case 2:
                        v.x += 2.0f;
                        break;
                    case 3:
                        v.x += 4.0f;
                        break;
                }
                this._transform.position = v;

                //if(RespawnManager.GetTeamPlayersRespawnFlag())
                //    RespawnManager.SetTeamPlayersRespawnFlag(false);

                _fallen = false;
                SetTextureFlashFX(Color.white, 0.75f);
                FXObjPooler._curInstance.Instantiate(_transform.position, 14, null, 0); //arg2= for fallenPlayerRespawnfx

                _gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect

                Time.timeScale = 1.0f;
                _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);
                _hitBehaviourTimer.SetTimerDuration(2.5f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                _respawned = true;
            }
            if (_hitBehaviourTimer.HadTimerBeenSet(_playerID))
			{
                if (_playerID != 0)
                {
                    RemovePlayerFromCamera();
                }

                _animatorControllerIndex = 0;
                _animControllerFilepath = "HumanAnimControllers/Unarmored/";
                AnimControllerSwapout();

                AudioManager.PlaySfx(20, 0);
                _fallen = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
                int campsiteID = RespawnManager.GetLatestCampsiteActivatedID();
                Vector3 v = RespawnManager.GetCampsiteTransform(campsiteID).position;
                //v.y += 3.0f;
                switch (_playerID)
                {
                    case 0:
                        v.x -= 2.0f;
                        break;
                    case 1:
                        v.x -= 4.0f;
                        break;
                    case 2:
                        v.x += 2.0f;
                        break;
                    case 3:
                        v.x += 4.0f;
                        break;
                }
                this._transform.position = v;

                //if (RespawnManager.GetTeamPlayersRespawnFlag())
                //    RespawnManager.SetTeamPlayersRespawnFlag(false);

                SetTextureFlashFX(Color.white, 0.75f);
                FXObjPooler._curInstance.Instantiate(_transform.position, 14, null, 0); //arg2= for fallenPlayerRespawnfx

                _gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect

                _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);
                _hitBehaviourTimer.SetTimerDuration(2.5f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);
                _respawned = true;
            }

        }

        if (StateManager.IsExhausted(_playerID))
		{
            _input.x = 0f;
            _input.y = 0f;
            _velocity.x = 0f;


            if (_isQuickHit)
                _isQuickHit = false;
            _attacked = false;
            _released = false;
            _smlAttack = 0;
            _smlAtkIncrement = 0;
            _comboAttack = 0;
            _knockAttack = 0;
            _dodge = false;

            _isTired = true;

            if (GameManager.GetPlayerStatUseFlag(_playerID) != 3)
            {
                GameManager.GetPlayerStatData(_playerID).Recover = _stats.GetStaminaRegainRate(true)*2;
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 1);
            }


            if (_behaviourTimer.HasTimerFinished(_playerID))
			{
                SetTextureFlashFX(Color.white, 0.1f);
                _IDController.SetExhaustedFlag(false);
                _isTired = false;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Idle);
            }
        }
    }

    public float GetCachedPowerAtkDur()
    {
        return _pwrAtkDurStamp;
    }

    public float GetAttackingPlayerHitDamage(bool timedHitWanted)
    {
        //if(StateManager.GetAttackState(_collidedPlayerId) == AttackStateData.CurrentState.ChargeStart)
        switch (StateManager.GetAttackState(GameManager.GetCurCombatPlayerIdAtk()))//_collidedPlayerId = -1 here
        {
            case AttackStateData.CurrentState.ChargeStart:
                if (!timedHitWanted)
                {
                    _hitImpact.y = 4.0f;

                    return 3.75f;
                }
                else
                    return 1.0f;
            case AttackStateData.CurrentState.ChargeMid:
                if (!timedHitWanted)
                {
                    _hitImpact.y = 6.0f;

                    return 5.0f;
                }
                else
                    return 2.0f;
            case AttackStateData.CurrentState.ChargeEnd:
                if (!timedHitWanted)
                {
                    _hitImpact.y = 8.0f;

                    return 5.5f;
                }
                else
                    return 3.0f;
            case AttackStateData.CurrentState.None:
                _hitImpact.y = 1f;

                if (!timedHitWanted)
                    return 1f;
                else
                    return 1f;
        }
        return 1.0f;
    }

    private void ResetPowerAttackTimers()
    {
        for (int i = 0; i < _powerAtkCloudTimer.Length; i++)
        {
            _powerAtkCloudTimer[i].ForceTimerEnd(_playerID);
            _powerAtkCloudTimer[i].HasTimerFinished(_playerID);
        }
        StateManager.SetAttackState(_playerID, AttackStateData.CurrentState.None);
    }

    private void UpdateStateAtkBehaviours()
    {
        switch (StateManager.GetAttackState(_playerID))
        {
            case AttackStateData.CurrentState.ChargeStart:
                if(_weapChrgeTexFlag)
                    SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xd5e400));
                if (_powerAtkCloudTimer[0].HasTimerFinished(_playerID))
                {
                    StateManager.SetAttackState(_playerID, AttackStateData.CurrentState.ChargeMid);
                    _powerAtkCloudTimer[1].StartTimer(_playerID);
                    GameManager.SetReleaseAtkEnemyData(AttackStateData.CurrentState.ChargeMid, _playerID);
                }
                break;
            case AttackStateData.CurrentState.ChargeMid:
                if (_weapChrgeTexFlag)
                    SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xc9783e));
                if (_powerAtkCloudTimer[1].HasTimerFinished(_playerID))
                {
                    StateManager.SetAttackState(_playerID, AttackStateData.CurrentState.ChargeEnd);
                    _powerAtkCloudTimer[2].StartTimer(_playerID);
                    GameManager.SetReleaseAtkEnemyData(AttackStateData.CurrentState.ChargeEnd, _playerID);
                }
                break;
            case AttackStateData.CurrentState.ChargeEnd:
                if (_weapChrgeTexFlag)
                    SwapColor(TexIndexData.ChargeIndicator, ColorFromInt(0xa12c1d));
                break;
            case AttackStateData.CurrentState.None:

                break;
        }

    }

    private void UpdateArmorState()
    {
        //Guard clause prevents repeating armor breakage animation.
        if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.None)
            return;


        
        if (GameManager.GetPlayerArmor(_playerID) <= 0.0)//0.09f)//0.016f)
        {
            //print("Armor=" + GameManager.GetPlayerArmor(_playerID));
            AudioManager.PlaySfx(10, _playerID);
            //_childFXController.SetPlayerDXId(2);
            GameManager.SetCurrentPlayerEquipC(_playerID, ItemData.ItemType.None, ItemData.ItemSubType.empty);
            _armorBroken = true;
            _animControllerFilepath = "HumanAnimControllers/Unarmored/";
            AnimControllerSwapout();

            StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.ArmorBroken);
            _equipGainedAnimTimer.SetTimestampedFlag(false, 1);
            _equipGainedAnimTimer.SetTimerDuration(1.0f, 1);
            _equipGainedAnimTimer.StartTimer(1);
        }
    }

    private void UpdateFallDamage()
	{
        if (GameManager._instance.GetScene() == GameManager.Level.TutorialScene)
            return;

        if (_falling)
        {
            if (!_fallDmgFlag)
            {
                _applyFallDmg = false;

                if (StateManager.GetPrevPlayerState(_playerID) == PlayerStateData.CurrentState.MidAirAttack ||
                    StateManager.IsMidAirAttack(_playerID))
				{
                    _fallTimer.SetTimestampedFlag(false, 0);
                    _fallTimer.SetTimerDuration(2.5f, 0);
                    _fallTimer.StartTimer(0);
                }
                else
				{
                    _fallTimer.SetTimestampedFlag(false, 0);
                    _fallTimer.SetTimerDuration(0.85f, 0);
                    _fallTimer.StartTimer(0);
                }



                _fallDmgFlag = true;
            }
            if (_fallTimer.HasTimerFinished(0))
            {
                //Still falling? Then safe amount of time to pass whilst passing has finished.
                _applyFallDmg = true;
            }
        }
        else
        {
            //Not falling? Have we flagged damage to be applied from a high landing?
            //Are we grounded?
            if (_controller.collisions.below)
            {
                if (_applyFallDmg)
                {
                    if (GameManager.GetCurrentPlayerEquipC(_playerID) != ItemData.ItemType.Armor)
                    {
                        GameManager.GetPlayerStatData(_playerID).Damage = 0.15;
                        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 0);
                    }
                    if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
					{
                        GameManager.GetPlayerStatData(_playerID).ArmorDamage = 0.15;
                        GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 2);
                    }
                    UpdateArmorState();
                    SetTextureFlashFX(Color.red, 0.1f);
                    SetUIDamageStatusText(15);
                    if (_hitShakeFlag)
                    {
                        //_proCamera2DShake.Shake(_shakePreset);
                        _hitShakeFlag = false;
                    }
                    _applyFallDmg = false;
                    _fallDmgFlag = false;
                }
                else
                {
                    //Apply Fall damage never got flagged as enabled upon landing.
                    //However the timer still remains finished on the next round of falling.
                    //We don't want this because it will trigger damage to be applied.
                    //So here we overwrite that time stamp so we don't get that behaviour.
                    if (_fallDmgFlag)
                    {
                        _fallTimer.SetTimestampedFlag(false, 0);
                        _fallDmgFlag = false;
                    }
                }
            }
            else
			{
                //In the air however not fallng, could be jumping or double jumping.
                //Here we don't want to apply fall damage, as this would lead to it being
                //applied, say if player jumped across horizontally and exceeded fall damage
                //flag timer. Undesired behavior.
                if (_fallDmgFlag)
                {
                    _fallTimer.SetTimestampedFlag(false, 0);
                    _fallDmgFlag = false;
                }
            }

        }
    }

    private void UpdateGravity()
    {
        if (StateManager.IsLockedComboAttack(_playerID))
            return;
        if (StateManager.IsMidAirAttack(_playerID))
            return;

        //UpdateFallDamage();

        //print("\nfalling=" + _falling);
        if (!_controller.collisions.below)
        {

            _velocity.y += _gravity * Time.deltaTime;


            SetFallAnim();
            //if (_falling)
            //    _smlAttack = 0;
        }
        else
        {

            if (_falling)//fall anim.
            {
                //SetUIDamageStatusText(20);


                //For midair attack then land, then gets stuck mid air attacking whilst landed, so these lines below.
                _behaviourTimer.ForceTimerEnd(_playerID);
                _behaviourTimer.SetTimestampedFlag(false, _playerID);
                //_smlAttack = 0;
                if(!_controller.collisions.platformDrop)
                    _falling = false;
                _jumped = false;
                _dblJumped = false;
                UpdateAnimator();
                FXObjPooler._curInstance.Instantiate(_transform.position, 1, null, 0); //arg2= 0 for runfx 1 for jumpfx
                AudioManager.PlaySfx(4, _playerID);
            }

        }
    }

    private void UpdateTransform()
    {
        _controller.Move(_velocity * Time.deltaTime, _input);
    }

    private void FlipLeftRightHandItems(bool onItemPickup)
    {
        if (!_targetController.IsTargetActive())
        {
            if (onItemPickup)
            {
                //Flip the sprite as appropriate.
                if (!_spriteRenderer.flipX)
                {
                    _faceDir = -1;
                    _spriteRenderer.flipX = true;

                    StateManager.SetPlayerDir(_playerID, _faceDir);
                    return;
                }
                if (_spriteRenderer.flipX)
                {
                    _faceDir = 1;
                    _spriteRenderer.flipX = false;

                    StateManager.SetPlayerDir(_playerID, _faceDir);
                    return;
                }
            }
            else
            {
                //Guard clause against this case as it'll interfer with logic for dodging.
                if (StateManager.IsDodgePrepare(_playerID))
                    return;
                if (StateManager.IsDodgeRelease(_playerID))
                    return;

                //Guard clause against this case as it'll interfer with logic for combo attacking.
                if (StateManager.IsHitByQuickAttack(_playerID))
                    return;

                //Flip the sprite as appropriate.
                if (_velocity.x < 0f && !_spriteRenderer.flipX)
                {
                    _faceDir = -1;
                    _spriteRenderer.flipX = true;

                    Vector3 v = _smallAttackPoint.localPosition;
                    v.x = -Mathf.Abs(v.x);
                    _smallAttackPoint.localPosition = v;

                    v = _powerAttackPoint.localPosition;
                    v.x = -Mathf.Abs(v.x);
                    _powerAttackPoint.localPosition = v;

                    StateManager.SetPlayerDir(_playerID, _faceDir);
                }
                if (_velocity.x > 0f && _spriteRenderer.flipX)
                {
                    _faceDir = 1;
                    _spriteRenderer.flipX = false;

                    Vector3 v = _smallAttackPoint.localPosition;
                    v.x = Mathf.Abs(v.x);
                    _smallAttackPoint.localPosition = v;

                    v = _powerAttackPoint.localPosition;
                    v.x = Mathf.Abs(v.x);
                    _powerAttackPoint.localPosition = v;

                    StateManager.SetPlayerDir(_playerID, _faceDir);
                }
                if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)//We want the attackers dir
                    GameManager.SetCurCombatPlayerAtkDir(_faceDir);
            }
        }
        if (_targetController.IsTargetActive())
        {
            //Ensure player is always facing target when it is set.

            //Flip the sprite as appropriate.
            if (_transform.position.x < _targetController.GetTargetPos().x)
            {
                //Edit face direction here affects some power attacks and quick attacks
                //as they use some of it for the logic internally
                _faceDir = 1;

                if (_spriteRenderer.flipX)
                    _spriteRenderer.flipX = false;
                else
                    return;

                SetAtkCollisionFaceDir(_faceDir);
                //Vector3 v = _smallAttackPoint.localPosition;
                //v.x = Mathf.Abs(v.x);
                //_smallAttackPoint.localPosition = v;

                //v = _powerAttackPoint.localPosition;
                //v.x = Mathf.Abs(v.x);
                //_powerAttackPoint.localPosition = v;

            }
            if (_transform.position.x > _targetController.GetTargetPos().x)
            {
                _faceDir = -1;

                if (!_spriteRenderer.flipX)
                    _spriteRenderer.flipX = true;
                else
                    return;

                SetAtkCollisionFaceDir(_faceDir);
                //Vector3 v = _smallAttackPoint.localPosition;
                //v.x = -Mathf.Abs(v.x);
                //_smallAttackPoint.localPosition = v;

                //v = _powerAttackPoint.localPosition;
                //v.x = -Mathf.Abs(v.x);
                //_powerAttackPoint.localPosition = v;

            }

            //StateManager.SetPlayerDir(_playerID, _faceDir);
        }

    }

    private void SetAtkCollisionFaceDir(int faceDir)
	{
        if(faceDir == 1)
		{
            Vector3 v = _smallAttackPoint.localPosition;
            v.x = Mathf.Abs(v.x);
            _smallAttackPoint.localPosition = v;

            v = _powerAttackPoint.localPosition;
            v.x = Mathf.Abs(v.x);
            _powerAttackPoint.localPosition = v;
            return;
        }
        if (faceDir == -1)
        {
            Vector3 v = _smallAttackPoint.localPosition;
            v.x = -Mathf.Abs(v.x);
            _smallAttackPoint.localPosition = v;

            v = _powerAttackPoint.localPosition;
            v.x = -Mathf.Abs(v.x);
            _powerAttackPoint.localPosition = v;
            return;
        }
    }

    public bool IsMoving()
	{
        if (_targetVelocityX != 0f)
            return true;
        return false;
	}

    public void IsInWater(bool b)
	{
        _isInWater = b;
        if (b)
		{
            _moveSpeed = _moveSpeed / 2;
            return; 
		}
        if(!b)
		{
            _moveSpeed = 3.75f;//3.5f;
            return;
        }
	}

    public bool IsSprinting()
	{
        return _isSprint;
	}

    public bool IsWeaponEquipped()
    {

        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword)
            return true;

        if (GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            return true;


        //if (GameManager.GetCurrentPlayerEquipB(_playerID) != ItemData.ItemType.Sword)


        return false;
    }

    public char CurrentWeaponEquipSlot()
	{
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword)
            return 'A';

        if (GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            return 'B';

        return 'E';
    }

    private bool SafeToMove()
    {
        //if (DialogueManager._instance.GetIsStoryTellingFlag()) //Yes BUT this gets in the way of how we use it in Training Grounds Level, so we use the below.
        //    return false;

        if (DialogueManager._instance.GetIsStoryTellingFlag())
		{
            //if (GameManager._instance._level != GameManager.Level.TrainingGroundScene)
            //    return false;
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
        }

        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.HasArmorBroken(_playerID))
            return false;
        if (StateManager.IsAttackPrepare(_playerID))
            return false;
        if (StateManager.IsAttackRelease(_playerID))
            return false;
        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsDying(_playerID))
            return false;
        if (StateManager.IsDead(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (_offsceen)
            return false;

        //Passed all checks return true, safe to move.
        return true;
    }

    private bool SafeToAttack()
    {
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.HasArmorBroken(_playerID))
            return false;
        //Guard clause.
        if (StateManager.IsJumping(_playerID))
            return false; //No attack mid air.

        //Guard clause.
        if (StateManager.IsDefencePrepare(_playerID))
            return false; //No attack during defence.

        if (StateManager.IsDefenceRelease(_playerID))
            return false; //No attack during defence.

        if (StateManager.IsKnockBackAttack(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        //Guard clause.
        if (StateManager.IsAttackRelease(_playerID))
            return false; //No attack restart/spam during attack release.

        //if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByReleaseAtk)
        //Guard clause.
        if (StateManager.IsHitByReleaseAttack(_playerID))
            return false; //No attack during hit state.
        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
            return false; //No attack during hit state.

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        //if (!IsWeaponEquipped())
        //    return false;

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;

        if(!IsWeaponEquipped())
            return false;


        //Passed all checks return true, safe to attack.
        return true;
    }

    private bool SafeToQuickAttack()
    {
        ////Guard clause.
        //if (StateManager.IsJumping(_playerID))
        //    return false; //No attack mid air.

        ////Guard clause.
        //if (StateManager.IsDefencePrepare(_playerID))
        //    return false; //No attack during defence.

        //if (StateManager.IsDefenceRelease(_playerID))
        //    return false; //No attack during defence.
        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.HasArmorBroken(_playerID))
            return false;

        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;

        //Guard clause.
        if (StateManager.IsAttackRelease(_playerID))
            return false; //No quick attack spam during attack release.

        //Guard clause.
        if (StateManager.IsHitByReleaseAttack(_playerID))
            return false; //No attack during hit state.
        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
            return false; //No attack during hit state.
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsDefencePrepare(_playerID))
            return false;

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;



        ////Prevents spamming attacks that break defence logic,anims. DOESN'T WORK
        //if (_isHitDefending)
        //    return false;

        //if (_isQuickHitDefending)
        //    return false;

        //if (!IsWeaponEquipped())
        //    return false;
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        //Passed all checks return true, safe to attack.
        return true;
    }

    private bool SafeToKnockAttack()
    {
        //Guard clause.

        if (!IsWeaponEquipped())
            return false;
        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.HasArmorBroken(_playerID))
            return false;

        //Guard clause.
        if (StateManager.IsAttackRelease(_playerID))
            return false; //No knock attack spam during attack release.

        //Guard clause.
        if (StateManager.IsHitByReleaseAttack(_playerID))
            return false; //No attack during hit state.
        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
            return false; //No attack during hit state.
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsDefencePrepare(_playerID))
            return false;

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;



        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        //Ensure this attack behaviour only happens when a weapon is in at least one hand.
        if (GameManager.GetCurrentPlayerEquipA(_playerID) != ItemData.ItemType.Sword &&
            GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Sword)
            return true;
        else if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Sword &&
            GameManager.GetCurrentPlayerEquipB(_playerID) != ItemData.ItemType.Sword)
            return true;
        else
            return false;

        //Passed all checks return true, safe to attack.
        return true;
    }

    private bool SafeToDefend()
    {
        //if (!IsWeaponEquipped())
        //    return false;
        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.HasArmorBroken(_playerID))
            return false;
        //Guard clause.
        if (StateManager.IsJumping(_playerID))
            return false; //No defending mid air.

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        //Guard clause.
        if (StateManager.IsAttackPrepare(_playerID))
            return false; //No defence during attack.

        //Guard clause.
        if (StateManager.IsAttackRelease(_playerID))
            return false; //No defence during attack.

        if (StateManager.IsDefenceRelease(_playerID))
            return false;  //No defence restart/spam during defence release.

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;



        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        //Passed all checks return true, safe to ddefend.
        return true;
    }

    private bool SafeToDodge()
    {
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        if (_isInWater)
            return false;

        if (StateManager.HasGainedEquip(_playerID))
            return false;

        if (StateManager.HasArmorBroken(_playerID))
            return false;

        if (StateManager.IsAttackPrepare(_playerID))
            return false; //No dodge during attack.

        if (StateManager.IsAttackRelease(_playerID))
            return false; //No dodge during attack.

        if (StateManager.IsDefencePrepare(_playerID))
            return false;  //No dodge restart/spam during defence prepare.

        if (StateManager.IsDefenceRelease(_playerID))
            return false;  //No dodge restart/spam during defence release.

        if (StateManager.IsQuickAttack(_playerID))
            return false;

        if (StateManager.IsUpwardAttack(_playerID))
            return false;

        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsJumping(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsHitByEnemyReleaseAttack(_playerID))
            return false; //No dodge

        if (StateManager.IsHitByReleaseAttack(_playerID))
            return false; //No dodge 

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        if(StateManager.IsHitByQuickAttack(_playerID))
            return false;



        if (StateManager.IsExhausted(_playerID))
            return false;

        if (RespawnManager.GetTeamPlayersRespawnFlag())
            return false;

        //Passed all checks return true, safe to ddefend.
        return true;
    }

    private bool SafeToJump()
    {
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        if (StateManager.HasGainedEquip(_playerID))
            return false;

        if (StateManager.HasArmorBroken(_playerID))
            return false;

        if (StateManager.IsDying(_playerID))
            return false;

        if (StateManager.IsAttackPrepare(_playerID))
            return false;

        if (StateManager.IsAttackRelease(_playerID))
            return false; //No jumping during an attack.

        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;

        if (StateManager.IsDefencePrepare(_playerID))
            return false;

        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;



        //Passed all checks return true, safe to jump.
        return true;
    }

    private bool SafeToAssignItem()
    {
        if (DialogueManager._instance.GetIsStoryTellingFlag())
            return false;
        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;

        if (GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.None)
            return true;
        if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.None)
            return true;

        if (StateManager.IsSlamOnGround(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID))
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;

        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        return false;
    }

    private bool SafeToSetIdle()
    {
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }

        if (StateManager.HasGainedEquip(_playerID))
            return false;

        if (StateManager.HasArmorBroken(_playerID))
            return false;

        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByReleaseAtk)
            return false;
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByEnemyReleaseAtk)
            return false;
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.QuickAttack)
            return false;
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByQuickAtk)
            return false;
        if (StateManager.IsKnockBackAtkPrepare(_playerID))
            return false;
        if (StateManager.IsKnockBackAttack(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;
        if (StateManager.IsDodgePrepare(_playerID))
            return false;
        if (StateManager.IsDodgeRelease(_playerID))
            return false;
        if (StateManager.IsSlamOnGround(_playerID))
            return false;

        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;

        if (StateManager.IsExhausted(_playerID))
            return false;
        if (StateManager.IsDefencePrepare(_playerID))
            return false;



        if (StateManager.IsLockedComboAttack(_playerID))
            return false;

        return true;
    }

    public bool SafeToAttackPlayer(int playerId)
    {
        //if (DialogueManager._instance.GetIsStoryTellingFlag())
        //{
        //    return false;
        //}
        if (DialogueManager._instance.GetIsStoryTellingFlag())
        {
            if (!DialogueManager._instance.IsAllowInputDuringStoryFlag())
                return false;
            else
                return true;
        }
        if (StateManager.IsDead(playerId))
            return false;
        if (StateManager.IsDodgePrepare(playerId))
            return false;
        if (StateManager.IsDodgeRelease(playerId))
            return false;
        if(StateManager.HasArmorBroken(playerId))
            return false;
        if (StateManager.IsSlamOnGround(playerId))
            return false;
        if (StateManager.IsRespawning(_playerID) || _respawned)
            return false;
        if (StateManager.IsLockedComboAttack(_playerID))
            return false;
        if (StateManager.HasGainedEquip(_playerID))
            return false;
        if (StateManager.IsSlamOnGroundByEnemy(_playerID))
            return false;
        if (StateManager.IsExhausted(_playerID))
            return false;

        return true;
    }

    public float GetPlayerVelocityX()
	{
        return _velocity.x;
	}

    public int GetPlayerID()
    {
        return _playerID;
    }

    public int GetFaceDir()
    {
        return _faceDir;
    }

    public IDController GetIDController()
	{
        return _IDController;
	}

    public void SetProcessLogicFlag(bool b)
    {
        //This is used with the PlayerMonitor.cs to prevent logic flow during slow motion
        //pause menu state. 

        //However setting this will conflict with some of the player state dead logic,
        //particularly the gameover flag is set when 1 player remains if the pause menu
        //behaviour is initiated. We prevent this by not setting the flag here if thha
        //particular player has now died.

        //Guard clause, holy fucking grail i tell thee.
        if (StateManager.IsDead(_playerID))
            return;

        _processUpdate = b;
    }

    public bool GetProcessLogicFlag()
	{
        return _processUpdate;
	}

    private void RemovePlayerFromCamera()
    {
        if (ProCamera2D.Instance.CameraTargets.Count == 1)
            return;

        for (int i = 0; i < ProCamera2D.Instance.CameraTargets.Count; i++)
        {
            //ProCamera2D.Instance.CameraTargets.RemoveAt(i);
            if (ProCamera2D.Instance.CameraTargets[i].TargetTransform.name == "Player 1")
            {
                if (_playerID == 0)
                {
                    ProCamera2D.Instance.CameraTargets.RemoveAt(i);
                    return;
                }
            }
            if (ProCamera2D.Instance.CameraTargets[i].TargetTransform.name == "Player 2")
            {
                if (_playerID == 1)
                {
                    ProCamera2D.Instance.CameraTargets.RemoveAt(i);
                    return;
                }
            }
            if (ProCamera2D.Instance.CameraTargets[i].TargetTransform.name == "Player 3")
            {
                if (_playerID == 2)
                {
                    ProCamera2D.Instance.CameraTargets.RemoveAt(i);
                    return;
                }
            }
            if (ProCamera2D.Instance.CameraTargets[i].TargetTransform.name == "Player 4")
            {
                if (_playerID == 3)
                {
                    ProCamera2D.Instance.CameraTargets.RemoveAt(i);
                    return;
                }
            }
            //if (ProCamera2D.Instance.CameraTargets[i].TargetTransform == null)
            //{
            //    ProCamera2D.Instance.CameraTargets.RemoveAt(i);
            //}
        }
    }

    private void NullifyAnimatorFlags()
    {
        _jumped = false;
        _dblJumped = false;
        if(!_controller.collisions.platformDrop)
            _falling = false;
        _attacked = false;
        _released = false;
        _smlAttack = 0;
        _isHit = false;
        _isQuickHit = false;
        _isHitDefending = false;
        _isQuickHitDefending = false;
        _defended = false;
        _isSlamOnGround = false;
        _applyHitFlag = false;
        _armorBroken = false;
        //_comboAttack = 0;
        _comboInputFlag = 0;

        _smlAtkIncrement = 0;
        _dodge = false;
        _isSprint = false;



        UpdateAnimator();
    }

    public void OnBecameInvisible()
    {
        //print("\noffscreen!"); //works! when scene window is closed
        if (!_offsceen)
        {
            _offsceen = true;
            _velocity.x = 0f;
        }
    }

    public void OnBecameVisible()
    {
        //print("\nonscreen!");
        if (_offsceen)
            _offsceen = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_smallAttackPoint == null)
            return;
        if (_powerAttackPoint == null)
            return;
        if (_aboveAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_smallAttackPoint.position, _smallAttackRange);
        Gizmos.DrawWireSphere(_powerAttackPoint.position, _powerAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_aboveAttackPoint.position, _powerAttackRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, 12.0f);
    }



    private void DetectCollisionsUpdate()
    {
        if (_enableTargUpdate)
        {

            #region EnemiesToTarget
            for (int i = 0; i < 2; i++) //1 doesn't work, must be < 2
            {
                //Detect enemy is within range of specified circle.
                Collider2D[] enemyTargets = Physics2D.OverlapCircleAll(this.gameObject.transform.position, 15.0f, _enemyLayers[i]);

                if(enemyTargets.Length < 1)
				{
                    _enableTargUpdate = false;
                    return;
                }

                int j = 0;

                




                foreach (Collider2D target in enemyTargets)
                {
                    //BARRELS AND OTHER ITEMS ARE BEING TARGETTED! FIX THIS!
                    if (target.CompareTag("Enemy"))
                    {
                        //if ((target.GetComponent("EnemyController") as EnemyController) == null)
                        //    return;

                        if (target.GetComponent<EnemyController>() == null)
                            break;// return;

                        float shortestDistanceSoFar = Vector3.Distance(_transform.position, enemyTargets[0].gameObject.transform.position);
                        GameObject closestObjectSoFar = enemyTargets[0].gameObject;

                        float currentDistance = 100f;

                        if(enemyTargets[i].GetComponent<EnemyController>() != null)
                            currentDistance = Vector3.Distance(_transform.position, enemyTargets[i].gameObject.transform.position); Vector3.Distance(_transform.position, enemyTargets[i].gameObject.transform.position);
                        
                        if (currentDistance < shortestDistanceSoFar)
                        {
                            closestObjectSoFar = enemyTargets[i].gameObject;
                            shortestDistanceSoFar = currentDistance;
                        }

                        int enemyID = target.GetComponent<EnemyController>()._enemyID;
                        if (!EnemyStatesManager.IsDead(enemyID))
                        {
                            _targetController.SetClosestTarget(closestObjectSoFar.transform, enemyID);
                            j++;
                        }


                    }

                }
            }
            _enableTargUpdate = false;
            _applyTargUpdate = true;
            #endregion
        }

        if (_enableAtkUpdate)
        {
            //Guard clause to ensure 1 cycle of damage processed & not continuous.
            if (_weaponCollidedFlag)
                return;

            #region AttackDetections
            for (int i = 0; i < 2; i++) // _enemyLayers[i] 0 = players, 1 = enemies, 2= obstacles
            {
                //Detect enemy is within range of specified circle.
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(_smallAttackPoint.position, _smallAttackRange, _enemyLayers[i]);

                if (_smlAttack > 0)
                    enemiesHit = Physics2D.OverlapCircleAll(_smallAttackPoint.position, _smallAttackRange, _enemyLayers[i]);
                if (_released)
                    enemiesHit = Physics2D.OverlapCircleAll(_powerAttackPoint.position, _powerAttackRange, _enemyLayers[i]);
                if(_isUpwardAtk)
                    enemiesHit = Physics2D.OverlapCircleAll(_aboveAttackPoint.position, _powerAttackRange, _enemyLayers[i]);

                //Deal damage to enemies.
                int j = 0;
                foreach (Collider2D enemy in enemiesHit)
                {

                    //print("\nYou've hit " + enemy.name);

                    if (enemy.CompareTag("Player"))
                    {
                        //if (StateManager.IsDead(0))
                        //    return;

                        if (!SafeToAttackPlayer(0))
                            return;

                        if (_playerID != 0) //This allows us to use larger hit detection circles that overlap the using player itself.
						{
                            _collidedPlayerId[j] = 0;
                            _weaponCollidedFlag = true;
                            _isEnemyCollidedFlag = false;
                            _hitShakeFlag = true;
                        }

                    }
                    if (enemy.CompareTag("Player2"))
                    {
                        //if (StateManager.IsDead(1))
                        //    return;

                        if (!SafeToAttackPlayer(1))
                            return;

                        if (_playerID != 1) //This allows us to use larger hit detection circles that overlap the using player itself.
						{
                            _collidedPlayerId[j] = 1;
                            _weaponCollidedFlag = true;
                            _isEnemyCollidedFlag = false;
                            _hitShakeFlag = true;
                        }

                    }
                    if (enemy.CompareTag("Player3"))
                    {
                        //if (StateManager.IsDead(2))
                        //    return;

                        if (!SafeToAttackPlayer(2))
                            return;

                        if (_playerID != 2) //This allows us to use larger hit detection circles that overlap the using player itself.
						{
                            _collidedPlayerId[j] = 2;
                            _weaponCollidedFlag = true;
                            _isEnemyCollidedFlag = false;
                            _hitShakeFlag = true;
                        }

                    }
                    if (enemy.CompareTag("Player4"))
                    {
                        //if (StateManager.IsDead(3))
                        //    return;

                        if (!SafeToAttackPlayer(3))
                            return;

                        if (_playerID != 3) //This allows us to use larger hit detection circles that overlap the using player itself.
						{
                            _collidedPlayerId[j] = 3;
                            _weaponCollidedFlag = true;
                            _isEnemyCollidedFlag = false;
                            _hitShakeFlag = true;
                        }

                    }
                    if (enemy.CompareTag("NPC"))
					{
                        if (enemy.GetComponent<NPCController>() == null)
                            return;

                        int npcID = enemy.GetComponent<NPCController>()._npcID;

                        if (!NPCStatesManager.IsDying(npcID))
						{
                            NPCStatesManager.SetNPCState(npcID, NPCStateData.CurrentState.Dying);
                        }

                    }
                    if (enemy.CompareTag("Tank"))
					{
                        if (enemy.GetComponent<TrollController>() != null)
                        {
                            enemy.GetComponent<TrollController>().IsHit(_playerID);
                            _hitShakeFlag = true;
                        }
                    }
                    if (enemy.CompareTag("Enemy"))
                    {

                        if (enemy.GetComponent<FXController>() != null) //Enemy beheaded - head OR dead body.
						{
                            if(enemy.gameObject.name == "BanditSeveredHeadObjA(Clone)")
							{
                                //Then we are attacking a severed bandit head. Lets make it do stuff :)
                                enemy.GetComponent<FXController>().HitBanditSeveredHead(_playerID);
                            }
                            if (enemy.gameObject.name == "BanditBodyObjA(Clone)")
                            {
                                //Then we are attacking a dead bandit body. Lets make it do stuff :)
                                enemy.GetComponent<FXController>().HitBanditDeadBody(_playerID);
                            }
                        }
                        if (enemy.GetComponent<EnemyController>() != null)
						{
                            //int enemyID = enemy.GetComponent<EnemyController>()._enemyID;
                            //SetKillingPlayerID() passes the player id that delivers killing blow and recievees the enemy id. Assists with KillRewarder.
                            int enemyID = enemy.GetComponent<EnemyController>().SetKillingPlayerID(_playerID);

                            //if(enemy.GetComponent<EnemyController>() != null)
                            //enemyID = enemy.GetComponent<EnemyController>()._enemyID;
                            if (EnemyStatesManager.IsDisabled(enemyID))
                            {
                                _enableAtkUpdate = false;
                                return;
                            }
                            if (EnemyStatesManager.IsDying(enemyID))
                            {
                                _enableAtkUpdate = false;
                                return;
                            }

                            if(EnemyStatesManager.IsDead(enemyID))
							{
                                EnemyStatesManager.SetEnemyState(enemyID, EnemyStateData.CurrentState.Disable);
                                //print("\nenemy state =" + EnemyStatesManager.GetEnemyState(enemyID));
                                _enableAtkUpdate = false;
                                return;
                            }

                            if (!EnemyStatesManager.IsDead(enemyID))
                            {


                               
                                //return;
                                _targetController.SetClosestTarget(enemy.transform, enemyID);

                                ////Guard clause
                                //if (j >= _collidedEnemyId.Length)
                                //    break;// return;
                                _weaponCollidedFlag = true;
                                _isEnemyCollidedFlag = true;
                                _hitShakeFlag = true;
                                _collidedEnemyId[j] = enemyID;
                                j++;
                                //Guard clause
                                if (j >= _collidedEnemyId.Length)
                                    break;// return;
                            }
                            else
							{
                                //print("\nsetting enemy state to disabled.");
                                //EnemyStatesManager.SetEnemyState(enemyID, EnemyStateData.CurrentState.Disable);
                                //FXObjPooler._curInstance.Instantiate(PlayerMonitor.GetBanditController(enemyID).transform.position, 39, null, enemyID); //dead bandit body fx
                            }



                        }
                        if (enemy.GetComponent<SkullbatController>() != null)
						{
                            PlayerMonitor.GetPlayerController(_playerID).SetUIDamageCounterText(100);
                            enemy.GetComponent<SkullbatController>().SetState(4,j, _playerID);

                            //FXObjPooler._curInstance.Instantiate(_transform.position, 35, null, _killingPlayerId); //arg2= 35 for burnedfx
                        }
                        if (enemy.GetComponent<StaticArcherController>() != null)
                        {
                            if (enemy.GetComponent<StaticArcherController>().GetState() == StaticArcherController.State.IsDying)
                            {
                                _enableAtkUpdate = false;
                                return;
                                //break;// return;
                            }
                            if (enemy.GetComponent<StaticArcherController>().GetState() == StaticArcherController.State.IsDeath)
                            {
                                enemy.GetComponent<StaticArcherController>().Hide();
                                //FXObjPooler._curInstance.Instantiate(enemy.GetComponent<StaticArcherController>().transform.position, 39, null, 0); //dead bandit body fx
                                _enableAtkUpdate = false;
                                return;
                                //break;// return;
                            }


                            enemy.GetComponent<StaticArcherController>().SetState(2, _playerID);
                            _hitShakeFlag = true;
                        }
                        if (enemy.GetComponent<ProjectileTrapController>() != null)
                        {
                            if (enemy.GetComponent<ProjectileTrapController>().GetState() == ProjectileTrapController.State.IsDying)
                            {
                                _enableAtkUpdate = false;
                                return;
                                //break;// return;
                            }
                            if (enemy.GetComponent<ProjectileTrapController>().GetState() == ProjectileTrapController.State.IsDeath)
                            {
                                enemy.GetComponent<ProjectileTrapController>().Hide();
                                //FXObjPooler._curInstance.Instantiate(enemy.GetComponent<StaticArcherController>().transform.position, 39, null, 0); //dead bandit body fx
                                _enableAtkUpdate = false;
                                return;
                                //break;// return;
                            }


                            enemy.GetComponent<ProjectileTrapController>().SetState(2, _playerID);
                            _hitShakeFlag = true;
                        }
                        //                  if (enemy.GetComponent<TrollController>() != null)
                        //{
                        //                      enemy.GetComponent<TrollController>().IsHit(_playerID);
                        //                      _hitShakeFlag = true;
                        //                  }
                    }
                    if (enemy.CompareTag("Portal"))
                    {
                        if (enemy.GetComponent<PortalController>() != null)
                        {
                            _comboInputFlag = 0; //Do't accumalate to combat attack initiation when attacking portals!

                            if (PortalManager.IsPortalAttacking())
                                return;
                            if (PortalManager.IsPortalTeleporting())
                                return;

                            int portalID = enemy.GetComponent<PortalController>()._portalId;

                            //SWORD DAMAGE AGAINST PORTAL
                            if(IsWeaponEquipped())
							{
                                if (StateManager.IsQuickAttack(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 1);//arg2 is damaged state.
                                if (StateManager.IsMidAirAttack(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 2);//arg2 is damaged state.
                                if (StateManager.IsAttackRelease(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 5);//arg2 is damaged state.
                                if (StateManager.IsKnockBackAttack(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 4);//arg2 is damaged state.
                                if (StateManager.IsMidAirAttackRelease(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 4);//arg2 is damaged state.
                                if (StateManager.IsUpwardAttack(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 3);//arg2 is damaged state.
                                if (StateManager.IsRunningQuickAttack(_playerID))
                                    PortalManager.SetPortalState(portalID, 2, 3);//arg2 is damaged state.
                            }
                            else //HAND OR SHIELD DAMAGE AGAINST PORTAL
							{
                                PortalManager.SetPortalState(portalID, 2, 0);//2 is damaged.
                            }
                            _hitShakeFlag = true;
                        }

                    }
                    if (enemy.CompareTag("Container"))//exists on enemy layer.
					{
                        _comboInputFlag = 0; //Do't accumalate to combat attack initiation when attacking containers!

                        if (IsWeaponEquipped())
						{
                            if (StateManager.IsQuickAttack(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 1, _playerID); //arg2=dmg against container.
                            if (StateManager.IsAttackRelease(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 6, _playerID);//arg2=dmg against container.
                            if (StateManager.IsMidAirAttack(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 2, _playerID); //arg2=dmg against container.
                            if (StateManager.IsKnockBackAttack(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 3, _playerID); //arg2=dmg against container.
                            if (StateManager.IsMidAirAttackRelease(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 3, _playerID); //arg2=dmg against container.
                            if (StateManager.IsUpwardAttack(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 3, _playerID); //arg2=dmg against container.
                            if (StateManager.IsRunningQuickAttack(_playerID))
                                enemy.GetComponent<ContainerController>().SetState(2, 4, _playerID); //arg2=dmg against container.
                        }
                        else
                            enemy.GetComponent<ContainerController>().SetState(1, 0, _playerID);

                        if(enemy.GetComponent<ContainerController>()._typeFlag == 3)//scarecrow target
						{
                            _targetController.SetClosestTarget(enemy.transform, -1);
                        }



                        //enemy.GetComponent<ContainerController>().SetState(1, 0, _playerID);


                    }
                    //j++; //ONLY IMPORTANT FOR HUMANOID ENEMY AI THAT WORK WITH ENEYSTATEMANAGER
                }
            }



            #endregion

            _enableAtkUpdate = false;
        }

        if (_weaponCollidedFlag)
        {
            if (_isEnemyCollidedFlag)
            {
                ProcessEnemyCollisions();
                return; //_isEnemyCollidedFlag is reset to false, be aware of below code & return;
            }
            if (!_isEnemyCollidedFlag)
            {
                ProcessPlayerCollisions();
                return;
            }
        }

    }

    private void ProcessPlayerCollisions()
    {
        #region DamageDealtLogic


        #region CalculateDamage
        switch (GameManager.GetCurrentPlayerEquipA(_playerID))
        {
            case ItemData.ItemType.None:
                _dmgA = 0.01;
                break;
            case ItemData.ItemType.Shield:
                _dmgA = 0.05;
                break;
            case ItemData.ItemType.Sword:
                _dmgA = 0.1;
                break;
        }
        switch (GameManager.GetCurrentPlayerEquipB(_playerID))
        {
            case ItemData.ItemType.None:
                _dmgA = 0.01;
                break;
            case ItemData.ItemType.Shield:
                _dmgA = 0.05;
                break;
            case ItemData.ItemType.Sword:
                _dmgA = 0.1;
                break;
        }

        _dmgSum = _dmgA + _dmgB;
        #endregion

        for (int i = 0; i < _collidedPlayerId.Length; i++)
        {
            if (_collidedPlayerId[i] != -1)
			{
                if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
                {

                    #region CalculateChargedAtkDmg
                    _dmgSum = _dmgSum * 2;
                    switch (StateManager.GetAttackState(_playerID))
                    {
                        case AttackStateData.CurrentState.ChargeStart:
                            _dmgSum = _dmgSum * 2;
                            break;
                        case AttackStateData.CurrentState.ChargeMid:
                            _dmgSum = _dmgSum * 3;
                            break;
                        case AttackStateData.CurrentState.ChargeEnd:
                            _dmgSum = _dmgSum * 4;
                            break;
                        case AttackStateData.CurrentState.None:
                            break;
                    }
                    #endregion

                    #region CalculateDmgAbsorbed
                    if (StateManager.GetPlayerState(_collidedPlayerId[i]) == PlayerStateData.CurrentState.HoldDefence && PlayerTargetIsDefendingCorrectDir(i))
                    {
                        if (GameManager.GetCurrentPlayerEquipA(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                            _dmgSum = _dmgSum / 4;
                        if (GameManager.GetCurrentPlayerEquipB(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                            _dmgSum = _dmgSum / 4;
                        StateManager.SetPlayerState(_collidedPlayerId[i], PlayerStateData.CurrentState.HitWhileDefending);
                    }
                    else
                    {
                        if (!StateManager.IsHitWhileDefending(_collidedPlayerId[i]))
						{
                            //StateManager.SetPlayerState(_collidedPlayerId[i], PlayerStateData.CurrentState.HitByReleaseAtk);
                            StateManager.SetPlayerState(_collidedPlayerId[i], PlayerStateData.CurrentState.SlamOnGround);
                        }
                        else
                        {
                            if (GameManager.GetCurrentPlayerEquipA(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                                _dmgSum = _dmgSum / 4;
                            if (GameManager.GetCurrentPlayerEquipB(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                                _dmgSum = _dmgSum / 4;
                        }
                    }
                    #endregion

                    #region ApplyDamageBasedOnArmorRating
                    if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) != ItemData.ItemType.Armor)
                        GameManager.GetPlayerStatData(_collidedPlayerId[i]).Damage = _dmgSum;
                    if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) == ItemData.ItemType.Armor)
                        GameManager.GetPlayerStatData(_collidedPlayerId[i]).ArmorDamage = _dmgSum / 2;
                    #endregion

                }

                if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.QuickAttack)
                {
                    #region CalculateDmgAbsorbed
                    if (StateManager.GetPlayerState(_collidedPlayerId[i]) == PlayerStateData.CurrentState.HoldDefence && PlayerTargetIsDefendingCorrectDir(i))
                    {
                        if (GameManager.GetCurrentPlayerEquipA(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                            _dmgSum = _dmgSum / 12;
                        if (GameManager.GetCurrentPlayerEquipB(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                            _dmgSum = _dmgSum / 12;
                        StateManager.SetPlayerState(_collidedPlayerId[i], PlayerStateData.CurrentState.QuickHitWhileDefending);
                    }
                    else
                    {
                        if (!StateManager.IsQuickHitWhileDefending(_collidedPlayerId[i]))
                            StateManager.SetPlayerState(_collidedPlayerId[i], PlayerStateData.CurrentState.HitByQuickAtk);
                        else
                        {
                            if (GameManager.GetCurrentPlayerEquipA(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                                _dmgSum = _dmgSum / 12;
                            if (GameManager.GetCurrentPlayerEquipB(_collidedPlayerId[i]) == ItemData.ItemType.Shield)
                                _dmgSum = _dmgSum / 12;
                        }
                    }
                    #endregion

                    #region ApplyDamageBasedOnArmorRating
                    if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) != ItemData.ItemType.Armor)
                        GameManager.GetPlayerStatData(_collidedPlayerId[i]).Damage = _dmgSum;
                    if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) == ItemData.ItemType.Armor)
                        GameManager.GetPlayerStatData(_collidedPlayerId[i]).ArmorDamage = _dmgSum / 2;
                    #endregion

                }

                #region UpdateStatsBasedOnArmorRating
                if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) != ItemData.ItemType.Armor)
                {
                    GameManager.UpdatePlayerStats(_collidedPlayerId[i],
                            GameManager.GetPlayerStatData(_collidedPlayerId[i]), 0);
                }
                if (GameManager.GetCurrentPlayerEquipC(_collidedPlayerId[i]) == ItemData.ItemType.Armor)
                {
                    GameManager.UpdatePlayerStats(_collidedPlayerId[i],
                            GameManager.GetPlayerStatData(_collidedPlayerId[i]), 2);
                }
                #endregion

                #endregion

                int hitPlayerId = _collidedPlayerId[i];
                int hitPlayerFaceDir = PlayerMonitor.GetPlayerFaceDirection(hitPlayerId);
                GameManager.SetPlayersInCombat(_playerID, hitPlayerId, _faceDir, hitPlayerFaceDir);
                _collidedPlayerId[i] = -1; //Moved too be reset in "hit states" so we can access the variable there. 
                //_collidedPlayerIndex = i;//_collidedPlayerId[_collidedPlayerIndex] 
            }


        }


        _weaponCollidedFlag = false;
    }

    private void ProcessEnemyCollisions()
    {
        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.QuickAttack ||
            StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.MidAirAttack ||
            StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.RunningQuickAttack ||
            StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.AttackUpward)
        {
            #region SetEnemyHitState
            for (int i = 0; i < _collidedEnemyId.Length; i++)
            {
                if (_collidedEnemyId[i] != -1)
                {
                    //GameManager.SetDebugEnemyIDTarget(_collidedEnemyId[i]);



                    if (EnemyStatesManager.IsDefencePrepare(_collidedEnemyId[i]) && EnemyIsDefendingCorrectDir(i))
                    {
                        EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.QuickHitWhileDefending);
                    }
                    else
                    {
                        if(_isUpwardAtk)
						{
                            EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitByUpwardAtk);
                        }
                        else
						{
                            if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller.collisions.below)
                                EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitByRunQuickAtk);
                            else
                                EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitByQuickAtk);
                        }

                    }
                    GameManager.SetEnemyHitByPlayer(_collidedEnemyId[i], _playerID, _faceDir);






                    _collidedEnemyId[i] = -1;//Is set upon enemy collision detection.
                    _weaponCollidedFlag = false;
                    _isEnemyCollidedFlag = false;
                    return;
                }
            }

            #endregion
        }

        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
        {

            #region SetEnemyHitState
            for (int i = 0; i < _collidedEnemyId.Length; i++)
            {
                if (_collidedEnemyId[i] != -1)
                {
                    //GameManager.SetDebugEnemyIDTarget(_collidedEnemyId[i]);
                    if (EnemyStatesManager.IsDefencePrepare(_collidedEnemyId[i]))// && EnemyIsDefendingCorrectDir(i))
                    {
                        EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitWhileDefending);
                    }
                    else
                    {
                        //EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitByReleaseAtk);
                        EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.SlamOnGround);
                    }

                    GameManager.SetEnemyHitByPlayer(_collidedEnemyId[i], _playerID, _faceDir);
                    _collidedEnemyId[i] = -1;//Is set upon enemy collision detection.

                    if(_weaponCollidedFlag)
                        _weaponCollidedFlag = false;
                    if(_isEnemyCollidedFlag)
                        _isEnemyCollidedFlag = false;
                    
                }
                else
                    return;
            }

            #endregion
        }


        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.KnockBackAttack ||
            StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.MidAirReleaseAttack)
        {
            #region SetEnemyHitState
            for (int i = 0; i < _collidedEnemyId.Length; i++)
            {
                if (_collidedEnemyId[i] != -1)
                {
                    //GameManager.SetDebugEnemyIDTarget(_collidedEnemyId[i]);



                    if (EnemyStatesManager.IsDefencePrepare(_collidedEnemyId[i]) && EnemyIsDefendingCorrectDir(i))
                    {
                        EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.QuickHitWhileDefending);
                    }
                    else
                    {
                        if(!EnemyStatesManager.IsHitByKnockBackAttack(_collidedEnemyId[i]))
                            EnemyStatesManager.SetEnemyState(_collidedEnemyId[i], EnemyStateData.CurrentState.HitByKnockBackAtk);
                        else
						{

                            _collidedEnemyId[i] = -1;//Is set upon enemy collision detection.
                            _weaponCollidedFlag = false;
                            _isEnemyCollidedFlag = false;
                            return;
                        }
                    }
                    GameManager.SetEnemyHitByPlayer(_collidedEnemyId[i], _playerID, _faceDir);






                    _collidedEnemyId[i] = -1;//Is set upon enemy collision detection.
                    _weaponCollidedFlag = false;
                    _isEnemyCollidedFlag = false;
                    return;
                }
            }

            #endregion
        }
        //_weaponCollidedFlag = false;
        //_isEnemyCollidedFlag = false;
    }

    public void SetUIDamageCounterText(int dmg)
	{
        _dmgCounterUIText.SetDmgText(dmg);
	}

    public void SetUIDamageStatusText(int dmg)
	{
        _dmgCounterUIText.SetFallDmgText(dmg);
    }

    public void SetUIBlockedCounterText(int defValue)
	{
        _dmgCounterUIText.SetBlockedText(defValue);

    }

    private bool EnemyIsDefendingCorrectDir(int index)
    {
        //If the enemy is defending and NOT facing same direction as player, then SUCCESSFUL defense.
        if (EnemyStatesManager.GetSpecificEnemyDir(_collidedEnemyId[index]) != _faceDir)
            return true;
        return false;
    }

    private bool PlayerTargetIsDefendingCorrectDir(int index)
    {
        //If the enemy is defending and NOT facing same direction as player, then SUCCESSFUL defense.
        if (StateManager.GetSpecificPlayerDir(_collidedPlayerId[index]) != _faceDir)
            return true;
        return false;
    }

    private void CheckForTeamPlayerRespawnLogic()
	{
        //So player has entered an already activated campsite.
        //We check for players that have fallen and respawn them
        //back into the game.
        int playerCount = PlayerMonitor.GetPlayerCount();//0;
        if (playerCount > 1)
        {
            int deathCount = 0;
            if (StateManager.IsDead(0))
                deathCount++;
            if (StateManager.IsDead(1))
                deathCount++;
            if (StateManager.IsDead(2))
                deathCount++;
            if (StateManager.IsDead(3))
                deathCount++;

            if (deathCount == 0)
                return;

            if (!RespawnManager.GetTeamPlayersRespawnFlag()) 
            {
                RespawnManager.SetTeamPlayersRespawnFlag(true);

                //The rest of the work will now take place over in GameoverUIController.cs
                //We use most of the logic already in place for a game over event.
                //Only this way, we cut out the gameover UI and voice over and simply play 
                //out the remaining logic for respawns.
            }
        }
    }

    private void CollisionFlags(Collider2D collision)
    {
        if (collision.CompareTag("PortalFlag"))
        {
            if (!PortalManager.GetPortalBattleFlag())
            {
                //print("\nPortal Battle Flagged!");
                //_cameraCinematics.Play(); //moved to the portalcontroller.cs
                AudioManager.PlaySfx(13, 0);
                PortalManager.SetPortalBattleFlag(true);
            }
        }
        if (collision.CompareTag("CampsiteFlag"))
        {
            if (PortalManager.GetPortalBattleFlag())
                return;

            int campsiteID = collision.GetComponent<CampfireController>().GetCampsiteID();

            if (!RespawnManager.HasCampsiteActivated(campsiteID)) //This works internally with respawn manager, incrementally progressing check points right now.
            {
                //print("\nCampsite Activated Flagged!");

                if (GameManager._instance._level == GameManager.Level.TutorialScene) //Make this check as causing input & behaviour issues.
				{
                    RespawnManager.SetCampsiteActivation(true, campsiteID);
                    return;
				}
				else
				{
                    _processUpdate = false;
                    _velocity.x = 0;
                    NullifyAnimatorFlags();
                    UpdateAnimator();

                    _cameraCinematics.CinematicTargets.Clear();
                    _cameraCinematics.LetterboxAmount = 0.133f;
                    _cameraCinematics.AddCinematicTarget(RespawnManager.GetCampsiteTransform(campsiteID), 2.5f, 2.15f, 2, EaseType.EaseIn, "", "", 0);
                    _cameraCinematics.Play();
                    AudioManager.PlaySfx(20, 0);
                    //_exitDemoTimer.StartTimer(0);
                    GameManager.Save(_playerID);
                    RespawnManager.SetCampsiteActivation(true, campsiteID);
                    CheckForTeamPlayerRespawnLogic();
                }

            }
            else
			{
                CheckForTeamPlayerRespawnLogic();
            }
        }
        if (collision.CompareTag("FallenFlag"))
		{
            if(!GameManager.GetRespawnedFlag(_playerID))
			{
                _velocity.x = 0;
                _velocity.y = 0;

                //Flagged here. Then flagged off over in PlayerUIStatController.cs
                _stats.Armor = 0;
                _stats.Currency = 0;
                _stats.Health = PlayerStatData._maxHealth / 2;
                _stats.Stamina = 0;
                _stats.KillCount = 0;

                GameManager.SetPlayerStats(_playerID, _stats);
                GameManager.ResetPlayerEquip(_playerID);

                //_animatorControllerIndex = 0;
                //_animControllerFilepath = "HumanAnimControllers/Unarmored/";
                //AnimControllerSwapout();

                AudioManager.PlaySfx(21, _playerID);
                FXObjPooler._curInstance.Instantiate(_transform.position, 14, null, 0); //arg2= for fallenPlayerRespawnfx
                                                                                        //AudioManager.PlaySfx(20, 0);
                _falling = true;
                _fallen = true;
                UpdateAnimator();
                //_isHit = true;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Respawning);
                _hitBehaviourTimer.SetTimestampedFlag(false, _playerID);
                _hitBehaviourTimer.SetTimerDuration(1.25f, _playerID);
                _hitBehaviourTimer.StartTimer(_playerID);

                SetTextureFlashFX(Color.red, 0.75f);

                RespawnManager.SetRespawnedCampsiteItems(true, _playerID);
                GameManager.SetRespawnFlag(true, _playerID); //Rest of work happens over in PlayerUIStatController.cs.

                _gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
            }

        }
        if (collision.CompareTag("Arrow"))
		{
            if (!SafeToAttackPlayer(_playerID))
                return;

            double dmg = 0.10;
            if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HoldDefence)
            {
                if (GameManager.GetCurrentPlayerEquipA(_playerID) == ItemData.ItemType.Shield)
                    dmg = _dmgSum / 6;
                if (GameManager.GetCurrentPlayerEquipB(_playerID) == ItemData.ItemType.Shield)
                    dmg = _dmgSum / 6;
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.QuickHitWhileDefending);
            }
            else
			{
                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByQuickAtk);
            }
            if (GameManager.GetCurrentPlayerEquipC(_playerID) != ItemData.ItemType.Armor)
                GameManager.GetPlayerStatData(_playerID).Damage = dmg;
            if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
                GameManager.GetPlayerStatData(_playerID).ArmorDamage = dmg * 1.40;// / 1.4;
            if (GameManager.GetCurrentPlayerEquipC(_playerID) != ItemData.ItemType.Armor)
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 0);
            if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
                GameManager.UpdatePlayerStats(_playerID, GameManager.GetPlayerStatData(_playerID), 2);
        }
        if (collision.CompareTag("Health"))
		{
            if (GameManager.GetPlayerStatData(_playerID).Health >= PlayerStatData._maxHealth)
                return;
            SetTextureFlashFX(Color.Lerp(Color.green, Color.white, Time.deltaTime), 0.75f);
        }

        if (collision.CompareTag("DeadBody"))
        {
            if (StateManager.IsQuickAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsMidAirAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsUpwardAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsMidAirAttackRelease(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsKnockBackAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsAttackRelease(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsRunningQuickAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }

        }

        if (collision.CompareTag("EndActFlag"))
		{


            if(!DialogueManager._instance.GetIsCurrentActEndFlag())
			{
                _processUpdate = false;

                StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.Run);
                NullifyAnimatorFlags();
                
                //_velocity.y = 0f;
                _velocity.x = 2.0f;

                if (_enableTargUpdate)
                    _enableTargUpdate = false;
                if (_applyTargUpdate)
                    _applyTargUpdate = false;
                _targetController.DeselectClosestTarget();
                UpdateAnimator();

                //HERE BELOW UNCOMMENT
                GameManager.Save(_playerID);
                GameManager.SaveConfig(true, true);//Set save flags to prevent prologue storys playing again on next time play through!

                DialogueManager._instance.SetCurrentActEndFlag(true);
            }

        }
        if (collision.CompareTag("StoryTellerFlag"))
        {

			if (GameManager.GetTankEnemyBattleFlag())
				return;
			if (_faceDir == -1) //Only activate when approaching from the left, facing right.
                return;

            if (!DialogueManager._instance.GetIsStoryTellingFlag())
            {
                _sprintSpeed = 1.0f;
                _processUpdate = false;
                _velocity.x = 0.0f;
                NullifyAnimatorFlags();
                UpdateAnimator();
                //MOVED TO DialogueManager.cs PluginTextObjs()
                //_cameraCinematics.CinematicTargets.Clear();
                //_cameraCinematics.LetterboxAmount = 0.125f;
                //_cameraCinematics.AddCinematicTarget(_transform, 4.0f, 10.0f, 2, EaseType.EaseIn, "", "", 0);
                //_cameraCinematics.Play();
                //_IDController.gameObject.SetActive(false);
                DialogueManager._instance.SetStoryTellingFlag(true, DialogueManager.DialogueFlag.storyteller, _playerID, 1, false);

                if (GameManager._instance.GetScene() == GameManager.Level.ForestSceneAct1)
				{
                    collision.gameObject.SetActive(false);
                }
                if (GameManager._instance.GetScene() == GameManager.Level.ForestSceneAct2)
                {
                    collision.gameObject.SetActive(false);
                }
            }

        }
        if (collision.CompareTag("MessageFlag"))
		{
            if (!DialogueManager._instance.GetIsStoryTellingFlag())
            {
                _sprintSpeed = 1.0f;
                _processUpdate = false;
                _velocity.x = 0.0f;

                switch (GameManager._instance._level)
				{

                    case GameManager.Level.TrainingGroundScene:
                        DialogueManager._instance.MessageFlagIncrement(); //Used so internally we can adjust training ground message PER TRIGGER we encounter.
                        DialogueManager._instance.SetStoryTellingFlag(true, DialogueManager.DialogueFlag.message, _playerID, 1, true); //Set the usuals, pre planned texts to display as appropriate.
                        //_processUpdate = true;//needs applying within dialogue manager.
                        collision.gameObject.SetActive(false); //Then of course remove this trigger.
                        break;

                    case GameManager.Level.ForestSceneAct2:
                        if(DialogueManager._instance.GetMessageFlag() < 1) //First msg encounter is this level, so should be 0 initially.
						{

                            NullifyAnimatorFlags();
                            UpdateAnimator();
                            DialogueManager._instance.SetStoryTellingFlag(true, DialogueManager.DialogueFlag.message, _playerID, 1, true);
                            DialogueManager._instance.SetMessageFlag(1); //Prevents this message repeating upon collision flag.
                        }
                        else
						{
                            collision.gameObject.SetActive(false);
                        }

                        break;
				}

            }
        }

        if (collision.CompareTag("MerchantFlag"))
        {
            if (_faceDir == -1) //Only activate when approaching from the left.
                return;

            if (StateManager.IsJumping(_playerID))
                return;

            //Don't trigger dialogue when the stall has been opened and active.
            //if (collision.GetComponent<MerchantController>().IsStallActivate())
            //    return;

            

            if (!DialogueManager._instance.GetIsStoryTellingFlag())
            {
                int dialogeCount = collision.GetComponent<MerchantController>().IncrementDialogueActivation();
                DialogueManager._instance.SetMerchantFinFlag(false);
                _sprintSpeed = 1.0f;
                _processUpdate = false;
                _velocity.x = 0.0f;
                NullifyAnimatorFlags();
                UpdateAnimator();
                //MOVED TO DialogueManager.cs PluginTextObjs()
                //_cameraCinematics.CinematicTargets.Clear();
                //_cameraCinematics.LetterboxAmount = 0.125f;
                //_cameraCinematics.AddCinematicTarget(_transform, 4.0f, 10.0f, 2, EaseType.EaseIn, "", "", 0);
                //_cameraCinematics.Play();
                //_IDController.gameObject.SetActive(false);
                DialogueManager._instance.SetStoryTellingFlag(true, DialogueManager.DialogueFlag.merchant, _playerID, dialogeCount, false);
            }

        }
        if (collision.CompareTag("MerchantFinFlag"))
		{
            if(!DialogueManager._instance.GetMerchantFinFlag())
                DialogueManager._instance.SetMerchantFinFlag(true);
        }
        if (collision.CompareTag("AudioChangeFlag"))
		{
            if(_faceDir == 1)
			{
                if (AudioManager.GetCurLevelAudioChangeFlag() == 2)
                {
                    AudioManager.SetCurLevelAudioChangeFlag(1); //arg is index of _forestAct_Ambience[1] "forestact02_ambience02"
                    return;
                }
                if (AudioManager.GetCurLevelAudioChangeFlag() == 1)
				{
                    AudioManager.SetCurLevelAudioChangeFlag(2); //arg is index of _forestAct_Ambience[1] "forestact02_ambience01"
                    return;
                }
               
            }
            if (_faceDir == -1)
            {
                if (AudioManager.GetCurLevelAudioChangeFlag() == 2)
				{
                    AudioManager.SetCurLevelAudioChangeFlag(1); //arg is index of _forestAct_Ambience[1] "forestact02_ambience02"
                    return;
                }
                if (AudioManager.GetCurLevelAudioChangeFlag() == 1)
				{
                    AudioManager.SetCurLevelAudioChangeFlag(2); //arg is index of _forestAct_Ambience[1] "forestact02_ambience01"
                    return;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionFlags(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //CollisionFlags(collision);
        //https://www.youtube.com/watch?v=sPiVz1k-fEs
        if (collision.CompareTag("DeadBody"))
        {
            if (StateManager.IsQuickAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsMidAirAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsUpwardAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsMidAirAttackRelease(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsKnockBackAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsAttackRelease(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }
            if (StateManager.IsRunningQuickAttack(_playerID))
            {
                collision.gameObject.SetActive(false);
                FXObjPooler._curInstance.Instantiate(collision.transform.position, 39, null, -1); //dead bandit body fx
                return;
            }

        }
    }

    private void SetTextureFlashFX(Color c, float time)
    {
        if (!_hitTexFlag)
        {
            _hitTextureTimer.SetTimerDuration(time, 0);
            _hitTextureTimer.StartTimer(0);
            SetSpriteColor(c);
            _processTexFxFlag = true;
            _hitTexFlag = true;
        }
    }

    private void InitColorSwapTex()
    {

        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;

        for (int i = 0; i < colorSwapTex.width; ++i)
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        colorSwapTex.Apply();


        _spriteRenderer.material.SetTexture("_SwapTex" + _playerID, colorSwapTex);

        _spriteColors = new Color[colorSwapTex.width];
        _colorSwapTex = colorSwapTex;

    }

    private void SwapColor(TexIndexData index, Color color)
    {
        //if (color == Color.black)
        //    return;

        _spriteColors[(int)index] = color;
        _colorSwapTex.SetPixel((int)index, 0, color);
    }

    private void SetSpriteColor(Color color)
    {
        for (int i = 0; i < _colorSwapTex.width; ++i)
        {
            //if (_colorSwapTex.GetPixel(i, 0).r == 0 && //Ignore black pixels.
            //   _colorSwapTex.GetPixel(i, 0).g == 0 &&
            //   _colorSwapTex.GetPixel(i, 0).b == 0)
            //    return;
            //else
            _colorSwapTex.SetPixel(i, 0, color);
        }
        _colorSwapTex.Apply();
    }

    private void ResetSetSpriteColor()
    {
        //bool ignoreFlag = false;
        //if (GameManager.GetCurrentPlayerEquipC(_playerID) == ItemData.ItemType.Armor)
        //    ignoreFlag = true;

        for (int i = 0; i < _colorSwapTex.width; ++i)
        {
            //if (ignoreFlag && i == 4)
            //    return;
            _colorSwapTex.SetPixel(i, 0, _spriteColors[i]);
        }
        _colorSwapTex.Apply();

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




	//public CameraTarget AddCameraTarget(Transform targetTransform, float targetInfluenceH = 1f, float targetInfluenceV = 1f, float duration = 0f)
	//{
	//    var newCameraTarget = new CameraTarget
	//    {
	//        TargetTransform = targetTransform,
	//        TargetInfluenceH = targetInfluenceH,
	//        TargetInfluenceV = targetInfluenceV,
	//    };

	//    CameraTargets.Add(newCameraTarget);

	//    if (duration > 0f)
	//    {
	//        newCameraTarget.TargetInfluence = 0f;
	//        StartCoroutine(AdjustTargetInfluenceRoutine(newCameraTarget, targetInfluenceH, targetInfluenceV, duration));
	//    }

	//    return newCameraTarget;
	//}






















	//void OnTriggerEnter2D(Collider2D collision)
	//{

	//    //if (collision.tag == "Item")
	//    if (collision.CompareTag("Item"))
	//    {
	//        ItemCollisionFlag();
	//    }
	//    if (collision.CompareTag("Lefthand") || collision.CompareTag("Righthand"))
	//    {
	//        _collidedPlayerId = 0;
	//        WeaponHitCollisionFlag();
	//    }
	//    if (collision.CompareTag("Lefthand2") || collision.CompareTag("Righthand2"))
	//    {
	//        _collidedPlayerId = 1;
	//        WeaponHitCollisionFlag();
	//    }
	//    if (collision.CompareTag("Lefthand3") || collision.CompareTag("Righthand3"))
	//    {
	//        _collidedPlayerId = 2;
	//        WeaponHitCollisionFlag();
	//    }
	//    if (collision.CompareTag("Lefthand4") || collision.CompareTag("Righthand4"))
	//    {
	//        _collidedPlayerId = 3;
	//        WeaponHitCollisionFlag();
	//    }

	//}

	//private void OnTriggerStay2D(Collider2D collision)
	//{
	//    if (collision.CompareTag("Lefthand"))
	//    {
	//        //WeaponCollisionFlag();
	//    }//https://www.youtube.com/watch?v=sPiVz1k-fEs
	//}

	//private void ItemCollisionFlag()
	//{

	//    if (!SafeToAssignItem())
	//        return;
	//    GameManager.SetCollisionOrderFlag(CollisionData.Participants.PlayerWithItem, _playerID);
	//    _itemCollidedFlag = true;
	//}

	//private void WeaponHitCollisionFlag()
	//{

	//    //Guard clause.
	//    if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.Dying)
	//        return;

	//    //Guard clause.
	//    if (_playerID == _collidedPlayerId) //Don't count weapon collision with it's parent.
	//        return;

	//    #region FlagPowerAttack(ed)
	//    if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
	//        GameManager.SetPlayersInCombat(_playerID, _collidedPlayerId, _faceDir);

	//    if (StateManager.GetPlayerState(_playerID) != PlayerStateData.CurrentState.ReleaseAttack &&
	//        StateManager.GetPlayerState(_collidedPlayerId) == PlayerStateData.CurrentState.ReleaseAttack)
	//    {
	//        //This will be the player that is being HIT.
	//        //We must enable the following flag to allow the logic to execute for
	//        //a player that's hit.
	//        if (!_enableAtkUpdate) //For the player that is HIT.
	//        {
	//            _enableAtkUpdate = true;
	//        }
	//    }
	//    #endregion

	//    #region FlagQuickAttack(ed)

	//    if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.QuickAttack)
	//        GameManager.SetPlayersInCombat(_playerID, _collidedPlayerId, _faceDir);

	//    if (StateManager.GetPlayerState(_playerID) != PlayerStateData.CurrentState.QuickAttack &&
	//        StateManager.GetPlayerState(_collidedPlayerId) == PlayerStateData.CurrentState.QuickAttack)
	//    {
	//        //This will be the player that is being HIT.
	//        //We must enable the following flag to allow the logic to execute for
	//        //a player that's hit.
	//        if (!_enableAtkUpdate) //For the player that is HIT.
	//        {
	//            _enableAtkUpdate = true;
	//        }
	//    }

	//    #endregion

	//    if (!_weaponCollidedFlag)
	//    {
	//        _weaponCollidedFlag = true;
	//    }
	//}

	//private void CollisionsFixedUpdate()
	//{
	//    if (_itemCollidedFlag)
	//    {
	//        switch(GameManager.GetCollisionOrderFlag(_playerID))
	//        {
	//            case CollisionData.Participants.ItemWithPlayer:
	//                GameManager.CallPlayerItemPickup(_playerID);
	//                //_lifeUp = GameManager.UpdatePlayerLifeUp(_playerID);
	//                GameManager.SetItemPickupPlayerId(_playerID);
	//                FlipLeftRightHandItems(true);
	//                break;
	//            case CollisionData.Participants.PlayerWithItem:
	//                GameManager.CallPlayerItemPickup(_playerID);
	//                //_lifeUp = GameManager.UpdatePlayerLifeUp(_playerID);
	//                GameManager.SetItemPickupPlayerId(_playerID);
	//                FlipLeftRightHandItems(true);
	//                break;
	//        }
	//        _itemCollidedFlag = false;
	//    }

	//    if (_enableAtkUpdate)
	//    {

	//        if (!_weaponCollidedFlag)
	//            return;

	//        #region SetAttackerCollisionScenrio
	//        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.ReleaseAttack)
	//            GameManager.SetCollisionOrderFlag(CollisionData.Participants.SwordWithPlayer, _playerID);

	//        if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.QuickAttack)
	//            GameManager.SetCollisionOrderFlag(CollisionData.Participants.SwordWithPlayer, _playerID);
	//        #endregion

	//        switch (GameManager.GetCollisionOrderFlag(_playerID))
	//        {
	//            case CollisionData.Participants.None:
	//                break;

	//            case CollisionData.Participants.PlayerWithSword:

	//                #region UpdateDamageDealt

	//                //Guard clause.
	//                if (StateManager.GetPlayerState(_playerID) == PlayerStateData.CurrentState.HitByReleaseAtk)
	//                    return;

	//                if(StateManager.GetPlayerState(_collidedPlayerId) == PlayerStateData.CurrentState.ReleaseAttack)
	//                {
	//                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByReleaseAtk);
	//                    _stats.Damage = 0.2f;
	//                    //PrintPlayersInCombatData();
	//                }
	//                if (StateManager.GetPlayerState(_collidedPlayerId) == PlayerStateData.CurrentState.QuickAttack)
	//                {
	//                    StateManager.SetPlayerState(_playerID, PlayerStateData.CurrentState.HitByQuickAtk);
	//                    _stats.Damage = 0.05f;
	//                    //PrintPlayersInCombatData();
	//                }

	//                GameManager.UpdatePlayerStats(_playerID, _stats);
	//                #endregion

	//                break;
	//            case CollisionData.Participants.SwordWithPlayer:

	//                GameManager.SetCollisionOrderFlag(CollisionData.Participants.None, _playerID);

	//                PrintPlayersInCombatData();

	//                break;
	//        }



	//        _enableAtkUpdate = false;
	//        _weaponCollidedFlag = false;
	//    }

	//}

	//private void PrintPlayersInCombatData()
	//{
	//    int attacker = -1000;
	//    int hit = -1000;

	//    attacker = GameManager.GetCurCombatPlayerIdAtk();
	//    hit = GameManager.GetCurCombatPlayerIdHit();
	//    print("\nPlayer" + (attacker + 1) + " attacked, " +
	//        "Player" + (hit + 1) + ". Hit throw dir is, " + GameManager.GetCurCombatPlayerAtkDir());

	//    GameManager.SetCurCombatStatus(false);
	//}

}
