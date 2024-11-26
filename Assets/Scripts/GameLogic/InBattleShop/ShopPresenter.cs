using System.Collections;
using UnityEngine;

namespace GameLogic.InBattleShop
{
    public class ShopPresenter : MonoBehaviour
    {
        [SerializeField] private ShopView shopView;
        [SerializeField] private UnitSpawner unitSpawner;

        private ShopModel shopModel;

        private const int MoneyPerSecond = 5;

        private void Awake()
        {
            shopModel = new ShopModel();

            shopView.NearFighterButton.onClick.AddListener(() => OnBuyButtonClicked(UnitSpawner.TypesOfUnits.NearFighter));
            shopView.DistanceFighterButton.onClick.AddListener(() => OnBuyButtonClicked(UnitSpawner.TypesOfUnits.DistanceFighter));
            shopView.LongDistanceFighterButton.onClick.AddListener(() => OnBuyButtonClicked(UnitSpawner.TypesOfUnits.LongDistanceFighter));
        }

        private void Start()
        {
            UpdateUI();
            StartCoroutine(AddMoneyOverTime());
        }

        private void OnBuyButtonClicked(UnitSpawner.TypesOfUnits type)
        {
            if (shopModel.BuyUnit(type))
            {
                unitSpawner.SpawnUnit(type);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            shopView.SetBalanceText($"Balance: {shopModel.PlayerBalance}");

            shopView.SetButtonState(shopView.NearFighterButton, shopModel.CanAfford(UnitSpawner.TypesOfUnits.NearFighter));
            shopView.SetButtonState(shopView.DistanceFighterButton, shopModel.CanAfford(UnitSpawner.TypesOfUnits.DistanceFighter));
            shopView.SetButtonState(shopView.LongDistanceFighterButton, shopModel.CanAfford(UnitSpawner.TypesOfUnits.LongDistanceFighter));
        }

        private IEnumerator AddMoneyOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                shopModel.AddMoney(MoneyPerSecond);
                UpdateUI();
            }
        }
    }
}