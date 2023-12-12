using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day10
{
    public class Day10Logic : IPuzzles
    {
        private const string fileName = "day10_Input.in";
        private const Int32 BufferSize = 128;
        private static Position start = new Position(0, 0);
        private List<Position> polygon = [];

        public string FirstPuzzle()
        {
            var map = ReadInput();

            GoToAllNeighboursBFS(map, start);

            return map.SelectMany(list => list)
                      .Max(tail => tail.Value)
                      .ToString();
        }

        public string SecondPuzzle()
        {
            var map = ReadInput();
            List<Position> potentiallyInside = [];

            var startNeighbour = map[start.Y][start.X].AvailableMoves.First();
            polygon.Add(start);
            if (map[startNeighbour.Y][startNeighbour.X].Corner)
            {
                polygon.Add(startNeighbour);
            }

            GoToAllNeighboursBFS(map, startNeighbour, true);

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (!map[i][j].Visited)
                    {
                        potentiallyInside.Add(new Position(j, i));
                    }
                }
            }

            var count = 0;

            foreach (var tail in potentiallyInside)
            {
                if (IsPointInPolygon(polygon, tail))
                {
                    count++;
                }
            }

            return count.ToString();
        }

        private static bool IsPointInPolygon(List<Position> polygon, Position testPoint)
        {
            var inside = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                var xi = polygon[i].X;
                var yi = polygon[i].Y;
                var xj = polygon[j].X;
                var yj = polygon[j].Y;

                var intersect = ((yi > testPoint.Y) != (yj > testPoint.Y)) && (testPoint.X < (xj - xi) * (testPoint.Y - yi) / (yj - yi) + xi);
                if (intersect)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private void GoToAllNeighboursBFS(List<List<Tail>> map, Position from, bool createPolygon = false)
        {
            Queue<Position> queue = new Queue<Position>();
            var tailToModify = map[from.Y][from.X];
            tailToModify.Visited = true;
            map[from.Y][from.X] = tailToModify;

            queue.Enqueue(from);

            while (queue.Count > 0)
            {
                Position v = queue.Dequeue();
                foreach (var move in map[v.Y][v.X].AvailableMoves)
                {
                    if (!map[move.Y][move.X].Visited)
                    {
                        if(createPolygon && map[move.Y][move.X].Corner)
                        {
                            polygon.Add(new Position(move.X, move.Y));
                        }

                        tailToModify = map[move.Y][move.X];
                        tailToModify.Visited = true;
                        tailToModify.Value = map[v.Y][v.X].Value + 1;
                        map[move.Y][move.X] = tailToModify;
                        queue.Enqueue(move);
                    }
                }
            }
        }

        private void GoToAllNeighboursDFS(List<List<Tail>> map, Position from, int value)
        {
            foreach (var availableMove in map[from.Y][from.X].AvailableMoves)
            {
                if (!map[availableMove.Y][availableMove.X].Visited)
                {
                    var tailToModify = map[availableMove.Y][availableMove.X];
                    tailToModify.Visited = true;
                    tailToModify.Value = value + 1;
                    map[availableMove.Y][availableMove.X] = tailToModify;

                    if (tailToModify.Corner)
                    {
                        polygon.Add(new Position(availableMove.X, availableMove.Y));
                    }

                    GoToAllNeighboursDFS(map, availableMove, value + 1);
                }
            }
        }

        private List<Position> FindStartNeighbours(List<List<Tail>> map, List<Position> allNeighbours, Position from)
        {
            List<Position> startAvailableMoves = [];

            foreach (var neighbour in allNeighbours)
            {
                if (neighbour.X >= 0 && neighbour.X < map.First().Count && neighbour.Y >= 0 && neighbour.Y < map.Count)
                {
                    if (map[neighbour.Y][neighbour.X].AvailableMoves.Contains(from))
                    {
                        startAvailableMoves.Add(new Position(neighbour.X, neighbour.Y));
                    }
                }
            }

            return startAvailableMoves;
        }

        private List<List<Tail>> ReadInput()
        {
            List<List<Tail>> map = [];
            List<Position> allStartNeighbours = [];

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                var lineNumber = 0;
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    List<Tail> tails = [];

                    for (int i = 0; i < line.Length; i++)
                    {
                        List<Position> availableMoves = [];
                        var corner = false;

                        var up = new Position(i, lineNumber - 1);
                        var down = new Position(i, lineNumber + 1);
                        var left = new Position(i - 1, lineNumber);
                        var right = new Position(i + 1, lineNumber);

                        switch (line[i])
                        {
                            case '|':
                                availableMoves.Add(up);
                                availableMoves.Add(down);
                                break;
                            case '-':
                                availableMoves.Add(left);
                                availableMoves.Add(right);
                                break;
                            case 'L':
                                availableMoves.Add(up);
                                availableMoves.Add(right);
                                corner = true;
                                break;
                            case 'J':
                                availableMoves.Add(up);
                                availableMoves.Add(left);
                                corner = true;
                                break;
                            case '7':
                                availableMoves.Add(down);
                                availableMoves.Add(left);
                                corner = true;
                                break;
                            case 'F':
                                availableMoves.Add(down);
                                availableMoves.Add(right);
                                corner = true;
                                break;
                            case '.':
                                break;
                            case 'S':
                                start = new Position(i, lineNumber);
                                allStartNeighbours = [up, down, left, right];
                                corner = true;
                                tails.Add(new Tail(availableMoves, corner, 0, true));
                                continue;
                            default:
                                break;
                        }

                        tails.Add(new Tail(availableMoves, corner));
                    }

                    map.Add(tails);

                    lineNumber++;
                }

                var pointToUpdate = map[start.Y][start.X];
                var availableMovesFromStart = FindStartNeighbours(map, allStartNeighbours, start);
                map[start.Y].Remove(pointToUpdate);
                pointToUpdate.AvailableMoves = availableMovesFromStart;
                map[start.Y].Insert(start.X, pointToUpdate);
            }

            return map;
        }

        private struct Tail
        {
            public Tail(List<Position> availableMoves, bool corner, int value = 0, bool visited = false)
            {
                AvailableMoves = availableMoves;
                Value = value;
                Visited = visited;
                Corner = corner;
            }

            public List<Position> AvailableMoves { get; set; }
            public int Value { get; set; }
            public bool Visited { get; set; }
            public bool Corner { get; set; }
        }

        private struct Position
        {
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}