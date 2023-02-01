using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public enum NPCAnimID
	{
		Idle = 0,
		Run = 1,
		Flee = 2,
		Jump = 3,
		Wait = 4,
		Hit = 5,
		Fall = 6,
		Defend = 7,
		Dying = 8,
		Dead = 9
	};
	public class NPCAnimator : Character2DBaseAnimator
	{
		private NPCAnimID _animationId;

		//NPCAnimControllers\Standard\NPC_Traveller.controller
		public NPCAnimator(Animator animator, string animFilePath, string animSubFilePath, string animControllerName)
					: base(animator: ref animator, baseAnimationControllerFilepath: animFilePath,
		  specificAnimControllerFilePath: animSubFilePath, theAnimationControllerName: animControllerName)
		{
			_animator = animator;
			_runtimeAnimatorController = _animator.runtimeAnimatorController;
			_baseAnimControllerFilepath = animFilePath;
			_specificAnimControllerFilepath = animSubFilePath;
			_theAnimationControllerName = animControllerName;

			_animationId = NPCAnimID.Idle;

			AddAnimation((int)NPCAnimID.Idle, "Idle");
			AddAnimation((int)NPCAnimID.Wait, "Rest");
			AddAnimation((int)NPCAnimID.Jump, "Jump");
			AddAnimation((int)NPCAnimID.Fall, "Fall");
			AddAnimation((int)NPCAnimID.Run, "Run");
			AddAnimation((int)NPCAnimID.Flee, "Flee");
			AddAnimation((int)NPCAnimID.Dying, "Dying");
			AddAnimation((int)NPCAnimID.Dead, "Dead");

		}

		public void SetAnimation(NPCAnimID animID) //=> PlayAnimation((int)animID);
		{
			_animationId = animID;
			PlayAnimation((int)_animationId);
		}
	}
}
