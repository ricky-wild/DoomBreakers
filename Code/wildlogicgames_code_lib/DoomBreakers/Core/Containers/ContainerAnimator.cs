using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public enum ContainerAnimationState
	{
		Container_Idle = 0,
		Container_Hit = 1,
		Container_Target = 2,
		Container_Destroy = 3
	};
	public class ContainerAnimator : Character2DBaseAnimator
	{
		//private ContainerAnimationState _animState;

		public ContainerAnimator(Animator animator, string str1, string str2, string str3)
			: base(animator: ref animator, baseAnimationControllerFilepath: str1, specificAnimControllerFilePath: str2, theAnimationControllerName: str3)
		{
			//All Containers Barrel, Crate, Chest, Tree Animation= Idle, Hit, Destroy, Target
			//_animState = ContainerAnimationState.Container_Idle;
			AddAnimation((int)ContainerAnimationState.Container_Idle, "Idle");
			AddAnimation((int)ContainerAnimationState.Container_Hit, "Hit");
			AddAnimation((int)ContainerAnimationState.Container_Target, "Target");
			AddAnimation((int)ContainerAnimationState.Container_Destroy, "Destroy");
		}

		public void PlayAnimation(ContainerAnimationState state) => PlayAnimation((int)state);
	}
}
