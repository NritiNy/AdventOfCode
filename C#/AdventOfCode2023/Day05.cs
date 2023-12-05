using AdventOfCodeLib;

public class Day05
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var seeds = input[0].Split(": ")[1].Split(" ").Select(s => long.Parse(s)).ToList();

        var maps = new List<List<(long from, long to, long diff)>>();
        List<(long from, long to, long diff)>? m = null;
        for (int i = 1; i < input.Count; i++)
        {
            var line = input[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                if (null != m) maps.Add(m);
                m = null;
                continue;
            }

            if (line.EndsWith(":"))
            {
                if (null != m) maps.Add(m);
                m = new List<(long from, long to, long diff)>();
                continue;
            }

            var dest = long.Parse(line.Split(" ")[0]);
            var src = long.Parse(line.Split(" ")[1]);
            var range = long.Parse(line.Split(" ")[2]);

            if (null != m)
            {
                m.Add((src, src + range - 1, dest - src));
            }
        }
        if (null != m) maps.Add(m);

        var location = long.MaxValue;
        foreach (var seed in seeds)
        {
            long s = seed;

            foreach(var map in maps)
            {
                foreach(var mapping in map)
                {
                    if (s >= mapping.from && s <= mapping.to)
                    {
                        s += mapping.diff;
                        break;
                    }                    
                }
            }

            location = Math.Min(location, s);
        }

        return location.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var seedTuples = new List<(long from, long to)>();
        var numbers = input[0].Split(": ")[1].Split(" ").Select(s => long.Parse(s)).ToList();
        for (int i = 0; i < numbers.Count; i += 2)
        {
            seedTuples.Add((numbers[i], numbers[i] + numbers[i + 1] - 1));
        }

        var maps = new List<List<(long from, long to, long diff)>>();
        List<(long from, long to, long diff)>? m = null;
        for (int i = 1; i < input.Count; i++)
        {
            var line = input[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                if (null != m) maps.Add(m);
                m = null;
                continue;
            }

            if (line.EndsWith(":"))
            {
                if (null != m) maps.Add(m);
                m = new List<(long from, long to, long diff)>();
                continue;
            }

            var dest = long.Parse(line.Split(" ")[0]);
            var src = long.Parse(line.Split(" ")[1]);
            var range = long.Parse(line.Split(" ")[2]);

            if (null != m)
            {
                m.Add((src, src + range - 1, dest - src));
            }
        }
        if (null != m) maps.Add(m);

        foreach (var ma in maps)
        {
            var map = ma.OrderBy(m => m.from);
            var newSeeds = new List<(long f, long t)>();
            foreach(var seed in seedTuples)
            {
                var s = seed;
                foreach(var mapping in map)
                {
                    if (s.from < mapping.from)
                    {
                        newSeeds.Add((s.from, Math.Min(s.to, mapping.from - 1)));
                        s.from = mapping.from;
                        if (s.from >= s.to) break;
                    }

                    if (s.from <= mapping.to)
                    {
                        newSeeds.Add((s.from + mapping.diff, Math.Min(s.to, mapping.to) + mapping.diff));
                        s.from = mapping.to + 1;
                        if (s.from >= s.to) break;
                    }
                }
                if (s.from <= s.to)
                {
                    newSeeds.Add(s);
                }   
            }

            seedTuples = newSeeds;
        }

        return seedTuples.Min(s => s.from).ToString();
    };
}