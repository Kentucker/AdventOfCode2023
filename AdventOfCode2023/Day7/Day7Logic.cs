﻿using AdventOfCode2023.Interfaces;
using System.Text;

namespace AdventOfCode2023.Day7
{
    public class Day7Logic : IPuzzles
    {
        private const string fileName = "day7_Input.in";
        private const Int32 BufferSize = 128;
        private static List<char> figuresSeniority = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

        public string FirstPuzzle()
        {
            long result = 0;

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                List<Hand> hands = [];
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var handAndBid = line.Split(' ');
                    var hand = handAndBid[0];
                    var bid = int.Parse(handAndBid[1]);

                    var type = VerifyHandType(hand);

                    hands.Add(new Hand(hand, bid, type));
                }

                hands = hands.OrderByDescending(hand => hand.HandType).ThenByDescending(str => str.Cards, new StringComparer()).ToList();

                for (int i = 0; i < hands.Count; i++)
                {
                    result += (i + 1) * hands[i].Bid;
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

                }
            }

            return result.ToString();
        }

        private int VerifyHandType(string hand)
        {
            var topOccurences = 0;

            switch (hand.Distinct().Count())
            {
                case 1:
                    return 1;
                case 2:
                    topOccurences = hand.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Count();
                    if (topOccurences == 4)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                case 3:
                    topOccurences = hand.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Count();
                    if(topOccurences == 3)
                    {
                        return 4;
                    }
                    else
                    {
                        return 5;
                    }
                case 4:
                    return 6;
                case 5:
                    return 7;
                default:
                    return 0;
            }
        }

        private class StringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                for (int i = 0; i < Math.Min(x.Length, y.Length); i++)
                {
                    int indexOfX = figuresSeniority.IndexOf(x[i]);
                    int indexOfY = figuresSeniority.IndexOf(y[i]);

                    if (indexOfX != indexOfY)
                    {
                        return indexOfX.CompareTo(indexOfY);
                    }
                }

                return x.Length.CompareTo(y.Length);
            }
        }

        struct Hand
        {
            public Hand(string cards, int bid, int handType)
            {
                Cards = cards;
                Bid = bid;
                HandType = handType;
            }

            public string Cards { get; set; }
            public int Bid { get; set; }
            public int HandType { get; set; }
        }
    }
}