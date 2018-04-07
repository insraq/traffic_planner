using Godot;
using System;
using System.Collections.Generic;

public partial class Car : Area2D
{
    [Export] public CarDirection Direction { get; set; } = CarDirection.Up;
    [Export] private int acce = 10;
    [Export] private int maxSpeed = 150;
    [Node("/root/ScoreManager")] private ScoreManager scoreManager;

    public HashSet<Area2D> overlap = new HashSet<Area2D>();
    public bool Counted { get; set; }

    private int speed;
    private Random rand = new Random();


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
        speed = maxSpeed;
        Counted = false;
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
            var myPos = GetGlobalPosition();
            var theirPos = car.GetGlobalPosition();
            if (car.Direction == CarDirection.Up && myPos.y < theirPos.y) { return; }
            if (car.Direction == CarDirection.Down && myPos.y > theirPos.y) { return; }
            if (car.Direction == CarDirection.Left && myPos.x < theirPos.x) { return; }
            if (car.Direction == CarDirection.Right && myPos.x > theirPos.x) { return; }
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

    private void OnAreaExited(Area2D area)
    {
        if (overlap.Count == 0)
        {
            return;
        }
        overlap.Remove(area);
        Start();
    }

    public override void _Process(float delta)
    {
        // Out of canvas, free
        if (!GetViewportRect().HasPoint(GetGlobalPosition()))
        {
            QueueFree();
        }
        if (speed > 0 && speed < maxSpeed)
        {
            speed += maxSpeed / acce;
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

    public void Stop()
    {
        speed = 0;
    }

    public void Start()
    {
        if (overlap.Count == 0)
        {
            speed = maxSpeed / acce;
        }
    }
}


