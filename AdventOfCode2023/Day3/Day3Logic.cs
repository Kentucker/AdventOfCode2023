using AdventOfCode2023.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day3
{
    public class Day3Logic : IPuzzles
    {
        private const string fileName = "day3_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            long result = 0;
            int schemaRow = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                Dictionary<Position, int> partNumbers = [];
                List<Position> symbolsPositionList = [];
                var numbersRegex = new Regex(@"\b\d+\b");
                var symbolsRegex = new Regex(@"[!$%^&*()_+|~=`{}[\]:;""'<>@#,?/-]");
                var asterisksRegex = new Regex(@"[*]");

                while ((line = streamReader.ReadLine()) != null)
                {
                    var numbersMatches = numbersRegex.Matches(line).ToArray();
                    var symbolsMatches = symbolsRegex.Matches(line).ToArray();

                    foreach (var match in numbersMatches)
                    {
                        partNumbers.Add(new Position(match.Index, schemaRow), int.Parse(match.Value));
                    }

                    foreach (var match in symbolsMatches)
                    {
                        symbolsPositionList.Add(new Position(match.Index, schemaRow));
                    }

                    schemaRow++;
                }

                result = ChooseAndSumNumbers(partNumbers, symbolsPositionList);
            }

            return result.ToString();
        }

        public string SecondPuzzle()
        {
            long result = 0;
            int schemaRow = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                Dictionary<Position, int> partNumbers = [];
                List<Position> asterisksPositionList = [];
                var numbersRegex = new Regex(@"\b\d+\b");
                var asterisksRegex = new Regex(@"[*]");

                while ((line = streamReader.ReadLine()) != null)
                {
                    var numbersMatches = numbersRegex.Matches(line).ToArray();
                    var asterisksMatches = asterisksRegex.Matches(line).ToArray();

                    foreach (var match in numbersMatches)
                    {
                        partNumbers.Add(new Position(match.Index, schemaRow), int.Parse(match.Value));
                    }

                    foreach (var match in asterisksMatches)
                    {
                        asterisksPositionList.Add(new Position(match.Index, schemaRow));
                    }

                    schemaRow++;
                }

                result = ChooseAndSumNumbersAroundAsterisks(partNumbers, asterisksPositionList);
            }

            return result.ToString();
        }

        private int ChooseAndSumNumbers(Dictionary<Position, int> partNumbers, List<Position> symbolsPositionList)
        {
            var sum = 0;

            foreach (var number in partNumbers)
            {
                if (HasSymbolAround(number, symbolsPositionList) is not null)
                {
                    sum += number.Value;
                }
            }

            return sum;
        }

        private long ChooseAndSumNumbersAroundAsterisks(Dictionary<Position, int> partNumbers, List<Position> symbolsPositionList)
        {
            long sum = 0;
            List<Gear> gearValues = [];

            foreach (var number in partNumbers)
            {
                var position = HasSymbolAround(number, symbolsPositionList);

                if (position is not null)
                {
                    gearValues.Add(new Gear(position, number.Value));
                }
            }

            sum = gearValues
                .GroupBy(g => g.Position.Value)
                .Where(samePosition => samePosition.Skip(1).Any())
                .Select(group => group.Select(g => g.Value).Aggregate((a, b) => a * b))
                .Sum(sum => sum);

            return sum;
        }

        private Position? HasSymbolAround(KeyValuePair<Position,int> number, List<Position> symbolsPositionList)
        {
            var xOffsets = Enumerable.Range(-1, number.Value.ToString().Length+2).ToList();
            var yOffsets = Enumerable.Range(-1, 3).ToList();

            for (int i = 0; i < number.Value.ToString().Length; i++)
            {
                foreach (var xOffset in xOffsets)
                {
                    foreach (var yOffset in yOffsets)
                    {
                        if (xOffset == 0 && yOffset == 0)
                        {
                            continue;
                        }

                        var adjacentPosition = new Position(number.Key.X + xOffset, number.Key.Y + yOffset);

                        if (symbolsPositionList.Contains(adjacentPosition))
                        {
                            return adjacentPosition;
                        }
                    }
                }
            }

            return null;
        }

        struct Position
        {
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        struct Gear
        {
            public Gear(Position? position, int value)
            {
                Position = position;
                Value = value;
            }

            public Position? Position { get; set; }
            public int Value { get; set; }
        }
    }
}