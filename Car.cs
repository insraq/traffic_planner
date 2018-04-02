using Godot;
using System;
using System.Collections.Generic;

public partial class Car : Area2D
{
    [Export] public CarDirection Direction { get; set; } = CarDirection.Up;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;

    private const int ACCE = 10;
    private const int MAX_SPEED = 150;
    private const int HEIGHT = 7;
    private const int WIDTH = 5;
    private int speed;
    private Random rand = new Random();
    public HashSet<Area2D> overlap = new HashSet<Area2D>();

    public int GetSpeed()
    {
        return speed;
    }

    public void SetDirection(CarDirection dir)
    {
        Direction = dir;
    }

    public CarDirection GetDirection()
    {
        return Direction;
    }

    public override void _Ready()
    {
        this.WireNodes();
        SetInitialRotate();
        speed = MAX_SPEED / ACCE;
    }

    private void SetInitialRotate()
    {
        if (Direction == CarDirection.Down)
        {
            Rotate(Mathf.Deg2Rad(180));
        }
        if (Direction == CarDirection.Left)
        {
            Rotate(Mathf.Deg2Rad(270));
        }
        if (Direction == CarDirection.Right)
        {
            Rotate(Mathf.Deg2Rad(90));
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Car car)
        {
            var myPos = GetHeadPosition();
            var theirPos = car.GetHeadPosition();
            if (
                (Direction == CarDirection.Up && myPos.y - HEIGHT < theirPos.y + WIDTH) ||
                (Direction == CarDirection.Down && myPos.y + HEIGHT > theirPos.y - WIDTH) ||
                (Direction == CarDirection.Left && myPos.x - HEIGHT < theirPos.x + WIDTH) ||
                (Direction == CarDirection.Right && myPos.x + HEIGHT > theirPos.x - WIDTH) ||
                car.overlap.Contains(this))
            {
                return;
            }
            overlap.Add(car);
            Stop();
        }

        if (area is Crossroad cr)
        {
            if (cr.CanPass(Direction))
            {
                return;
            }
            Stop();
        }
    }

    public Vector2 GetHeadPosition()
    {
        var myPos = GetGlobalPosition();
        return new Vector2(myPos.x, myPos.y - HEIGHT);
    }

    private void OnAreaExited(Area2D area)
    {
        overlap.Remove(area);
        if (overlap.Count == 0)
        {
            Start();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (speed > 0 && speed < MAX_SPEED)
        {
            speed += MAX_SPEED / ACCE;
        }

        if (Direction == CarDirection.Up)
        {
            GlobalPosition += new Vector2(0, -speed * delta);
        }

        if (Direction == CarDirection.Down)
        {
            GlobalPosition += new Vector2(0, speed * delta);
        }

        if (Direction == CarDirection.Left)
        {
            GlobalPosition += new Vector2(-speed * delta, 0);
        }

        if (Direction == CarDirection.Right)
        {
            GlobalPosition += new Vector2(speed * delta, 0);
        }
    }

    public override void _Process(float delta)
    {
        // Out of canvas, free
        if (!GetViewportRect().HasPoint(GetGlobalPosition()))
        {
            QueueFree();
        }
    }

    public void Stop()
    {
        speed = 0;
    }

    public void Start()
    {
        speed = MAX_SPEED / ACCE;
    }
}


