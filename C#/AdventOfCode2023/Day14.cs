using AdventOfCodeLib;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.Versioning;

public static class Day14
{
    enum Direction
    {
        North,
        West,
        South,
        East
    }

    static List<string> Tilt(ref List<string> platform, Direction direction)
    {
        var newPlatform = new List<string>();
        foreach (var line in platform) { newPlatform.Add(line); }

        switch (direction)
        {
            case Direction.North:
                for (int r = 1; r < newPlatform.Count; ++r)
                {
                    var row = newPlatform[r];
                    for (int column = 0; column < row.Length; ++column)
                    {
                        var _r = r - 1;
                        if (row[column] == 'O')
                        {
                            while (_r >= 0 && newPlatform[_r][column] == '.')
                            {
                                newPlatform[_r + 1] = newPlatform[_r + 1].Remove(column, 1).Insert(column, ".");
                                newPlatform[_r] = newPlatform[_r].Remove(column, 1).Insert(column, "O");
                                _r--;
                            }
                        }
                    }
                }
                break;
            case Direction.South:
                for (int r = newPlatform.Count - 2; r >= 0; --r)
                {
                    var row = newPlatform[r];
                    for (int column = 0; column < row.Length; ++column)
                    {
                        var _r = r + 1;
                        if (row[column] == 'O')
                        {
                            while (_r < newPlatform.Count && newPlatform[_r][column] == '.')
                            {
                                newPlatform[_r - 1] = newPlatform[_r - 1].Remove(column, 1).Insert(column, ".");
                                newPlatform[_r] = newPlatform[_r].Remove(column, 1).Insert(column, "O");
                                _r++;
                            }
                        }
                    }
                }
                break;
            case Direction.West:
                for (int c = 1; c < newPlatform[0].Length; ++c)
                {
                    for (int row = 0; row < newPlatform.Count; ++row)
                    {
                        var _c = c - 1;
                        if (newPlatform[row][c] == 'O')
                        {
                            while (_c >= 0 && newPlatform[row][_c] == '.')
                            {
                                newPlatform[row] = newPlatform[row].Remove(_c + 1, 1).Insert(_c + 1, ".");
                                newPlatform[row] = newPlatform[row].Remove(_c, 1).Insert(_c, "O");
                                _c--;
                            }
                        }
                    }
                }
                break;
            case Direction.East:
                for (int c = newPlatform[0].Length - 2; c >= 0; --c)
                {
                    for (int row = 0; row < newPlatform.Count; ++row)
                    {
                        var _c = c + 1;
                        if (newPlatform[row][c] == 'O')
                        {
                            while (_c < newPlatform[row].Length && newPlatform[row][_c] == '.')
                            {
                                newPlatform[row] = newPlatform[row].Remove(_c - 1, 1).Insert(_c - 1, ".");
                                newPlatform[row] = newPlatform[row].Remove(_c, 1).Insert(_c, "O");
                                _c++;
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }

        return newPlatform;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        input = Tilt(ref input, Direction.North);
        return input.Reverse<string>().Select((line, i) => line.Where(c => c == 'O').Count() * (i + 1)).Sum().ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var directions = new List<Direction> { Direction.North, Direction.West, Direction.South, Direction.East };
        var platform = input;
        var platformOrientations = new List<string> { string.Join(";", platform) };

        long limit = 1000000000;
        for (long i = 1; i <= limit; ++i)
        {
            for (int d = 0; d < 4; ++d)
            {
                platform = Tilt(ref platform, directions[d]);
            }

            var orientation = string.Join(";", platform);
            if (platformOrientations.Contains(orientation))
            {
                var idx = platformOrientations.IndexOf(orientation);
                var remainingCircles = (limit - i) % (i - idx);

                for (int j = 0; j < remainingCircles; ++j)
                {
                    for (int d = 0; d < 4; ++d)
                    {
                        platform = Tilt(ref platform, directions[d]);
                    }
                }
                break;
            }
            else
            {
                platformOrientations.Add(orientation);
            }
        }

        return platform.Reverse<string>().Select((line, i) => line.Where(c => c == 'O').Count() * (i + 1)).Sum().ToString();
    };
}