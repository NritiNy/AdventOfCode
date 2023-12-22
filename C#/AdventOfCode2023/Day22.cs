using AdventOfCodeLib;

public static class Day22
{
    public record Brick
    {
        public char ID;
        public (int x, int y, int z) Corner1;
        public (int x, int y, int z) Corner2;
    }
    public static List<Brick> EnforceGravity(ref List<Brick> bricks)
    {
        var oldStack = new List<Brick>();
        var newStack = new List<Brick>(bricks);

        while (!newStack.SequenceEqual(oldStack))
        {
            oldStack = new List<Brick>(newStack);
            newStack.Clear();

            var stack = new Dictionary<(int x, int y, int z), bool>();
            foreach (var brick in oldStack)
            {
                var initialPositions = new List<(int x, int y, int z)>();
                var dropPositions = new List<(int x, int y, int z)>();

                int _x = brick.Corner1.x;
                do
                {
                    int _y = brick.Corner1.y;
                    do
                    {
                        int _z = brick.Corner1.z;
                        do
                        {
                            initialPositions.Add((_x, _y, _z));
                            dropPositions.Add((_x, _y, _z - 1));

                            _z += Math.Sign(brick.Corner2.z - brick.Corner1.z);
                        } while (_z != brick.Corner2.z + Math.Sign(brick.Corner2.z - brick.Corner1.z));

                        _y += Math.Sign(brick.Corner2.y - brick.Corner1.y);
                    } while (_y != brick.Corner2.y + Math.Sign(brick.Corner2.y - brick.Corner1.y));

                    _x += Math.Sign(brick.Corner2.x - brick.Corner1.x);
                } while (_x != brick.Corner2.x + Math.Sign(brick.Corner2.x - brick.Corner1.x));

                if (dropPositions.Any(p => p.z <= 0))
                {
                    foreach (var p in initialPositions)
                    {
                        stack[p] = true;
                    }
                    newStack.Add(brick);
                    continue;
                }

                if (!dropPositions.Any(p => stack.GetValueOrDefault(p, false)))
                {
                    foreach (var p in dropPositions)
                    {
                        stack[p] = true;
                    }

                    newStack.Add(brick with { Corner1 = (brick.Corner1.x, brick.Corner1.y, brick.Corner1.z - 1), Corner2 = (brick.Corner2.x, brick.Corner2.y, brick.Corner2.z - 1) });
                }
                else
                {
                    foreach (var p in initialPositions)
                    {
                        stack[p] = true;
                    }
                    newStack.Add(brick);
                }
            }

        }

        return newStack;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var bricks = new List<Brick>();
        var c = 'A';
        foreach (var line in input)
        {
            var (x1, y1, z1) = (int.Parse(line.Split("~")[0].Split(",")[0]), int.Parse(line.Split("~")[0].Split(",")[1]), int.Parse(line.Split("~")[0].Split(",")[2]));
            var (x2, y2, z2) = (int.Parse(line.Split("~")[1].Split(",")[0]), int.Parse(line.Split("~")[1].Split(",")[1]), int.Parse(line.Split("~")[1].Split(",")[2]));

            bricks.Add(new Brick() with { ID = c, Corner1 = (x1, y1, z1), Corner2 = (x2, y2, z2) });
            c++;
        }
        bricks = bricks.OrderBy(b => Math.Min(b.Corner1.z, b.Corner2.z)).ToList();
        bricks = EnforceGravity(ref bricks);

        var count = 0;
        foreach(var brick in bricks)
        {
            var missing = bricks.Where(b => b.ID != brick.ID).ToList();
            var missingWithGravity = EnforceGravity(ref missing);
            if (missingWithGravity.SequenceEqual(missing)) count++;
        }

        return count.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var bricks = new List<Brick>();
        var c = 'A';
        foreach (var line in input)
        {
            var (x1, y1, z1) = (int.Parse(line.Split("~")[0].Split(",")[0]), int.Parse(line.Split("~")[0].Split(",")[1]), int.Parse(line.Split("~")[0].Split(",")[2]));
            var (x2, y2, z2) = (int.Parse(line.Split("~")[1].Split(",")[0]), int.Parse(line.Split("~")[1].Split(",")[1]), int.Parse(line.Split("~")[1].Split(",")[2]));

            bricks.Add(new Brick() with { ID = c, Corner1 = (x1, y1, z1), Corner2 = (x2, y2, z2) });
            c++;
        }
        bricks = bricks.OrderBy(b => Math.Min(b.Corner1.z, b.Corner2.z)).ToList();
        bricks = EnforceGravity(ref bricks);

        var count = 0;
        foreach (var brick in bricks)
        {
            var missing = bricks.Where(b => b.ID != brick.ID).ToList();
            var missingWithGravity = EnforceGravity(ref missing);
            if (!missingWithGravity.SequenceEqual(missing))
            {
                count += missing.Count - missing.Intersect(missingWithGravity).Count();
            }
        }

        return count.ToString();
    };
}