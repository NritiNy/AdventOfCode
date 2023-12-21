using AdventOfCodeLib;

public static class Day16
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    struct Node
    {
        public (int x, int y) Position;
        public Direction Direction;
    }

    static long GetEnergizedTileCount(ref List<string> input, ref Dictionary<(int x, int y), char> components, Node start)
    {
        var all = new List<Node>();
        var queue = new List<Node> { start };
        var positions = new HashSet<(int x, int y)>();
        while (queue.Count > 0)
        {
            var node = queue[0];
            queue.RemoveAt(0);

            var nextPosition = node.Direction switch
            {
                Direction.Right => (node.Position.x + 1, node.Position.y),
                Direction.Up => (node.Position.x, node.Position.y - 1),
                Direction.Down => (node.Position.x, node.Position.y + 1),
                Direction.Left => (node.Position.x - 1, node.Position.y),
                _ => node.Position
            };

            if (nextPosition.Item1 < 0 || nextPosition.Item1 >= input[0].Length) continue;
            if (nextPosition.Item2 < 0 || nextPosition.Item2 >= input.Count) continue;

            if (all.Select(n => n.Position).Contains(node.Position))
            {
                if (all.Where(n => n.Position == node.Position).Any(n => n.Direction == node.Direction))
                {
                    continue;
                }
            }
            all.Add(node);

            var newNode = new Node() { Position = nextPosition, Direction = node.Direction };
            if (components.ContainsKey(nextPosition))
            {
                switch (components[nextPosition])
                {
                    case '/':
                        switch (node.Direction)
                        {
                            case Direction.Up:
                                newNode.Direction = Direction.Right;
                                break;
                            case Direction.Down:
                                newNode.Direction = Direction.Left;
                                break;
                            case Direction.Left:
                                newNode.Direction = Direction.Down;
                                break;
                            case Direction.Right:
                                newNode.Direction = Direction.Up;
                                break;
                            default:
                                break;
                        }
                        break;
                    case '\\':
                        switch (node.Direction)
                        {
                            case Direction.Up:
                                newNode.Direction = Direction.Left;
                                break;
                            case Direction.Down:
                                newNode.Direction = Direction.Right;
                                break;
                            case Direction.Left:
                                newNode.Direction = Direction.Up;
                                break;
                            case Direction.Right:
                                newNode.Direction = Direction.Down;
                                break;
                            default:
                                break;
                        }
                        break;
                    case '|':
                        switch (node.Direction)
                        {
                            case Direction.Up:
                                newNode.Direction = Direction.Up;
                                break;
                            case Direction.Down:
                                newNode.Direction = Direction.Down;
                                break;
                            case Direction.Left:
                            case Direction.Right:
                                newNode.Direction = Direction.Up;
                                var newNode2 = new Node() { Position = nextPosition, Direction = Direction.Down };
                                queue.Add(newNode2);
                                positions.Add(newNode2.Position);
                                break;
                            default:
                                break;
                        }
                        break;
                    case '-':
                        switch (node.Direction)
                        {
                            case Direction.Up:
                            case Direction.Down:
                                newNode.Direction = Direction.Left;
                                var newNode2 = new Node() { Position = nextPosition, Direction = Direction.Right };
                                queue.Add(newNode2);
                                positions.Add(newNode2.Position);
                                break;
                            case Direction.Left:
                                newNode.Direction = Direction.Left;
                                break;
                            case Direction.Right:
                                newNode.Direction = Direction.Right;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            queue.Add(newNode);
            positions.Add(newNode.Position);
        }

        return positions.Count();
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var components = new Dictionary<(int x, int y), char>();
        for (int r = 0; r < input.Count; ++r)
        {
            var line = input[r];
            for (int c = 0; c < line.Length; ++c)
            {
                if (line[c] != '.') components[(c, r)] = line[c];
            }
        }

        var start = new Node() { Position = (-1, 0), Direction = Direction.Right };
        return GetEnergizedTileCount(ref input, ref components, start).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var components = new Dictionary<(int x, int y), char>();
        for (int r = 0; r < input.Count; ++r)
        {
            var line = input[r];
            for (int c = 0; c < line.Length; ++c)
            {
                if (line[c] != '.') components[(c, r)] = line[c];
            }
        }

        var starts = new List<Node>();
        for (int r = 0; r < input.Count; ++r)
        {
            starts.Add(new Node() { Position = (-1, r), Direction = Direction.Right });
            starts.Add(new Node() { Position = (input[r].Length, r), Direction = Direction.Left });
        }
        for (int c = 0; c < input[0].Length; ++c)
        {
            starts.Add(new Node() { Position = (c, -1), Direction = Direction.Down });
            starts.Add(new Node() { Position = (c, input.Count), Direction = Direction.Up });
        }

        long max = 0;
        foreach (var start in starts)
        {
            max = Math.Max(max, GetEnergizedTileCount(ref input, ref components, start));
        }

        return max.ToString();
    };
}