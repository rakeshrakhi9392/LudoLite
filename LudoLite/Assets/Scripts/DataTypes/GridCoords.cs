namespace Assets.Scripts.DataTypes
{
    public struct GridCoords
    {
        public int Row { get; }
        public int Col { get; }

        public GridCoords(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool Equals(GridCoords other)
        {
            return Row == other.Row && Col == other.Col;
        }
    }
}