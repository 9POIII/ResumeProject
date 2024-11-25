using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class UnitSpawner : MonoBehaviour
    {
        public enum TypesOfUnits
        {
            NearFighter,
            DistanceFighter,
            LongDistanceFighter,
        }

        public static event Action SpawnUnitEvent;

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
                unit.name = $"{unit.name} {Random.Range(0, 1337)}";
                unit.SetEnemyBuilding(enemyBuilding);
                SpawnUnitEvent?.Invoke();
            }
        }
        private IEnumerator TestSpawnCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            
            SpawnUnit(TypesOfUnits.NearFighter);
            
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            
            SpawnUnit(TypesOfUnits.DistanceFighter);
            
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            SpawnUnit(TypesOfUnits.LongDistanceFighter);
        }
    }
}