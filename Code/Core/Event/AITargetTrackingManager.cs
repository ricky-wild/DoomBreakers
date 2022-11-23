

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum EnemyAI
	{
        Bandit = 0,
        Skeleton = 1
	};
    public class AITargetTrackingManager : MonoBehaviour
    {
        //<summary>
        //The AI target tracking manager purpose is to retrieve and supply targets to persue for
        //individual enemy AI. The targets will be binded by each enemy ID assigned and used
        //as appropriate via their ID' and type (see EnemyAI enum above).
        //Relevant data will be plugged in via BanditCollision.cs trigger and pulled out-used from
        //BanditPersue : BanditBaseState class.
        //</summary>

        private static AITargetTrackingManager _targetTrackingEventManager;


        private static Dictionary<int, Transform> _banditTargetTransforms;
        public static AITargetTrackingManager _instance
        {
            get //When we access our instance from another place, we'll setup as appropriate if required.
            {
                if (!_targetTrackingEventManager)
                {
                    //FindObjectOfType isn't a cheap call but we only do this once, if not at all.
                    _targetTrackingEventManager = FindObjectOfType(typeof(AITargetTrackingManager)) as AITargetTrackingManager;

                    if (!_targetTrackingEventManager)
                        print("\nAITargetTrackingManager= You need an active AITargetTrackingManager script attached to a GameObject within the scene!");
                    else
                        _targetTrackingEventManager.Setup();
                }

                return _targetTrackingEventManager;
            }
        }
        private void Setup()
        {
            if (_banditTargetTransforms == null)
                _banditTargetTransforms = new Dictionary<int, Transform>();
        }


        public static void AssignTargetTransform(Transform targetTransform, int forEnemyId, EnemyAI forEnemyType)
        {
            switch(forEnemyType)
			{
                case EnemyAI.Bandit:
                    _banditTargetTransforms.Add(forEnemyId, targetTransform);
                    break;
                case EnemyAI.Skeleton:
                    break;
			}
            
        }
        public static Transform GetAssignedTargetTransform(int forEnemyId, EnemyAI forEnemyType)
		{
            switch (forEnemyType)
            {
                case EnemyAI.Bandit:
                    return _banditTargetTransforms[forEnemyId];
                case EnemyAI.Skeleton:
                    return null;
            }

            return null;
		}

    }
}
