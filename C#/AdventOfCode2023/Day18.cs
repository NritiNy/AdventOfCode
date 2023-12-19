using AdventOfCodeLib;

public static class Day18
{
    static double GetPolygonArea(ref List<(long x, long y)> corners)
    {
        double area = 0;
        int j = corners.Count - 1;
        for(int i = 0; i < corners.Count; ++i)
        {
            area += (corners[j].x + corners[i].x) * (corners[j].y - corners[i].y);
            j = i;
        }
        return Math.Abs(area / 2.0);
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        (long x, long y) currentPosition = (0, 0);
        var corners = new List<(long x, long y)>();
        long pathLength = 0;

        foreach (var line in input)
        {
            var direction = line.Split(" ")[0];
            var lenght = long.Parse(line.Split(" ")[1]);

            currentPosition = direction switch
            {
                "R" => (currentPosition.x + lenght, currentPosition.y),
                "L" => (currentPosition.x - lenght, currentPosition.y),
                "U" => (currentPosition.x, currentPosition.y - lenght),
                "D" => (currentPosition.x, currentPosition.y + lenght),
                _ => currentPosition
            };

            corners.Add(currentPosition);
            pathLength += lenght;
        }

        return (GetPolygonArea(ref corners) + pathLength / 2 + 1).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        (long x, long y) currentPosition = (0, 0);
        var corners = new List<(long x, long y)>();
        long pathLength = 0;

        foreach (var line in input)
        {
            var direction = int.Parse(line[^2].ToString());
            var lenght = long.Parse(line.Split(" ")[2].Replace("#", "").Replace("(", "").Replace(")", "").Substring(0, 5), System.Globalization.NumberStyles.AllowHexSpecifier);

            currentPosition = direction switch
            {
                0 => (currentPosition.x + lenght, currentPosition.y),
                2 => (currentPosition.x - lenght, currentPosition.y),
                3 => (currentPosition.x, currentPosition.y - lenght),
                1 => (currentPosition.x, currentPosition.y + lenght),
                _ => currentPosition
            };

            corners.Add(currentPosition);
            pathLength += lenght;
        }

        return (GetPolygonArea(ref corners) + pathLength / 2 + 1).ToString();
    };
}