using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day9
{
    public class Day9Logic : IPuzzles
    {
        private const string fileName = "day9_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            int result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var values = line.Split(' ');

                    var sequence = values.Select(int.Parse).ToList();

                    var nextValue = CalculateNextValue(sequence);

                    result += nextValue;
                }
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

        private int CalculateNextValue(List<int> sequence)
        {
            var sum = sequence.Last();

            while (sequence.Distinct().Count() != 1)
            {
                List<int> differences = [];

                for (int i = 1; i < sequence.Count; i++)
                {
                    differences.Add(sequence[i] - sequence[i - 1]);
                }

                sum += differences.Last();
                sequence = differences;
            }

            return sum;
        }
    }
}