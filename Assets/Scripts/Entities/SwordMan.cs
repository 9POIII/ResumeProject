using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Entities
{
    public class SwordMan : BaseEntity, IMovable, ICanAttack
    {
        [SerializeField] private List<BaseEntity> targets;
        [SerializeField] private BaseEntity currentTarget;
        [SerializeField] private int damage;
        [SerializeField] private float attackCooldown;

        [Header("Distances")] 
        [SerializeField] private float distanceToStop;

        private Rigidbody2D rb;
        private float distanceToTarget;
        private float lastAttackTime;
        
        private void Awake()
        {
            targets = new List<BaseEntity>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
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

        public void Move(Vector2 direction)
        {
            Vector2 moveto = (direction - (Vector2)transform.position).normalized;
            transform.Translate(moveto * Speed * Time.deltaTime);
        }

        public void UseWeapon(int value)
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

        private void FindDamageableTargets()
        {
            targets.Clear();
            BaseEntity[] allObjects = FindObjectsOfType<BaseEntity>();

            foreach (var obj in allObjects)
            {
                if (obj.IsEnemy)
                {
                    targets.Add(obj);
                }
            }
        }

        private void FindNearestTarget()
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

        private bool IsTargetStillAlive(BaseEntity target)
        {
            return target != null && target.gameObject.activeInHierarchy;
        }
    }
}