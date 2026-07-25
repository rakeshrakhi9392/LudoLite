using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.UI.Tiles;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Logic
{
    public static class ChipMover
    {
        private const float MoveStepDelaySeconds = 0.3f;

        public static readonly GridCoords StartTileGridPositionInTileGrid = new(1, 2);

        public static readonly GridCoords BottomRightGridPositionInTileGrid = new(5, 2);

        public static readonly GridCoords BottomLeftGridPositionInTileGrid = new(5, 0);

        public static readonly GridCoords BottomMiddleGridPositionInTileGrid = new(5, 1);

        /// <summary>
        /// Moves the playerChip depending on the value of GameplayManager.Instance.LastDiceRoll.
        /// If dice roll value is more than the remaining steps to home, then skip moving
        /// </summary>
        /// <param name="playerChip"></param>
        /// <param name="ludoBoardUiComponent"></param>
        /// <returns></returns>
        public static IEnumerator Move(PlayerChip playerChip, LudoBoardUiComponent ludoBoardUiComponent)
        {
            if (playerChip.IsHomeReached) yield break;

            if (DistanceToHome(playerChip, ludoBoardUiComponent) < GameplayManager.Instance.LastDiceRoll) yield break;

            for (var i = 1; i <= GameplayManager.Instance.LastDiceRoll; i++)
            {
                MoveOneStep(playerChip, ludoBoardUiComponent);

                GameplayManager.Instance.PlayChipMoveAudio();

                yield return new WaitForSeconds(MoveStepDelaySeconds);
            }
        }

        /// <summary>
        /// Move one step towards player home then update chip parent transforms
        /// </summary>
        /// <param name="playerChip"></param>
        /// <param name="ludoBoardUiComponent"></param>
        private static void MoveOneStep(PlayerChip playerChip, LudoBoardUiComponent ludoBoardUiComponent)
        {
            UpdateChipWithNextGridTilesAndPos(playerChip, ludoBoardUiComponent);

            UpdateChipTransformParent(playerChip, ludoBoardUiComponent);
        }

        /// <summary>
        /// Update chip grid coordinates after moving one step towards player home according to the following flow (implemented logic is based on non-rotated tiles):
        /// - If position is reset then jump to the first position of the same tile color as chip
        /// - else if the chip already reached home then set a flag and do nothing
        /// - otherwise follow steps moving logic explained in the function 
        /// </summary>
        /// <param name="playerChip"></param>
        /// <param name="ludoBoardUiComponent"></param>
        private static void UpdateChipWithNextGridTilesAndPos(PlayerChip playerChip, LudoBoardUiComponent ludoBoardUiComponent)
        {
            var clockwiseNextTilesGridMap = new Dictionary<TilesGrid, TilesGrid>()
            {
                { ludoBoardUiComponent.TilesGridYellow, ludoBoardUiComponent.TilesGridGreen },
                { ludoBoardUiComponent.TilesGridGreen, ludoBoardUiComponent.TilesGridRed },
                { ludoBoardUiComponent.TilesGridRed, ludoBoardUiComponent.TilesGridBlue },
                { ludoBoardUiComponent.TilesGridBlue, ludoBoardUiComponent.TilesGridYellow }
            };

            var homeTileGrid = ludoBoardUiComponent.GetStartingTilesGridForColorOption(playerChip.PlayerColorOption);

            if (playerChip.IsPositionReset)
            {
                playerChip.CurrentTilesGrid = homeTileGrid;
                    ludoBoardUiComponent.GetStartingTilesGridForColorOption(playerChip.PlayerColorOption);
                playerChip.CurrentGridPosition = StartTileGridPositionInTileGrid;

                playerChip.IsPositionReset = false;
                return;
            }

            var newGridPosition = new GridCoords(playerChip.CurrentGridPosition.Row, playerChip.CurrentGridPosition.Col);

            if (playerChip.CurrentTilesGrid == homeTileGrid &&
                playerChip.CurrentGridPosition.Equals(BottomMiddleGridPositionInTileGrid))
            {
                playerChip.IsHomeReached = true;
            }
            else if (playerChip.CurrentTilesGrid == homeTileGrid &&
                playerChip.CurrentGridPosition.Col == 1)
            {
                // Start going closely towards home (<= 6 steps)
                newGridPosition = new GridCoords(playerChip.CurrentGridPosition.Row + 1, playerChip.CurrentGridPosition.Col);
            }
            else if (playerChip.CurrentGridPosition.Row > 0 && playerChip.CurrentGridPosition.Col == 0)
            {
                // Moving up on the left edge of each grid
                newGridPosition = new GridCoords(playerChip.CurrentGridPosition.Row - 1, playerChip.CurrentGridPosition.Col);
            }
            else if (playerChip.CurrentGridPosition.Row == 0 && playerChip.CurrentGridPosition.Col < playerChip.CurrentTilesGrid.GridTiles.GetLength(1) - 1)
            {
                // moving from right to left on the top edge of the grid
                newGridPosition = new GridCoords(playerChip.CurrentGridPosition.Row, playerChip.CurrentGridPosition.Col + 1);
            }
            else if (playerChip.CurrentGridPosition.Equals(BottomRightGridPositionInTileGrid))
            {
                // jump to the bottom left of the next grid
                playerChip.CurrentTilesGrid = clockwiseNextTilesGridMap[playerChip.CurrentTilesGrid];
                newGridPosition = BottomLeftGridPositionInTileGrid;
            }
            else
            {
                // Moving down on the right edge of each grid
                newGridPosition = new GridCoords(playerChip.CurrentGridPosition.Row + 1, playerChip.CurrentGridPosition.Col);
            }

            playerChip.CurrentGridPosition = newGridPosition;
        }

        /// <summary>
        /// Move chip by changing its transform to be child of the result tile
        /// </summary>
        /// <param name="playerChip"></param>
        /// <param name="ludoBoardUiComponent"></param>
        private static void UpdateChipTransformParent(PlayerChip playerChip, LudoBoardUiComponent ludoBoardUiComponent)
        {
            if (playerChip.IsHomeReached)
            {
                playerChip.transform.SetParent(
                    ludoBoardUiComponent.GetHomeTransformForColorOption(playerChip.PlayerColorOption));
            }
            else
            {
                playerChip.transform.SetParent(playerChip.CurrentTilesGrid
                    .GridTiles[playerChip.CurrentGridPosition.Row, playerChip.CurrentGridPosition.Col].transform);
            }

            playerChip.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Calculates remaining steps to reach chip home
        /// </summary>
        /// <param name="playerChip"></param>
        /// <param name="ludoBoardUiComponent"></param>
        /// <returns>if remaining steps are less than or equal six, return steps otherwise return int.MaxValue</returns>
        private static int DistanceToHome(PlayerChip playerChip, LudoBoardUiComponent ludoBoardUiComponent)
        {
            var homeTileGrid = ludoBoardUiComponent.GetStartingTilesGridForColorOption(playerChip.PlayerColorOption);

            if (playerChip.CurrentTilesGrid == homeTileGrid &&
                playerChip.CurrentGridPosition.Col == 1)
            {
                return BottomLeftGridPositionInTileGrid.Row - playerChip.CurrentGridPosition.Row + 1;
            }

            return int.MaxValue;;
        }
    }
}
