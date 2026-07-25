using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tiles
{
    public class TileComponent : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;

        [SerializeField] private Image _arrowImage;

        public void InitializeTile(Color bgColor, Color tileThemeColor, bool arrowVisible)
        {
            _arrowImage.enabled = arrowVisible;
            _bgImage.color = bgColor;

            if (arrowVisible)
            {
                _arrowImage.color = tileThemeColor;
            }
        }
    }
}
