using AdventOfCodeLib;

public class Day10
{
    static (int x, int y) FollowPipe(ref List<List<char>> map, (int x, int y) pos, (int x, int y) prv_pos, char pipe)
    {
        var possibilities = new List<(int x, int y)>();

        switch (pipe)
        {
            case '|':
                possibilities = new List<(int x, int y)> { (pos.x, pos.y - 1), (pos.x, pos.y + 1) };
                break;
            case '-':
                possibilities = new List<(int x, int y)> { (pos.x - 1, pos.y), (pos.x + 1, pos.y) };
                break;
            case 'L':
                possibilities = new List<(int x, int y)> { (pos.x, pos.y - 1), (pos.x + 1, pos.y) };
                break;
            case 'J':
                possibilities = new List<(int x, int y)> { (pos.x, pos.y - 1), (pos.x - 1, pos.y) };
                break;
            case '7':
                possibilities = new List<(int x, int y)> { (pos.x - 1, pos.y), (pos.x, pos.y + 1) };
                break;
            case 'F':
                possibilities = new List<(int x, int y)> { (pos.x + 1, pos.y), (pos.x, pos.y + 1) };
                break;
            default:
                break;
        }

        possibilities.Remove(prv_pos);
        return possibilities[0];  
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        (int x, int y) start = (0, 0);

        var map = new List<List<char>>();
        for (int i = 0; i < input.Count;++i)
        {
            var line = input[i];
            map.Add(line.ToCharArray().ToList());
            if (line.Contains("S"))
            {
                start = (line.IndexOf('S'), i);
            }
        }

        (int x, int y) current = start;
        var prv = current;

        if (map[start.y - 1][start.x] == '|' || map[start.y - 1][start.x] == 'F' || map[start.y - 1][start.x] == '7')
        {
            current = (start.x, start.y - 1);
        }
        else if (map[start.y][start.x - 1] == '-' || map[start.y][start.x - 1] == 'L' || map[start.y][start.x - 1] == 'F')
        {
            current = (start.x - 1, start.y);
        }
        else if (map[start.y + 1][start.x] == '|' || map[start.y + 1][start.x] == 'L' || map[start.y + 1][start.x] == 'J')
        {
            current = (start.x, start.y + 1);
        }
        else if (map[start.y][start.x + 1] == '-' || map[start.y][start.x + 1] == 'J' || map[start.y][start.x + 1] == '7')
        {
            current = (start.x + 1, start.y);
        }

        var steps = 1;
        do
        {
            (prv, current) = (current, FollowPipe(ref map, current, prv, map[current.y][current.x]));
            steps++;
        } while (current != start);

        return ((steps + 1) / 2).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        (int x, int y) start = (0, 0);

        var map = new List<List<char>>();
        for (int i = 0; i < input.Count; ++i)
        {
            var line = input[i];
            map.Add(line.ToCharArray().ToList());
            if (line.Contains("S"))
            {
                start = (line.IndexOf('S'), i);
            }
        }

        (int x, int y) current = start;
        var prv = current;

        if (map[start.y - 1][start.x] == '|' || map[start.y - 1][start.x] == 'F' || map[start.y - 1][start.x] == '7')
        {
            current = (start.x, start.y - 1);
        }
        else if (map[start.y][start.x - 1] == '-' || map[start.y][start.x - 1] == 'L' || map[start.y][start.x - 1] == 'F')
        {
            current = (start.x - 1, start.y);
        }
        else if (map[start.y + 1][start.x] == '|' || map[start.y + 1][start.x] == 'L' || map[start.y + 1][start.x] == 'J')
        {
            current = (start.x, start.y + 1);
        }
        else if (map[start.y][start.x + 1] == '-' || map[start.y][start.x + 1] == 'J' || map[start.y][start.x + 1] == '7')
        {
            current = (start.x + 1, start.y);
        }

        var steps = 1;
        var path = new List<(int x, int y)> { start, current };
        do
        {
            (prv, current) = (current, FollowPipe(ref map, current, prv, map[current.y][current.x]));
            path.Add(current);
            steps++;
        } while (current != start);
        path.RemoveAt(path.Count - 1);

        var minX = path.Select(x => x.x).Min();
        var maxX = path.Select(x => x.x).Max();
        var minY = path.Select(x => x.y).Min();
        var maxY = path.Select(x => x.y).Max();

        var cornerSet = new HashSet<(int x, int y)>();
        for(int i= 1; i < path.Count - 1; ++i)
        {
            if (path[i - 1].x != path[i + 1].x && path[i - 1].y != path[i + 1].y) cornerSet.Add(path[i]);
        }

        var enclosedCounter = 0;
        var corners = cornerSet.ToArray();
        for(int _y = minY; _y <= maxY; ++_y)
        {
            for (int _x = minX; _x <= maxX; ++_x)
            {
                if (path.Contains((_x, _y))) continue;

                if (IsPointInPolygon(ref corners, (_x, _y)))
                {
                    enclosedCounter++;
                }
            }
        }

        return enclosedCounter.ToString();
    };

    //
    //https://stackoverflow.com/a/14998816
    //
    public static bool IsPointInPolygon(ref (int X, int Y)[] polygon, (int X, int Y) testPoint)
    {
        bool result = false;
        int j = polygon.Length - 1;
        for (int i = 0; i < polygon.Length; i++)
        {
            if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y ||
                polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
            {
                if (polygon[i].X + (testPoint.Y - polygon[i].Y) /
                   (polygon[j].Y - polygon[i].Y) *
                   (polygon[j].X - polygon[i].X) < testPoint.X)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }
}