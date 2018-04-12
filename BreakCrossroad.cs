using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BreakCrossroad
{

    private int _startTime = 0;
    private List<Tuple<Crossroad, int>> _schedule;

    public BreakCrossroad(List<Tuple<Crossroad, int>> schedule)
    {
        _schedule = schedule;
        _startTime = OS.GetTicksMsec();
    }

    public void Process(Node node)
    {
        var now = OS.GetTicksMsec();
        var s = _schedule[0];
        if (now - _startTime >= s.Item2)
        {
            s.Item1.SetMode(Crossroad.Mode.None);
            _schedule = _schedule.Skip(1).ToList();
        }
    }
}
