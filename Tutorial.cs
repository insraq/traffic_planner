using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Tutorial : Label
{
    [Node("../CarSpawn1")] private CarSpawn bottom;
    [Node("../CarSpawn2")] private CarSpawn left;
    [Node("../CarSpawn3")] private CarSpawn right;
    [Node("../CarSpawn4")] private CarSpawn top;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;
    [Node("Hand")] private Node2D hand;

    private Queue<Step> steps = new Queue<Step>();
    private string subtitle;

    public override void _Ready()
    {
        this.WireNodes();
        scoreManager.Countdown = 15f;
        var spawners = new List<CarSpawn> { bottom, left, right, top };
        spawners.ForEach((s) => s.AutoSpawn = false);

        steps.Enqueue(new Step("Welcome, the new traffic light\nTouch to continue...", () => { }));
        steps.Enqueue(new Step("Traffic comes from all directions\nAnd they need to pass the crossroad", () =>
        {
            left.AutoSpawn = true;
            right.AutoSpawn = true;
            left.SpawnChance = 0.5f;
        }));
        steps.Enqueue(new Step("You can touch the crossroad\nto switch allowed directions", () =>
        {
            spawners.ForEach((s) => s.SpawnChance = 0.1f);
            spawners.ForEach((s) => s.AutoSpawn = true);
            hand.Visible = true;
        }));
        steps.Enqueue(new Step("If the traffic jam reaches the\nend of the screen, you lose a life", () =>
        {
            hand.Visible = false;
        }));
        steps.Enqueue(new Step("Now hold on for antoehr 15s\nGood luck and have fun", () =>
        {
            spawners.ForEach((s) => s.SpawnChance = 0.25f);
            scoreManager.StartCountdown(scoreManager.Countdown);
        }));

        NextStep();
    }

    private void NextStep()
    {
        Text = "";
        if (steps.Count == 0)
        {
            return;
        }
        Step step = steps.Dequeue();
        subtitle = step.Subtitle;
        step.CodeToRun();
    }

    public override void _Process(float delta)
    {
        if (subtitle.Length == 0)
        {
            return;
        }
        Text += subtitle[0];
        subtitle = subtitle.Substring(1);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed())
        {
            if (subtitle.Length > 0)
            {
                Text += subtitle;
                subtitle = "";
            }
            else
            {
                NextStep();
            }

        }
    }

    public struct Step
    {
        public string Subtitle;
        public Action CodeToRun;

        public Step(string s, Action ctr)
        {
            Subtitle = s;
            CodeToRun = ctr;
        }
    }
}