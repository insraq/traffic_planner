using Godot;
using System;

public class Score : Label
{

    [Export] private int MaxLife = 10;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;

    public override void _Ready()
    {
        this.WireNodes();
        scoreManager.Connect("LifeHasChanged", this, "OnLifeChange");
        scoreManager.Life = MaxLife;
    }

    public void OnLifeChange(int newLife, int oldLife)
    {
        if (newLife < 0)
        {
            newLife = 0;
        }
        SetText($"Life: {newLife}");
    }

}
