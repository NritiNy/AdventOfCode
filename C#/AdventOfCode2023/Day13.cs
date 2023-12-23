using AdventOfCodeLib;

public static class Day13
{
    
    static long CountColumns(ref List<List<char>> pattern, long skip = -1)
    {
        for(int i = 1; i <= pattern[0].Count - 1; ++i)
        {
            if (i == skip) continue;

            var foundReflection = true;
            var c = 1;
            for(int j = i; j-c >= 0 && j + c - 1 < pattern[0].Count; ++c)
            {
                if (!pattern.Select(l => l[j - c]).SequenceEqual(pattern.Select(l => l[j + c - 1])))
                {
                    foundReflection = false;
                    break;
                }
            }

            if (foundReflection) return i;
        }

        return 0;
    }

    static long CountRows(ref List<List<char>> pattern, long skip = -1)
    {
        for (int i = 1; i <= pattern.Count - 1; ++i)
        {
            if (i == skip) continue;

            var foundReflection = true;
            var c = 1;
            for (int j = i; j - c >= 0 && j + c - 1 < pattern.Count; ++c)
            {
                if (!pattern[j - c].SequenceEqual(pattern[j + c - 1]))
                {
                    foundReflection = false;
                    break;
                }
            }

            if (foundReflection) return i;
        }

        return 0;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        long columnCount = 0;
        long rowCount = 0;

        var pattern = new List<List<char>>();
        foreach(var line in input)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                pattern.Add(line.Select(c => c).ToList());
                continue;
            }

            rowCount += CountRows(ref pattern);
            columnCount += CountColumns(ref pattern);

            pattern.Clear();
        }

        rowCount += CountRows(ref pattern);
        columnCount += CountColumns(ref pattern);
        return (columnCount + 100 * rowCount).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        long columnCount = 0;
        long rowCount = 0;

        void ClearSmudge(ref List<List<char>> pattern)
        {
            var localRowCount = CountRows(ref pattern);
            var localColumnCount = CountColumns(ref pattern);

            var foundReplacement = false;
            for (int r = 0; r < pattern.Count && !foundReplacement; ++r)
            {
                for (int c = 0; c < pattern[r].Count && !foundReplacement; ++c)
                {
                    pattern[r][c] = pattern[r][c] == '#' ? '.' : '#';

                    var newRowCount = CountRows(ref pattern, localRowCount);
                    if (newRowCount > 0 && newRowCount != localRowCount)
                    {
                        rowCount += newRowCount;
                        foundReplacement = true;
                        break;
                    }

                    var newColumnCount = CountColumns(ref pattern, localColumnCount);
                    if (newColumnCount > 0 && newColumnCount != localColumnCount)
                    {
                        columnCount += newColumnCount;
                        foundReplacement = true;
                        break;
                    }

                    pattern[r][c] = pattern[r][c] == '#' ? '.' : '#';
                }
            }
        }

        var pattern = new List<List<char>>();
        foreach (var line in input)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                pattern.Add(line.Select(c => c).ToList());
                continue;
            }

            ClearSmudge(ref pattern);
            pattern.Clear();
        }
        ClearSmudge(ref pattern);

        return (columnCount + 100 * rowCount).ToString();
    };
}