using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day10
{
    public class Day10Logic : IPuzzles
    {
        private const string fileName = "day10_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            Position start = new Position(0, 0);
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
                                break;
                            case 'J':
                                availableMoves.Add(up);
                                availableMoves.Add(left);
                                break;
                            case '7':
                                availableMoves.Add(down);
                                availableMoves.Add(left);
                                break;
                            case 'F':
                                availableMoves.Add(down);
                                availableMoves.Add(right);
                                break;
                            case '.':
                                break;
                            case 'S':
                                start = new Position(i, lineNumber);
                                allStartNeighbours = [ up, down, left, right ];
                                break;
                            default:
                                break;
                        }

                        tails.Add(new Tail(availableMoves));
                    }

                    map.Add(tails);

                    lineNumber++;
                }

                var xyz = map[start.Y][start.X];
                var availableMovesFromStart = FindStartNeighbours(map, allStartNeighbours, start);
                map[start.Y].Remove(xyz);
                xyz.AvailableMoves = availableMovesFromStart;
                map[start.Y].Insert(start.X, xyz);

                GoToAllNeighbours(map, start);
            }

            return map.SelectMany(list => list)
                      .Max(tail => tail.Value)
                      .ToString();
        }

        public string SecondPuzzle()
        {
            long result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {

                }
            }

            return result.ToString();
        }

        private void GoToAllNeighbours(List<List<Tail>> map, Position from)
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
                        tailToModify = map[move.Y][move.X];
                        tailToModify.Visited = true;
                        tailToModify.Value = map[v.Y][v.X].Value + 1;
                        map[move.Y][move.X] = tailToModify;
                        queue.Enqueue(move);
                    }
                }
            }
        }

        private List<Position> FindStartNeighbours(List<List<Tail>> map, List<Position> allNeighbours, Position from)
        {
            List<Position> startAvailableMoves = [];

            foreach (var neighbour in allNeighbours)
            {
                if(neighbour.X >=0 && neighbour.X < map.First().Count && neighbour.Y >=0 && neighbour.Y < map.Count)
                {
                    if (map[neighbour.Y][neighbour.X].AvailableMoves.Contains(from))
                    {
                        startAvailableMoves.Add(new Position(neighbour.X, neighbour.Y));
                    }
                }
            }

            return startAvailableMoves;
        }

        private struct Tail
        {
            public Tail(List<Position> availableMoves, int value = 0, bool visited = false)
            {
                AvailableMoves = availableMoves;
                Value = value;
                Visited = visited;
            }

            public List<Position> AvailableMoves { get; set; }
            public int Value { get; set; }
            public bool Visited { get; set; }
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