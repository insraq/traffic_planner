using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BreakCrossroad : Node
{
    private int startTime = 0;
    private List<Tuple<string, int>> schedule = new List<Tuple<string, int>> {
        new Tuple<string, int>("Crossroad", 5000),
        new Tuple<string, int>("Crossroad2", 8000),
        new Tuple<string, int>("Crossroad3", 8000),
        new Tuple<string, int>("Crossroad4", 18000),
        new Tuple<string, int>("Crossroad5", 18000),
        new Tuple<string, int>("Crossroad6", 20000),
    };

    public override void _Ready()
    {
        this.WireNodes();
        startTime = OS.GetTicksMsec();
    }

    public override void _Process(float delta)
    {
        var now = OS.GetTicksMsec();
        var s = schedule[0];
        if (now - startTime >= s.Item2)
        {
            ((Crossroad)GetNode(s.Item1)).SetMode(Crossroad.Mode.None);
            schedule = schedule.Skip(1).ToList();
        }
    }
}
