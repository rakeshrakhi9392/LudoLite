using Assets.Scripts.Gameplay.Logic;
using Assets.Scripts.UI.Dice;
using UnityEngine;

namespace Assets.Scripts.UI.Game
{
    public class GamePanelUiComponent : MonoBehaviour
    {
        public PlayerChip[] PlayerChips => _playerChips;

        private const float MinButtonsPanelHeight = 300f;

        [SerializeField] private LudoBoardUiComponent _ludoBoardUiComponent;

        [SerializeField] private RectTransform _rtButtonsPanel;

        [SerializeField] private PlayerChip[] _playerChips;

        // Start is called before the first frame update
        void Start()
        {
            var boardPixelSize = Mathf.Min(Screen.height - MinButtonsPanelHeight, Screen.width);
            var buttonsPanelHeight = Screen.height - boardPixelSize;

            _ludoBoardUiComponent.FixSize(boardPixelSize);
            _rtButtonsPanel.sizeDelta = new Vector2(Screen.width, buttonsPanelHeight);

            foreach (var chip in _playerChips)
            {
                chip.FixSize(_ludoBoardUiComponent.CellPixelSize);
            }
            
        }

        public void RollButtonClick(DiceAnimate diceAnimate)
        {
            GameplayManager.Instance.RollDice(diceAnimate);
        }

        public void ResetButtonClick()
        {
            GameplayManager.Instance.ResetPositions(_playerChips);
        }
    }
}
