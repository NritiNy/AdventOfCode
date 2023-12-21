using AdventOfCodeLib;
using System.Security.Cryptography.X509Certificates;

public static class Day21
{

    static void GetNeighbours((int x, int y) position, ref HashSet<(int x, int y)> rocks, ref HashSet<(int x, int y)> neighbours)
    {
        if (!rocks.Contains((position.x - 1, position.y))) neighbours.Add((position.x - 1, position.y));
        if (!rocks.Contains((position.x + 1, position.y))) neighbours.Add((position.x + 1, position.y));
        if (!rocks.Contains((position.x, position.y - 1))) neighbours.Add((position.x, position.y - 1));
        if (!rocks.Contains((position.x, position.y + 1))) neighbours.Add((position.x, position.y + 1));
    }

    static void GetNeighboursInfinite((int x, int y) position, int maxX, int maxY, ref HashSet<(int x, int y)> rocks, ref HashSet<(int x, int y)> neighbours)
    {
        var xInc = Math.Abs(position.x / maxX) + 1;
        var yInc = Math.Abs(position.y / maxY) + 1;
        if (!rocks.Contains(((position.x - 1 + xInc * maxX) % maxX, (position.y + yInc * maxY) % maxY))) neighbours.Add((position.x - 1, position.y));
        if (!rocks.Contains(((position.x + 1 + xInc * maxX) % maxX, (position.y + yInc * maxY) % maxY))) neighbours.Add((position.x + 1, position.y));
        if (!rocks.Contains(((position.x + xInc * maxX) % maxX, (position.y - 1 + yInc * maxY) % maxY))) neighbours.Add((position.x, position.y - 1));
        if (!rocks.Contains(((position.x + xInc * maxX) % maxX, (position.y + 1 + yInc * maxY) % maxY))) neighbours.Add((position.x, position.y + 1));
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var rocks = new HashSet<(int x, int y)>();
        (int x, int y) start = (0, 0);
        for (int y = 0; y < input.Count; ++y)
        {
            for (int x = 0; x < input[y].Length; ++x)
            {
                if (input[y][x] == '#')
                {
                    rocks.Add((x, y));
                }
                else if (input[y][x] == 'S')
                {
                    start = (x, y);
                }
            }
        }

        var positions = new HashSet<(int x, int y)> { start };
        HashSet<(int x, int y)> neighbours = new HashSet<(int x, int y)>();
        for (int stepCount = 0; stepCount < 64; ++stepCount)
        {
            neighbours.Clear();
            foreach (var pos in positions)
            {
                GetNeighbours(pos, ref rocks, ref neighbours);
            }
            positions.Clear();
            foreach (var n in neighbours)
            {
                positions.Add(n);
            }
        }

        return positions.Count.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        const long maxSteps = 26501365;

        var rocks = new HashSet<(int x, int y)>();
        (int x, int y) start = (0, 0);
        for (int y = 0; y < input.Count; ++y)
        {
            for (int x = 0; x < input[y].Length; ++x)
            {
                if (input[y][x] == '#')
                {
                    rocks.Add((x, y));
                }
                else if (input[y][x] == 'S')
                {
                    start = (x, y);
                }
            }
        }

        var (maxX, maxY) = (input[0].Length, input.Count);
        long startStep = maxSteps % maxX;
        long diffIncrement = 0;

        var posCounter = new List<long>();
        var positions = new HashSet<(int x, int y)> { start };
        posCounter.Add(positions.LongCount());
        HashSet<(int x, int y)> neighbours = new HashSet<(int x, int y)>();
        for (int stepCount = 0; stepCount < 500; ++stepCount)
        {
            neighbours.Clear();
            foreach (var pos in positions)
            {
                GetNeighboursInfinite(pos, maxX, maxY, ref rocks, ref neighbours);
            }

            positions.Clear();
            foreach (var n in neighbours)
            {
                positions.Add(n);
            }

            posCounter.Add(positions.LongCount());

            if (stepCount == maxX * 2 + startStep)
            {
                List<long>tmp = new();
                for (int i = stepCount; i >= 0; i -= maxX)
                {
                    tmp.Add(posCounter[i]);
                }
                tmp.Reverse();
                diffIncrement = (tmp[2] - tmp[1]) - (tmp[1] - tmp[0]);
                break;
            }
        }

        startStep = maxX * 2 + startStep;
        long posCount = posCounter[(int)startStep];
        long increment = posCounter[(int)startStep] - posCounter[(int)(startStep - maxX)];
        while (startStep != maxSteps)
        {
            increment += diffIncrement;
            posCount += increment;
            startStep += maxX;
        }

        return posCount.ToString();
    };
}