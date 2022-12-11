
namespace DoomBreakers
{
	interface IBanditAnimator //: MonoBehaviour
	{
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(BanditAnimatorController animatorController);

	}
}
