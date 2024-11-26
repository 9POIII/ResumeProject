namespace Entities.Player
{
    public class TestPlayerArcher : CombatEntity
    {
        protected override void Awake()
        {
            base.Awake();
            distanceToStop = 2f;
        }
    }
}