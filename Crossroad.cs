using Godot;
using System;

public class Crossroad : Area2D
{
    public enum Mode { Horizontal, Vertical, None };

    [Export] private Mode _mode = Mode.None;
    [Export] private Texture horizontal;
    [Export] private Texture vertical;
    [Export] private Texture none;

    [Node("./Image")] private Sprite image;


    public override void _Ready()
    {
        this.WireNodes();
        SetMode(Mode.Horizontal);
    }

    public Mode GetMode()
    {
        return _mode;
    }

    public void SetMode(Mode mode)
    {
        _mode = mode;
        switch (_mode)
        {
            case Mode.Horizontal:
                image.SetTexture(horizontal);
                StarCars();
                return;
            case Mode.Vertical:
                image.SetTexture(vertical);
                StarCars();
                return;
            default:
                image.SetTexture(none);
                return;
        }

    }

    public Boolean CanPass(Car.CarDirection direction)
    {
        return GetMode() == Mode.Vertical && (direction == Car.CarDirection.Up || direction == Car.CarDirection.Down) ||
            GetMode() == Mode.Horizontal && (direction == Car.CarDirection.Left || direction == Car.CarDirection.Right);
    }

    private void StarCars()
    {
        var areas = GetOverlappingAreas();
        foreach (var area in areas)
        {
            if (area is Car car && CanPass(car.GetDirection()) && car.GetSpeed() == 0)
            {
                car.Start();
            }
        }
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed())
        {
            if (GetMode() == Mode.Horizontal)
            {
                SetMode(Mode.Vertical);
                return;
            }

            SetMode(Mode.Horizontal);
            return;
        }
    }


    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
