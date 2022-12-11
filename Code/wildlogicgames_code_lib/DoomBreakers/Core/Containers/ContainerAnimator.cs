using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public enum ContainerAnimationState
	{
		Container_Idle = 0,
		Container_Hit = 1,
		Container_Destroy = 2,
	}
	class ContainerAnimator : Character2DBaseAnimator
	{
		private ContainerAnimationState _animationState;

		public ContainerAnimator(Animator animator, string str1, string str2, string str3)
			: base(animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{

		}
	}
}
