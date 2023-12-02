using AdventOfCode2023.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day2
{
    public class Day2Logic : IPuzzles
    {
        private const string fileName = "day2_Input.in";
        private const Int32 BufferSize = 128;
        private const int redMax = 12;
        private const int greenMax = 13;
        private const int blueMax = 14;

        public string FirstPuzzle()
        {
            int result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                int lineNumber = 0;
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineNumber++;
                    string search = ": ";
                    string singleGame = line.Substring(line.IndexOf(search) + search.Length);

                    var rounds = singleGame.Split(';', StringSplitOptions.TrimEntries);

                    if (CheckWhetherPossible(rounds))
                    {
                        result += lineNumber;
                    }
                }
            }

            return result.ToString();
        }

        public string SecondPuzzle()
        {
            var result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string search = ": ";
                    string singleGame = line.Substring(line.IndexOf(search) + search.Length);

                    var rounds = singleGame.Split(';', StringSplitOptions.TrimEntries);

                    result += CalculateGamePower(rounds);
                }
            }

            return result.ToString();
        }

        private bool CheckWhetherPossible(string[] rounds)
        {
            foreach (var round in rounds)
            {
                var grabs = round.Split(',', StringSplitOptions.TrimEntries);

                foreach (var grab in grabs)
                {
                    var number = Regex.Matches(grab, @"\d+").FirstOrDefault();
                    if (int.TryParse(number?.ToString(), out int count))
                    {
                        if (grab.EndsWith("red") && count > redMax)
                        {
                            return false;
                        }
                        else if (grab.EndsWith("green") && count > greenMax)
                        {
                            return false;
                        }
                        else if (grab.EndsWith("blue") && count > blueMax)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int CalculateGamePower(string[] rounds)
        {
            var gamePower = 1;
            IDictionary<string, int> minimumOfEachColor = new Dictionary<string, int>
                        {
                            { "red", 0 },
                            { "green", 0},
                            { "blue", 0}
                        };

            foreach (var round in rounds)
            {
                var grabs = round.Split(',', StringSplitOptions.TrimEntries);

                foreach (var grab in grabs)
                {
                    var number = Regex.Matches(grab, @"\d+").FirstOrDefault();
                    if (int.TryParse(number?.ToString(), out int count))
                    {
                        if (grab.EndsWith("red") && count > minimumOfEachColor["red"])
                        {
                            minimumOfEachColor["red"] = count;
                        }
                        else if (grab.EndsWith("green") && count > minimumOfEachColor["green"])
                        {
                            minimumOfEachColor["green"] = count;
                        }
                        else if (grab.EndsWith("blue") && count > minimumOfEachColor["blue"])
                        {
                            minimumOfEachColor["blue"] = count;
                        }
                    }
                }
            }

            foreach (var count in minimumOfEachColor.Values)
            {
                gamePower *= count;
            }

            return gamePower;
        }
    }
}