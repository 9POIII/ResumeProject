using Entities;

namespace Interfaces
{
    public interface IWeaponUsage
    {
        public void Use(BaseEntity target, int damage);
    }
}