using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Level6 : Node
{
    private BreakCrossroad breakCrossroad;

    public override void _Ready()
    {
        this.WireNodes();
        breakCrossroad = new BreakCrossroad(new List<Tuple<Crossroad, int>> {
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad"), 5000),
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad2"), 8000),
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad3"), 8000),
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad4"), 18000),
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad5"), 18000),
            new Tuple<Crossroad, int>((Crossroad)GetNode("Crossroad6"), 20000),
        });
    }

    public override void _Process(float delta)
    {
        breakCrossroad.Process(this);
    }
}