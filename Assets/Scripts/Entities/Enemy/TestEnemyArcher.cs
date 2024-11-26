namespace Entities.Enemy
{
    public class TestEnemyArcher : CombatEntity
    {
        protected override void Awake()
        {
            base.Awake();
            distanceToStop = 2f;
        }
    }
}