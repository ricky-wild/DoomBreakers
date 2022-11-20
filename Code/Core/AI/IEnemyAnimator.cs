
namespace DoomBreakers
{
	interface IEnemyAnimator //: MonoBehaviour
	{
		void UpdateAnimator(IPlayerBehaviours playerBehaviour);
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(BanditAnimatorController animatorController);

	}
}

