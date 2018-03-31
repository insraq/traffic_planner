using Godot;
using System;

public class ScoreManager : Node2D
{
    private const int MAX_CONCURRENT_SOUND = 2;
    private int concurrentSound = 0;
    private int _life;

    [Export]
    public int Life
    {
        get { return _life; }
        set
        {
            EmitSignal("LifeHasChanged", value, _life);
            _life = value;
            if (_life < 0)
            {
                OS.Alert("Game over!");
                GetTree().SetPause(true);
            }
        }
    }

    public override void _Ready()
    {
        AddUserSignal("LifeHasChanged");
    }

    public void PlaySoundStart()
    {
        concurrentSound++;
    }

    public void PlaySoundEnd()
    {
        if (concurrentSound <= 0)
        {
            GD.Print("WARNING: PlaySoundEnd() called when concurrentSound <= 0");
        }
        concurrentSound--;
    }

    public Boolean ShouldPlaySound()
    {
        return concurrentSound < MAX_CONCURRENT_SOUND;
    }
}
