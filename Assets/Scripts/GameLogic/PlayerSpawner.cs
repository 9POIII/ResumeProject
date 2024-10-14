using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace GameLogic
{
    public class PlayerSpawner : MonoBehaviour
    {
        public enum TypesOfUnits
        {
            NearFighter,
            DistanceFighter,
            LongDistanceFighter,
        }

        public static Action<TypesOfUnits> PlayerSpawnUnit;
        private Dictionary<TypesOfUnits, GameObject> unitPrefabs;

        [SerializeField] private GameObject nearFighterPrefab;
        [SerializeField] private GameObject distanceFighterPrefab;
        [SerializeField] private GameObject longDistanceFighterPrefab;
        
        [SerializeField] private Transform spawnPoint;
        
        [SerializeField] private Building enemyBuilding;

        private void Awake()
        {
            unitPrefabs = new Dictionary<TypesOfUnits, GameObject>
            {
                { TypesOfUnits.NearFighter, nearFighterPrefab },
                { TypesOfUnits.DistanceFighter, distanceFighterPrefab },
                { TypesOfUnits.LongDistanceFighter, longDistanceFighterPrefab }
            };

            PlayerSpawnUnit += SpawnUnit;
        }

        private void Start()
        {
            StartCoroutine(TestSpawnCoroutine());
        }

        public void SpawnUnit(TypesOfUnits typeOfUnit)
        {
            if (unitPrefabs.TryGetValue(typeOfUnit, out GameObject unitPrefab))
            {
                CombatEntity unit = Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity)
                    .GetComponent<CombatEntity>();
                unit.SetEnemyBuilding(enemyBuilding);
            }
        }

        private void OnDestroy()
        {
            PlayerSpawnUnit -= SpawnUnit;
        }

        private IEnumerator TestSpawnCoroutine()
        {
            yield return new WaitForSeconds(3f);
            
            SpawnUnit(TypesOfUnits.NearFighter);
            
            yield return new WaitForSeconds(3f);
            
            SpawnUnit(TypesOfUnits.DistanceFighter);
            
            yield return new WaitForSeconds(3f);
            
            SpawnUnit(TypesOfUnits.LongDistanceFighter);
        }
    }
}