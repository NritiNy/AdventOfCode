using AdventOfCodeLib;

public static class Day02
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var red = 0;
        var green = 0;
        var blue = 0;
        var sum = 0;
        foreach (var line in input)
        {
            var game = line.Split(": ")[^1];
            var id = int.Parse(line.Split(": ")[0].Split(" ")[^1]);
            var states = game.Split("; ");

            foreach (var state in states)
            {
                red = 0;
                green = 0;
                blue = 0;

                var segments = state.Split(", ");
                foreach (var segment in segments)
                {
                    switch (segment.Split(" ")[^1])
                    {
                        case "red":
                            red += int.Parse(segment.Split(" ")[0]);
                            break;
                        case "green":
                            green += int.Parse(segment.Split(" ")[0]);
                            break;
                        case "blue":
                            blue += int.Parse(segment.Split(" ")[0]);
                            break;
                        default:
                            break;
                    }
                }

                if (12 < red || 13 < green || 14 < blue)
                {
                    break;
                }
            }

            if (!(12 < red || 13 < green || 14 < blue))
            {
                sum += id;
            }
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var cubeDict = new Dictionary<string, int>();
        var sum = 0;
        foreach (var line in input)
        {
            var game = line.Split(": ")[^1];
            var id = int.Parse(line.Split(": ")[0].Split(" ")[^1]);
            var states = game.Split("; ");

            cubeDict["red"] = 0;
            cubeDict["green"] = 0;
            cubeDict["blue"] = 0;

            foreach (var state in states)
            {
                var segments = state.Split(", ");
                foreach (var segment in segments)
                {
                    var value = int.Parse(segment.Split(" ")[0]);
                    var color = segment.Split(" ")[^1];

                    cubeDict[color] = Math.Max(cubeDict[color], value);
                }
            }

            sum += cubeDict["red"] * cubeDict["green"] * cubeDict["blue"];
        }

        return sum.ToString();
    };
}
