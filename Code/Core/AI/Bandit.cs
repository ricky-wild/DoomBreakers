using UnityEngine;

namespace DoomBreakers
{
    public class Bandit : MonoBehaviour, IEnemy
    {
        private void InitializeBandit()
        {

        }

        private void Awake()
        {
            InitializeBandit();
        }

        void Start()
        {
            //_playerAnimator.SetAnimatorController(AnimatorController.Player_with_broadsword_with_shield_controller, false);
        }
        void Update() { }

        public void UpdatePlayerPathFinding()
		{

		}
        public void UpdateStateBehaviours()
		{

		}
        public void UpdateAnimator()
		{

		}
        public void UpdateCollisions()
		{

		}
    }
}


