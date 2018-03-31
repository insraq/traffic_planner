using Godot;
using System;
using System.Collections.Generic;

public class Tutorial : Label
{
    [Node("../CarSpawn1")] private CarSpawn bottom;
    [Node("../CarSpawn2")] private CarSpawn left;
    [Node("../CarSpawn3")] private CarSpawn right;
    [Node("../CarSpawn4")] private CarSpawn top;
    [Node("Arrow")] private Sprite arrow;

    private Queue<Step> steps = new Queue<Step>();
    private string subtitle;

    public override void _Ready()
    {
        this.WireNodes();

        bottom.ShouldSpawn = false;
        left.ShouldSpawn = false;
        right.ShouldSpawn = false;
        top.ShouldSpawn = false;

        steps.Enqueue(new Step("Welcome to Traffic Police\nTouch to continue...", () => { }));
        steps.Enqueue(new Step("Traffic comes from all directions\nAnd they need to pass the crossroad", () =>
        {
            left.ShouldSpawn = true;
            right.ShouldSpawn = true;
        }));
        steps.Enqueue(new Step("You can touch the crossroad\nto switch allowed directions", () =>
        {
            top.ShouldSpawn = true;
            bottom.ShouldSpawn = true;
            arrow.Visible = true;
        }));
        steps.Enqueue(new Step("If the traffic reaches the\nend of the screen, you lose a life", () =>
        {
            arrow.Visible = false;
        }));
        steps.Enqueue(new Step("Now you know the drill\nGood luck and have fun", () => { }));

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

    private void OnTimeOut()
    {
        if (subtitle.Length == 0)
        {
            return;
        }
        Text += subtitle[0];
        subtitle = subtitle.Substring(1);
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