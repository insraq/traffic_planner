using Godot;
using System;

public class HUD : Node2D
{

    [Export] private int maxLife = 10;
    [Export] private float autoCountdown = 0;
    [Export] private PackedScene nextLevel;
    [Export] private bool debug;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;
    [Node("Score")] private Label score;
    [Node("ScoreShadow")] private Label scoreShadow;
    [Node("Countdown")] private Label countdown;
    [Node("CountdownShadow")] private Label countdownShadow;
    [Node("Result")] private Panel result;
    [Node("Result/Title")] private Label title;
    [Node("Result/Button")] private Label button;

    private bool startCountdown;
    private Action _onPress;

    public override void _Ready()
    {
        this.WireNodes();
        result.SetPauseMode(PauseModeEnum.Process);
        scoreManager.Connect("LifeHasChanged", this, "UpdateLife");
        scoreManager.Connect("LevelFail", this, "GameOver");
        scoreManager.Connect("LevelPass", this, "NextLevel");
        scoreManager.Life = maxLife;
        if (autoCountdown > 0)
        {
            scoreManager.StartCountdown(autoCountdown);
        }
    }

    public override void _Process(float delta)
    {
        var s = scoreManager.Countdown.ToString("F2");
        countdown.SetText($"Countdown {s}");
        countdownShadow.SetText($"Countdown {s}");
    }

    private void UpdateLife()
    {
        var l = scoreManager.Life;
        score.SetText($"Life {l}");
        scoreShadow.SetText($"Life {l}");
    }

    private void StartCountdown(float cd)
    {
        startCountdown = true;
    }

    private void GameOver()
    {
        if (debug) return;
        ShowResult("Game Over", "Touch to Retry", () =>
        {
            GetTree().SetPause(false);
            GetTree().ReloadCurrentScene();
        });
        GetTree().SetPause(true);
    }

    private void NextLevel()
    {
        if (debug) return;
        ShowResult("Mission Accomplished", "Next Level", () =>
        {
            GetTree().SetPause(false);
            GetTree().ChangeSceneTo(nextLevel);
        });
        GetTree().SetPause(true);
    }

    private void ShowResult(string titleText, string buttonText, Action onPress)
    {
        title.SetText(titleText);
        button.SetText(buttonText);
        _onPress = onPress;
        result.Visible = true;
    }

    private void OnGUIEvent(InputEvent @event)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed() && _onPress != null)
        {
            _onPress();
            _onPress = null;
            result.Visible = false;
        }
    }
}
