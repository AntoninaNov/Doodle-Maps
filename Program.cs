using Kse.Algorithms.Samples;

const int height = 35;
const int width = 90;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = height,
    Width = width,
});

string[,] map = generator.Generate();
var visited = new List<Point>();
GetShortestPath(map, new Point(0, 0), new Point(30, 30));

void GetShortestPath(string[,] map, Point start, Point goal)
{   
    var distanceAndOrigin = new GetMaxStructure();
    var visit = new List<Point>();
    var visited = new List<Point>();
    
    visit.Add(start);
    distanceAndOrigin.Add(start, 0, start);
    while (visit.Count != 0)
    {
        var point = distanceAndOrigin.GetElement();
        var neighbours = GetNeighbours(point.Key);
        foreach (var neighbour in neighbours)
        {
            if (!visited.Contains(neighbour))
            {
                if (!visit.Contains(neighbour)) visit.Add(neighbour);
                distanceAndOrigin.Add(neighbour, 1 + point.Value.Item1, point.Key);
            }
        }

        visit.Remove(point.Key);
        visited.Add(point.Key);
    }
    visited.Clear();
    var (path, distance) = distanceAndOrigin.WritePath(goal, start);
    Console.WriteLine(distance);
    new MapPrinter().Print(map, path);
}

List<Point> GetNeighbours(Point point)
{
    var neighbours = new List<Point>();
    if (point.Row > 0 && map[point.Row, point.Column] != "█")
    {
        neighbours.Add(new Point(point.Column, point.Row - 1));
    }
    else if (point.Row < height && map[point.Row, point.Column] != "█")
    {
        neighbours.Add(new Point(point.Column, point.Row + 1));
    }
    if (point.Column > 0 && map[point.Row, point.Column] != "█")
    {
        neighbours.Add(new Point(point.Column - 1, point.Row));
    }
    else if (point.Column < width && map[point.Row, point.Column] != "█")
    {
        neighbours.Add(new Point(point.Column + 1, point.Row));
    }
    //if (point is { Row: > 0, Column: > 0 }) neighbours.Add(new Point(point.Column - 1, point.Row - 1));
    //if (point is { Row: > 0, Column: < width }) neighbours.Add(new Point(point.Column + 1, point.Row - 1));
    //if (point is { Row: < height, Column: > 0 }) neighbours.Add(new Point(point.Column - 1, point.Row + 1));
    //if (point is { Row: < height, Column: < width }) neighbours.Add(new Point(point.Column + 1, point.Row + 1));
    return neighbours;
}

public class GetMaxStructure
{
    private Dictionary<Point, (int, Point)> _points = new Dictionary<Point, (int, Point)>();

    public void Add(Point element, int distance, Point origin)
    {
        if (!_points.ContainsKey(element) || _points[element].Item1 > distance) _points[element] = (distance, origin);
    }

    public KeyValuePair<Point, (int, Point)> GetElement()
    {
        if (_points.Count == 1)
        {
            return _points.First();
        }

        var min = _points.First();
        foreach (var point in _points)
        {
            if (point.Value.Item1 < min.Value.Item1)
            {
                min = point;
            }
        }

        _points.Remove(min.Key);
        return min;
    }

    public (List<Point>, int) WritePath(Point endPoint, Point start)
    {
        var points = new List<Point>();
        var distance = _points[endPoint].Item1;
        while (true)
        {
            points.Add(endPoint);
            if (Equals(start, endPoint))
            {
                points.Reverse();
                return (points, distance);
            }
            endPoint = _points[endPoint].Item2;
        }
    }
}