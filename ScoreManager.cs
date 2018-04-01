using Godot;
using System;

public class ScoreManager : Node
{
    public bool Mute { get; set; } = false;

    private const int MAX_CONCURRENT_SOUND = 2;
    private int concurrentSound = 0;
    private int _life = 0;
    private float _countdown = 0f;
    private bool shouldCountdown = false;

    public int Life
    {
        get { return _life >= 0 ? _life : 0; }
        set
        {
            EmitSignal("LifeHasChanged", value, _life);
            _life = value;
            if (_life < 0)
            {
                EmitSignal("LevelFail");
                shouldCountdown = false;
            }
        }
    }

    public float Countdown
    {
        get { return _countdown >= 0 ? _countdown : 0; }
        set { _countdown = value; }
    }


    public void StartCountdown(float cd)
    {
        Countdown = cd;
        shouldCountdown = true;
    }

    public override void _Ready()
    {
        AddUserSignal("LifeHasChanged");
        AddUserSignal("LevelFail");
        AddUserSignal("LevelPass");
    }

    public override void _Process(float delta)
    {
        if (!shouldCountdown)
        {
            return;
        }
        if (Countdown <= 0)
        {
            EmitSignal("LevelPass");
            shouldCountdown = false;
        }
        Countdown -= delta;
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
        return !Mute && concurrentSound < MAX_CONCURRENT_SOUND;
    }
}
