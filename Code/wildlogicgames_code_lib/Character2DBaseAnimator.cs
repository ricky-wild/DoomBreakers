using System;
using UnityEngine;
using System.Collections.Generic;

namespace wildlogicgames
{
	public class Character2DBaseAnimator
	{
		protected Animator _animator;
		protected string _baseAnimControllerFilepath;
		protected string _specificAnimControllerFilepath;
		protected string _theAnimationControllerName;

		protected Dictionary<int, string> _animations;

		protected RuntimeAnimatorController _runtimeAnimatorController;

		public Character2DBaseAnimator(ref Animator animator, string baseAnimationControllerFilepath, string specificAnimControllerFilePath, string theAnimationControllerName)
		{
			_animator = animator;
			_baseAnimControllerFilepath = baseAnimationControllerFilepath;
			_specificAnimControllerFilepath = specificAnimControllerFilePath;
			_theAnimationControllerName = theAnimationControllerName;
			_animator.runtimeAnimatorController = Resources.Load(_baseAnimControllerFilepath + "/" + specificAnimControllerFilePath + "/" + _theAnimationControllerName) as RuntimeAnimatorController;

			if (_animations == null) _animations = new Dictionary<int, string>();

			_runtimeAnimatorController = _animator.runtimeAnimatorController;
		}

		public void SetBaseAnimFilePath(string filePathStr) => _baseAnimControllerFilepath = filePathStr;
		public void SetSpecificAnimFilePath(string filePathStr) => _specificAnimControllerFilepath = filePathStr;
		public string GetBaseAnimFilePath() => _baseAnimControllerFilepath;
		public string GetSpecificAnimFilePath() => _specificAnimControllerFilepath;
		public Animator GetAnimator() => _animator;

		public void AddAnimation(int idKey, string animationName)
		{
			if (!_animations.ContainsKey(idKey)) _animations.Add(idKey, animationName);
		}

		public void PlayAnimation(int idKey)
		{
			if(_animations.ContainsKey(idKey)) _animator.Play(_animations[idKey]);
		}
	}
}
