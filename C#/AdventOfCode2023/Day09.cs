using AdventOfCodeLib;

public class Day09
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        long sum = 0;

        foreach (var line in input)
        {
            var numbers = line.Split(" ").Select(n => long.Parse(n)).ToList();
            var lastValues = new List<long> { numbers[^1] };

            do
            {
                numbers = numbers.Select((n, i) => i == 0 ? 0 : n - numbers[i - 1]).ToList().GetRange(1, numbers.Count - 1);
                lastValues.Add(numbers[^1]);
            } while (!numbers.All(n => n == 0));
            sum += lastValues.Aggregate((v1, v2) => v1 + v2);
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        long sum = 0;

        foreach (var line in input)
        {
            var numbers = line.Split(" ").Select(n => long.Parse(n)).ToList();
            var firstValues = new List<long> { numbers[0] };

            do
            {
                numbers = numbers.Select((n, i) => i == 0 ? 0 : n - numbers[i - 1]).ToList().GetRange(1, numbers.Count - 1);
                firstValues.Add(numbers[0]);
            } while (!numbers.All(n => n == 0));
            sum += firstValues.Reverse<long>().Aggregate((v1, v2) => v2 - v1);
        }

        return sum.ToString();
    };
}