using AdventOfCodeLib;
using System.Reflection;


var year = DateTime.Now.Year;
var day = DateTime.Now.Day;

var puzzle = new Puzzle(year, day);
var input = puzzle.GetInputLines();


Type? Day = Assembly.GetExecutingAssembly().GetType($"Day{day:00}");
if (null == Day)
{
    Console.WriteLine($"No solutions available yet for Day {day:00}");
    return;
}
Console.WriteLine($"Day{day:00}");


FieldInfo? Part1Field = Day.GetField("Part1", BindingFlags.Static | BindingFlags.Public);
if (null == Part1Field || null == Part1Field.GetValue(Day))
{
    Console.WriteLine($"No solution available yet for Day {day:00} Part1");
    return;
}

if (Part1Field.GetValue(Day) is Puzzle.Solution Part1)
{
    var example = puzzle.GetPart1Example();
    if (null != example && example.Answer != Part1(example.Lines))
    {
        Console.WriteLine($"Failed example for part 1:\nExpected '{example.Answer}' but got '{Part1(example.Lines)}'\n");
    }
    else
    {
        var solution = Part1(input);
        Console.WriteLine($"Part1: {puzzle.Submit1(solution)} ({solution})");
    } 
} 
else
{
    Console.WriteLine($"No solution available yet for Day {day:00} Part1");
    return;
}


FieldInfo? Part2Field = Day.GetField("Part2", BindingFlags.Static | BindingFlags.Public);
if (null == Part2Field || null == Part2Field.GetValue(Day))
{
    Console.WriteLine($"No solution available yet for Day {day:00} Part2");
    return;
}

if (Part2Field.GetValue(Day) is Puzzle.Solution Part2)
{
    var example = puzzle.GetPart2Example();
    if (null != example && example.Answer != Part2(example.Lines))
    {
        Console.WriteLine($"Failed example for part 2:\nExpected '{example.Answer}' but got '{Part2(example.Lines)}'\n");
    }
    else
    {
        var solution = Part2(input);
        Console.WriteLine($"Part2: {puzzle.Submit2(solution)} ({solution})");
    }
}
else
{
    Console.WriteLine($"No solution available yet for Day {day:00} Part1");
    return;
}