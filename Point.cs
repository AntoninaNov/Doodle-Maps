namespace Kse.Algorithms.Samples
{
    public struct Point
    {
        public int Column { get; }
        public int Row { get; }

        public Point(int column, int row)
        {
            Column = column;
            Row = row;
        }
        public Point Add(Point other)
        {
            return new Point(Column + other.Column, Row + other.Row);
        }
    }
}