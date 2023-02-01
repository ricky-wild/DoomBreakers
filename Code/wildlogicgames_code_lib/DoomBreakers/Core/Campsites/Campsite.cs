using System;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;
using SpriteTrail;
using Com.LuisPedroFonseca.ProCamera2D;

namespace DoomBreakers
{
	public enum CampfireAnimID
	{
		Deactivated = 0,
		Activated = 1,
		Running = 2
	};

	[RequireComponent(typeof(SpriteTrail.SpriteTrail))]
	//[RequireComponent(typeof(Character2DBaseAnimator))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Collider2D))]
	public class Campsite : MonoBehaviour
	{
		private static Campsite _instance;

		[Header("Campsite ID")]
		public int _campsiteID;

		[Header("ProCamera2DCinematics Reference")]
		[Tooltip("Will internally retrieve upon Setup() otherwise.")]
		public ProCamera2DCinematics _cameraCinematics;

		private Transform _transform;
		private Collider2D _collider2d;
		private SpriteRenderer _spriteRenderer;
		private SpriteTrail.SpriteTrail _spriteTrailFX;
		private Character2DBaseAnimator _2DAnimator;
		private Animator _animator;
		private CampfireAnimID _animationId;

		private ITimer _timer;
		private bool _setup = false;

		private void Setup()
		{
			_instance = this;
			_transform = this.transform;
			_collider2d = GetComponent<Collider2D>();
			_animator = this.GetComponent<Animator>();
			_spriteRenderer = this.GetComponent<SpriteRenderer>();
			_spriteTrailFX = this.GetComponent<SpriteTrail.SpriteTrail>();
			_cameraCinematics = ProCamera2D.Instance.GetComponent<ProCamera2DCinematics>();

			//anims "activated", "running", "deactivated"
			//CampsiteAnimControllers\Campfires\Campfire.controller
			_2DAnimator = new Character2DBaseAnimator(ref _animator, "CampsiteAnimControllers", "Campfires", "Campfire");
			_2DAnimator.AddAnimation(0, "deactivated");
			_2DAnimator.AddAnimation(1, "activated");
			_2DAnimator.AddAnimation(2, "running");
			_animationId = CampfireAnimID.Deactivated;

			_timer = new Timer();
			LevelEventManager.PluginCampsites(_campsiteID, ref _instance);//this);
			_setup = true;
		}
		public CampfireAnimID GetState() => _animationId;
		//private void Awake() => Setup();
		private void Update() //=> _2DAnimator.PlayAnimation((int)_animationId);
		{
			if(!_setup) Setup();
			_2DAnimator.PlayAnimation((int)_animationId);

			if(_animationId == CampfireAnimID.Activated)
				if (_timer.HasTimerFinished()) _animationId = CampfireAnimID.Running;
		}
		public void Activate() //=> _animationId = CampfireAnimID.Activated;
		{
			_animationId = CampfireAnimID.Activated;
			_timer.StartTimer(1.0f);
			LevelEventManager.PlayCinematic(_transform, 3.0f, 3.0f, 1.5f);
		}
	}
}
