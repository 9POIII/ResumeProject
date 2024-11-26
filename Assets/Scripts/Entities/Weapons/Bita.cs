using UnityEngine;

namespace Entities.Weapons
{
    public class BatWeapon : Weapon
    {
        public override void Use(BaseEntity target, int damage)
        {
            if (animator != null)
            {
                enemy = target;
                this.damage = damage;
                animator.SetBool(IsAttacking, true);
            }
        }

        private void ApplyDamage()
        {
            if (enemy != null && enemy.Health > 0)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}