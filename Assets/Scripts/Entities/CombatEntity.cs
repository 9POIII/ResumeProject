using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Weapons;
using GameLogic;
using UnityEngine;

namespace Entities
{
    public abstract class CombatEntity : BaseEntity
    {
        [SerializeField] protected List<BaseEntity> targets;
        [SerializeField] protected BaseEntity currentTarget;
        [SerializeField] protected Building enemyBuilding;
        [SerializeField] protected int damage;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected Animator bitaAnimator;
        [SerializeField] protected Weapon weapon;

        [Header("Distances")]
        [SerializeField] protected float distanceToStop;

        protected float distanceToTarget;
        protected float lastAttackTime;

        private bool isAttackingBuilding;
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        protected override void Awake()
        {
            base.Awake();
            targets = new List<BaseEntity>();
            UnitSpawner.SpawnUnitEvent += OnEnemySpawned;
        }

        protected virtual void Update()
        {
            if (currentTarget != null)
            {
                distanceToTarget = Vector2.Distance(transform.position, 
                    currentTarget.GetComponent<Collider2D>().ClosestPoint(transform.position));
                if (distanceToTarget <= distanceToStop)
                {
                    UseWeapon(damage);
                }
                else
                {
                    Move(currentTarget.transform.position);
                }
            }
            else
            {
                OnEnemySpawned();
            }
        }

        private void OnEnemySpawned()
        {
            FindDamageableTargets();

            if (!isAttackingBuilding || (currentTarget == null || !IsTargetStillAlive(currentTarget)))
            {
                FindNearestTarget();
            }
        }

        protected virtual void Move(Vector2 direction)
        {
            Vector2 moveto = (direction - (Vector2)transform.position).normalized;
            transform.Translate(moveto * Speed * Time.deltaTime);
            bitaAnimator.SetBool(IsAttacking, false);
        }

        protected virtual void UseWeapon(int value)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (currentTarget != null && IsTargetStillAlive(currentTarget))
                {
                    lastAttackTime = Time.time;
                    weapon?.Use(currentTarget, value);
                }
            }
        }

        public override void TakeDamage(int value)
        {
            Health -= value;

            if (Health <= 0)
            {
                Die();
            }
        }

        public override void Die()
        {
            if (currentTarget != null)
            {
                currentTarget.OnUnitDeath -= OnTargetDeath;
            }
            base.Die();
        }

        private void OnTargetDeath()
        {
            //Debug.Log($"Target {currentTarget.name} is dead. Searching for new target.");
            FindDamageableTargets();
            FindNearestTarget();
        }

        public void SetEnemyBuilding(Building building)
        {
            enemyBuilding = building;
        }

        protected virtual void FindDamageableTargets()
        {
            targets.Clear();
            BaseEntity[] allObjects = FindObjectsOfType<BaseEntity>();
            foreach (var obj in allObjects)
            {
                if (IsValidTarget(obj))
                {
                    targets.Add(obj);
                }
            }

            if (targets.Count == 0 && enemyBuilding != null)
            {
                currentTarget = enemyBuilding;
            }
        }

        protected virtual bool IsValidTarget(BaseEntity target)
        {
            return target.IsEnemy != this.IsEnemy;
        }

        protected virtual void FindNearestTarget()
        {
            if (currentTarget != null)
            {
                currentTarget.OnUnitDeath -= OnTargetDeath;
            }

            float closestDistance = float.MaxValue;
            BaseEntity nearestTarget = null;

            foreach (var target in targets)
            {
                if (!IsTargetStillAlive(target))
                {
                    continue;
                }

                float distanceToTargetLocal = Vector2.Distance(transform.position, target.transform.position);
                if (distanceToTargetLocal < closestDistance)
                {
                    closestDistance = distanceToTargetLocal;
                    nearestTarget = target;
                }
            }

            if (nearestTarget != null)
            {
                currentTarget = nearestTarget;
                isAttackingBuilding = false;
                currentTarget.OnUnitDeath += OnTargetDeath;
            }
            else
            {
                currentTarget = enemyBuilding;
                isAttackingBuilding = true;
            }
        }

        private bool IsTargetStillAlive(BaseEntity target)
        {
            return target != null && target.gameObject.activeInHierarchy;
        }

        private void OnDestroy()
        {
            UnitSpawner.SpawnUnitEvent -= OnEnemySpawned;

            if (currentTarget != null)
            {
                currentTarget.OnUnitDeath -= OnTargetDeath;
            }
        }

        private void OnDisable()
        {
            UnitSpawner.SpawnUnitEvent -= OnEnemySpawned;

            if (currentTarget != null)
            {
                currentTarget.OnUnitDeath -= OnTargetDeath;
            }
        }
    }
}