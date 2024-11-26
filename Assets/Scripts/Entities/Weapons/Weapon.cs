using System;
using Interfaces;
using UnityEngine;

namespace Entities.Weapons
{
    public abstract class Weapon : MonoBehaviour, IWeaponUsage
    {
        protected Animator animator;
        protected static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        protected BaseEntity enemy;
        protected int damage;

        protected void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public abstract void Use(BaseEntity target, int damage);
    }
}