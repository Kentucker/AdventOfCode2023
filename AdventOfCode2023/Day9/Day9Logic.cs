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
            long result = 0;

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
            long result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var values = line.Split(' ');

                    var sequence = values.Select(int.Parse).ToList();

                    var previousValue = CalculatePreviousValue(sequence);

                    result += previousValue;
                }
            }

            return result.ToString();
        }

        private long CalculateNextValue(List<int> sequence)
        {
            long sum = sequence.Last();

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

        private long CalculatePreviousValue(List<int> sequence)
        {
            List<int> firstNumbers = [ sequence.First() ];

            while (sequence.Distinct().Count() != 1)
            {
                List<int> differences = [];

                for (int i = 1; i < sequence.Count; i++)
                {
                    differences.Add(sequence[i] - sequence[i - 1]);
                }

                firstNumbers.Add(differences.First());
                sequence = differences;
            }

            var firstValueInInitialRow = 0;

            for (int i = firstNumbers.Count - 1; i >= 0 ; i--)
            {
                firstValueInInitialRow = firstNumbers[i] - firstValueInInitialRow;
            }

            return firstValueInInitialRow;
        }
    }
}