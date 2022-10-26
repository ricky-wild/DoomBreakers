
namespace DoomBreakers
{
	interface IBanditAnimator //: MonoBehaviour
	{
		void UpdateAnimator(IBanditBehaviours banditBehaviour);
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(BanditAnimatorController animatorController);

	}
}
