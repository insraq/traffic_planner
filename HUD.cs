using Godot;
using System;
using System.Text.RegularExpressions;

public class HUD : Node2D
{

    [Export] private int maxLife = 10;
    [Export] private float autoCountdown = 0;
    [Export] private bool debug = ScoreManager.Debug;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;
    [Node("Score")] private Label score;
    [Node("ScoreShadow")] private Label scoreShadow;
    [Node("Countdown")] private Label countdown;
    [Node("CountdownShadow")] private Label countdownShadow;
    [Node("Result")] private Panel result;
    [Node("Result/Title")] private Label title;
    [Node("Result/Button")] private LinkButton button;
    [Node("MenuPanel")] private Panel menuPanel;
    [Node("MenuPanel/CreditContent")] private Label creditContent;
    [Node("MenuPanel/DebugOnly")] private Node2D debugOnly;

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
        SaveGame();
        if (debug)
        {
            debugOnly.Visible = true;
        }
    }

    private void SaveGame()
    {
        var file = new File();
        file.Open(ScoreManager.SaveFilePath, (int)File.ModeFlags.Write);
        file.StoreString(GetTree().GetCurrentScene().GetFilename());
        file.Close();
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
        ShowResult("F) Game Over", "Touch to Retry", () =>
        {
            GetTree().SetPause(false);
            GetTree().ReloadCurrentScene();
        });
        GetTree().SetPause(true);
    }

    private void NextLevel()
    {
        if (debug) return;
        ShowResult(GetSuccessfulTitle(), "Next Level", () =>
        {
            GetTree().SetPause(false);
            GetTree().ChangeScene(GetNextScene());
        });
        GetTree().SetPause(true);
    }

    private string GetNextScene()
    {
        var s = GetTree().GetCurrentScene().GetFilename();
        Match m = Regex.Match(s, @"Level(\d+)");
        var n = int.Parse(m.Groups[1].Value) + 1;
        return $"res://Level{n}.tscn";
    }

    private string GetSuccessfulTitle()
    {
        var l = scoreManager.Life;
        if (l >= 10)
        {
            return "A) Perfect";
        }
        else if (l >= 7)
        {
            return "B) Well done";
        }
        else if (l >= 4)
        {
            return "C) Good effort";
        }
        return "D) Barely pass";
    }

    private void ShowResult(string titleText, string buttonText, Action onPress)
    {
        title.SetText(titleText);
        button.SetText(buttonText.ToUpper());
        _onPress = onPress;
        result.Visible = true;
    }

    private void OnResultButtonPressed()
    {
        if (_onPress != null)
        {
            _onPress();
        }
        _onPress = null;
        result.Visible = false;
    }

    private void OnMenuGUIInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed())
        {
            menuPanel.Visible = true;
        }
    }

    private void OnRestartPressed()
    {
        GetTree().ReloadCurrentScene();
    }

    private void OnBackPressed()
    {
        menuPanel.Visible = false;
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    private void OnMusicToggled(bool pressed)
    {
        AudioServer.SetBusMute(0, !pressed);
    }

    private void OnCreditDown()
    {
        creditContent.SetVisible(true);
    }

    private void OnCreditUp()
    {
        creditContent.SetVisible(false);
    }

    private void OnClearSavePressed()
    {
        var dir = new Directory();
        dir.Remove(ScoreManager.SaveFilePath);
    }

}