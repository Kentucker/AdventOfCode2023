using AdventOfCode2023.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day6
{
    public class Day6Logic : IPuzzles
    {
        private const string fileName = "day6_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            int result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                var times = streamReader.ReadLine()
                            .Split("Time:")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x)).ToList();

                var recordDistances = streamReader.ReadLine()
                            .Split("Distance:")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x)).ToList();

                var possibleWaysToWin = Enumerable.Repeat(0, recordDistances.Count()).ToList();

                for (int i = 0; i < times.Count(); i++)
                {
                    for (int j = 1; j < times[i]; j++)
                    {
                        var distance = (times[i] - j) * j;

                        if (distance > recordDistances[i])
                        {
                            possibleWaysToWin[i]++;
                        }
                    }
                }

                result = possibleWaysToWin.Aggregate((a, b) => a * b);
            }

            return result.ToString();
        }

        public string SecondPuzzle()
        {
            long result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                var inputString = Regex.Replace(streamReader.ReadLine(), @"\s+", "");
                var time = inputString
                            .Split("Time:", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => long.Parse(x)).FirstOrDefault();

                inputString = Regex.Replace(streamReader.ReadLine(), @"\s+", "");
                var recordDistance = inputString
                            .Split("Distance:", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => long.Parse(x)).FirstOrDefault();

                long possibleWaysToWin = 0;

                for (long j = 1; j < time; j++)
                {
                    var distance = (time - j) * j;

                    if (distance > recordDistance)
                    {
                        possibleWaysToWin++;
                    }
                }

                result = possibleWaysToWin;
            }

            return result.ToString();
        }
    }
}