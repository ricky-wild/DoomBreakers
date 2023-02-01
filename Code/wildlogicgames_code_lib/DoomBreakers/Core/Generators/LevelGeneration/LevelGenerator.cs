using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public enum PlatformID //Must corrispond to GameObject[] _prefabPlatformObjects index assignment order
    {
        Platform_Forest_Ground_Earth = 0,
        Platform_Forest_Ground_Stone = 1,
        Platform_Forest_Bridge_Stone = 2,
        Platform_Forest_Start = 3,
        Platform_Forest_End = 4,
        Platform_Forest_Campsite = 5,
    };
    public enum PitfallSizeID
	{
        Pitfall_Size_Sml = 0,
        Pitfall_Size_Med = 1,
        Pitfall_Size_Lrg = 2
    }
    public enum StepSizeID
    {
        Step_Size_Sml = 3, //we use these values to divide.
        Step_Size_Med = 2,
        Step_Size_Lrg = 1
    }
    public enum PlatformGenFormID
	{
        Form_FlatGround = 0,
        Form_FlatGroundWithGaps = 1,
        Form_CurveUp = 2,
        Form_CurveDown = 3,
        Form_StepsUp = 4,
        Form_StepsDown = 5
	}

    public enum TreeID //Must corrispond to GameObject[] _prefabTreeObjects index assignment order
    {
        Tree_Oak_Sml = 0,
        Tree_Oak_Med = 1,
        Tree_Oak_Lrg = 2,
        Tree_Pear_Sml = 3,
    };
    public enum GrassID //Must corrispond to GameObject[] _grassTreeObjects index assignment order
    {
        Grass_A = 0,
    };
    public enum WallID
	{
        Wall_A = 0,
	};
    public enum HillsideID
    {
        Hillside_A = 0,
        Hillside_B = 1,
    };
    public enum BackgroundID
	{
        Background_Forest = 0,
	};
    public enum TorchID
    {
        Torch_Forest = 0,
    };
    public enum CanopyID
    {
        Canopy_Forest_Vines = 0,
    };

    public class LevelGenerator : MonoBehaviour
    {

        public struct PlatformGenData
		{
            public bool newGenChain;
            public float PREV_HEIGHT;
            public Vector3 POS, NEXT_POS;
            public Vector2 groundObjBounds, bridgeObjBounds;
            public PlatformID previousPlatformID;
            public PlatformGenFormID currentPlatformFormID, previousPlatformFormID;
            public bool isBridgeConnection;
            public int BlockIncrement;

            public Vector3 PREV_GRASS_POS;
            public void Reset()
			{
                newGenChain = false;
                PREV_HEIGHT = 0f;
                POS = Vector3.zero;
                NEXT_POS = Vector3.zero;
                groundObjBounds = Vector2.zero;
                bridgeObjBounds = Vector2.zero;
                PREV_GRASS_POS = Vector3.zero;
                isBridgeConnection = false;
                BlockIncrement = 0;
			}
            public float GetHeightDiff(float heightA, float heightB) => Math.Abs(heightA - heightB); //Math.Abs() get diff regardless of which is larger.

        }
        

        private Transform _transform;

        [Header("Prefab Platforms to Generate")]
        [Tooltip("These must be Platform Prefabs following enum PlatformID order.")]
        public GameObject[] _prefabPlatformObjects;

        private int _maxPlatformObjs = 200;
        private int _platformChunkCount = 5;
        private StepSizeID _stepSize;
        private PitfallSizeID _gapSize;
        public PlatformGenData _platGenData;

        public Dictionary<PlatformID, List<GameObject>> _platformObjectsDict;


        [Header("Prefab Trees to Generate")]
        [Tooltip("These must be Tree Prefabs following enum TreeID order.")]
        public GameObject[] _prefabTreeObjects;
        private int _maxTreeObjs = 300;
        private int _percetageOfNoTreeSpawn = 35;
        private bool _includeTrees;
        private TreeID _treeTypeToSpawn;
        public Dictionary<TreeID, List<GameObject>> _treeObjectsDict;


        [Header("Prefab Grass to Generate")]
        [Tooltip("These must be Grass Prefabs following enum GrassID order.")]
        public GameObject[] _prefabGrassObjects;
        private int _maxGrassObjs = 600;
        private int _percetageOfNoGrassSpawn = 45;
        private bool _includeGrass;
        public Dictionary<GrassID, List<GameObject>> _grassObjectsDict;


        [Header("Prefab Wall to Generate")]
        [Tooltip("These must be Grass Prefabs following enum GrassID order.")]
        public GameObject[] _prefabWallObjects;
        private int _maxWallObjs = 350;
        private int _percetageOfNoWallSpawn = 35;
        private bool _includeWall;
        public Dictionary<WallID, List<GameObject>> _wallObjectsDict;


        [Header("Prefab Hillside to Generate")]
        [Tooltip("These must be Grass Prefabs following enum GrassID order.")]
        public GameObject[] _prefabHillsideObjects;
        private int _maxHillsideObjs = 250;
        private int _percetageOfNoHillsideSpawn = 20;
        private bool _includeHillside;
        public Dictionary<HillsideID, List<GameObject>> _hillSideObjectsDict;


        [Header("Prefab Backgrounds to Generate")]
        [Tooltip("These must be Background Prefabs following enum BackgroundID order.")]
        public GameObject[] _prefabBackgroundObjects;
        private int _maxBkgdObjs = 20;
        private int _percetageOfNoBkgdSpawn = 20;
        private bool _includeBkgd;
        public Dictionary<BackgroundID, List<GameObject>> _bkgdObjectsDict;


        [Header("Prefab Torch to Generate")]
        [Tooltip("These must be Torch Prefabs following enum TorchID order.")]
        public GameObject[] _prefabTorchObjects;
        private int _maxTorchObjs = 200;
        private int _percetageOfNoTorchSpawn = 20;
        private bool _includeTorch;
        public Dictionary<TorchID, List<GameObject>> _torchObjectsDict;


        [Header("Prefab Canopy to Generate")]
        [Tooltip("These must be Canopy Prefabs following enum TorchID order.")]
        public GameObject[] _prefabCanopyObjects;
        private int _maxCanopyObjs = 200;
        private int _percetageOfNoCanopySpawn = 20;
        private bool _includeCanopy;
        public Dictionary<CanopyID, List<GameObject>> _canopyObjectsDict;




        private Dictionary<int, Vector3> _playerItemSpawnPosDict;
        private int _playerItemSpawnIndex = 0;
        public Dictionary<int, Vector3> GetPlayerItemSpawnDict() => _playerItemSpawnPosDict;

        private Dictionary<int, Vector3> _enemySpawnPosDict;
        private int _enemySpawnIndex = 0;
        public Dictionary<int, Vector3> GetEnemySpawnDict() => _enemySpawnPosDict;

        private void Setup()
		{
            _transform = this.transform;

            if (_prefabPlatformObjects == null) return;

            if (_platformObjectsDict == null) _platformObjectsDict = new Dictionary<PlatformID, List<GameObject>>();
            if (_treeObjectsDict == null) _treeObjectsDict = new Dictionary<TreeID, List<GameObject>>();
            if (_grassObjectsDict == null) _grassObjectsDict = new Dictionary<GrassID, List<GameObject>>();
            if (_wallObjectsDict == null) _wallObjectsDict = new Dictionary<WallID, List<GameObject>>();
            if (_hillSideObjectsDict == null) _hillSideObjectsDict = new Dictionary<HillsideID, List<GameObject>>();
            if (_bkgdObjectsDict == null) _bkgdObjectsDict = new Dictionary<BackgroundID, List<GameObject>>();
            if (_torchObjectsDict == null) _torchObjectsDict = new Dictionary<TorchID, List<GameObject>>();
            if (_canopyObjectsDict == null) _canopyObjectsDict = new Dictionary<CanopyID, List<GameObject>>();

            PopulatePlatformLists();
            PopulateTreeLists();
            PopulateGrassLists();
            PopulateWallLists();
            PopulateHillsideLists();
            PopulateBackgroundLists();
            PopulateTorchLists();
            PopulateCanopyLists();

            if (_playerItemSpawnPosDict == null) _playerItemSpawnPosDict = new Dictionary<int, Vector3>();
            if (_enemySpawnPosDict == null) _enemySpawnPosDict = new Dictionary<int, Vector3>();

            InitializeGeneration();

        }
        private void PopulateCanopyLists()
        {
            _canopyObjectsDict.Add(CanopyID.Canopy_Forest_Vines, new List<GameObject>());

            for (int i = 0; i < _maxCanopyObjs; i++)
            {
                AddCanopyObject(CanopyID.Canopy_Forest_Vines);
            }
        }
        private void AddCanopyObject(CanopyID canopyID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabCanopyObjects[(int)canopyID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_canopyObjectsDict.TryGetValue(canopyID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetCanopyObject(CanopyID canopyID)
        {
            List<GameObject> list;
            for (int i = 0; i < _canopyObjectsDict.Count; i++)
            {
                list = _canopyObjectsDict[canopyID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_canopyObjectsDict[canopyID][j].activeInHierarchy)
                    {
                        return _canopyObjectsDict[canopyID][j];
                    }
                }

            }

            return null;
        }
        private float GetCanopyObjectWidth(CanopyID canopyID, GameObject canopyObj)
        {
            if (canopyObj == null) return 0f;
            if (canopyObj.GetComponent<SpriteRenderer>() == null) return 0f;


            return canopyObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            //return treeObj.transform.localScale.x;
        }
        private float GetCanopyObjecHeight(CanopyID canopyID, GameObject canopyObj)
        {
            if (canopyObj == null) return 0f;
            if (canopyObj.GetComponent<SpriteRenderer>() == null) return 0f;


            return canopyObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            //return treeObj.transform.localScale.y;
        }






        private void PopulateTorchLists()
        {
            _torchObjectsDict.Add(TorchID.Torch_Forest, new List<GameObject>());

            for (int i = 0; i < _maxTorchObjs; i++)
            {
                AddTorchObject(TorchID.Torch_Forest);
            }
        }
        private void AddTorchObject(TorchID torchID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabTorchObjects[(int)torchID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_torchObjectsDict.TryGetValue(torchID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetTorchObject(TorchID torchID)
        {
            List<GameObject> list;
            for (int i = 0; i < _torchObjectsDict.Count; i++)
            {
                list = _torchObjectsDict[torchID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_torchObjectsDict[torchID][j].activeInHierarchy)
                    {
                        return _torchObjectsDict[torchID][j];
                    }
                }

            }

            return null;
        }
        private float GetTorchObjectWidth(TorchID torchID, GameObject torchObj)
        {
            if (torchObj == null) return 0f;
            //if (torchObj.GetComponent<SpriteRenderer>() == null) return 0f;

            float widthFromChild = 0;
            for (int k = 0; k < torchObj.transform.childCount; k++) //larger trees
            {
                GameObject gameObject = torchObj.transform.GetChild(k).gameObject;
                if (gameObject.GetComponent<SpriteRenderer>() == null) return 0f;

                widthFromChild = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            }

            return widthFromChild;
            //return treeObj.transform.localScale.x;
        }
        private float GetTorchObjecHeight(TorchID torchID, GameObject torchObj)
        {
            if (torchObj == null) return 0f;
            //if (torchObj.GetComponent<SpriteRenderer>() == null) return 0f;

            float heightFromChild = 0;
            for (int k = 0; k < torchObj.transform.childCount; k++) //larger trees
            {
                GameObject gameObject = torchObj.transform.GetChild(k).gameObject;
                if (gameObject.GetComponent<SpriteRenderer>() == null) return 0f;

                heightFromChild += gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            }

            return heightFromChild;
            //return treeObj.transform.localScale.x;
        }






        private void PopulateBackgroundLists()
        {
            _bkgdObjectsDict.Add(BackgroundID.Background_Forest, new List<GameObject>());

            for (int i = 0; i < _maxBkgdObjs; i++)
            {
                AddBackgroundObject(BackgroundID.Background_Forest);
            }
        }
        private void AddBackgroundObject(BackgroundID bkgdID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabBackgroundObjects[(int)bkgdID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_bkgdObjectsDict.TryGetValue(bkgdID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetBackgroundObject(BackgroundID bkgdID)
        {
            List<GameObject> list;
            for (int i = 0; i < _bkgdObjectsDict.Count; i++)
            {
                list = _bkgdObjectsDict[bkgdID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_bkgdObjectsDict[bkgdID][j].activeInHierarchy)
                    {
                        return _bkgdObjectsDict[bkgdID][j];
                    }
                }

            }

            return null;
        }
        private float GetBackgroundObjectWidth(BackgroundID bkgdID, GameObject bkgdObj)
        {
            if (bkgdObj == null) return 0f;
            if (bkgdObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return bkgdObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            //return treeObj.transform.localScale.x;
        }
        private float GetBackgroundObjectHeight(BackgroundID bkgdID, GameObject bkgdObj)
        {
            if (bkgdObj == null) return 0f;
            if (bkgdObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return bkgdObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }






        private void PopulateHillsideLists()
        {
            _hillSideObjectsDict.Add(HillsideID.Hillside_A, new List<GameObject>());
            _hillSideObjectsDict.Add(HillsideID.Hillside_B, new List<GameObject>());

            for (int i = 0; i < _maxHillsideObjs; i++)
            {
                AddHillsideObject(HillsideID.Hillside_A);
                AddHillsideObject(HillsideID.Hillside_B);
            }
        }
        private void AddHillsideObject(HillsideID hillSideID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabHillsideObjects[(int)hillSideID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_hillSideObjectsDict.TryGetValue(hillSideID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetHillsideObject(HillsideID hillSideID)
        {
            List<GameObject> list;
            for (int i = 0; i < _hillSideObjectsDict.Count; i++)
            {
                list = _hillSideObjectsDict[hillSideID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_hillSideObjectsDict[hillSideID][j].activeInHierarchy)
                    {
                        return _hillSideObjectsDict[hillSideID][j];
                    }
                }

            }

            return null;
        }
        private float GetHillsideObjectWidth(HillsideID hillSideID, GameObject hillSidObj)
        {
            if (hillSidObj == null) return 0f;
            if (hillSidObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return hillSidObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            //return treeObj.transform.localScale.x;
        }
        private float GetHillsideObjectHeight(HillsideID hillSideID, GameObject hillSidObj)
        {
            if (hillSidObj == null) return 0f;
            if (hillSidObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return hillSidObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }






        private void PopulateWallLists()
        {
            _wallObjectsDict.Add(WallID.Wall_A, new List<GameObject>());

            for (int i = 0; i < _maxWallObjs; i++)
            {
                AddWallObject(WallID.Wall_A);
            }
        }
        private void AddWallObject(WallID wallID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabWallObjects[(int)wallID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_wallObjectsDict.TryGetValue(wallID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetWallObject(WallID wallID)
        {
            List<GameObject> list;
            for (int i = 0; i < _wallObjectsDict.Count; i++)
            {
                list = _wallObjectsDict[wallID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_wallObjectsDict[wallID][j].activeInHierarchy)
                    {
                        return _wallObjectsDict[wallID][j];
                    }
                }

            }

            return null;
        }
        private float GetWallObjectWidth(WallID wallID, GameObject wallObj)
        {
            if (wallObj == null) return 0f;
            if (wallObj.GetComponent<MeshRenderer>() == null) return 0f;

            return wallObj.GetComponent<MeshRenderer>().bounds.size.x;
            //return treeObj.transform.localScale.x;
        }
        private float GetWallObjectHeight(WallID wallID, GameObject wallObj)
        {
            if (wallObj == null) return 0f;
            if (wallObj.GetComponent<MeshRenderer>() == null) return 0f;

            return wallObj.GetComponent<MeshRenderer>().bounds.size.y;
        }





        private void PopulateGrassLists()
        {
            _grassObjectsDict.Add(GrassID.Grass_A, new List<GameObject>());

            for (int i = 0; i < _maxGrassObjs; i++)
            {
                AddGrassObject(GrassID.Grass_A);
            }
        }
        private void AddGrassObject(GrassID grassID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabGrassObjects[(int)grassID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_grassObjectsDict.TryGetValue(grassID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetGrassObject(GrassID grassID)
        {
            List<GameObject> list;
            for (int i = 0; i < _grassObjectsDict.Count; i++)
            {
                list = _grassObjectsDict[grassID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_grassObjectsDict[grassID][j].activeInHierarchy)
                    {
                        return _grassObjectsDict[grassID][j];
                    }
                }

            }

            return null;
        }
        private float GetGrassObjectWidth(GrassID grassID, GameObject grassObj)
        {
            if (grassObj == null) return 0f;
            if (grassObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return grassObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            //return treeObj.transform.localScale.x;
        }
        private float GetGrassObjectHeight(GrassID grassID, GameObject grassObj)
        {
            if (grassObj == null) return 0f;
            if (grassObj.GetComponent<SpriteRenderer>() == null) return 0f;

            return grassObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }






        private void PopulateTreeLists()
		{
            _treeObjectsDict.Add(TreeID.Tree_Oak_Sml, new List<GameObject>());
            _treeObjectsDict.Add(TreeID.Tree_Oak_Med, new List<GameObject>());
            _treeObjectsDict.Add(TreeID.Tree_Oak_Lrg, new List<GameObject>());
            _treeObjectsDict.Add(TreeID.Tree_Pear_Sml, new List<GameObject>());

            for (int i = 0; i < _maxTreeObjs; i++)
			{
                AddTreeObject(TreeID.Tree_Oak_Sml);
                AddTreeObject(TreeID.Tree_Oak_Med);
                AddTreeObject(TreeID.Tree_Oak_Lrg);
                AddTreeObject(TreeID.Tree_Pear_Sml);
            }
		}
        private void AddTreeObject(TreeID treeID)
        {
            GameObject newGameObject = (GameObject)Instantiate(_prefabTreeObjects[(int)treeID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_treeObjectsDict.TryGetValue(treeID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

        }
        private GameObject GetTreeObject(TreeID treeID)
        {
            List<GameObject> list;
            for (int i = 0; i < _treeObjectsDict.Count; i++)
            {
                list = _treeObjectsDict[treeID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_treeObjectsDict[treeID][j].activeInHierarchy)
                    {
                        return _treeObjectsDict[treeID][j];
                    }
                }

            }

            return null;
        }
        private float GetTreeObjectWidth(TreeID treeID, GameObject treeObj)
        {
            if (treeObj == null) return 0f;
            if (treeObj.GetComponent<SpriteRenderer>() == null && _treeTypeToSpawn == TreeID.Tree_Oak_Sml) return 0f;

            float widthFromChild = 0;
            for (int k = 0; k < treeObj.transform.childCount; k++) //larger trees
            {
                GameObject gameObject = treeObj.transform.GetChild(k).gameObject;
                if (gameObject.GetComponent<SpriteRenderer>() == null) return 0f;

                widthFromChild = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            }

            if (_treeTypeToSpawn == TreeID.Tree_Oak_Sml) return treeObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            if (_treeTypeToSpawn == TreeID.Tree_Pear_Sml) return treeObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            return widthFromChild;
            //return treeObj.transform.localScale.x;
        }
        private float GetTreeObjectHeight(TreeID treeID, GameObject treeObj)
        {
            if (treeObj == null) return 0f;

            if (treeObj.GetComponent<SpriteRenderer>() == null && _treeTypeToSpawn == TreeID.Tree_Oak_Sml) return 0f;

            float largerTreeHeightStack = 0;
            for (int k = 0; k < treeObj.transform.childCount; k++) //larger trees
            {
                GameObject gameObject = treeObj.transform.GetChild(k).gameObject;
                if (gameObject.GetComponent<SpriteRenderer>() == null) return 0f;

                largerTreeHeightStack += gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            }

            if (_treeTypeToSpawn == TreeID.Tree_Oak_Sml) return treeObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            if (_treeTypeToSpawn == TreeID.Tree_Pear_Sml) return treeObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

            return largerTreeHeightStack;

        }







        private void InitializePlatformBounds()
        {
 
        }
        private void PopulatePlatformLists()
		{
            _platGenData.POS = _transform.position;

            _platformObjectsDict.Add(PlatformID.Platform_Forest_Ground_Earth, new List<GameObject>());
            _platformObjectsDict.Add(PlatformID.Platform_Forest_Ground_Stone, new List<GameObject>());
            _platformObjectsDict.Add(PlatformID.Platform_Forest_Bridge_Stone, new List<GameObject>());
            _platformObjectsDict.Add(PlatformID.Platform_Forest_Start, new List<GameObject>());
            _platformObjectsDict.Add(PlatformID.Platform_Forest_Campsite, new List<GameObject>());
            _platformObjectsDict.Add(PlatformID.Platform_Forest_End, new List<GameObject>());

            for (int i = 0; i < _maxPlatformObjs; i++)
            {
                AddPlatformObject(PlatformID.Platform_Forest_Ground_Earth);
                AddPlatformObject(PlatformID.Platform_Forest_Ground_Stone);
                AddPlatformObject(PlatformID.Platform_Forest_Bridge_Stone);
                if (i == 0) AddPlatformObject(PlatformID.Platform_Forest_Start);
                if (i == 0) AddPlatformObject(PlatformID.Platform_Forest_Campsite);
                if (i == 0) AddPlatformObject(PlatformID.Platform_Forest_End);
            }

            InitializePlatformBounds();
        }
        private void AddPlatformObject(PlatformID platformID)
		{
            GameObject newGameObject = (GameObject)Instantiate(_prefabPlatformObjects[(int)platformID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            if (_platformObjectsDict.TryGetValue(platformID, out List<GameObject> listPlatforms)) listPlatforms.Add(newGameObject);

            
            Vector2 platformColliderWidth = newGameObject.GetComponent<Collider2D>().bounds.extents;

        }
        private GameObject GetPlatformObject(PlatformID platformID)
        {
            List<GameObject> list;
            for (int i = 0; i < _platformObjectsDict.Count; i++)
            {
                list = _platformObjectsDict[platformID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_platformObjectsDict[platformID][j].activeInHierarchy)
                    {
                        return _platformObjectsDict[platformID][j];
                    }
                }

            }

            return null;
        }
        private float GetPlatformObjectWidth(PlatformID platformID, GameObject platformObj)
		{
            //GameObject platformObj = GetPlatformObject(platformID);
            if (platformObj == null) return 0f;
            if (platformObj.GetComponent<Collider2D>() == null) return 0f;

            Vector2 vector2 = platformObj.GetComponent<Collider2D>().bounds.extents;

            float textureTransparantEdgeWidth = 0.025f;

            //Extent returns half the width of the collider.
            return (vector2.x * 2)- textureTransparantEdgeWidth;
        }
        private float GetPlatformObjectHeight(PlatformID platformID, GameObject platformObj)
		{
            if (platformObj == null) return 0f;
            if (platformObj.GetComponent<MeshRenderer>() == null) return 0f;

            return platformObj.transform.localScale.y;

		}



        private void InitializeGeneration()
		{
            _stepSize = StepSizeID.Step_Size_Med;
            _gapSize = PitfallSizeID.Pitfall_Size_Med;
            _platformChunkCount = 3;
            _includeTrees = true;
            _treeTypeToSpawn = TreeID.Tree_Pear_Sml;
            _includeGrass = true;
            _includeWall = true;
            _includeHillside = true;
            _includeBkgd = true;
            _includeTorch = true;
            _includeCanopy = true;




			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
			//GenerateStart();
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGroundWithGaps);
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			//GenerateCheckpoint();
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
			//Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			//GenerateEnd();





			//Testing forms 23 total so far.
			//_stepSize = StepSizeID.Step_Size_Med;
			//_gapSize = PitfallSizeID.Pitfall_Size_Med;
			//_platformChunkCount = 3;
			//_includeTrees = true;
			//_includeGrass = true;
			//0-5 good
			//5-10 good
			//10 - 15 good
			//15 - 20 good
			//20 -23 good
			//for (int i = 0; i < 23; i++)
			//    GenerateCluster(i);




			//Test not bad generation.
			//_platformChunkCount = 3;
			//for (int i = 0; i < 13; i++) GenerateCluster(i);

			////Fairly good generation.
			//_platformChunkCount = 4;
			//int j = 0;
			//for (int i = 0; i < 35; i++)
			//{
			//	j = wildlogicgames.Utilities.GetRandomNumberInt(0, 35);
			//	GenerateCluster(j);
			//}


			////GOOD
			//_platformChunkCount = 5;
			//int j = 0;
			//int iterations = 35;
			//for (int i = 0; i < iterations; i++)
			//{
			//	j = wildlogicgames.Utilities.GetRandomNumberInt(0, iterations);
			//	GenerateCluster(j);
			//	_platformChunkCount = wildlogicgames.Utilities.GetRandomNumberInt(3, 6);
			//}



			//BEST SO FAR
			_platformChunkCount = 5;
			int j = 0;
			int iterations = 20;//35;
			int decision = 0;
			_treeTypeToSpawn = TreeID.Tree_Pear_Sml;
			Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
			GenerateStart();
			Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			for (int i = 0; i < iterations; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, iterations);
				GenerateCluster(j);
				_platformChunkCount = wildlogicgames.Utilities.GetRandomNumberInt(3, 6);

				decision = wildlogicgames.Utilities.GetRandomNumberInt(0, 110);
				if (decision >= 0 && decision < 33) _treeTypeToSpawn = TreeID.Tree_Oak_Sml;
				if (decision >= 33 && decision < 66) _treeTypeToSpawn = TreeID.Tree_Pear_Sml;
				if (decision >= 66 && decision <= 110) _treeTypeToSpawn = TreeID.Tree_Oak_Med;


				if (i == iterations / 2)
				{
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
					GenerateCheckpoint();
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
				}
			}
			Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
			GenerateEnd();
			//_treeTypeToSpawn

			////INTERESTING.
			//_platformChunkCount = 3;
			//int j = 0;
			//for (int i = 0; i < 35; i++)
			//{
			//    _platformChunkCount = wildlogicgames.Utilities.GetRandomNumberInt(3, 4);
			//    j = wildlogicgames.Utilities.GetRandomNumberInt(0, 35);
			//    if (i > 10)
			//    {
			//        _stepSize = StepSizeID.Step_Size_Sml;
			//        _platformChunkCount = wildlogicgames.Utilities.GetRandomNumberInt(2, 5);
			//        j = wildlogicgames.Utilities.GetRandomNumberInt(0, 35);
			//    }
			//    GenerateCluster(j);
			//}


			//Fairly good generation.
			//int j = 0;
			//for (int i = 0; i < 13; i++)
			//{
			//	_platformChunkCount = wildlogicgames.Utilities.GetRandomNumberInt(2, 6);
			//	j = wildlogicgames.Utilities.GetRandomNumberInt(0, 13);
			//	GenerateCluster(j);
			//}





			//for (int i = 0; i < 3; i++) GenerateRandomDownHill();
			//for (int i = 0; i < 3; i++) GenerateRandomLevelGround();
			//for (int i = 0; i < 3; i++) GenerateRandomDownHill();

		}


		private void GenerateCluster(int i)
		{
            switch(i)
			{

                case 0:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
					break;
                case 1:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
					break;
                case 2:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
					break;
                case 3:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
					break;
                case 4:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
					break;
                case 5:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
					break;
                case 6:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    break;
                case 7:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					break;
                case 8:
					//WORKING COMBINATION
					Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGroundWithGaps);
					Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
					Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGroundWithGaps);
					break;
                case 9:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    break;
                case 10:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    break;
                case 11:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    break;
                case 12:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    break;
                case 13:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    break;


                case 14:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    break;
                case 15:
                    //WORKING COMBINATION  
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    break;

                case 16:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    break;
                case 17:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    break;
                case 18:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    break;
                case 19:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    break;
                case 20:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    break;
                case 21:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    break;
                case 22:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Bridge_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    break;
                case 23:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    break;
                case 24:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    break;


                case 25:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    break;
                case 26:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    break;
                case 27:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    break;
                case 28:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    break;
                case 29:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    break;

                case 30:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_FlatGround);
                    break;
                case 31:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
                    Generate(PlatformID.Platform_Forest_Ground_Stone, PlatformGenFormID.Form_FlatGround);
                    break;

                case 32:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    break;
                case 33:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_StepsUp);
                    break;
                case 34:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    break;
                case 35:
                    //WORKING COMBINATION
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveUp);
                    Generate(PlatformID.Platform_Forest_Ground_Earth, PlatformGenFormID.Form_CurveDown);
                    break;
            }
		}





        private void Generate(PlatformID platformID, PlatformGenFormID platformGenFormID)
		{
            _platGenData.currentPlatformFormID = platformGenFormID;
            switch(platformGenFormID)
			{
                case PlatformGenFormID.Form_FlatGround:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateFlatGround(platformID, i + 1);
                    break;

                case PlatformGenFormID.Form_FlatGroundWithGaps:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateFlatGroundWithPitfalls(platformID, _gapSize, i + 1);
                    break;

                case PlatformGenFormID.Form_CurveUp:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateSlopeUpToFlatGround(platformID, i + 1);
                    break;

                case PlatformGenFormID.Form_CurveDown:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateSlopeDownToFlatGround(platformID, i + 1);
                    break;

                case PlatformGenFormID.Form_StepsUp:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateStepsUpToFlatGround(platformID, _stepSize, i + 1);
                    break;

                case PlatformGenFormID.Form_StepsDown:
                    for (int i = 0; i < _platformChunkCount; i++) GenerateStepsDownToFlatGround(platformID, _stepSize, i + 1);
                    break;
            }
            _platGenData.previousPlatformFormID = platformGenFormID;
        }



        private void GenerateStart()
        {
          
        }
        private void GenerateCheckpoint()
        {
            
        }
        private void GenerateEnd()
		{
            
        }




        private void GenerateFlatGround(PlatformID localPlatformID, int i)
        {
            
        }
        private void GenerateFlatGroundWithPitfalls(PlatformID localPlatformID, PitfallSizeID pitallSizeId, int i)
        {
            
        }


        private void GenerateSlopeDownToFlatGround(PlatformID localPlatformID, int i)
        {

            
        }
        private void GenerateSlopeUpToFlatGround(PlatformID localPlatformID, int i)
        {

            
        }
        

        private void GenerateStepsUpToFlatGround(PlatformID localPlatformID, StepSizeID stepSize, int i)
        {
            
        }
        private void GenerateStepsDownToFlatGround(PlatformID localPlatformID, StepSizeID stepSize, int i)
        {
            
        }



        private void IncludeTreeGeneration(PlatformID localPlatformID, int i, float x, float y, float z, Vector3 latestPlatformPos, bool layerIsForeground)
		{
            

        }
        private void IncludeGrassGeneration(PlatformID localPlatformID, int i, float x, float y, float z, int spawnIndex, Vector3 latestPlatformPos, bool layerIsForeground)
        {
           

        }
        private void IncludeWallGeneration(PlatformID localPlatformID, int i, float x, float y, float z, int spawnIndex, Vector3 latestPlatformPos)
        {
            
        }
        private void IncludeHillsideGeneration(PlatformID localPlatformID, int i, float x, float y, float z, int spawnIndex, Vector3 latestPlatformPos)
		{
            
        }
        private void IncludeBackgroundGeneration(PlatformID localPlatformID, int i, float x, float y, float z, int spawnIndex, Vector3 latestPlatformPos)
		{
            
        }
        private void IncludeTorchGeneration(PlatformID localPlatformID, int i, float x, float y, float z, int spawnIndex, Vector3 latestPlatformPos)
        {
            
        }
        private void IncludeCanopyGeneration(PlatformID localPlatformID, int i, float x, float y, float z, bool isOnWall, Vector3 latestPlatformPos)
        {
            
        }



        private void IncludeEquipmentSpawnPos(Vector3 randPosition)
		{
            if (_platGenData.BlockIncrement < 6) return;
            Vector3 playerItemSpawnLocation = randPosition;
            playerItemSpawnLocation.y += 5.0f;
            _playerItemSpawnPosDict.Add(_playerItemSpawnIndex, playerItemSpawnLocation);
            _playerItemSpawnIndex++;
        }
        private void IncludeEnemySpawnPos(Vector3 randPosition)
        {
            if (_platGenData.BlockIncrement < 16) return;
            Vector3 enemySpawnLocation = randPosition;
            enemySpawnLocation.y += 30.0f;
            int dir = wildlogicgames.Utilities.GetRandomNumberInt(-1, 1);
            if (dir == -1 ) enemySpawnLocation.x -= 1.0f;
            if (dir == 1) enemySpawnLocation.x += 1.0f;
            _enemySpawnPosDict.Add(_playerItemSpawnIndex, enemySpawnLocation);
            _enemySpawnIndex++;
        }



        private void Awake() => Setup();
		void Start() { }
        void Update() { }
    }
}


