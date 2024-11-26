using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.InBattleShop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Button nearFighterButton;
        [SerializeField] private Button distanceFighterButton;
        [SerializeField] private Button longDistanceFighterButton;
        [SerializeField] private TMP_Text balanceText;

        public Button NearFighterButton => nearFighterButton;
        public Button DistanceFighterButton => distanceFighterButton;
        public Button LongDistanceFighterButton => longDistanceFighterButton;

        public void SetBalanceText(string text)
        {
            balanceText.text = text;
        }

        public void SetButtonState(Button button, bool interactable)
        {
            button.interactable = interactable;
        }
    }
}