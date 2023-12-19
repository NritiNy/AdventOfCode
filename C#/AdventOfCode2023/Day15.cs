using AdventOfCodeLib;

public static class Day15
{

    public static Puzzle.Solution Part1 = (input) =>
    {
        return input[0].Replace("\n", "").Split(",").Select(step =>
        {
           
            var hash = 0;

            foreach(var c in step)
            {
                hash = ((hash + c) * 17) % 256;
            }
            return hash;
        }).Sum().ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var steps = input[0].Replace("\n", "").Split(",").ToList();

        var HASHMAP = new Dictionary<int, List<(string label, int focalLength)>>();
        foreach(var step in steps)
        {
            var splitIdx = Math.Max(step.IndexOf("="), step.IndexOf("-"));
            var label = step.Substring(0, splitIdx);
            var operation = step.Substring(splitIdx, 1);

            var hash = 0;
            foreach (var c in label)
            {
                hash = ((hash + c) * 17) % 256;
            }

            if (operation == "-" && HASHMAP.ContainsKey(hash))
            {
                HASHMAP[hash] = HASHMAP[hash].Where(b => b.label != label).ToList();
            }
            else if (operation == "=")
            {
                var focalLength = int.Parse(step.Substring(splitIdx + 1));

                if (!HASHMAP.ContainsKey(hash))
                {
                    HASHMAP[hash] = new List<(string label, int focalLength)>();
                }

                if (HASHMAP[hash].Count(l => l.label == label) > 0)
                {
                    var idx = HASHMAP[hash].Select((l, i) => (l.label, i)).Where(l => l.label == label).Select(l => l.i).First();
                    HASHMAP[hash].RemoveAt(idx);
                    HASHMAP[hash].Insert(idx, (label, focalLength));
                }
                else
                {
                    HASHMAP[hash].Add((label, focalLength));
                }
            }
        }

        long sum = 0;
        for(int i = 0; i <= 255; ++i)
        {
            if (!HASHMAP.ContainsKey(i)) continue;

            var lenses = HASHMAP[i];
            for(int j = 1; j <= lenses.Count;++j)
            {
                var lens = lenses[j - 1];
                sum += (i + 1) * j * lens.focalLength;
            }
        }

        return sum.ToString();
    };
}