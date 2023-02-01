using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

namespace DoomBreakers
{
	[RequireComponent(typeof(MeshRenderer))]
	public class MeshTextureController : MonoBehaviour
	{
		[Header("Texture Setup ID")]
		[Tooltip("This ID determines texture co-od if auto apply.")]
		public int _textureID;

		public bool _autoTexValuesApply;

		[Header("Texture Coordinates")]
		[Tooltip("The values that we set within the material shader")]
		public float _tiling_x;
		public float _tiling_y;
		public float _offset_x;
		public float _offset_y;

		[Header("MeshRenderer Component")]
		[Tooltip("Ensure we have the reference here.")]
		public MeshRenderer _meshRenderer;
		private Renderer _renderer;
		private Material _tempMaterial;

		private ITimer _updateTimer;
		private const float _renderUpdateWaitTime = 1.0f;

		private Transform _t;
		private Vector3 _v;
		private bool _deactivatedFlag, _activatedFlag;

		//public MeshTextureController() => Setup();
		private void Awake() => Setup();
		private void Setup()
		{
			if(_meshRenderer == null) _meshRenderer = this.GetComponent<MeshRenderer>();
			_meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;

			if(_autoTexValuesApply) PopulateTexValues(_textureID);

			//Now apply Texture Settings.
			if (this.GetComponent<Renderer>() == null)
			{
				print("\n<Renderer>() = null. Unable to apply texture settings.");
				return;
			}
			if (this.GetComponent<Renderer>().sharedMaterial == null)
			{
				print("\n<Renderer>().sharedMaterial = null. Unable to apply texture settings.");
				return;
			}

			_updateTimer = new Timer();
			_updateTimer.StartTimer(_renderUpdateWaitTime);


			//if(!Application.isEditor) return;
			_renderer = this.GetComponent<Renderer>();
			_tempMaterial = new Material(_renderer.sharedMaterial);
			_renderer.sharedMaterial = _tempMaterial;

			_renderer.sharedMaterial.mainTextureOffset = new Vector2(_offset_x, _offset_y);
			_renderer.sharedMaterial.mainTextureScale = new Vector2(_tiling_x, _tiling_y);

			_t = this.transform;
			_v = _t.position;
			_deactivatedFlag = false;
			_activatedFlag = false;
		}

		private void PopulateTexValues(int index)
		{
			switch (index)
			{
				//Main forest floor, connector building block.
				//Scale.x = 5.958823
				//Scale.y = 5.84
				case 0:
					_tiling_x = 0.1886f;
					_tiling_y = 0.189f;
					_offset_x = 0.0f;
					_offset_y = 0.81f;
					break;
				//Main forest floor, standalone building block.
				case 1:
					_tiling_x = 0.1886f;
					_tiling_y = 0.189f;
					_offset_x = 0.189f;
					_offset_y = 0.81f;
					break;
				//Main ruin stone forest floor, connector building block.
				case 2:
					_tiling_x = 0.1886f;
					_tiling_y = 0.189f;
					_offset_x = 0.378f;
					_offset_y = 0.81f;
					break;
				//Main ruin stone forest floor, standalone building block.
				case 3:
					_tiling_x = 0.1886f;
					_tiling_y = 0.189f;
					_offset_x = 0.566f;
					_offset_y = 0.81f;
					break;
				//Main forest floor, floating building block.
				case 4:
					_tiling_x = 0.0955f;
					_tiling_y = 0.06f;
					_offset_x = 0f;
					_offset_y = 0.751f;
					break;
				//Main ruin stone wall A.
				case 5:
					_tiling_x = 0.0955f;
					_tiling_y = 0.099f;
					_offset_x = 0.754f;
					_offset_y = 0.903f;
					break;
				//Main ruin stone wall B.
				case 6:
					_tiling_x = 0.0955f;
					_tiling_y = 0.099f;
					_offset_x = 0.849f;
					_offset_y = 0.903f;
					break;
				//Main ruin stone wall C.
				case 7:
					_tiling_x = 0.0955f;
					_tiling_y = 0.032f;
					_offset_x = 0.849f;
					_offset_y = 0.872f;
					break;
				//Main ruin stone wall D.
				case 8:
					_tiling_x = 0.031f;
					_tiling_y = 0.099f;
					_offset_x = 0.378f;
					_offset_y = 0.81f;
					break;
				//Main ruin stone floating building block..
				case 9:
					_tiling_x = 0.0955f;
					_tiling_y = 0.06f;
					_offset_x = 0.19f;
					_offset_y = 0.718f;
					break;
				//Main ruin stone floating building block..
				case 10:
					_tiling_x = 0.0955f;
					_tiling_y = 0.032f;
					_offset_x = 0.19f;
					_offset_y = 0.7795f;
					break;
				//Main ruin stone wall with window.
				case 11:
					_tiling_x = 0.0955f;
					_tiling_y = 0.099f;
					_offset_x = 0.849f;
					_offset_y = 0.903f;
					break;
				//Main Lookout Tower wall with window.
				case 12:
					_tiling_x = 0.1294f;
					_tiling_y = 0.105f;
					_offset_x = 0.2855f;
					_offset_y = 0.705f;
					break;
				//Main Lookout Tower top.
				case 13:
					_tiling_x = 0.129f;
					_tiling_y = 0.105f;
					_offset_x = 0.4135f;
					_offset_y = 0.705f;
					break;
				//Main Lookout Tower wall without window.
				case 14:
					_tiling_x = 0.131f;
					_tiling_y = 0.105f;
					_offset_x = 0.542f;
					_offset_y = 0.705f;
					break;
				//Main statue carry the weight.
				case 15:
					_tiling_x = 0.09f;
					_tiling_y = 0.105f;
					_offset_x = 0.67f;
					_offset_y = 0.705f;
					break;
				//Main statue soldier sword&shield.
				case 16:
					_tiling_x = 0.08f;
					_tiling_y = 0.105f;
					_offset_x = 0.754f;
					_offset_y = 0.705f;
					break;
				//Main ruin stone wall with window 2.
				case 17:
					_tiling_x = 0.0955f;
					_tiling_y = 0.095f;
					_offset_x = 0.754f;
					_offset_y = 0.81f;
					break;
				//Main statue kings throne.
				case 18:
					_tiling_x = 0.09f;
					_tiling_y = 0.105f;
					_offset_x = 0.84f;
					_offset_y = 0.705f;
					break;
				//Main ruin stone wall E.
				case 19:
					_tiling_x = 0.108f;
					_tiling_y = 0.042f;
					_offset_x = 0.849f;
					_offset_y = 0.83f;
					break;
				//Main ruin stone wall F.
				case 20:
					_tiling_x = 0.028f;
					_tiling_y = 0.042f;
					_offset_x = 0.956f;
					_offset_y = 0.83f;
					break;
			}
		}

		private void UpdateEditorMaterial()
		{
			if (!Application.isEditor) return;
			if (_updateTimer == null) return;

			if(_updateTimer.HasTimerFinished())
			{
				_renderer.sharedMaterial.mainTextureOffset = new Vector2(_offset_x, _offset_y);
				_renderer.sharedMaterial.mainTextureScale = new Vector2(_tiling_x, _tiling_y);

				_updateTimer.Reset();
				_updateTimer.StartTimer(_renderUpdateWaitTime);
			}


		}

		void Update() => UpdateEditorMaterial();



		private bool IsOnScreen()
		{
			_v = ProCamera2D.Instance.GameCamera.WorldToViewportPoint(_t.position);

			//if(_v.x > 0 && _v.x < 1 && _v.y > 0 && _v.y < 1)
			if (_v.x > -1 && _v.x < 1.5f && _v.y > -1 && _v.y < 1.5f)
			{
				return true;
			}


			return false;

		}

	}
}
