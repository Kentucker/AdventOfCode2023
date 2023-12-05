using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day5
{
    public class Day5Logic : IPuzzles
    {
        private const string fileName = "day5_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            long result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                line = streamReader.ReadLine();
                var seeds = line.Split("seeds:")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
                var guards = Enumerable.Repeat(false, seeds.Count).ToList();

                while ((line = streamReader.ReadLine()) != null)
                {
                    line = line.Trim();
                    
                    if (line.Contains("map"))
                    {
                        guards = Enumerable.Repeat(false, seeds.Count).ToList();
                        continue;
                    }
                    else if (line == "")
                    {
                        continue;
                    }

                    var singleLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
                    var destinationRangeStart = singleLine[0];
                    var sourceRangeStart = singleLine[1];
                    var rangeLength = singleLine[2];

                    for(int i = 0; i < seeds.Count; i++)
                    {
                        if (guards[i] == false && seeds[i] >= sourceRangeStart && seeds[i] <= sourceRangeStart + rangeLength)
                        {
                            seeds[i] = seeds[i] + destinationRangeStart - sourceRangeStart;
                            guards[i] = true;
                        }
                    }
                }

                result = seeds.Min(x => x);
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
    }
}