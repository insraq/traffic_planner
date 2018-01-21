using Godot;
using System;
using System.Collections.Generic;

public class Car : Area2D
{
    public enum Direction { Up, Down, Left, Right };
    private const int SPEED = 150;
    private const int HEIGHT = 7;
    private const int WIDTH = 5;
    private const int ACCE = 10;
    private int speed = SPEED / ACCE;
    private Random rand = new Random();
    public HashSet<Area2D> overlap;

    [Export]
    private Direction direction = Direction.Up;

    public int GetSpeed()
    {
        return speed;
    }

    public void SetDirection(Direction dir)
    {
        direction = dir;
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public override void _Ready()
    {
        if (direction == Direction.Down)
        {
            Rotate(Mathf.Deg2Rad(180));
        }
        if (direction == Direction.Left)
        {
            Rotate(Mathf.Deg2Rad(270));
        }
        if (direction == Direction.Right)
        {
            Rotate(Mathf.Deg2Rad(90));
        }
        overlap = new HashSet<Area2D>();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Car car)
        {
            var myPos = GetHeadPosition();
            var theirPos = car.GetHeadPosition();
            if (
                (direction == Direction.Up && myPos.y - HEIGHT < theirPos.y + WIDTH) ||
                (direction == Direction.Down && myPos.y + HEIGHT > theirPos.y - WIDTH) ||
                (direction == Direction.Left && myPos.x - HEIGHT < theirPos.x + WIDTH) ||
                (direction == Direction.Right && myPos.x + HEIGHT > theirPos.x - WIDTH) ||
                car.overlap.Contains(this))
            {
                return;
            }
            overlap.Add(car);
            Stop();
        }

        if (area is Crossroad cr)
        {
            if (cr.CanPass(direction))
            {
                return;
            }
            Stop();
        }
    }

    public Vector2 GetHeadPosition()
    {
        var myPos = GetPosition();
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
        if (speed > 0 && speed < SPEED)
        {
            speed += SPEED / ACCE;
        }

        if (direction == Direction.Up)
        {
            Position += new Vector2(0, -speed * delta);
        }

        if (direction == Direction.Down)
        {
            Position += new Vector2(0, speed * delta);
        }

        if (direction == Direction.Left)
        {
            Position += new Vector2(-speed * delta, 0);
        }

        if (direction == Direction.Right)
        {
            Position += new Vector2(speed * delta, 0);
        }
    }

    public override void _Process(float delta)
    {
        // Out of canvas, free
        if (!GetViewportRect().HasPoint(GetPosition()))
        {
            QueueFree();
        }
    }

    public void Stop()
    {
        if (rand.Next(1, 4) == 1)
        {
            ((AudioStreamPlayer)GetNode("Honk")).Play();
        }
        speed = 0;
    }

    public void Start()
    {
        speed = SPEED / ACCE;
    }
}