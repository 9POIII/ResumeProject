using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Weapons
{
    public class BatWeapon : Weapon
    {
        private Animator animator;
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        private BaseEntity enemy;
        private int damage;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

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
                Debug.Log($"Damage applied to {enemy.name}");
            }
        }
    }
}