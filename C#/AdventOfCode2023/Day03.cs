using AdventOfCodeLib;

public static class Day03
{
    static List<int> GetSurroundingNumbers(string[] lines, int columnIdx)
    {
        var result = new List<int>();
        var numberString = "";

        // line 1
        var line = lines[0].ToCharArray();
        var cIdx = columnIdx > 0 ? columnIdx - 1 : columnIdx;
        while (cIdx >= 0 && char.IsDigit(line[cIdx])) 
        {
            numberString = line[cIdx] + numberString;
            cIdx--;
        }
        cIdx = columnIdx;
        while (cIdx < line.Length && char.IsDigit(line[cIdx]))
        {
            numberString = numberString + line[cIdx];
            cIdx++;
        }
        if (int.TryParse(numberString, out var number))
        {
            result.Add(number);
        }
        numberString = "";


        if (columnIdx < line.Length - 1 && cIdx < columnIdx + 1)
        {
            cIdx = columnIdx + 1;
            while (cIdx < line.Length && char.IsDigit(line[cIdx]))
            {
                numberString = numberString + line[cIdx];
                cIdx++;
            }
            if (int.TryParse(numberString, out number))
            {
                result.Add(number);
            }
            numberString = "";
        }

        // line 2
        line = lines[1].ToCharArray();
        if (columnIdx > 0)
        {
            cIdx = columnIdx - 1;
            while (cIdx >= 0 && char.IsDigit(line[cIdx]))
            {
                numberString = line[cIdx] + numberString;
                cIdx--;
            }
            if (int.TryParse(numberString, out number))
            {
                result.Add(number);
            }
            numberString = "";
        }
        
        if (columnIdx < line.Length - 1)
        {
            cIdx = columnIdx + 1;
            while (cIdx < line.Length && char.IsDigit(line[cIdx]))
            {
                numberString = numberString + line[cIdx];
                cIdx++;
            }
            if (int.TryParse(numberString, out number))
            {
                result.Add(number);
            }
            numberString = "";
        }

        // line 3
        line = lines[2].ToCharArray();
        cIdx = columnIdx > 0 ? columnIdx - 1 : columnIdx;
        while (cIdx >= 0 && char.IsDigit(line[cIdx]))
        {
            numberString = line[cIdx] + numberString;
            cIdx--;
        }
        cIdx = columnIdx;
        while (cIdx < line.Length && char.IsDigit(line[cIdx]))
        {
            numberString = numberString + line[cIdx];
            cIdx++;
        }
        if (int.TryParse(numberString, out number))
        {
            result.Add(number);
        }
        numberString = "";

        if (columnIdx < line.Length - 1 && cIdx < columnIdx + 1)
        {
            cIdx = columnIdx + 1;
            while (cIdx < line.Length && char.IsDigit(line[cIdx]))
            {
                numberString = numberString + line[cIdx];
                cIdx++;
            }
            if (int.TryParse(numberString, out number))
            {
                result.Add(number);
            }
            numberString = "";
        }

        return result;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var sum = 0;
        for(var lineIdx = 0; lineIdx < input.Count; ++lineIdx)
        {
            var chars = input[lineIdx].ToCharArray();

            for(var columnIdx = 0; columnIdx < chars.Length; ++columnIdx)
            {
                var c = chars[columnIdx];
                if ('.' == c || char.IsDigit(c)) continue;

                var line1 = lineIdx > 0 ? input[lineIdx - 1] : new string('.', chars.Length);
                var line2 = string.Join("", chars);
                var line3 = lineIdx < input.Count - 1 ? input[lineIdx + 1] : new string('.', chars.Length);

                sum += GetSurroundingNumbers(new string[] { line1, line2, line3 }, columnIdx).Sum();
            }
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var sum = 0;
        for (var lineIdx = 0; lineIdx < input.Count; ++lineIdx)
        {
            var chars = input[lineIdx].ToCharArray();

            for (var columnIdx = 0; columnIdx < chars.Length; ++columnIdx)
            {
                var c = chars[columnIdx];
                if ('.' == c || char.IsDigit(c)) continue;

                var line1 = lineIdx > 0 ? input[lineIdx - 1] : new string('.', chars.Length);
                var line2 = string.Join("", chars);
                var line3 = lineIdx < input.Count - 1 ? input[lineIdx + 1] : new string('.', chars.Length);

                var surroundingNumbers = GetSurroundingNumbers(new string[] { line1, line2, line3 }, columnIdx);


                if ('*' == c && surroundingNumbers.Count == 2)
                {
                    sum += surroundingNumbers[0] * surroundingNumbers[1];
                }
            }
        }

        return sum.ToString();
    };
}
