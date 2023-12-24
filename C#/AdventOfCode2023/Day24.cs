using AdventOfCodeLib;

using Microsoft.Z3;

public static class Day24
{
    static bool CrossedInPast(((double x, double y, double z) Position, (double x, double y, double z) Velocity) hailStone, (double x, double y) position)
    {
        if (hailStone.Velocity.x > 0 && position.x < hailStone.Position.x)
        {
            return true;
        }
        else if (hailStone.Velocity.x < 0 && position.x > hailStone.Position.x)
        {
            return true;
        }
        else if (hailStone.Velocity.y > 0 && position.y < hailStone.Position.y)
        {
            return true;
        }
        else if (hailStone.Velocity.y < 0 && position.y > hailStone.Position.y)
        {
            return true;
        }

        return false;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var hailstones = new List<((double x, double y, double z) Position, (double x, double y, double z) Velocity)>();

        foreach (var line in input)
        {
            var (x, y, z) = (double.Parse(line.Split(" @ ")[0].Split(", ")[0]), double.Parse(line.Split(" @ ")[0].Split(", ")[1]), double.Parse(line.Split(" @ ")[0].Split(", ")[2]));
            var (_x, _y, _z) = (double.Parse(line.Split(" @ ")[1].Split(", ")[0]), double.Parse(line.Split(" @ ")[1].Split(", ")[1]), double.Parse(line.Split(" @ ")[1].Split(", ")[2]));

            hailstones.Add(((x, y, z), (_x, _y, _z)));
        }

        long count = 0;
        for (int i = 0; i < hailstones.Count; ++i)
        {
            var hailstoneA = hailstones[i];
            for (int j = i + 1; j < hailstones.Count; ++j)
            {
                var hailstoneB = hailstones[j];

                var A1 = hailstoneA.Velocity.y;
                var B1 = -hailstoneA.Velocity.x;
                var C1 = A1 * hailstoneA.Position.x + B1 * hailstoneA.Position.y;

                var A2 = hailstoneB.Velocity.y;
                var B2 = -hailstoneB.Velocity.x;
                var C2 = A2 * hailstoneB.Position.x + B2 * hailstoneB.Position.y;

                double det = A1 * B2 - A2 * B1;
                if (0 != det)
                {
                    double x = (B2 * C1 - B1 * C2) / det;
                    double y = (A1 * C2 - A2 * C1) / det;

                    var pastA = CrossedInPast(hailstoneA, (x, y));
                    var pastB = CrossedInPast(hailstoneB, (x, y));
                    if (pastA || pastB)
                    {
                        continue;
                    }
                    else if (x >= 200000000000000 && x <= 400000000000000 && y >= 200000000000000 && y <= 400000000000000)
                    {
                        count++;
                    }
                }
            }
        }

        return count.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var hailstones = new List<((long x, long y, long z) Position, (long x, long y, long z) Velocity)>();

        foreach (var line in input)
        {
            var (x1, y1, z1) = (long.Parse(line.Split(" @ ")[0].Split(", ")[0]), long.Parse(line.Split(" @ ")[0].Split(", ")[1]), long.Parse(line.Split(" @ ")[0].Split(", ")[2]));
            var (_x, _y, _z) = (long.Parse(line.Split(" @ ")[1].Split(", ")[0]), long.Parse(line.Split(" @ ")[1].Split(", ")[1]), long.Parse(line.Split(" @ ")[1].Split(", ")[2]));

            hailstones.Add(((x1, y1, z1), (_x, _y, _z)));
        }

        var ctx = new Context();
        var solver = ctx.MkSolver();

        var x = ctx.MkRealConst("x");
        var y = ctx.MkRealConst("y");
        var z = ctx.MkRealConst("z");

        var vx = ctx.MkRealConst("vx");
        var vy = ctx.MkRealConst("vy");
        var vz = ctx.MkRealConst("vz");

        for (int i = 0; i < hailstones.Count; ++i)
        {
            var t_c = ctx.MkRealConst($"t_{i}");

            var sp = hailstones[i].Position;
            var sv = hailstones[i].Velocity;

            solver.Add(ctx.MkEq(x + vx * t_c, sp.x + sv.x * t_c));
            solver.Add(ctx.MkEq(y + vy * t_c, sp.y + sv.y * t_c));
            solver.Add(ctx.MkEq(z + vz * t_c, sp.z + sv.z * t_c));
        }

        solver.Check();
        var res = solver.Model.Eval(x + y + z).ToString();
        ctx.Dispose();

        return res;
    };
}