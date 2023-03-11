using Kse.Algorithms.Samples;

const int height = 35;
const int width = 90;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = height,
    Width = width,
    //Noise = 0.3f,
});

string[,] map = generator.Generate();
var visited = new List<Point>();

Console.WriteLine("Write (even; [<=88;<=34]) coordinates, please: ");
var startAndEnd = Console.ReadLine();
while (startAndEnd.Length != 0)
{
    var startEnd = startAndEnd.Split(" ").ToList();
    var column1 = Convert.ToInt32(startEnd[0]);
    var row1 = Convert.ToInt32(startEnd[1]);
    var column2 = Convert.ToInt32(startEnd[2]);
    var row2 = Convert.ToInt32(startEnd[3]);
    GetShortestPath(map, new Point(column1, row1), new Point(column2, row2));
    Console.WriteLine("Write (even; [<=88;<=34]) coordinates, please: ");
    startAndEnd = Console.ReadLine();
}


void GetShortestPath(string[,] map, Point start, Point goal)
{   
    var distanceAndOrigin = new GetMaxStructure();
    var visit = new List<Point>();
    var visited = new List<Point>();
    
    visit.Add(start);
    distanceAndOrigin.Add(start, 0, start);
    while (visit.Count != 0)
    {
        var (point, number) = distanceAndOrigin.GetElement(visit);
        var neighbours = GetNeighbours(point);
        foreach (var neighbour in neighbours)
        {
            if (!visited.Contains(neighbour))
            {
                if (!visit.Contains(neighbour)) visit.Add(neighbour);
                distanceAndOrigin.Add(neighbour, 1 + number, point);
            }
        }

        visit.Remove(point);
        visited.Add(point);
    }
    visited.Clear();
    var (path, distance) = distanceAndOrigin.WritePath(goal, start);
    Console.WriteLine(distance);
    new MapPrinter().Print(map, path);
}

List<Point> GetNeighbours(Point point)
{
    var neighbours = new List<Point>();
    if (point.Row > 0 && map[point.Column, point.Row - 1] != "█")
    {
        neighbours.Add(new Point(point.Column, point.Row - 1));
    }
    if (point.Row < height - 1 && map[point.Column, point.Row + 1] != "█")
    {
        neighbours.Add(new Point(point.Column, point.Row + 1));
    }
    if (point.Column > 0 && map[point.Column - 1, point.Row] != "█")
    {
        neighbours.Add(new Point(point.Column - 1, point.Row));
    }
    if (point.Column < width - 1 && map[point.Column + 1, point.Row] != "█")
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

    public (Point, int) GetElement(List<Point> toVisit)
    {
        if (_points.Count == 1)
        {
            return (_points.Keys.First(), _points.Values.First().Item1);
        }

        var min = toVisit[0];
        var minNum = _points[toVisit[0]].Item1;
        foreach (var point in toVisit)
        {
            if (_points[point].Item1 < minNum)
            {
                min = point;
                minNum = _points[point].Item1;
            }
        }

        return (min, _points[min].Item1);
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