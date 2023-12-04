using AdventOfCodeLib;

public class Day04
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var sum = 0;

        foreach (var line in input)
        {
            var winning = line.Split(": ")[^1].Split(" | ")[0].Split(" ").Select(s => string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s)).Where(n => n > 0).ToHashSet();
            var having = line.Split(": ")[^1].Split(" | ")[^1].Split(" ").Select(s => string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s)).Where(n => n > 0).ToHashSet();

            var both = winning.Intersect(having);
            if (both.Count() > 0)
            {
                sum += (int)Math.Pow(2, both.Count() - 1);
            }
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        int[] map = Enumerable.Repeat(1, input.Count).ToArray();

        for (int i = 0; i < input.Count; i++)
        {
            var line = input[i];

            var winning = line.Split(": ")[^1].Split(" | ")[0].Split(" ").Select(s => string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s)).Where(n => n > 0).ToHashSet();
            var having = line.Split(": ")[^1].Split(" | ")[^1].Split(" ").Select(s => string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s)).Where(n => n > 0).ToHashSet();

            var both = winning.Intersect(having);
            for (int k = 0; k < both.Count(); ++k)
            {
                if (i + k + 1 < input.Count)
                {
                    map[i + k + 1] += map[i];
                }
            }
        }

        return map.Sum().ToString();
    };
}

