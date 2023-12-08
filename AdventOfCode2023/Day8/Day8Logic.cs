using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day8
{
    public class Day8Logic : IPuzzles
    {
        private const string fileName = "day8_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            List<Network> network = [];
            string instructions = string.Empty;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                instructions = streamReader.ReadLine();
                streamReader.ReadLine(); // skip empty line
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var node = new String(
                        line.Where(Char.IsLetter)
                            .ToArray());

                    var networkNode = SplitInParts(node, 3);

                    network.Add(new Network(networkNode.ElementAt(0), networkNode.ElementAt(1), networkNode.ElementAt(2)));
                }
            }

            var currentPosition = "AAA";
            var numberOfSteps = 0;

            while (currentPosition != "ZZZ")
            {
                var instruction = instructions[numberOfSteps % instructions.Length];

                if (instruction == 'R')
                {
                    currentPosition = network.Single(x => x.Source == currentPosition).Right;
                }
                else if (instruction == 'L')
                {
                    currentPosition = network.Single(x => x.Source == currentPosition).Left;
                }

                numberOfSteps++;
            }

            return numberOfSteps.ToString();
        }

        public string SecondPuzzle()
        {
            List<Network> network = [];
            string instructions = string.Empty;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                instructions = streamReader.ReadLine();
                streamReader.ReadLine(); // skip empty line
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var node = new String(
                        line.Where(Char.IsLetter)
                            .ToArray());

                    var networkNode = SplitInParts(node, 3);

                    network.Add(new Network(networkNode.ElementAt(0), networkNode.ElementAt(1), networkNode.ElementAt(2)));
                }
            }

            var currentPositions = network.Where(x => x.Source.EndsWith('A')).ToList();
            List<int> listOfNumberOfSteps = [];

            foreach (var position in currentPositions)
            {
                var numberOfStepsLocal = 0;
                var currentPosition = position.Source;

                while (currentPosition.Last() != 'Z')
                {
                    var instruction = instructions[numberOfStepsLocal % instructions.Length];

                    if (instruction == 'R')
                    {
                        currentPosition = network.Single(x => x.Source == currentPosition).Right;
                    }
                    else if (instruction == 'L')
                    {
                        currentPosition = network.Single(x => x.Source == currentPosition).Left;
                    }

                    numberOfStepsLocal++;
                }

                listOfNumberOfSteps.Add(numberOfStepsLocal);
            }

            long leastCommonMultiple = 1;

            foreach (var numberOfSteps in listOfNumberOfSteps)
            {
                leastCommonMultiple = LeastCommonMultiple(leastCommonMultiple, numberOfSteps);
            }

            return leastCommonMultiple.ToString();
        }

        static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonFactor(a, b)) * b;
        }

        private IEnumerable<string> SplitInParts(string? s, Int32 partLength)
        {
            for (var i = 0; i < s?.Length; i += partLength)
            {
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }

        struct Network
        {
            public Network(string source, string left, string right)
            {
                Source = source;
                Left = left;
                Right = right;
            }

            public string Source { get; set; }
            public string Left { get; set; }
            public string Right { get; set; }
        }
    }
}