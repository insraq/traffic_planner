using Godot;
using System;

public class ScoreManager : Node
{
    public bool Mute { get; set; } = false;
    public const string SaveFilePath = "user://save";
    public const bool Debug = true;

    private const int SoundPlayIntervalMsec = 5000;
    private int lastSoundPlay = 0;
    private int _life = 0;
    private float _countdown = 0f;
    private bool shouldCountdown = false;


    public int Life
    {
        get { return _life >= 0 ? _life : 0; }
        set
        {
            _life = value;
            EmitSignal("LifeHasChanged");
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
        LoadGame();
    }

    private void LoadGame()
    {
        if (Debug)
        {
            return;
        }
        var file = new File();
        if (!file.FileExists(SaveFilePath))
        {
            return;
        }
        file.Open(SaveFilePath, (int)File.ModeFlags.Read);
        var lastScene = file.GetAsText();
        file.Close();
        GetTree().ChangeScene(lastScene);
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

    public void PlaySound(AudioStreamPlayer player)
    {
        if (OS.GetTicksMsec() - lastSoundPlay > SoundPlayIntervalMsec)
        {
            lastSoundPlay = OS.GetTicksMsec();
            player.Play();
        }
    }

}
