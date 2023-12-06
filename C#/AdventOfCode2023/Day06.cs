using AdventOfCodeLib;

public class Day06
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var times = input[0].Split(": ")[1].Split("  ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(t => int.Parse(t)).ToList();
        var distances = input[1].Split(": ")[1].Split("  ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(t => int.Parse(t)).ToList();

        var counts = new List<int>();
        for(int i = 0; i < times.Count; ++i)
        {
            var time = times[i];
            var distance = distances[i];

            var count = 0;
            for  (int t = 0; t < time; ++t)
            {
                if (distance < (t * (time - t))) count++;
            }
            counts.Add(count);
        }
        return counts.Aggregate((f, s) => f * s).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var time = long.Parse(string.Join("", input[0].Split(": ")[1].Split("  ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)));
        var distance = long.Parse(string.Join("", input[1].Split(": ")[1].Split("  ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)));

        var count = 0;
        for (long t = 0; t < time; ++t)
        {
            if (distance < (t * (time - t))) count++;
        }

        return count.ToString();
    };
}