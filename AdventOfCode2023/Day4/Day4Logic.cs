using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day4
{
    public class Day4Logic : IPuzzles
    {
        private const string fileName = "day4_Input.in";
        private const Int32 BufferSize = 128;

        public string FirstPuzzle()
        {
            double result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var numberOfWinningNumbersInARound = NumberOfWinningNumbers(line);

                    if (numberOfWinningNumbersInARound > 0)
                    {
                        result += Math.Pow(2, numberOfWinningNumbersInARound - 1);
                    }
                }
            }

            return result.ToString();
        }

        public string SecondPuzzle()
        {
            var lineCount = File.ReadLines(fileName).Count();
            List<int> scratchcardsNumber = Enumerable.Repeat(1, lineCount).ToList();

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                int cardNumber = 0;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var numberOfCards = scratchcardsNumber[cardNumber];

                    for (int i = 0; i < numberOfCards; i++)
                    {
                        var numberOfWinningNumbersInARound = NumberOfWinningNumbers(line);

                        for (int j = cardNumber + 1; j < cardNumber + numberOfWinningNumbersInARound + 1; j++)
                        {
                            scratchcardsNumber[j]++;
                        }
                    }

                    cardNumber++;
                }
            }

            return scratchcardsNumber.Sum().ToString();
        }

        private int NumberOfWinningNumbers(string round)
        {
            string search = ": ";
            string singleGame = round.Substring(round.IndexOf(search) + search.Length);

            var numbers = singleGame.Split('|', StringSplitOptions.TrimEntries);
            var winningNumbers = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var myNumbers = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            List<string> numbersMatched = myNumbers.Intersect(winningNumbers).ToList();

            return numbersMatched.Count;
        }
    }
}