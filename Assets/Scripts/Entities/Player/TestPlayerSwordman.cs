namespace Entities.Player
{
    public class TestPlayerSwordman : CombatEntity
    {
        protected override void Awake()
        {
            base.Awake();
            distanceToStop = 0.2f;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}