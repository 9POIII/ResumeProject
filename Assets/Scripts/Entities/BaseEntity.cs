using Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEntity : MonoBehaviour, IDamageable, ICanBeKilled
    {
        [SerializeField] private float speed;
        [SerializeField] private int health;

        protected float Speed { get; set; }
        protected int Health { get; set; }
        public abstract void TakeDamage(int value);

        public virtual void Die()
        {
            Destroy(gameObject);   
        }
    }
}
