using UnityEngine;

namespace Entities.Weapons
{
    public class Bow : Weapon
    {
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform arrowSpawnPoint;
        public override void Use(BaseEntity target, int damage)
        {
            if (animator != null)
            {
                enemy = target;
                this.damage = damage;
                animator.SetBool(IsAttacking, true);
            }
        }

        private void ShootArrow()
        {
            GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            Arrow arrow = arrowObj.GetComponent<Arrow>();

            if (arrow != null)
            {
                arrow.Initialize(enemy, damage);
            }
        }
    }
}