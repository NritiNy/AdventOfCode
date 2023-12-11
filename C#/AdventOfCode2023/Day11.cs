using AdventOfCodeLib;

public class Day11
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var map = new List<List<char>>();
        var positions = new List<(int x, int y)> ();

        foreach(var line in input)
        {
            map.Add(line.ToCharArray().ToList());
            if (!line.Contains("#"))
            {
                map.Add(Enumerable.Repeat('.', line.Length).ToList());
            }
        }

        for (int c = 0; c < map[0].Count; ++c)
        {
            var column = map.Select(l => l[c]).ToList();
            if (!column.Contains('#'))
            {
                for(int _row = 0; _row < map.Count; ++_row)
                {
                    map[_row].Insert(c, '.');
                }
                c++;
            }
        }

        for(int row = 0; row < map.Count;++row)
        {
            var line = map[row];
            for(int column = 0; column < line.Count; ++column)
            {
                if (map[row][column] == '#')
                {
                    positions.Add((column, row));
                }
            }
        }

        long sum = 0;
        for(int i = 0; i < positions.Count - 1;++i)
        {
            var pos1 = positions[i];
            for(int j = i + 1; j < positions.Count; ++j)
            {
                var pos2 = positions[j];
                sum += Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
            }
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var factor = 1000000;
        var positions = new List<(int x, int y)>();

        for (int row = 0; row < input.Count; ++row)
        {
            var line = input[row];
            for (int column = 0; column < line.Length; ++column)
            {
                if (line[column] == '#')
                {
                    positions.Add((column, row));
                }
            }
        }

        var newPositions = new List<(int x, int y)>();
        var rowCounter = 0;
        for (int row = 0; positions.Count > 0 && row <= positions.Select(p => p.y).Max(); ++row)
        {
            if (positions.Any(p => p.y == row))
            {
                var tmp = positions.Where(p => p.y == row).Select(p => (p.x, p.y + (rowCounter > 0 ? rowCounter * factor - (1 * rowCounter) : 0))).ToList();
                positions.RemoveAll(p => p.y == row);
                newPositions.AddRange(tmp);
                continue;
            }
            
            rowCounter++;
        }
        positions = newPositions.ToList();

        newPositions.Clear();
        var columnCounter = 0;
        for (int column = 0; positions.Count > 0 && column <= positions.Select(p => p.x).Max(); ++column)
        {
            if (positions.Any(p => p.x == column))
            {
                var tmp = positions.Where(p => p.x == column).Select(p => (p.x + (columnCounter > 0 ? columnCounter * factor - (1 * columnCounter) : 0), p.y)).ToList();
                positions.RemoveAll(p => p.x == column);
                newPositions.AddRange(tmp);
                continue;
            }

            columnCounter++;
        }
        positions = newPositions.ToList();

        long sum = 0;  
        for (int i = 0; i < positions.Count - 1; ++i)
        {
            var pos1 = positions[i];
            for (int j = i + 1; j < positions.Count; ++j)
            {
                var pos2 = positions[j];
                sum += Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
            }
        }

        return sum.ToString();
    };
}