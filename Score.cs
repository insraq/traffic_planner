using Godot;
using System;

public class Score : Node2D
{

    [Export] private int MaxLife = 10;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;
    [Node("Score")] private Label score;
    [Node("Countdown")] private Label _countdown;

    private bool startCountdown;

    public override void _Ready()
    {
        this.WireNodes();
        scoreManager.Connect("LifeHasChanged", this, "UpdateLife");
        scoreManager.Connect("CountdownStarted", this, "StartCountdown");
        scoreManager.Life = MaxLife;
    }

    public void UpdateLife(int newLife, int oldLife)
    {
        if (newLife < 0)
        {
            newLife = 0;
        }
        score.SetText($"Life {newLife}");
    }

    public override void _Process(float delta)
    {
        if (startCountdown)
        {
            scoreManager.Countdown -= delta;
            var s = scoreManager.Countdown.ToString("F2");
            _countdown.SetText($"Countdown {s}");
        }
    }

    public void StartCountdown(float cd)
    {
        startCountdown = true;
    }

}
