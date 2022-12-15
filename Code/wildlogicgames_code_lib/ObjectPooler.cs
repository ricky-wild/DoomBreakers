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
    };

    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler _instance;


        [Header("Prefabs To Pool")]
        [Tooltip("The GameObjects we need to use.")]
        public GameObject[] _prefabGameObjects;

        [Header("Typical Pool Size")]
        [Tooltip("The starting size of each pool.")]
        public int _poolSize;

        //[SerializeField]
        public Dictionary<PrefabID, List<GameObject>> _pooledObjectsDict;

        private Transform _transform;
        private Vector3 _vectorOffset;
        private bool _poolCanExpand;

        private void Awake() => Setup();
        private void Setup()
		{
            _instance = this;
            _transform = this.transform;

            if (_prefabGameObjects == null) return;

            if (_pooledObjectsDict == null) _pooledObjectsDict = new Dictionary<PrefabID, List<GameObject>>();

            _poolCanExpand = true;

            _pooledObjectsDict.Add(PrefabID.Prefab_RunningDustFX, new List<GameObject>());
            _pooledObjectsDict.Add(PrefabID.Prefab_JumpingDustFX, new List<GameObject>());
            for (int i = 0; i < _poolSize; i++)
			{
                AddGameObject(PrefabID.Prefab_RunningDustFX);
                AddGameObject(PrefabID.Prefab_JumpingDustFX);
            }

            //_pooledObjectsDict.Add(PrefabID.Prefab_JumpingDustFX, new List<GameObject>());
            //_pooledObjectsDict.Add(PrefabID.Prefab_BloodHitFX, new List<GameObject>());
            //_pooledObjectsDict.Add(PrefabID.Prefab_DustHitFX, new List<GameObject>());
            //_pooledObjectsDict.Add(PrefabID.Prefab_ArmorHitFX, new List<GameObject>());

        }


        private GameObject GetPooledObject(PrefabID prefabID)
		{
            List<GameObject> list;
            for (int i = 0; i < _pooledObjectsDict.Count; i++)
            {
                list = _pooledObjectsDict[prefabID];
                for(int j =0; j < list.Count; j++)
				{
                    if (!_pooledObjectsDict[prefabID][j].activeInHierarchy)
                    {
                        return _pooledObjectsDict[prefabID][j];
                    }
                }

            }

            //Failing to return, we must expand the pool and return a new one.
            if (!_poolCanExpand)
                return null;

            GameObject newGameObject = (GameObject)Instantiate(_prefabGameObjects[(int)prefabID]);
            _pooledObjectsDict[prefabID].Add(newGameObject);
            return newGameObject;

		}
        private void AddGameObject(PrefabID prefabID)
        {
            //Reference to the on-disk asset. Don't do anything to that,Instead, use Instantiate<GameObject>() and use the copy.

            GameObject newGameObject = (GameObject)Instantiate(_prefabGameObjects[(int)prefabID]);
            newGameObject.transform.parent = _transform;
            newGameObject.SetActive(false);

            //Try to get the list associated with prefabID and then add a new game object to it, if it exists.
            if (_pooledObjectsDict.TryGetValue(prefabID, out List<GameObject> list)) list.Add(newGameObject);
        }
        private GameObject GetGameObject(PrefabID prefabID, int index) => _pooledObjectsDict[prefabID][index];


		public void InstantiateForPlayer(PrefabID prefabID, Transform positionToSpawn, int playerId, int direction)
		{
            GameObject obj = GetPooledObject(prefabID);

            if (obj == null) return;
            if (positionToSpawn == null) return;

            _vectorOffset = positionToSpawn.position;
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

