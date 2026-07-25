using UnityEngine;

namespace Assets.Scripts.UI.Tiles
{
    public class TileFactory
    {
        public static TileComponent CreateTile(GameObject tilePrefab, Color bgColor, Color tileThemeColor, bool arrowVisible, Transform parent)
        {
            var tile = Object.Instantiate(tilePrefab, parent).GetComponent<TileComponent>();
            
            tile.InitializeTile(bgColor, tileThemeColor, arrowVisible);

            return tile;
        }
    }
}
