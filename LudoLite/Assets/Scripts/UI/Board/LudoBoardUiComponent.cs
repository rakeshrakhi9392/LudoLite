using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.UI.StartPositions;
using Assets.Scripts.UI.Tiles;
using UnityEngine;

public class LudoBoardUiComponent : MonoBehaviour
{
    public const int LudoBoardGridSize = 15;

    public const int TileGridNumColumns = 3;

    public const int TileGridNumRows = 6;

    public const int StartPositionsGridSize = 6;

    public const int HomeGridSize = 3;

    public float CellPixelSize => _boardPixelSize / LudoBoardGridSize;

    public TilesGrid TilesGridYellow => _tilesGridYellow;
    public TilesGrid TilesGridGreen => _tilesGridGreen;
    public TilesGrid TilesGridBlue => _tilesGridBlue;
    public TilesGrid TilesGridRed => _tilesGridRed;

    [SerializeField] private RectTransform _rtLudoBoard;

    [SerializeField] private StartPositionsComponent _startPositionsYellow;
    [SerializeField] private StartPositionsComponent _startPositionsGreen;
    [SerializeField] private StartPositionsComponent _startPositionsBlue;
    [SerializeField] private StartPositionsComponent _startPositionsRed;

    [SerializeField] private RectTransform _rtHome;

    [SerializeField] private TilesGrid _tilesGridYellow;
    [SerializeField] private TilesGrid _tilesGridGreen;
    [SerializeField] private TilesGrid _tilesGridBlue;
    [SerializeField] private TilesGrid _tilesGridRed;

    [SerializeField] private RectTransform _homeYellow;

    [SerializeField] private RectTransform _homeGreen;

    [SerializeField] private RectTransform _homeBlue;

    [SerializeField] private RectTransform _homeRed;

    private TilesGrid[] _tilesGridArray;

    private StartPositionsComponent[] _startPositionsArray;

    private float _boardPixelSize;

    private void Awake()
    {
        _startPositionsArray = new []
        {
            _startPositionsYellow,
            _startPositionsGreen,
            _startPositionsBlue,
            _startPositionsRed,
        };

        _tilesGridArray = new[]
        {
            _tilesGridYellow,
            _tilesGridGreen,
            _tilesGridBlue,
            _tilesGridRed,
        };
    }

    public void FixSize(float boardPixelSize)
    {
        _boardPixelSize = boardPixelSize;
        var startPositionsPixelSize = StartPositionsGridSize * CellPixelSize;
        var homePixelSize = HomeGridSize * CellPixelSize;

        _rtLudoBoard.sizeDelta = new Vector2(boardPixelSize, boardPixelSize);

        foreach (var startPos in _startPositionsArray)
        {
            startPos.GetComponent<RectTransform>().sizeDelta = new Vector2(startPositionsPixelSize, startPositionsPixelSize);

            startPos.Positions[0].anchoredPosition = new Vector2(-CellPixelSize, CellPixelSize);
            startPos.Positions[1].anchoredPosition = new Vector2(CellPixelSize, CellPixelSize);
            startPos.Positions[2].anchoredPosition = new Vector2(-CellPixelSize, -CellPixelSize);
            startPos.Positions[3].anchoredPosition = new Vector2(CellPixelSize, -CellPixelSize);
        }

        foreach (var tg in _tilesGridArray)
        {
            tg.FixSize(CellPixelSize);
        }


        _rtHome.sizeDelta = new Vector2(homePixelSize, homePixelSize);
    }

    public Vector2 GetPixelPositionForRowCol(int row, int col)
    {
        return new Vector2(col * CellPixelSize, -row * CellPixelSize);
    }

    public StartPositionsComponent GetStartPositionComponentForColorOption(PlayerColorOption playerColorOption)
    {
        var map = new Dictionary<PlayerColorOption, StartPositionsComponent>()
        {
            { PlayerColorOption.Yellow, _startPositionsYellow },
            { PlayerColorOption.Green, _startPositionsGreen },
            { PlayerColorOption.Blue, _startPositionsBlue },
            { PlayerColorOption.Red, _startPositionsRed }
        };

        return map[playerColorOption];
    }

    public TilesGrid GetStartingTilesGridForColorOption(PlayerColorOption playerColorOption)
    {
        var map = new Dictionary<PlayerColorOption, TilesGrid>()
        {
            { PlayerColorOption.Yellow, _tilesGridYellow },
            { PlayerColorOption.Green, _tilesGridGreen },
            { PlayerColorOption.Blue, _tilesGridBlue },
            { PlayerColorOption.Red, _tilesGridRed }
        };

        return map[playerColorOption];
    }

    public RectTransform GetHomeTransformForColorOption(PlayerColorOption playerColorOption)
    {
        var map = new Dictionary<PlayerColorOption, RectTransform>()
        {
            { PlayerColorOption.Yellow, _homeYellow },
            { PlayerColorOption.Green, _homeGreen },
            { PlayerColorOption.Blue, _homeBlue },
            { PlayerColorOption.Red, _homeRed }
        };

        return map[playerColorOption];
    }
}
