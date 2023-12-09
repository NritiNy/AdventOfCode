using AdventOfCodeLib;

public class Day08
{
    public static Puzzle.Solution Part1 = (input) =>
    {
        var nodes = new Dictionary<string, (string left, string right)>();
        var sequence = input[0];

        for(int i = 1; i < input.Count;++i)
        {
            var line = input[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            var node = line.Split(" = ")[0];
            var left = line.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ")[0];
            var right = line.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ")[1];

            nodes.Add(node, (left, right));
        }

        var stepCount = 0;
        var currentNode = "AAA";

        while (currentNode != "ZZZ")
        {
            var idx = stepCount % sequence.Length;
            var direction = sequence[idx];
            currentNode = direction == 'R' ? nodes[currentNode].right : nodes[currentNode].left;
            stepCount++;
        }

        return stepCount.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var nodes = new Dictionary<string, (string left, string right)>();

        var sequence = input[0].Trim();

        for (int i = 1; i < input.Count; ++i)
        {
            var line = input[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            var node = line.Split(" = ")[0];
            var left = line.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ")[0];
            var right = line.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ")[1];

            nodes.Add(node, (left, right));
        }

        var currentNodes = nodes.Keys.Where(n => n.EndsWith("A")).ToList();
        var stepsCounter = Enumerable.Repeat(1, currentNodes.Count).Select(i => (long)i).ToList();

        var stepCounter = 0;
        while (!currentNodes.All(n => n.EndsWith("Z")))
        {
            var idx = stepCounter % sequence.Length;
            var direction = sequence[idx];

            var newNodes = new List<string>();
            foreach(var currentNode in currentNodes)
            {
                newNodes.Add(direction == 'R' ? nodes[currentNode].right : nodes[currentNode].left);
            }
            currentNodes = newNodes;
            stepCounter++;

            for (int i = 0; i < stepsCounter.Count; ++i)
            {
                if (currentNodes[i].EndsWith("Z") && stepsCounter[i] == 1)
                {
                    stepsCounter[i] = stepCounter;
                }
            } 

            if (stepsCounter.All(v => v != 1)) break;
        }

        long gcd(long n1, long n2) => n2 == 0 ? n1 : gcd(n2, n1 % n2);
        return stepsCounter.Aggregate((v1, v2) => v1 * v2 / gcd(v1, v2)).ToString();
    };
}