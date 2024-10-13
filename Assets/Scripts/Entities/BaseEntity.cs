using Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEntity : MonoBehaviour, IDamageable, ICanBeKilled
    {
        [SerializeField] private float speed;
        [SerializeField] private int health;

        public bool IsEnemy;
        protected float Speed 
        { 
            get => speed; 
            set => speed = value; 
        }
        protected int Health 
        { 
            get => health; 
            set => health = value; 
        }

        public abstract void TakeDamage(int value);

        public virtual void Die()
        {
            Destroy(gameObject);   
        }
    }
}