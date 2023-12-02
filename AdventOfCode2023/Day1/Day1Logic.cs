using AdventOfCode2023.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day1
{
    public class Day1Logic : IPuzzles
    {
        private const string fileName = "day1_Input.in";
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
                    var numbersInARow = Regex.Matches(line, @"\d")
                        .Select(x => x.Value)
                        .ToList();

                    result += SumInASingleRow(numbersInARow);
                }
            }

            return result.ToString();
        }

        public string SecondPuzzle() 
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            int result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    List<string> numbersInARow = [];

                    for(int i = 0; i<line.Length; i++)
                    {
                        if (char.IsNumber(line[i]))
                        {
                            numbersInARow.Add(line[i].ToString());
                        }

                        for (int j = 1; j <= 9; j++)
                        {
                            if (line.Substring(i).StartsWith(digits[j]))
                            {
                                numbersInARow.Add(j.ToString());
                                //i+=digits[j].Length-1; Add this line for better performance when assuming, that strings can't overlap
                                break;
                            }
                        }
                    }

                    result += SumInASingleRow(numbersInARow);
                }
            }

            return result.ToString();
        }

        private static int SumInASingleRow(List<string> numbersInARow)
        {
            var firstNumber = numbersInARow.FirstOrDefault() ?? "";
            var lastNumber = numbersInARow.LastOrDefault() ?? "";

            try
            {
                var sumInARow = 10 * Int32.Parse(firstNumber) + Int32.Parse(lastNumber);
                return sumInARow;
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}