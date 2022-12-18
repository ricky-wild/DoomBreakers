using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class BaseFX :MonoBehaviour
	{
		protected Transform _transform;
		protected Vector3 _velocity;
		protected Animator _animator;
		protected SpriteRenderer _spriteRenderer;
		protected int _direction;
		protected float _targetVelocityX, _targetVelocityY;

		protected Color _colour;
		protected float _alpha;
		protected float _alphaFreq;
		protected bool _applyAlphaFadeFlag;

		protected bool _process;


		//public BaseFX(int direction) => Setup(direction);
		public virtual void Setup(int direction)
		{
			_transform = this.transform;
			_velocity = Vector3.zero;
			_direction = direction;
			_targetVelocityX = 0f;
			_targetVelocityY = 0f;
			_spriteRenderer = this.GetComponent<SpriteRenderer>();
			//_animator = this.GetComponent<Animator>();
			//_animator.Play(""); //FXController.controller
			_alpha = 1.0f;
			_alphaFreq = 0.005f;
			_applyAlphaFadeFlag = true;
			if (_spriteRenderer != null)
			{
				_colour = _spriteRenderer.color;
				_colour.a = _alpha;
				_spriteRenderer.color = _colour;
			}
			FlipSprite();

		}
		public virtual void SetDirection(int dir) //=> _direction = dir;
		{
			Setup(dir);
			_process = true;
		}
		void Awake() { }
		void Start() { }


		public virtual void Update()
		{
			if (!_process) return;

			ApplyTransparency();

			_transform.Translate(_velocity * Time.deltaTime);

			if (_alpha < 0.05f)
			{
				_process = false;
				this.gameObject.SetActive(false);
			}
		}
		public virtual void FlipSprite()
		{
			if (_direction == -1) //Moving Left
			{
				if(!_spriteRenderer.flipX)
					_spriteRenderer.flipX = true;
				return;
			}
			if (_direction == 1) //Moving Right
			{
				if (_spriteRenderer.flipX)
					_spriteRenderer.flipX = false;
				return;
			}
		}
		public virtual void ApplyTransparency()
		{
			if (!_applyAlphaFadeFlag) return;
			_alpha -= _alphaFreq;
			_colour.a = _alpha;
			_spriteRenderer.color = _colour;
		}
	}
}
