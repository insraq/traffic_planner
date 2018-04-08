using Godot;
using System;

public class Level5 : Node
{
    [Node("Crossroad4")] private Crossroad cr4;
    [Node("CR4IsBroken")] private Label cr4IsBroken;
    [Node("Hand")] private Sprite hand;

    public override void _Ready()
    {
        this.WireNodes();
        GetTree().CreateTimer(10).Connect("timeout", this, nameof(CR4IsBroken));
        cr4.Connect(Crossroad.ModeHasChanged, this, nameof(CR4IsFixed));
    }

    private void CR4IsBroken()
    {
        cr4.SetMode(Crossroad.Mode.None);
        cr4IsBroken.SetVisible(true);
        hand.SetVisible(true);
    }

    private void CR4IsFixed(Crossroad.Mode newMode, Crossroad.Mode oldMode)
    {
        if (oldMode == Crossroad.Mode.None && newMode != Crossroad.Mode.None)
        {
            cr4IsBroken.SetVisible(false);
            hand.SetVisible(false);
        }
    }
}
