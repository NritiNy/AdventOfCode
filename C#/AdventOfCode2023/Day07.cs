using AdventOfCodeLib;

public class Day07
{ 
    static int CompareCards(string hand1, string hand2, bool includeJoker = false)
    {
        var cardValues = new Dictionary<char, int> { {'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'J', 11 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, { '7', 7 }, { '6', 6 }, { '5', 5 }, { '4', 4 }, { '3', 3 }, { '2', 2 }, };

        var cardCounts1 = new Dictionary<char, int>();
        var cardCounts2 = new Dictionary<char, int>();


        foreach(char c in hand1)
        {
            if(!cardCounts1.ContainsKey(c)) 
            {
                cardCounts1.Add(c, 0);
            }
            cardCounts1[c]++;
        }

        foreach (char c in hand2)
        {
            if (!cardCounts2.ContainsKey(c))
            {
                cardCounts2.Add(c, 0);
            }
            cardCounts2[c]++;
        }

        if (includeJoker)
        {
            cardValues['J'] = 1;

            if (hand1.Contains("J"))
            {
                var js = cardCounts1['J'];
                if (cardCounts1.Keys.Count > 1)
                {
                    cardCounts1.Remove('J');
                }
                foreach (var key in cardCounts1.Keys)
                {
                    if (key == 'J') continue;

                    if (cardCounts1[key] == cardCounts1.Values.Max())
                    {
                        cardCounts1[key] += js;
                        break;
                    }
                }
            }

            if (hand2.Contains("J"))
            {
                var js = cardCounts2['J'];
                if (cardCounts2.Keys.Count > 1)
                {
                    cardCounts2.Remove('J');
                }
                
                foreach (var key in cardCounts2.Keys)
                {
                    if (key == 'J') continue;

                    if (cardCounts2[key] == cardCounts2.Values.Max())
                    {
                        cardCounts2[key] += js;
                        break;
                    }
                }
            }
        }

        var maxCount1 = cardCounts1.Values.Max();
        var maxCount2 = cardCounts2.Values.Max();
        var isFullHouse1 = cardCounts1.Values.Min() == 2;
        var isFullHouse2 = cardCounts2.Values.Min() == 2;
        var isTwoPair1 = cardCounts1.Values.Distinct().Count() == 2 && cardCounts1.Values.Count() == 3 && maxCount1 == 2;
        var isTwoPair2 = cardCounts2.Values.Distinct().Count() == 2 && cardCounts2.Values.Count() == 3 && maxCount2 == 2;

        if (maxCount1 == maxCount2)
        {
            if (isFullHouse1 && !isFullHouse2)
            {
                return 1;
            } 
            else if (isFullHouse2 && !isFullHouse1) 
            {
                return -1;
            }
            else if(isTwoPair1 && !isTwoPair2)
            {
                return 1;
            }
            else if (!isTwoPair1 && isTwoPair2)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i< hand1.Length; ++i)
                {
                    if (hand1[i] != hand2[i])
                    {
                        var card1 = hand1[i];
                        var card2 = hand2[i];

                        return cardValues[card1].CompareTo(cardValues[card2]);
                    }
                }
            }
        }
        else
        {
            return Math.Sign(maxCount1 - maxCount2);
        }

        return 0;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var cards = new List<(string hand, long bid, long rank)>();

        foreach (var line in input)
        {
            cards.Add((line.Split(" ")[0], long.Parse(line.Split(" ")[1]), 1));
        }
        cards.Sort((f, s) => CompareCards(f.hand, s.hand));

        return cards.Select((c, i) => c.bid * (i+1)).Sum().ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var cards = new List<(string hand, long bid, long rank)>();

        foreach (var line in input)
        {
            cards.Add((line.Split(" ")[0], long.Parse(line.Split(" ")[1]), 1));
        }
        cards.Sort((f, s) => CompareCards(f.hand, s.hand, true));

        return cards.Select((c, i) => c.bid * (i + 1)).Sum().ToString();
    };
}