using UnityEngine;

namespace Entities
{
    public class Building : BaseEntity
    {
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
    }
}