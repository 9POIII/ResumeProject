using UnityEngine;

namespace Entities.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Use(BaseEntity target, int damage);
    }
}