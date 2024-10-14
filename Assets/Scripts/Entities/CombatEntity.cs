using System.Collections.Generic;
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

        [Header("Distances")] 
        [SerializeField] protected float distanceToStop;

        protected Rigidbody2D rb;
        protected float distanceToTarget;
        protected float lastAttackTime;

        protected virtual void Awake()
        {
            targets = new List<BaseEntity>();
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            FindDamageableTargets();

            if (currentTarget == null || !IsTargetStillAlive(currentTarget))
            {
                FindNearestTarget();
            }

            if (currentTarget != null)
            {
                distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);

                if (distanceToTarget <= distanceToStop)
                {
                    UseWeapon(damage);
                }
                else
                {
                    Move(currentTarget.transform.position);
                }
            }
        }

        protected virtual void Move(Vector2 direction)
        {
            Vector2 moveto = (direction - (Vector2)transform.position).normalized;
            transform.Translate(moveto * Speed * Time.deltaTime);
        }

        protected virtual void UseWeapon(int value)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (currentTarget != null && IsTargetStillAlive(currentTarget))
                {
                    currentTarget.TakeDamage(value);
                    lastAttackTime = Time.time;
                }
                else
                {
                    FindNearestTarget();
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
            Destroy(gameObject);
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

            currentTarget = nearestTarget ?? enemyBuilding; 
        }

        private bool IsTargetStillAlive(BaseEntity target)
        {
            return target != null && target.gameObject.activeInHierarchy;
        }
    }
}