using System;
using UnityEngine;


namespace wildlogicgames
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class RaycastController2D : MonoBehaviour
	{
		public struct RaycastPoints
		{
			public Vector2 _topLeft, _topRight, _botLeft, _botRight;
		}
		public struct RaycastUtility
		{
			public int _horizontalCount, _verticalCount;
			public float _horizontalSpacing, _verticalSpacing;
		}

		[Header("LayerMask to Collide With")]
		public LayerMask _layerMask;
		protected BoxCollider2D _boxCollider2D;
		protected Bounds _cachedBounds;
		protected RaycastUtility _raycastUtility;
		protected RaycastPoints _raycastPoints;
		protected Vector2 _cachedVector2;

		protected const float _boundsBorderExpansion = 0.05f;//0.015f;

		public virtual void Start() => Setup();
		private void Setup()
		{
			_boxCollider2D = GetComponent<BoxCollider2D>();
			_raycastUtility._horizontalCount = 4;
			_raycastUtility._verticalCount = 4;

			//Get the bounds of the 2d box collider.
			_cachedBounds = _boxCollider2D.bounds;
			_cachedBounds.Expand(_boundsBorderExpansion * -2);

			//Set the raycast spacing based on the bounds & initial var settings.
			_raycastUtility._horizontalSpacing = _cachedBounds.size.x / (_raycastUtility._horizontalCount - 1);
			_raycastUtility._verticalSpacing = _cachedBounds.size.y  / (_raycastUtility._verticalCount - 1);

			_cachedVector2 = new Vector2();
		}
		protected void UpdateRaycasts()
		{
			_cachedBounds = _boxCollider2D.bounds;
			_cachedBounds.Expand(_boundsBorderExpansion * -2);

			_raycastPoints._botLeft = PluginBoundsVector(_cachedBounds.min.x, _cachedBounds.min.y);
			_raycastPoints._botRight = PluginBoundsVector(_cachedBounds.max.x, _cachedBounds.min.y);

			_raycastPoints._topLeft = PluginBoundsVector(_cachedBounds.min.x, _cachedBounds.max.y);
			_raycastPoints._topRight = PluginBoundsVector(_cachedBounds.max.x, _cachedBounds.max.y);

		}

		private Vector2 PluginBoundsVector(float x, float y)
		{
			_cachedVector2.x = x;
			_cachedVector2.y = y;
			return _cachedVector2;
		}
	}
}
