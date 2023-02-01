using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;


namespace DoomBreakers
{

	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Banner : MonoBehaviour
	{
		[Header("Banner Anim ID")]
		public int _bannerAnimID;

		private Character2DBaseAnimator _2DAnimator;
		private Animator _animator;

		//public Banner(Animator animator, string animFilePath, string animSubFilePath, string animControllerName)
		//	: base(animator: ref animator, baseAnimationControllerFilepath: animFilePath,
		//		  specificAnimControllerFilePath: animSubFilePath, theAnimationControllerName: animControllerName)
		//{
		//	_animator = animator;
		//	_runtimeAnimatorController = _animator.runtimeAnimatorController;
		//	_baseAnimControllerFilepath = animFilePath;
		//	_specificAnimControllerFilepath = animSubFilePath;
		//	_theAnimationControllerName = animControllerName;
		//}
		//Resources\PropAnimControllers\Banners\Banner.controller

		private void Awake() => Setup();
		private void Setup()
		{
			_animator = this.GetComponent<Animator>();
			_2DAnimator = new Character2DBaseAnimator(ref _animator, "PropAnimControllers", "Banners", "Banner");
			_2DAnimator.AddAnimation(0, "Banner1");
			_2DAnimator.AddAnimation(1, "Banner2");
			_2DAnimator.AddAnimation(2, "Banner3");
			_2DAnimator.AddAnimation(3, "Banner4");
			_2DAnimator.AddAnimation(4, "Banner5");
		}

		private void Update() => _2DAnimator.PlayAnimation(_bannerAnimID);
	}
}
