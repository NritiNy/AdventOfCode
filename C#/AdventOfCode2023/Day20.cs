using AdventOfCodeLib;
using System.Xml.Linq;

public static class Day20
{
    enum Pulse
    {
        Low,
        High
    }

    abstract class Module
    {
        protected readonly string _name;
        public abstract string Name { get; }
        public List<string> Outputs { get; protected set; } = new List<string>();

        protected Module(string name) 
        {
            _name = name;
        }

        public abstract void SetInput(Module from, Pulse input);
        public abstract bool GetOutput(out Pulse output);

        public abstract string GetState();
    }

    class Broadcaster : Module
    {
        public override string Name { get => _name; }

        public Broadcaster() : base("broadcaster") { }

        public override bool GetOutput(out Pulse output)
        {
            output = Pulse.Low;
            return true;
        }

        public override void SetInput(Module from, Pulse input) { }

        public override string GetState() => "";
    }

    class FlipFlop : Module
    {
        private List<Pulse> _currentInput = new List<Pulse>();
        public Pulse State { get; set; } = Pulse.Low;

        public override string Name => "%" + _name;

        public FlipFlop(string name) : base(name) { }


        public override void SetInput(Module from, Pulse input)
        {
            _currentInput.Add(input);
        }

        public override bool GetOutput(out Pulse output) 
        {
            output = Pulse.Low;
            if (_currentInput.Count == 0) return false;

            var input = _currentInput[0];
            _currentInput.RemoveAt(0);
            if (input == Pulse.High) return false;

            State = State == Pulse.Low ? Pulse.High : Pulse.Low;
            output = State;
            return true;
        }

        public override string GetState() => State.ToString();
    }

    class Conjunction : Module
    {
        public readonly int InputCount;
        public Dictionary<string, Pulse> _lastInputs = new Dictionary<string, Pulse>();

        public override string Name => "&" + _name;

        public Conjunction(string name, int inputCount) : base(name) 
        {
            InputCount = inputCount;
        }

        public override void SetInput(Module from, Pulse input)
        {
            _lastInputs[from.Name] = input;
        }

        public override bool GetOutput(out Pulse output)
        {
            if (_lastInputs.Keys.Count != InputCount)
            {
                output = Pulse.High;
            }
            else
            {
                output = _lastInputs.Values.All(p => p == Pulse.High) ? Pulse.Low : Pulse.High;
            }

            return true;
        }

        public override string GetState()
        {
            return string.Join("", _lastInputs.Keys.Select(k => ((byte)_lastInputs[k])));
        }
    }

    static (long low, long high) CountPulsesForButtonPress(ref Dictionary<string, Module> moduleMap)
    {
        long lowPulseCounter = 1;
        long highPulseCounter = 0;

        var processNext = new List<string> { "broadcaster" };
        var count = 0;
        while (processNext.Count > 0)
        {
            List<string> modulesWithInput = new ();
            foreach(var moduleName in processNext)
            {
                if (!moduleMap.ContainsKey(moduleName)) continue;

                var module = moduleMap[moduleName];
                Pulse output = Pulse.Low;
                if (!module.GetOutput(out output)) continue;

                foreach(var outputModuleName in module.Outputs)
                {
                    if (Pulse.Low == output)
                    {
                        lowPulseCounter++;
                    }
                    else if (Pulse.High == output)
                    {
                        highPulseCounter++;
                    }

                    if (!moduleMap.ContainsKey(outputModuleName)) continue;

                    var outputModule = moduleMap[outputModuleName];
                    outputModule.SetInput(module, output);
                    modulesWithInput.Add(outputModuleName);
                }
            }

            processNext = modulesWithInput;
            count++;
        }

        return (lowPulseCounter, highPulseCounter);
    }

    public static Puzzle.Solution Part1 = (input) =>
    {
        const int BUTTON_PRESSES = 1000;

        Dictionary<string, Module> moduleMap = new Dictionary<string, Module>();
        foreach(var _l in input)
        {
            var line = _l.Replace("&gt;", ">").Replace("&amp;", "&");
            var outputs = line.Split(" -> ")[1].Split(", ").ToList();
            if (line.StartsWith("%"))
            {
                var name = line.Substring(1, line.IndexOf(" ->")).Trim();
                
                var module = new FlipFlop(name);
                module.Outputs.AddRange(outputs);
                moduleMap.Add(name, module);
            }
            else if (line.StartsWith("&"))
            {
                var name = line.Substring(1, line.IndexOf(" ->")).Trim();
                int inputCounter = 0;
                foreach(var l in input)
                {
                    var o = l.Replace("&gt;", ">").Replace("&amp;", "&").Split(" -> ")[1].Split(", ").ToList();
                    if (o.Contains(name)) inputCounter++;
                }

                var module = new Conjunction(name, inputCounter);
                module.Outputs.AddRange(outputs);
                moduleMap.Add(name, module);
            }
            else if (line.StartsWith("broadcaster"))
            {
                var module = new Broadcaster();
                module.Outputs.AddRange(outputs);
                moduleMap.Add("broadcaster", module);
            }
        }

        long lowPulseCounter = 0;
        long highPulseCounter = 0;
        for(int  i = 0; i < BUTTON_PRESSES; ++i)
        {
            var (low, high) = CountPulsesForButtonPress(ref moduleMap);
            lowPulseCounter += low;
            highPulseCounter += high;
        }

        return (lowPulseCounter * highPulseCounter).ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        Dictionary<string, Module> moduleMap = new Dictionary<string, Module>();
        foreach (var _l in input)
        {
            var line = _l.Replace("&gt;", ">").Replace("&amp;", "&");
            var outputs = line.Split(" -> ")[1].Split(", ").ToList();
            if (line.StartsWith("%"))
            {
                var name = line.Substring(1, line.IndexOf(" ->")).Trim();

                var module = new FlipFlop(name);
                module.Outputs.AddRange(outputs);
                moduleMap.Add(name, module);
            }
            else if (line.StartsWith("&"))
            {
                var name = line.Substring(1, line.IndexOf(" ->")).Trim();
                int inputCounter = 0;
                foreach (var l in input)
                {
                    var o = l.Replace("&gt;", ">").Replace("&amp;", "&").Split(" -> ")[1].Split(", ").ToList();
                    if (o.Contains(name)) inputCounter++;
                }

                var module = new Conjunction(name, inputCounter);
                module.Outputs.AddRange(outputs);
                moduleMap.Add(name, module);
            }
            else if (line.StartsWith("broadcaster"))
            {
                var module = new Broadcaster();
                module.Outputs.AddRange(outputs);
                moduleMap.Add("broadcaster", module);
            }
        }

        var dependencies = moduleMap.Values.Where(m => m.Outputs.Contains("rx")).Select(m => m.Name).ToList();
        var visited = new HashSet<string>();
        while (dependencies.Count > 0)
        {
            var newDependecies = new List<string>();
            foreach (var dd in dependencies)
            {
                var d = dd.Replace("%", "").Replace("&", "");
                visited.Add(d);
                var tmp = moduleMap.Values.Where(m => m.Outputs.Contains(d)).Select(m => m.Name);
                newDependecies.AddRange(tmp.Where(_d => _d.StartsWith("&")).Select(n => n.Replace("%", "").Replace("&", "")).Where(_d => !visited.Contains(_d)));
            }

            if (newDependecies.Count > 0)
            {
                dependencies = newDependecies;
            }
            else
            {
                break;
            }
        }
        Dictionary<string, List<string>> stateMap = dependencies.ToDictionary(s => s, s => new List<string>());
        Dictionary<string, long> loopFound = dependencies.ToDictionary(s => s, s => (long)0);

        var pressCounter = 1;
        var pulseDelivered = false;
        while (!pulseDelivered)
        {
            var processNext = new List<string> { "broadcaster" };
            while (processNext.Count > 0)
            {
                List<string> modulesWithInput = new();
                foreach (var moduleName in processNext)
                {
                    if (!moduleMap.ContainsKey(moduleName)) continue;

                    var module = moduleMap[moduleName];
                    Pulse output = Pulse.Low;
                    if (!module.GetOutput(out output)) continue;

                    
                    foreach (var outputModuleName in module.Outputs)
                    {
                        if (!moduleMap.ContainsKey(outputModuleName)) continue;

                        if ("rx" == outputModuleName && output == Pulse.Low)
                        {
                            pulseDelivered= true;
                            break;
                        }

                        var outputModule = moduleMap[outputModuleName];
                        outputModule.SetInput(module, output);
                        modulesWithInput.Add(outputModuleName);
                    }
                }

                processNext = modulesWithInput;

                foreach(var s in dependencies)
                {
                    var state = moduleMap[s].GetState();
                    if (state == string.Join("", Enumerable.Repeat(1, (moduleMap[s] as Conjunction).InputCount)))
                    {
                        loopFound[s] = pressCounter;
                    }
                }

                if (loopFound.Values.All(n => n > 0))
                {
                    long gcd(long n1, long n2) => n2 == 0 ? n1 : gcd(n2, n1 % n2);
                    return loopFound.Values.Aggregate((v1, v2) => v1 * v2 / gcd(v1, v2)).ToString();
                }
            }

            pressCounter++;
        }

        return "";
    };
}