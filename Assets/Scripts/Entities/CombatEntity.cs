using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public abstract class CombatEntity : BaseEntity
    {
        [SerializeField] protected List<BaseEntity> targets;
        [SerializeField] protected BaseEntity currentTarget;
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
                distanceToTarget = Vector2.Distance(gameObject.transform.position, currentTarget.transform.position);

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

        public virtual void Move(Vector2 direction)
        {
            Vector2 moveto = (direction - (Vector2)transform.position).normalized;
            transform.Translate(moveto * Speed * Time.deltaTime);
        }

        public virtual void UseWeapon(int value)
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
            currentTarget = nearestTarget; 
        }

        protected bool IsTargetStillAlive(BaseEntity target)
        {
            return target != null && target.gameObject.activeInHierarchy;
        }
    }
}