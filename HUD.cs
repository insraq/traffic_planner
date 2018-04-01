using Godot;
using System;

public class HUD : Node2D
{

    [Export] private int MaxLife = 10;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;
    [Node("Score")] private Label score;
    [Node("Countdown")] private Label countdown;
    [Node("Result")] private Panel result;
    [Node("Result/Title")] private Label title;
    [Node("Result/Button")] private Label button;

    private bool startCountdown;
    private Action _onPress;

    public override void _Ready()
    {
        this.WireNodes();
        scoreManager.Connect("LifeHasChanged", this, "UpdateLife");
        scoreManager.Connect("LevelFail", this, "GameOver");
        scoreManager.Life = MaxLife;
    }

    public override void _Process(float delta)
    {
        var s = scoreManager.Countdown.ToString("F2");
        countdown.SetText($"Countdown {s}");
    }

    private void UpdateLife(int newLife, int oldLife)
    {
        score.SetText($"Life {newLife}");
    }

    private void StartCountdown(float cd)
    {
        startCountdown = true;
    }

    private void GameOver()
    {
        ShowResult("Game Over", "Touch to Retry", () =>
        {
            GetTree().ReloadCurrentScene();
        });
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
