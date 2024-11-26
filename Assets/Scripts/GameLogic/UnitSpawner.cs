using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class UnitSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class EnemySpawnSettings
        {
            public TypesOfUnits UnitType;
            public int MaxSpawnCount = 5;
            public float SpawnInterval = 2f;
        }

        public enum TypesOfUnits
        {
            NearFighter,
            DistanceFighter,
            LongDistanceFighter,
        }

        public static event System.Action SpawnUnitEvent;

        private Dictionary<TypesOfUnits, GameObject> unitPrefabs;

        [SerializeField] private GameObject nearFighterPrefab;
        [SerializeField] private GameObject distanceFighterPrefab;
        [SerializeField] private GameObject longDistanceFighterPrefab;
        
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Building enemyBuilding;
        
        [SerializeField] private List<EnemySpawnSettings> enemySpawnSettings;

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
            foreach (var settings in enemySpawnSettings)
            {
                StartCoroutine(SpawnEnemies(settings));
            }
        }

        private IEnumerator SpawnEnemies(EnemySpawnSettings settings)
        {
            int spawnCount = 0;

            while (spawnCount < settings.MaxSpawnCount)
            {
                yield return new WaitForSeconds(settings.SpawnInterval);
                SpawnUnit(settings.UnitType);
                spawnCount++;
            }
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
    }
}