using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class EnemyAIGenerator : MonoBehaviour
	{
		private Transform _transform;

		[Header("Level Generator Reference")]
		[Tooltip("We need to retrieve the positions available for enemy start spawns.")]
		public LevelGenerator _levelGenerator;

		[Header("Prefab Bandit to Generate")]
		[Tooltip("")]
		public GameObject _prefabBanditObject;

		[Header("Prefab BanditArcher to Generate")]
		[Tooltip("")]
		public GameObject _prefabBanditArcherObject;

		private Dictionary<int, Vector3> _enemySpawnPosDict;
		private List<int> _spawnPosUsedList;
		private int _banditToSpawn = 60;
		private int _archerBanditToSpawn = 1;//20;

		private bool _setup = false;
		private void Setup()
		{
			_transform = this.transform;
			if (_enemySpawnPosDict == null) _enemySpawnPosDict = new Dictionary<int, Vector3>();
			if (_spawnPosUsedList == null) _spawnPosUsedList = new List<int>();
			_enemySpawnPosDict = _levelGenerator.GetEnemySpawnDict();

			//SpawnBandits();
			SpawnArcherBandits();

			_setup = true;
		}


		private void SpawnBandits()
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < _banditToSpawn; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
						}
					}

				}




				
				obj = (GameObject)Instantiate(_prefabBanditObject);
				obj.GetComponent<Bandit>()._enemyID = i;
				obj.GetComponent<Bandit>().InitializeBandit();
				//obj.SetActive(true);//Internally do this in the InitializeBandit() method call, as _enemyID not set to OnEnable().
				//obj.transform.position = _enemySpawnPosDict[j]; //<-- HERE'S THE PROBLEM

				Vector3 spawnPos;
				if (_enemySpawnPosDict.TryGetValue(j, out spawnPos)) 
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
					if (_enemySpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}



				obj.transform.parent = _transform;
			}
		}

		private void SpawnArcherBandits()
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < _archerBanditToSpawn; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
						}
					}

				}
				obj = (GameObject)Instantiate(_prefabBanditArcherObject);
				obj.GetComponent<BanditArcher>()._enemyID = _banditToSpawn+ i; //= i;//<-- HERE'S THE PROBLEM
				obj.GetComponent<BanditArcher>().InitializeBanditArcher();
				//obj.SetActive(true);//Internally do this in the InitializeBandit() method call, as _enemyID not set to OnEnable().
				//obj.transform.position = _enemySpawnPosDict[j]; 

				Vector3 spawnPos;
				if (_enemySpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _enemySpawnPosDict.Count - 1);
					if (_enemySpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}
				obj.transform.parent = _transform;
			}
		}

		//private void Awake() => Setup();
		void Start() { }
		void Update()
		{
			if (!_setup) Setup(); //LevelGenerator.cs must Awake() before this.
		}
	}
}
