using AdventOfCode2023.Interfaces;

namespace AdventOfCode2023.Day5
{
    public class Day5Logic : IPuzzles
    {
        private const string fileName = "day5_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            var almanac = ReadAlmanac(fileName);

            var localMinimum = almanac.Seeds.Max(x => x);

            foreach (var seed in almanac.Seeds)
            {
                var currentSeedNumber = seed;
                var seedPosition = ProcessSingleSeed(almanac.Mappings, currentSeedNumber);

                localMinimum = long.Min(seedPosition, localMinimum);
            }

            return localMinimum.ToString();
        }

        public string SecondPuzzle()
        {
            var almanac = ReadAlmanac(fileName);

            var minimum = almanac.Seeds.Max(x => x);

            var tasksList = new List<Task<long>>();

            for (int i = 0; i < almanac.Seeds.Count; i += 2)
            {
                var currentPair = i;
                var task = Task.Run(() => ProcessSeedsForRange(almanac, currentPair));
                tasksList.Add(task);
            }

            Task.WaitAll([.. tasksList]);

            minimum = tasksList.Min(x => x.Result);

            return minimum.ToString();
        }

        private long ProcessSingleSeed(List<Mapper> mappings, long currentSeedNumber)
        {
            foreach (var mapping in mappings)
            {
                foreach (var map in mapping.Maps)
                {
                    if (currentSeedNumber >= map.SourceRangeStart && currentSeedNumber < map.SourceRangeStart + map.RangeLength)
                    {
                        currentSeedNumber = currentSeedNumber + map.DestinationRangeStart - map.SourceRangeStart;
                        break;
                    }
                }
            }

            return currentSeedNumber;
        }

        private long ProcessSeedsForRange(Almanac almanac, int index)
        {
            var localMinimum = almanac.Seeds.Max(x => x);

            var from = almanac.Seeds[index];
            var to = almanac.Seeds[index] + almanac.Seeds[index + 1];

            for (long i = from; i < to; i++)
            {
                var currentSeedNumber = i;
                var seedPosition = ProcessSingleSeed(almanac.Mappings, currentSeedNumber);

                localMinimum = long.Min(seedPosition, localMinimum);
            }

            return localMinimum;
        }

        private Almanac ReadAlmanac(string fileName)
        {
            var file = File.ReadAllLines(fileName);
            var fileLinesList = new List<string>(file);

            var seeds = fileLinesList[0].Split("seeds:")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();

            int lineNumber = 1;
            int mapperNumber = -1;
            List<Mapper> mappings = [];

            while (lineNumber < fileLinesList.Count)
            {
                if (fileLinesList[lineNumber].Contains("map"))
                {
                    lineNumber++;
                    continue;
                }
                else if (fileLinesList[lineNumber] == "")
                {
                    lineNumber++;
                    mapperNumber++;
                    mappings.Add(new Mapper(mapperNumber, []));
                    continue;
                }

                var singleLine = fileLinesList[lineNumber].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
                var destinationRangeStart = singleLine[0];
                var sourceRangeStart = singleLine[1];
                var rangeLength = singleLine[2];

                var map = new Map(destinationRangeStart, sourceRangeStart, rangeLength);
                mappings.Where(x => x.Id == mapperNumber).FirstOrDefault().Maps.Add(map);

                lineNumber++;
            }


            var almanac = new Almanac(seeds, mappings);

            return almanac;
        }

        struct Almanac
        {
            public Almanac(List<long> seeds, List<Mapper> mappings)
            {
                Seeds = seeds;
                Mappings = mappings;
            }

            public List<long> Seeds {  get; set; }
            public List<Mapper> Mappings { get; set; }
        }

        struct Mapper
        {
            public Mapper(int id, List<Map> maps)
            {
                Id = id;
                Maps = maps;
            }

            public int Id { get; set; }
            public List<Map> Maps { get; set; }
        }

        struct Map
        {
            public Map(long destinationRangeStart, long sourceRangeStart, long rangeLength)
            {
                DestinationRangeStart = destinationRangeStart;
                SourceRangeStart = sourceRangeStart;
                RangeLength = rangeLength;
            }

            public long DestinationRangeStart { get; set; }
            public long SourceRangeStart { get; set; }
            public long RangeLength { get; set; }
        }
    }
}