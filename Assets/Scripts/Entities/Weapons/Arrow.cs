using UnityEngine;

namespace Entities.Weapons
{
    public class Arrow : MonoBehaviour
    {
        private BaseEntity target;
        private int damage;
        private float speed = 3f;

        public void Initialize(BaseEntity target, int damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
                HitTarget();
        }

        private void HitTarget()
        {
            if (target != null)
                target.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}