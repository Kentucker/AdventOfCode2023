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
            int result = 0;

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

        private int ChooseAndSumNumbers(Dictionary<Position, int> partNumbers, List<Position> symbolsPositionList)
        {
            var sum = 0;

            foreach (var number in partNumbers)
            {
                if (HasSymbolAround(number, symbolsPositionList))
                {
                    sum += number.Value;
                }
            }

            return sum;
        }

        private bool HasSymbolAround(KeyValuePair<Position,int> number, List<Position> symbolsPositionList)
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
                            return true;
                        }
                    }
                }
            }

            return false;
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
    }
}