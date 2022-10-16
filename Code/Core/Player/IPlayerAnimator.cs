
namespace DoomBreakers
{
	interface IPlayerAnimator //: MonoBehaviour
	{
		void UpdateAnimator(IPlayerBehaviours playerBehaviour);
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(AnimatorController animatorController, bool withArmor);
		//void SetAnimationState(string animationState);
	}
}

