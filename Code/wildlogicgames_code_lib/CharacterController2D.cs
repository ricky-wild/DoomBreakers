using System;
using UnityEngine;

namespace wildlogicgames
{
	public class CharacterController2D : RaycastController2D
	{
		[Header("Enable Raycast Render in Editor")]
		[Tooltip("Allows us to render and see what's happening.")]
		public bool _enableRaycastTestDraws;

		public enum CollidedDir
		{
			Below = 0,
			Above = 1,
			Left = 2,
			Right = 3
		}//Order corrisponds to the bool[] _collidedDirection; 
		public struct CollisionDetail
		{
			public bool[] _collidedDirection;
			public int _faceDirection;
			public bool _platformEdge, _ignoreEdgeDetection;
			public void Setup()
			{
				_collidedDirection = new bool[4];
				for (int i = 0; i < _collidedDirection.Length; i++) _collidedDirection[i] = false;
				_faceDirection = 1;
				_platformEdge = false;
			}
			public void Reset()
			{
				for (int i = 0; i < _collidedDirection.Length; i++) _collidedDirection[i] = false;
				_faceDirection = 1;
				_platformEdge = false;
				_ignoreEdgeDetection = false;
			}
			public void SetCollidedDir(CollidedDir collidedDir, bool b) => _collidedDirection[(int)collidedDir] = b;
			public bool GetCollidedDir(CollidedDir collidedDir) => _collidedDirection[(int)collidedDir];
		}

		public CollisionDetail _collisionDetail;
		private Vector2 _movementInputVector2;

		private Vector2 _raycastOriginVector2, _tempVector2;
		private RaycastHit2D _raycastHit2D;
		private float _directionX;
		private float _directionY;
		private float _raycastLength;

		//Use a value that detects a pit drop deep enough that it doesn't interfere with jumping and being hit into sky for enemy AI.
		private const float _pitDropDetectionMax = 400.0f; 
		private const float _maxClimbAngle = 60f;
		private const float _maxDecendAngle = 60f;
		private float _currentSlopeAngle, _distanceToSlopeFrom, _previousSlopeAngle;

		private Transform _transform;

		public CollisionDetail GetCollision() => _collisionDetail;
		public void IgnoreEdgeDetection(bool b) => _collisionDetail._ignoreEdgeDetection = b;

		public override void Start()
		{
			base.Start();
			_collisionDetail.Setup();
			_transform = this.transform;
		}
		public void UpdateMovement(Vector3 velocity, Vector2 inputVector2, bool edgeDetection, bool stoodOnPlatform = false)
		{
			UpdateRaycasts();
			_collisionDetail.Reset();
			_movementInputVector2 = inputVector2;

			if (velocity.x != 0)
				_collisionDetail._faceDirection = (int)Mathf.Sign(velocity.x);

			//if (velocity.y < 0) DescendSlope();
			UpdateHorizontalCollisions(ref velocity);
			UpdateVerticalCollisions(ref velocity, edgeDetection);

			_transform.Translate(velocity);

			if (stoodOnPlatform) _collisionDetail._collidedDirection[0] = true;//_collisionDetail.SetCollidedDir(CollidedDir.Below, true);
		}
		private void UpdateHorizontalCollisions(ref Vector3 velocity)
		{
			_directionX = _collisionDetail._faceDirection;
			_raycastLength = Mathf.Abs(velocity.x) + _boundsBorderExpansion;
			if (Mathf.Abs(velocity.x) < _boundsBorderExpansion) _raycastLength = 2 * _boundsBorderExpansion;

			for (int i = 0; i < _raycastUtility._horizontalCount; i++)
			{

				//_raycastOriginVector2 = (_directionX == -1) ? _raycastPoints._botLeft : _raycastPoints._botRight;
				if (_directionX == -1) _raycastOriginVector2 = _raycastPoints._botLeft;
				if (_directionX == 1) _raycastOriginVector2 = _raycastPoints._botRight;

				_raycastOriginVector2 += Vector2.up * (_raycastUtility._horizontalSpacing * i);

				_raycastHit2D = Physics2D.Raycast(_raycastOriginVector2, 
					(Vector2.right * _directionX), _raycastLength, _layerMask);

				if(_enableRaycastTestDraws) Debug.DrawRay(_raycastOriginVector2, Vector2.right * 
					_directionX * _raycastLength, Color.red);

				if (!_raycastHit2D) break;//return;

				if (_raycastHit2D.distance == 0) continue;

				//Make a check for a slope collision.
				//UpdateSlopeClimbCheck(ref velocity, i);
				_currentSlopeAngle = Vector2.Angle(_raycastHit2D.normal, Vector2.up);

				if(_currentSlopeAngle > _maxClimbAngle)
				{
					velocity.x = (_raycastHit2D.distance - _boundsBorderExpansion) * _directionX;

					//Ensure the first collision is detected and stop at that point of contact.
					_raycastLength = _raycastHit2D.distance;

					if (_directionX == -1) _collisionDetail._collidedDirection[2] = true; //_collisionDetail.SetCollidedDir(CollidedDir.Left, true);
					if (_directionX == 1) _collisionDetail._collidedDirection[3] = true; //_collisionDetail.SetCollidedDir(CollidedDir.Right, true);

				}
			}
		}
		private void UpdateVerticalCollisions(ref Vector3 velocity, bool edgeDetection)
		{
			if (velocity.y == 0) return;

			_directionY = Mathf.Sign(velocity.y); 
			_raycastLength = Mathf.Abs(velocity.y) + _boundsBorderExpansion;

			for (int i = 0; i < _raycastUtility._verticalCount; i++)
			{

				if (_directionY == -1 && _directionX == -1)
				{
					_raycastOriginVector2 = _raycastPoints._botLeft;
					_tempVector2 = Vector2.left;
				}
				if (_directionY == -1 && _directionX == 1)
				{
					_raycastOriginVector2 = _raycastPoints._botRight;
					_tempVector2 = Vector2.right;
				}
				if (_directionY == 1 && _directionX == -1)
				{
					_raycastOriginVector2 = _raycastPoints._topLeft;
					_tempVector2 = Vector2.left;
				}
				if (_directionY == 1 && _directionX == 1)
				{
					_raycastOriginVector2 = _raycastPoints._topRight;
					_tempVector2 = Vector2.right;
				}

				_raycastOriginVector2 += Vector2.right * (_raycastUtility._verticalSpacing * (i + velocity.x));




				if(edgeDetection) DetectPitfall();





				_raycastHit2D = Physics2D.Raycast(_raycastOriginVector2, Vector2.up * _directionY, _raycastLength, _layerMask);

				if (_enableRaycastTestDraws) Debug.DrawRay(_raycastOriginVector2, Vector2.up *
											_directionY * _raycastLength, Color.green);

				if (!_raycastHit2D) break;//return;

				if(_raycastHit2D.collider.tag == "PassPlatform")
				{
					//If moving up, pass through the platform.
					if (_directionY == 1 || _raycastHit2D.distance == 0) continue;

					if (_movementInputVector2.y == -1) continue;// return; //Pass through the platform.
				}

				velocity.y = (_raycastHit2D.distance - _boundsBorderExpansion) * _directionY;

				//Ensure the first collision is detected and stop at that point of contact.
				_raycastLength = _raycastHit2D.distance;

				//collisions.below = directionY == -1;
				//collisions.above = directionY == 1;

				if (_directionY == -1) _collisionDetail._collidedDirection[0] = true; //_collisionDetail.SetCollidedDir(CollidedDir.Below, true);
				if (_directionY == 1) _collisionDetail._collidedDirection[1] = true; //_collisionDetail.SetCollidedDir(CollidedDir.Above, true);
			}
		}
		private void DetectPitfall()
		{
			if (_collisionDetail._ignoreEdgeDetection) return;

			Vector2 tempVector2 = _raycastOriginVector2;

			//if (_directionX == -1) tempVector2.x -= 1.0f;
			//if (_directionX == 1) tempVector2.x += 1.0f;
			//_raycastHit2D = Physics2D.Raycast(tempVector2, Vector2.up * _directionY, _raycastLength*5.5f, _layerMask);


			if (_directionX == -1)
			{
				_raycastHit2D = Physics2D.Raycast(_raycastPoints._botLeft, Vector2.up * _directionY, _raycastLength * _pitDropDetectionMax, _layerMask);
				tempVector2 = _raycastPoints._botLeft;
			}
			if (_directionX == 1)
			{
				_raycastHit2D = Physics2D.Raycast(_raycastPoints._botRight, Vector2.up * _directionY, _raycastLength * _pitDropDetectionMax, _layerMask);
				tempVector2 = _raycastPoints._botRight;
			}

			if (_enableRaycastTestDraws) Debug.DrawRay(tempVector2, Vector2.up *
											_directionY * _raycastLength * _pitDropDetectionMax, Color.green);

			if (!_raycastHit2D)
			{
				if (_directionY == -1) //Bottom left no raycast hit detected.
				{
					if (_raycastHit2D.collider == null) 
						_collisionDetail._platformEdge = true;

					//if (_collisionDetail._collidedDirection[0]) //AND we're on the ground.
					//{
					//	if (_raycastHit2D.collider == null) _collisionDetail._platformEdge = true;
					//}
				}
			}
		}
		private void UpdateSlopeClimbCheck(ref Vector3 velocity, int i)
		{		
			if (i == 0 && _currentSlopeAngle <= _maxClimbAngle)
			{
				_distanceToSlopeFrom = 0f;

				if (_currentSlopeAngle != _previousSlopeAngle)
				{
					_distanceToSlopeFrom = _raycastHit2D.distance - _boundsBorderExpansion;

					//Subtract distance from initial slope collision, from velocityX.
					velocity.x -= _distanceToSlopeFrom * _directionX;
				}

				//ClimbSlope(ref velocity, slopeAngle);

				////We also want to add that distance back on once finished
				////climbing the slope.
				//velocity.x += distToSlopeStarta * directionX;
			}
		}
	}
}
