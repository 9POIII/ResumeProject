using System;
using Entities.Player;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] private EntityView entityView;
        public event Action OnUnitDeath;

        [SerializeField] private float speed; 
        private int health;
        [SerializeField] private int maxHealth;

        public bool IsEnemy;
        protected float Speed
        {
            get => speed;
            set => speed = value;
        }
        public int Health
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                entityView?.UpdateStats(health, maxHealth);
            }
        }
        
        protected virtual void Awake()
        {
            Health = maxHealth;
            entityView?.UpdateStats(health, maxHealth);
            Debug.Log(Health);
        }

        public abstract void TakeDamage(int value);

        public virtual void Die()
        {
            OnUnitDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}