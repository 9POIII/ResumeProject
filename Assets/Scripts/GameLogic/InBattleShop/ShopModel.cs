using System.Collections.Generic;

namespace GameLogic.InBattleShop
{
    public class ShopModel
    {
        public int PlayerBalance { get; private set; }
        private Dictionary<UnitSpawner.TypesOfUnits, int> unitPrices;

        public ShopModel()
        {
            PlayerBalance = 100;
            unitPrices = new Dictionary<UnitSpawner.TypesOfUnits, int>
            {
                { UnitSpawner.TypesOfUnits.NearFighter, 10 },
                { UnitSpawner.TypesOfUnits.DistanceFighter, 20 },
                { UnitSpawner.TypesOfUnits.LongDistanceFighter, 30 }
            };
        }

        public int GetUnitPrice(UnitSpawner.TypesOfUnits type)
        {
            return unitPrices.GetValueOrDefault(type, 0);
        }

        public bool CanAfford(UnitSpawner.TypesOfUnits type)
        {
            return PlayerBalance >= GetUnitPrice(type);
        }

        public bool BuyUnit(UnitSpawner.TypesOfUnits type)
        {
            int price = GetUnitPrice(type);
            if (PlayerBalance >= price)
            {
                PlayerBalance -= price;
                return true;
            }

            return false;
        }

        public void AddMoney(int amount)
        {
            PlayerBalance += amount;
        }
    }
}