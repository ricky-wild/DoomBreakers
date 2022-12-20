using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum PrefabID //Must Corrispond to _prefabGameObjects[INDEX]
    {
        Prefab_RunningDustFX = 0,               //_prefabGameObjects[0] = Prefab_RunningDustFX obj
        Prefab_JumpingDustFX = 1,
        Prefab_BloodHitFX = 2,
        Prefab_DustHitFX = 3,
        Prefab_ArmorHitFX = 4,
        Prefab_ArmorObtainedFX = 5,
        Prefab_HealingFX = 6,
        Prefab_ArrowShotFX = 7
    };

    public class ObjectPooler : MonoBehaviour
    {
        public enum PoolUsageFlag
		{
            PoolingFor_Player = 0,
            PoolingFor_Enemy = 1
		}

        public static ObjectPooler _instance;


        [Header("Prefabs To Pool")]
        [Tooltip("The order must corrispond to enum PrefabID")]
        public GameObject[] _prefabGameObjects;

        [Header("Typical Pool Size")]
        [Tooltip("The starting size of each pool.")]
        public int _poolSize;

        //[SerializeField]
        public Dictionary<PrefabID, List<GameObject>> _pooledObjectsPlayerDict;
        public Dictionary<PrefabID, List<GameObject>> _pooledObjectsEnemyDict;

        private Transform _transform;
        private Vector3 _vectorOffset;
        private bool _poolCanExpand;

        private void Awake() => Setup();
        private void Setup()
		{
            _instance = this;
            _transform = this.transform;

            if (_prefabGameObjects == null) return;

            if (_pooledObjectsPlayerDict == null) _pooledObjectsPlayerDict = new Dictionary<PrefabID, List<GameObject>>();
            if (_pooledObjectsEnemyDict == null) _pooledObjectsEnemyDict = new Dictionary<PrefabID, List<GameObject>>();

            _poolCanExpand = true;


            SetupPlayerPool();
            SetupEnemyPool();

        }

        private void SetupPlayerPool()
		{
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_RunningDustFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_JumpingDustFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_BloodHitFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_DustHitFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_ArmorHitFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_ArmorObtainedFX, new List<GameObject>());
            _pooledObjectsPlayerDict.Add(PrefabID.Prefab_HealingFX, new List<GameObject>());
            for (int i = 0; i < _poolSize; i++)
            {
                AddGameObject(PrefabID.Prefab_RunningDustFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_JumpingDustFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_BloodHitFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_DustHitFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_ArmorHitFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_ArmorObtainedFX, PoolUsageFlag.PoolingFor_Player);
                AddGameObject(PrefabID.Prefab_HealingFX, PoolUsageFlag.PoolingFor_Player);
            }
        }
        private void SetupEnemyPool()
        {
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_RunningDustFX, new List<GameObject>());
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_JumpingDustFX, new List<GameObject>());
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_BloodHitFX, new List<GameObject>());
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_DustHitFX, new List<GameObject>());
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_ArmorHitFX, new List<GameObject>());
            _pooledObjectsEnemyDict.Add(PrefabID.Prefab_ArrowShotFX, new List<GameObject>());

            for (int i = 0; i < _poolSize; i++)
            {
                AddGameObject(PrefabID.Prefab_RunningDustFX, PoolUsageFlag.PoolingFor_Enemy);
                AddGameObject(PrefabID.Prefab_JumpingDustFX, PoolUsageFlag.PoolingFor_Enemy);
                AddGameObject(PrefabID.Prefab_BloodHitFX, PoolUsageFlag.PoolingFor_Enemy);
                AddGameObject(PrefabID.Prefab_DustHitFX, PoolUsageFlag.PoolingFor_Enemy);
                AddGameObject(PrefabID.Prefab_ArmorHitFX, PoolUsageFlag.PoolingFor_Enemy);
                AddGameObject(PrefabID.Prefab_ArrowShotFX, PoolUsageFlag.PoolingFor_Enemy);
            }
        }

        private GameObject GetPlayerPooledObject(PrefabID prefabID)
		{
            List<GameObject> list;
            for (int i = 0; i < _pooledObjectsPlayerDict.Count; i++)
            {
                list = _pooledObjectsPlayerDict[prefabID];
                for(int j =0; j < list.Count; j++)
				{
                    if (!_pooledObjectsPlayerDict[prefabID][j].activeInHierarchy)
                    {
                        return _pooledObjectsPlayerDict[prefabID][j];
                    }
                }

            }

            //Failing to return, we must expand the pool and return a new one.
            if (!_poolCanExpand)
                return null;

            GameObject newGameObject = (GameObject)Instantiate(_prefabGameObjects[(int)prefabID]);
            _pooledObjectsPlayerDict[prefabID].Add(newGameObject);
            return newGameObject;

		}
        private GameObject GetEnemyPooledObject(PrefabID prefabID)
        {
            List<GameObject> list;
            for (int i = 0; i < _pooledObjectsEnemyDict.Count; i++)
            {
                list = _pooledObjectsEnemyDict[prefabID];
                for (int j = 0; j < list.Count; j++)
                {
                    if (!_pooledObjectsEnemyDict[prefabID][j].activeInHierarchy)
                    {
                        return _pooledObjectsEnemyDict[prefabID][j];
                    }
                }

            }

            //Failing to return, we must expand the pool and return a new one.
            if (!_poolCanExpand)
                return null;

            GameObject newGameObject = (GameObject)Instantiate(_prefabGameObjects[(int)prefabID]);
            _pooledObjectsEnemyDict[prefabID].Add(newGameObject);
            return newGameObject;

        }
        private void AddGameObject(PrefabID prefabID, PoolUsageFlag poolUsageFlag)
        {
            //Reference to the on-disk asset. Don't do anything to that,Instead, use Instantiate<GameObject>() and use the copy.

            GameObject newGameObject = (GameObject)Instantiate(_prefabGameObjects[(int)prefabID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            switch(poolUsageFlag)
			{
                case PoolUsageFlag.PoolingFor_Player:
                    //Try to get the list associated with prefabID and then add a new game object to it, if it exists.
                    if (_pooledObjectsPlayerDict.TryGetValue(prefabID, out List<GameObject> listPlayer)) listPlayer.Add(newGameObject);
                    break;
                case PoolUsageFlag.PoolingFor_Enemy:
                    //Try to get the list associated with prefabID and then add a new game object to it, if it exists.
                    if (_pooledObjectsEnemyDict.TryGetValue(prefabID, out List<GameObject> listEnemy)) listEnemy.Add(newGameObject);
                    break;
            }

        }
        private GameObject GetGameObject(PrefabID prefabID, int index) => _pooledObjectsPlayerDict[prefabID][index];


		public void InstantiateForPlayer(PrefabID prefabID, Transform positionToSpawn, int playerId, int direction)
		{
            GameObject obj = GetPlayerPooledObject(prefabID);

            if (obj == null) return;
            if (positionToSpawn == null) return;

            _vectorOffset = positionToSpawn.position;
            _vectorOffset.z -= 0.25f;//Position in front of player sprite.
            float distanceToPlayerFootX = 0f;
            float distanceToPlayerFootY = 0f;

            switch (prefabID)
			{
                case PrefabID.Prefab_RunningDustFX:
                    distanceToPlayerFootX = 0.85f;
                    distanceToPlayerFootY = 1.2f;
                    if (direction == 1) _vectorOffset.x -= distanceToPlayerFootX;
                    if (direction == -1) _vectorOffset.x += distanceToPlayerFootX;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<RunningDustFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_JumpingDustFX:
                    distanceToPlayerFootX = 0.1f;
                    distanceToPlayerFootY = 0.9f;
                    if (direction == 1) _vectorOffset.x -= distanceToPlayerFootX;
                    if (direction == -1) _vectorOffset.x += distanceToPlayerFootX;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<JumpingDustFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_BloodHitFX:
                    distanceToPlayerFootX = 0.25f;
                    distanceToPlayerFootY = 0.5f;
                    if (direction == 1) _vectorOffset.x -= distanceToPlayerFootX;
                    if (direction == -1) _vectorOffset.x += distanceToPlayerFootX;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<BloodHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_DustHitFX:
                    distanceToPlayerFootX = 0.5f;
                    distanceToPlayerFootY = 0.5f;
                    if (direction == 1) _vectorOffset.x += distanceToPlayerFootX;
                    if (direction == -1) _vectorOffset.x -= distanceToPlayerFootX;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<DustHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_ArmorHitFX:
                    distanceToPlayerFootX = 0.25f;
                    distanceToPlayerFootY = 0.5f;
                    if (direction == 1) _vectorOffset.x -= distanceToPlayerFootX;
                    if (direction == -1) _vectorOffset.x += distanceToPlayerFootX;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<ArmorHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_ArmorObtainedFX:
                    distanceToPlayerFootX = 0f;
                    distanceToPlayerFootY = 0.5f;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<ArmorObtainedFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_HealingFX:
                    distanceToPlayerFootX = 0f;
                    distanceToPlayerFootY = 0.5f;
                    _vectorOffset.y -= distanceToPlayerFootY;
                    obj.GetComponent<HealingFX>().SetDirection(direction);
                    break;
            }


            obj.transform.position = _vectorOffset;
            //obj.transform.rotation = positionToSpawn.rotation;
            obj.transform.parent = _transform;
            obj.SetActive(true);
        }

        public void InstantiateForEnemy(PrefabID prefabID, Transform positionToSpawn, int enemyId, int direction)
		{
            GameObject obj = GetEnemyPooledObject(prefabID);

            if (obj == null) return;
            if (positionToSpawn == null) return;

            _vectorOffset = positionToSpawn.position;
            _vectorOffset.z -= 0.25f;//Position in front of player sprite.
            float distanceToEnemyFootX = 0f;
            float distanceToEnemyFootY = 0f;

            switch (prefabID)
            {
                case PrefabID.Prefab_RunningDustFX:
                    distanceToEnemyFootX = 0.85f;
                    distanceToEnemyFootY = 1.2f;
                    if (direction == 1) _vectorOffset.x -= distanceToEnemyFootX;
                    if (direction == -1) _vectorOffset.x += distanceToEnemyFootX;
                    _vectorOffset.y -= distanceToEnemyFootY;
                    obj.GetComponent<RunningDustFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_JumpingDustFX:
                    distanceToEnemyFootX = 0.1f;
                    distanceToEnemyFootY = 0.9f;
                    if (direction == 1) _vectorOffset.x -= distanceToEnemyFootX;
                    if (direction == -1) _vectorOffset.x += distanceToEnemyFootX;
                    _vectorOffset.y -= distanceToEnemyFootY;
                    obj.GetComponent<JumpingDustFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_BloodHitFX:
                    distanceToEnemyFootX = 0.25f;
                    distanceToEnemyFootY = 0.5f;
                    if (direction == 1) _vectorOffset.x -= distanceToEnemyFootX;
                    if (direction == -1) _vectorOffset.x += distanceToEnemyFootX;
                    _vectorOffset.y -= distanceToEnemyFootY;
                    obj.GetComponent<BloodHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_DustHitFX:
                    distanceToEnemyFootX = 1.0f;
                    distanceToEnemyFootY = 1.0f;
                    if (direction == 1) _vectorOffset.x += distanceToEnemyFootX;
                    if (direction == -1) _vectorOffset.x -= distanceToEnemyFootX;
                    _vectorOffset.y -= distanceToEnemyFootY;
                    obj.GetComponent<DustHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_ArmorHitFX:
                    distanceToEnemyFootX = 0.25f;
                    distanceToEnemyFootY = 0.5f;
                    if (direction == 1) _vectorOffset.x -= distanceToEnemyFootX;
                    if (direction == -1) _vectorOffset.x += distanceToEnemyFootX;
                    _vectorOffset.y -= distanceToEnemyFootY;
                    obj.GetComponent<ArmorHitFX>().SetDirection(direction);
                    break;
                case PrefabID.Prefab_ArrowShotFX:
                    obj.GetComponent<ArrowShotFX>().SetDirection(direction);
                    obj.transform.rotation = positionToSpawn.rotation;
                    break;

            }

            obj.transform.position = _vectorOffset;
            //obj.transform.rotation = positionToSpawn.rotation;
            obj.transform.parent = _transform;
            obj.SetActive(true);
        }


        void Start() { }
        void Update() { }
    }
}

