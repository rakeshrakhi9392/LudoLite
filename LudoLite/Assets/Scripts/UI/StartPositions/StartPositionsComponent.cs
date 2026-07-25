using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.StartPositions
{
    public class StartPositionsComponent : MonoBehaviour
    {
        public RectTransform[] Positions => _positions;

        [SerializeField] private PlayerColorOption _playerColorOption;

        [SerializeField] private Sprite _yellowSprite;

        [SerializeField] private Sprite _greenSprite;

        [SerializeField] private Sprite _blueSprite;

        [SerializeField] private Sprite _redSprite;

        [SerializeField] RectTransform[] _positions;

        private void Start()
        {
            var colorSpriteMap = new Dictionary<PlayerColorOption, Sprite>()
            {
                { PlayerColorOption.Yellow, _yellowSprite },
                { PlayerColorOption.Green, _greenSprite },
                { PlayerColorOption.Blue, _blueSprite },
                { PlayerColorOption.Red, _redSprite }
            };

            GetComponent<Image>().sprite = colorSpriteMap[_playerColorOption];
        }
    }
}
