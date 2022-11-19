
namespace DoomBreakers
{
	interface IPlayerAnimator //: MonoBehaviour
	{
		void UpdateAnimator(IPlayerBehaviours playerBehaviour);
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(IPlayerEquipment playerEquipment);//AnimatorController animatorController, bool withArmor);
		//void SetAnimationState(string animationState);
	}
}

