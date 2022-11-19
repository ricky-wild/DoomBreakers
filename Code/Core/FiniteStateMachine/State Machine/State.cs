using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DoomBreakers
{

	public abstract class State //: MonoBehaviour
	{

		//<summary>
		//All these variables are required for the various Player Behaviours. 
		//So we embody them within a state and usem as appropriate with each
		//dervived player state we create and set.
		//</summary>

		//protected StateMachine _stateMachine;
		//protected Controller2D _controller2D;
		//protected Vector3 _velocity;
		//protected Transform _transform;
		//protected float _targetVelocityX, _maxJumpVelocity, _moveSpeed, _sprintSpeed, _gravity;
		//protected int _quickAttackIncrement; //4+ variations of this animation.
		//protected bool _dodgedLeftFlag;//, _jumpedFlag;

		//protected float _quickAtkWaitTime, _gainedEquipWaitTime;
		//protected ITimer _behaviourTimer, _dodgedTimer, _spriteColourSwapTimer;

		public State()
		{
		}

		public abstract void UpdateBehaviour();
		public abstract void IsIdle();
		public abstract void IsMoving();
		public abstract void IsJumping();
		public abstract void IsFalling();
		public abstract void IsDodging();
	}
}

