using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Crossroad : Area2D
{
    public enum Mode { Horizontal, Vertical, None };

    [Export] private Mode _mode = Mode.Horizontal;
    [Export] private Texture horizontal;
    [Export] private Texture vertical;
    [Export] private Texture none;
    [Export] private bool touchable = true;
    [Node("Image")] private Sprite image;

    public const string ModeHasChanged = "ModeHasChanged";

    private const int OuterIdx = 0;
    private HashSet<Car> outerOverlap = new HashSet<Car>();
    private HashSet<Car> innerOverlap = new HashSet<Car>();

    public override void _Ready()
    {
        this.WireNodes();
        AddUserSignal(ModeHasChanged);
        SetMode(_mode);
    }

    public void SetTouchable(bool isTouchable)
    {
        touchable = isTouchable;
    }

    public void SetMode(Mode mode)
    {
        EmitSignal(ModeHasChanged, mode, _mode);
        _mode = mode;
        switch (_mode)
        {
            case Mode.Horizontal:
                image.SetTexture(horizontal);
                return;
            case Mode.Vertical:
                image.SetTexture(vertical);
                return;
            default:
                image.SetTexture(none);
                return;
        }
    }

    public void ToggleMode()
    {
        switch (_mode)
        {
            case Mode.Horizontal:
                SetMode(Mode.Vertical);
                return;
            case Mode.Vertical:
                SetMode(Mode.Horizontal);
                return;
            default:
                SetMode(Mode.None);
                return;
        }
    }

    public bool DirectionAllowed(CarDirection direction)
    {
        if (_mode == Mode.Vertical)
        {
            return direction == CarDirection.Up || direction == CarDirection.Down;
        }
        if (_mode == Mode.Horizontal)
        {
            return direction == CarDirection.Left || direction == CarDirection.Right;
        }
        return false;
    }

    public bool CanPass(CarDirection direction)
    {
        var otherDirectionCount = innerOverlap
            .Where((c) => !DirectionAllowed(c.Direction))
            .Count();
        return otherDirectionCount == 0 && DirectionAllowed(direction);
    }

    public override void _Process(float delta)
    {
        var otherDirectionCount = innerOverlap
            .Where((c) => !DirectionAllowed(c.Direction))
            .Count();
        if (otherDirectionCount == 0)
        {
            var carsToStart = outerOverlap.Where((c) => c.GetSpeed() == 0 && DirectionAllowed(c.Direction));
            foreach (var car in carsToStart)
            {
                car.Start();
            }
        }
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed() && shapeIdx == OuterIdx && touchable)
        {
            if (_mode == Mode.Horizontal)
            {
                SetMode(Mode.Vertical);
                return;
            }

            SetMode(Mode.Horizontal);
            return;
        }
    }

    private void OnAreaShapeEntered(int areaId, Area2D area, int areaShapeIdx, int selfShapeIdx)
    {
        if (area is Car car)
        {
            if (selfShapeIdx == OuterIdx)
            {
                outerOverlap.Add(car);
            }
            else
            {
                innerOverlap.Add(car);
            }
        }
    }


    private void OnAreaShapeExited(int areaId, Area2D area, int area_shapeIdx, int selfShapeIdx)
    {
        if (area is Car car)
        {
            if (selfShapeIdx == OuterIdx)
            {
                outerOverlap.Remove(car);
            }
            else
            {
                innerOverlap.Remove(car);
            }
        }
    }

}