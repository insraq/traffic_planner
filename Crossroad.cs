using Godot;
using System;

public class Crossroad : Area2D
{
    public enum Mode { Horizontal, Vertical, None };

    [Export]
    private Mode _mode = Mode.None;

    public override void _Ready()
    {
        SetMode(Mode.Horizontal);
        // Called every time the node is added to the scene.
        // Initialization here
    }

    public Mode GetMode()
    {
        return _mode;
    }

    public void SetMode(Mode mode)
    {
        var image = ((Sprite)GetNode("Image"));

        _mode = mode;
        switch (_mode)
        {
            case Mode.Horizontal:
                image.SetTexture((Texture)ResourceLoader.Load("road_x_hori.png"));
                StarCars();
                return;
            case Mode.Vertical:
                image.SetTexture((Texture)ResourceLoader.Load("road_x_vert.png"));
                StarCars();
                return;
            default:
                image.SetTexture((Texture)ResourceLoader.Load("road_x.png"));
                return;
        }

    }

    public Boolean CanPass(Car.Direction direction)
    {
        return GetMode() == Mode.Vertical && (direction == Car.Direction.Up || direction == Car.Direction.Down) ||
            GetMode() == Mode.Horizontal && (direction == Car.Direction.Left || direction == Car.Direction.Right);
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
