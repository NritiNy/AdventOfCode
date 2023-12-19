using AdventOfCodeLib;

public static class Day19
{
    struct Condition
    {
        public string PartName;
        public string Check;
        public long Value;
        public string ToWorkflow;
    }

    struct Part
    {
        public long x;
        public long m;
        public long a;
        public long s;
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        var workflows = new Dictionary<string, List<Condition>>();
        long accepted = 0;

        foreach(var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;

            if (!line.StartsWith("{"))
            {
                var workflowName = line.Split("{")[0];
                var conditions = line.Split("{")[1].Replace("}", "").Split(",").Select(c =>
                {
                    if (!c.Contains(":"))
                    {
                        return new Condition() { ToWorkflow = c };
                    }

                    var partName = c.Substring(0, 1);
                    var check = c.Substring(1, 1);
                    var value = long.Parse(c.Substring(2, c.IndexOf(":") - 2));
                    var toWorkflow = c.Split(":")[1];

                    return new Condition()
                    {
                        PartName = partName,
                        Check = check,
                        Value = value,
                        ToWorkflow = toWorkflow
                    };
                }).ToList();

                workflows.Add(workflowName, conditions);
            }
            else
            {
                var parts = line.Replace("{", "").Replace("}", "").Split(",");
                var part = new Part()
                {
                    x = long.Parse(parts[0].Split("=")[1]),
                    m = long.Parse(parts[1].Split("=")[1]),
                    a = long.Parse(parts[2].Split("=")[1]),
                    s = long.Parse(parts[3].Split("=")[1])
                };

                var currentWorkflowName = "in";
                while (currentWorkflowName != "A" && currentWorkflowName != "R")
                {
                    var workflow = workflows[currentWorkflowName];
                    foreach(var condition in workflow)
                    {
                        if (string.IsNullOrEmpty(condition.PartName))
                        {
                            currentWorkflowName = condition.ToWorkflow;
                            break;
                        }

                        var prv_workflowName = currentWorkflowName;
                        currentWorkflowName = condition.PartName switch
                        {
                            "x" => (condition.Check == ">" ? part.x > condition.Value ? condition.ToWorkflow : currentWorkflowName : part.x < condition.Value ? condition.ToWorkflow : currentWorkflowName),
                            "m" => (condition.Check == ">" ? part.m > condition.Value ? condition.ToWorkflow : currentWorkflowName : part.m < condition.Value ? condition.ToWorkflow : currentWorkflowName),
                            "a" => (condition.Check == ">" ? part.a > condition.Value ? condition.ToWorkflow : currentWorkflowName : part.a < condition.Value ? condition.ToWorkflow : currentWorkflowName),
                            "s" => (condition.Check == ">" ? part.s > condition.Value ? condition.ToWorkflow : currentWorkflowName : part.s < condition.Value ? condition.ToWorkflow : currentWorkflowName),
                            _ => currentWorkflowName
                        };

                        if (prv_workflowName != currentWorkflowName) break;
                    }
                }
                if (currentWorkflowName == "A")
                {
                    accepted += part.x + part.m + part.a + part.s;
                }
            }
        }

        return accepted.ToString();
    };

    record CombinationTracker
    {
        public long MinX = 1;
        public long MaxX = 4000;
        public long MinM = 1;
        public long MaxM = 4000;
        public long MinA = 1;
        public long MaxA = 4000;
        public long MinS = 1;
        public long MaxS = 4000;

        public string CurrentWorkflow = "";

        public bool IsValid => (MaxX - MinX) > 0 && (MaxM - MinM) > 0 && (MaxA - MinA) > 0 && (MaxS - MinS) > 0;
        public long AcceptableCombinations => (MaxX - MinX + 1) * (MaxM - MinM + 1) * (MaxA - MinA + 1)* (MaxS - MinS + 1);
    }

    public static Puzzle.Solution Part2 = (input) =>
    {
        var workflows = new Dictionary<string, List<Condition>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) break;

            if (!line.StartsWith("{"))
            {
                var workflowName = line.Split("{")[0];
                var conditions = line.Split("{")[1].Replace("}", "").Split(",").Select(c =>
                {
                    if (!c.Contains(":"))
                    {
                        return new Condition() { ToWorkflow = c };
                    }

                    var partName = c.Substring(0, 1);
                    var check = c.Substring(1, 1);
                    var value = long.Parse(c.Substring(2, c.IndexOf(":") - 2));
                    var toWorkflow = c.Split(":")[1];

                    return new Condition()
                    {
                        PartName = partName,
                        Check = check,
                        Value = value,
                        ToWorkflow = toWorkflow
                    };
                }).ToList();

                workflows.Add(workflowName, conditions);
            }
        }

        long acceptableCombinations = 0;
        var queue = new List<CombinationTracker> { new CombinationTracker() { CurrentWorkflow = "in"} };
        while (queue.Count > 0)
        {
            var current = queue[0];
            queue.RemoveAt(0);

            if (current.CurrentWorkflow == "A")
            {
                acceptableCombinations += current.AcceptableCombinations;
                continue;
            }
            else if (current.CurrentWorkflow == "R")
            {
                continue;
            }

            var conditions = workflows[current.CurrentWorkflow];
            CombinationTracker wPath = current with { };
            foreach(var condition in conditions ) 
            {
                wPath.CurrentWorkflow = condition.ToWorkflow;
                CombinationTracker next = wPath with {  };

                if (condition.Check == ">")
                {
                    switch (condition.PartName)
                    {
                        case "x":
                            next = wPath with { MinX = condition.Value + 1 };
                            wPath.MaxX = condition.Value;
                            break;
                        case "m":
                            next = wPath with { MinM = condition.Value + 1 };
                            wPath.MaxM = condition.Value;
                            break;
                        case "a":
                            next = wPath with { MinA = condition.Value + 1 };
                            wPath.MaxA = condition.Value;
                            break;
                        case "s":
                            next = wPath with { MinS = condition.Value + 1 };
                            wPath.MaxS = condition.Value;
                            break;
                        default: 
                            break;
                    }
                }
                else if (condition.Check == "<")
                {
                    switch (condition.PartName)
                    {
                        case "x":
                            next = wPath with { MaxX = condition.Value - 1};
                            wPath.MinX = condition.Value;
                            break;
                        case "m":
                            next = wPath with { MaxM = condition.Value - 1 };
                            wPath.MinM = condition.Value;
                            break;
                        case "a":
                            next = wPath with { MaxA = condition.Value - 1 };
                            wPath.MinA = condition.Value;
                            break;
                        case "s":
                            next = wPath with { MaxS = condition.Value - 1 };
                            wPath.MinS = condition.Value;
                            break;
                        default:
                            break;
                    }
                }

                if (next.IsValid)
                {
                    queue.Add(next);
                }
            }
        }

        return acceptableCombinations.ToString();
    };
}