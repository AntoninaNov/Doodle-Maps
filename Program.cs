using System.Collections.Generic;

namespace Kse.Algorithms.Samples
{
    public class DijkstraAlgorithm
    {
        public static List<Point> GetShortestPath(string[,] mapArray, Point start, Point goal)
        {
            var shortestPath = new List<Point>();

            var distanceFromStart = new Dictionary<Point, int>();
            distanceFromStart[start] = 0;

            var priorityQueue = new PriorityQueue<Point, int>();
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count != 0)
            {
                var current = priorityQueue.Dequeue();
                if (current.Equals(goal))
                {
                    break;
                }

                var neighborPoints = GetNeighbours(mapArray, current);
                foreach (var next in neighborPoints)
                {
                    var n = int.Parse(mapArray[next.Column, next.Row]);
                    var newDistance = distanceFromStart[current] + n;

                    if (!distanceFromStart.ContainsKey(next) || newDistance < distanceFromStart[next])
                    {
                        distanceFromStart[next] = newDistance;
                        var priority = newDistance;
                        priorityQueue.Enqueue(next, priority);
                    }
                }
            }

            var currentNode = goal;
            while (!currentNode.Equals(start))
            {
                shortestPath.Add(currentNode);
                var minDistance = int.MaxValue;
                Point next = GetNeighbours(mapArray, currentNode).FirstOrDefault();

                foreach (var neighbour in GetNeighbours(mapArray, currentNode))
                {
                    if (distanceFromStart.ContainsKey(neighbour) && distanceFromStart[neighbour] < minDistance)
                    {
                        minDistance = distanceFromStart[neighbour];
                        next = neighbour;
                    }
                }

                currentNode = next;
            }

            shortestPath.Add(start);
            shortestPath.Reverse();
            return shortestPath;
        }

        public static List<Point> GetNeighbours(string[,] map, Point point)
        {
            var neighborPoints = new List<Point>();
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            foreach (var offset in new[] { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) })
            {
                var neighbour = point.Add(offset);
                if (neighbour.Column >= 0 && neighbour.Column < width && neighbour.Row >= 0 && neighbour.Row < height &&
                    map[neighbour.Column, neighbour.Row] != "█")
                {
                    neighborPoints.Add(neighbour);
                }
            }

            return neighborPoints;
        }

        public static void Main(string[] args)
        {
            var generator = new MapGenerator(new MapGeneratorOptions()
            {
                Height = 35,
                Width = 90,
                Seed = 10,
                Noise = .1f,
                AddTraffic = true,
            });

            string[,] mapArray = generator.Generate();
            var visited = new List<Point>();

            Console.WriteLine("Write (even; [<=88;<=34]) coordinates, please: ");
            var startAndEnd = Console.ReadLine();
            
            

            while (startAndEnd.Length != 0)
            {
                var startEnd = startAndEnd.Split(" ");
                var column1 = Convert.ToInt32(startEnd[0]);
                var row1 = Convert.ToInt32(startEnd[1]);
                var column2 = Convert.ToInt32(startEnd[2]);
                var row2 = Convert.ToInt32(startEnd[3]);

                var startPoint = new Point(column1, row1);
                var endPoint = new Point(column2, row2);

                var path = AStarAlgorithm(mapArray, startPoint, endPoint);
                new MapPrinter().Print(mapArray, path);
                Console.WriteLine($"The shortest path from ({column1}, {row1}) to ({column2}, {row2}) is:");

                foreach (var point in path)
                {
                    Console.WriteLine($"({point.Column}, {point.Row})");
                }

                Console.WriteLine("Write (even; [<=88;<=34]) coordinates, please: ");
                startAndEnd = Console.ReadLine();
                mapArray = generator.Generate();
            }
        }
        
        public static List<Point> AStarAlgorithm(string[,] mapArray, Point start, Point goal)
        {
            var shortestPath = new List<Point>();
            var properColumn = goal.Column;
            var properRow = goal.Row;

            var distanceStartEnd = new Dictionary<Point, (int, int, Point)>();
            distanceStartEnd[start] = (0, int.Abs(start.Column - properColumn) + int.Abs(start.Row - properRow), start);

            var priorityQueue = new PriorityQueue<Point, int>();
            priorityQueue.Enqueue(start, distanceStartEnd[start].Item1 + distanceStartEnd[start].Item2);

            while (priorityQueue.Count != 0)
            {
                var current = priorityQueue.Dequeue();
                if (current.Equals(goal))
                {
                    break;
                }

                var neighborPoints = GetNeighbours(mapArray, current);
                foreach (var next in neighborPoints)
                {
                    var n = int.Parse(mapArray[next.Column, next.Row]);
                    var newDistance = distanceStartEnd[current].Item1 + n;

                    if (!distanceStartEnd.ContainsKey(next) || newDistance < distanceStartEnd[next].Item1)
                    {
                        distanceStartEnd[next] = (newDistance, int.Abs(next.Column - properColumn) + int.Abs(next.Row - properRow), current);
                        var priority = distanceStartEnd[start].Item1 + distanceStartEnd[start].Item2;
                        priorityQueue.Enqueue(next, priority);
                    }
                }
            }

            var currentNode = goal;
            //var distance = distanceStartEnd[goal].Item1 + distanceStartEnd[goal].Item2;
            while (true)
            {
                shortestPath.Add(currentNode);
                if (Equals(start, currentNode))
                {
                    shortestPath.Reverse();
                    return shortestPath;
                }
                currentNode = distanceStartEnd[currentNode].Item3;
            }
        }

    }
}