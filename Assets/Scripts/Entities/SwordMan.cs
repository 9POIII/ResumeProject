using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Entities
{
    public class SwordMan : BaseEntity, IMovable, ICanAttack
    {
        [SerializeField] private List<IDamageable> targets;
        [SerializeField] private IDamageable currentTarget;

        private void Awake()
        {
            targets = new List<IDamageable>();
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
                Move(((MonoBehaviour)currentTarget).transform.position);
            }
        }

        public void Move(Vector2 direction)
        {
            Vector2 targetPosition = (currentTarget as MonoBehaviour).transform.position;
            Vector2 direction1 = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(direction1 * Speed * Time.deltaTime);
        }

        public void UseWeapon(int value)
        {
            if (currentTarget != null && IsTargetStillAlive(currentTarget))
            {
                currentTarget.TakeDamage(value);
            }
            else
            {
                FindNearestTarget();
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
            MonoBehaviour[] allObjects = FindObjectsOfType<MonoBehaviour>();

            foreach (var obj in allObjects)
            {
                if (obj is IDamageable damageable)
                {
                    targets.Add(damageable);
                }
            }
        }

        private void FindNearestTarget()
        {
            float closestDistance = float.MaxValue;
            IDamageable nearestTarget = null;

            foreach (var target in targets)
            {
                var targetGameObject = target as MonoBehaviour;

                if (targetGameObject == null || !targetGameObject.gameObject.activeInHierarchy)
                {
                    continue;
                }

                float distanceToTarget = Vector2.Distance(transform.position, targetGameObject.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    nearestTarget = target;
                }
            }
            currentTarget = nearestTarget; 
        }

        private bool IsTargetStillAlive(IDamageable target)
        {
            MonoBehaviour targetGameObject = target as MonoBehaviour;
            return targetGameObject != null && targetGameObject.gameObject.activeInHierarchy;
        }
    }
}