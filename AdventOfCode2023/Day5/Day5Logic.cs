﻿using AdventOfCode2023.Interfaces;
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

                    for (int i = 0; i < seeds.Count; i++)
                    {
                        if (guards[i] == false && seeds[i] >= sourceRangeStart && seeds[i] < sourceRangeStart + rangeLength)
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
            var almanac = ReadAlmanac(fileName);

            var minimum = almanac.Seeds.Max(x => x);

            var tasksList = new List<Task<long>>();

            for (int j = 0; j < almanac.Seeds.Count; j += 2)
            {
                var currentPair = j;
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