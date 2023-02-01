using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class PlayerEquipGenerator : MonoBehaviour
	{
		private Transform _transform;

		[Header("Level Generator Reference")]
		[Tooltip("We need to retrieve the positions available for equipment spawns.")]
		public LevelGenerator _levelGenerator;

		[Header("Prefab Broadswords to Generate")]
		[Tooltip("")]
		public GameObject[] _prefabBroadswordsObjects;

		[Header("Prefab Longswords to Generate")]
		[Tooltip("")]
		public GameObject[] _prefabLongswordsObjects;

		[Header("Prefab Mace to Generate")]
		[Tooltip("")]
		public GameObject[] _prefabMaceObjects;

		[Header("Prefab Shields to Generate")]
		[Tooltip("")]
		public GameObject[] _prefabShieldsObjects;

		[Header("Prefab Breastplates to Generate")]
		[Tooltip("")]
		public GameObject[] _prefabBreastplatesObjects;



		private Dictionary<int, Vector3> _equipmentSpawnPosDict;
		private List<int> _spawnPosUsedList;

		private bool _setup = false;
		private void Setup()
		{
			_transform = this.transform;
			if (_equipmentSpawnPosDict == null) _equipmentSpawnPosDict = new Dictionary<int, Vector3>();
			if (_spawnPosUsedList == null) _spawnPosUsedList = new List<int>();
			_equipmentSpawnPosDict = _levelGenerator.GetPlayerItemSpawnDict();


			int playerCount = 1;
			int playerMaxCount = 4;
			int randomEquipAmount = 0;
			int randomEquipTypeAvailable = 0;

			int min = playerCount;
			int max = (playerCount * playerMaxCount) * wildlogicgames.Utilities.GetRandomNumberInt(2, playerMaxCount);//min * 2;
			randomEquipAmount = wildlogicgames.Utilities.GetRandomNumberInt(min, max);
			randomEquipTypeAvailable = 3;// wildlogicgames.Utilities.GetRandomNumberInt(0, 3);//4 types, bronze, iron,steel,ebony
			SpawnBroadswords(randomEquipAmount, randomEquipTypeAvailable);

			randomEquipAmount = wildlogicgames.Utilities.GetRandomNumberInt(min, max);
			randomEquipTypeAvailable = 3;//wildlogicgames.Utilities.GetRandomNumberInt(0, 3);//4 types, bronze, iron,steel,ebony
			SpawnLongswords(randomEquipAmount, randomEquipTypeAvailable);

			randomEquipAmount = wildlogicgames.Utilities.GetRandomNumberInt(min, max);
			randomEquipTypeAvailable = 3;//wildlogicgames.Utilities.GetRandomNumberInt(0, 3);//4 types, bronze, iron,steel,ebony
			SpawnMaces(randomEquipAmount, randomEquipTypeAvailable);

			randomEquipAmount = wildlogicgames.Utilities.GetRandomNumberInt(min, max);
			randomEquipTypeAvailable = 3;//wildlogicgames.Utilities.GetRandomNumberInt(0, 3);//4 types, bronze, iron,steel,ebony
			SpawnShields(randomEquipAmount, randomEquipTypeAvailable);

			randomEquipAmount = wildlogicgames.Utilities.GetRandomNumberInt(min, max);
			randomEquipTypeAvailable = 3;//wildlogicgames.Utilities.GetRandomNumberInt(0, 3);//4 types, bronze, iron,steel,ebony
			SpawnBreastplates(randomEquipAmount, randomEquipTypeAvailable);


			_setup = true;
		}
		private void SpawnBroadswords(int randomBroadswordAmount, int randomBroadswordTypeAvailable)
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < randomBroadswordAmount; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
						}
					}

				}




				int k = wildlogicgames.Utilities.GetRandomNumberInt(0, randomBroadswordTypeAvailable);
				obj = (GameObject)Instantiate(_prefabBroadswordsObjects[k]);

				obj.GetComponent<Sword>()._itemID = i;
				obj.GetComponent<Sword>()._swordID = PlayerItem.IsBroadsword;
				obj.GetComponent<Sword>()._weaponType = EquipmentWeaponType.Broadsword;
				randomBroadswordTypeAvailable = k;
				if (randomBroadswordTypeAvailable == 0) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Bronze;
				if (randomBroadswordTypeAvailable == 1) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Iron;
				if (randomBroadswordTypeAvailable == 2) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Steel;
				if (randomBroadswordTypeAvailable == 3) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Ebony;

				obj.SetActive(true);
				//obj.transform.position = _equipmentSpawnPosDict[j];
				Vector3 spawnPos;
				if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
					if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}
				obj.transform.parent = _transform;
			}
		}
		private void SpawnLongswords(int randomLongswordAmount, int randomLongswordTypeAvailable)
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < randomLongswordAmount; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
						}
					}

				}




				int k = wildlogicgames.Utilities.GetRandomNumberInt(0, randomLongswordTypeAvailable);
				obj = (GameObject)Instantiate(_prefabLongswordsObjects[k]);//_prefabBroadswordsObjects[k];
				obj.GetComponent<Sword>()._itemID = i;
				obj.GetComponent<Sword>()._swordID = PlayerItem.IsLongsword;
				obj.GetComponent<Sword>()._weaponType = EquipmentWeaponType.Longsword;
				randomLongswordTypeAvailable = k;
				if (randomLongswordTypeAvailable == 0) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Bronze;
				if (randomLongswordTypeAvailable == 1) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Iron;
				if (randomLongswordTypeAvailable == 2) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Steel;
				if (randomLongswordTypeAvailable == 3) obj.GetComponent<Sword>()._materialType = EquipmentMaterialType.Ebony;

				obj.SetActive(true);
				//obj.transform.position = _equipmentSpawnPosDict[j];
				Vector3 spawnPos;
				if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
					if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}
				obj.transform.parent = _transform;
			}
		}
		private void SpawnMaces(int randomMaceAmount, int randomMaceTypeAvailable)
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < randomMaceAmount; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
						}
					}

				}




				int k = wildlogicgames.Utilities.GetRandomNumberInt(0, randomMaceTypeAvailable);
				obj = (GameObject)Instantiate(_prefabMaceObjects[k]);//_prefabBroadswordsObjects[k];
				obj.GetComponent<Mace>()._itemID = i;
				obj.GetComponent<Mace>()._maceID = PlayerItem.IsMace;
				obj.GetComponent<Mace>()._weaponType = EquipmentWeaponType.MorningstarMace;
				randomMaceTypeAvailable = k;
				if (randomMaceTypeAvailable == 0) obj.GetComponent<Mace>()._materialType = EquipmentMaterialType.Bronze;
				if (randomMaceTypeAvailable == 1) obj.GetComponent<Mace>()._materialType = EquipmentMaterialType.Iron;
				if (randomMaceTypeAvailable == 2) obj.GetComponent<Mace>()._materialType = EquipmentMaterialType.Steel;
				if (randomMaceTypeAvailable == 3) obj.GetComponent<Mace>()._materialType = EquipmentMaterialType.Ebony;

				obj.SetActive(true);
				//obj.transform.position = _equipmentSpawnPosDict[j];
				Vector3 spawnPos;
				if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
					if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}
				obj.transform.parent = _transform;
			}
		}
		private void SpawnShields(int randomShieldAmount, int randomShieldTypeAvailable)
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < randomShieldAmount; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
						}
					}

				}




				int k = wildlogicgames.Utilities.GetRandomNumberInt(0, randomShieldTypeAvailable);
				obj = (GameObject)Instantiate(_prefabShieldsObjects[k]);//_prefabBroadswordsObjects[k];
				obj.SetActive(true);
				obj.GetComponent<Shield>()._itemID = i;
				PlayerItem shieldID = PlayerItem.IsShield;
				obj.GetComponent<Shield>()._shieldType = EquipmentArmorType.Shield;
				EquipmentMaterialType materialType = EquipmentMaterialType.Bronze;
				randomShieldTypeAvailable = k;
				if (randomShieldTypeAvailable == 0) materialType = EquipmentMaterialType.Bronze;
				if (randomShieldTypeAvailable == 1) materialType = EquipmentMaterialType.Iron;
				if (randomShieldTypeAvailable == 2) materialType = EquipmentMaterialType.Steel;
				if (randomShieldTypeAvailable == 3) materialType = EquipmentMaterialType.Ebony;
				obj.GetComponent<Shield>().Initialize(obj.GetComponent<SpriteRenderer>(), obj.GetComponent<Animator>(),
					PlayerAnimatorController.Weapon_equipment_to_pickup, ItemAnimationState.IdleShield, shieldID, materialType);

				//obj.transform.position = _equipmentSpawnPosDict[j];
				Vector3 spawnPos;
				if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
					if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
						obj.transform.position = spawnPos;
				}
				obj.transform.parent = _transform;
			}
		}
		private void SpawnBreastplates(int randomBreastplateAmount, int randomBreastplateTypeAvailable)
		{
			GameObject obj;
			int j = 0;
			for (int i = 0; i < randomBreastplateAmount; i++)
			{
				j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
				if (!_spawnPosUsedList.Contains(j))
					_spawnPosUsedList.Add(j);
				else
				{
					for (int l = 0; l < _spawnPosUsedList.Count; l++)
					{
						if (j == _spawnPosUsedList[l])
						{
							l = 0;
							j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
						}
					}

				}




				int k = wildlogicgames.Utilities.GetRandomNumberInt(0, randomBreastplateTypeAvailable);
				obj = (GameObject)Instantiate(_prefabBreastplatesObjects[k]);//_prefabBroadswordsObjects[k];
				obj.SetActive(true);
				obj.GetComponent<Breastplate>()._itemID = i;
				PlayerItem armorID = PlayerItem.IsBreastPlate;
				obj.GetComponent<Breastplate>()._armorType = EquipmentArmorType.Breastplate;
				EquipmentMaterialType materialType = EquipmentMaterialType.Bronze;
				randomBreastplateTypeAvailable = k;
				if (randomBreastplateTypeAvailable == 0) materialType = EquipmentMaterialType.Bronze;
				if (randomBreastplateTypeAvailable == 1) materialType = EquipmentMaterialType.Iron;
				if (randomBreastplateTypeAvailable == 2) materialType = EquipmentMaterialType.Steel;
				if (randomBreastplateTypeAvailable == 3) materialType = EquipmentMaterialType.Ebony;
				obj.GetComponent<Breastplate>().Initialize(obj.GetComponent<SpriteRenderer>(), obj.GetComponent<Animator>(),
					PlayerAnimatorController.Weapon_equipment_to_pickup, ItemAnimationState.IdleBreastplate, armorID, materialType);


				//obj.transform.position = _equipmentSpawnPosDict[j];
				Vector3 spawnPos;
				if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
					obj.transform.position = spawnPos;
				else
				{
					j = wildlogicgames.Utilities.GetRandomNumberInt(0, _equipmentSpawnPosDict.Count - 1);
					if (_equipmentSpawnPosDict.TryGetValue(j, out spawnPos))
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
