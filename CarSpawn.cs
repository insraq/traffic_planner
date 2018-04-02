using Godot;
using System;
using System.Collections.Generic;

public class CarSpawn : Area2D
{
    [Export] private Car.CarDirection direction;
    [Export] private readonly PackedScene carScene1;
    [Export] private readonly PackedScene carScene2;
    [Export] private readonly PackedScene carScene3;
    [Export] private readonly NodePath spawnTargetNode;
    [Export] public bool AutoSpawn { get; set; } = true;
    [Export] public float SpawnChance { get; set; } = 0.5f;

    [Node("/root/ScoreManager")] private readonly ScoreManager scoreManager;
    [Node("Explode")] private readonly Sprite explode;
    [Node("Animation")] private readonly AnimationPlayer animationPlayer;
    [Node("Timer")] private readonly Timer timer;
    [Node("Honk")] private readonly AudioStreamPlayer honk;

    private readonly Random random = new Random();
    private readonly List<PackedScene> carScenes = new List<PackedScene>();
    private Node spawnTarget;

    public override void _Ready()
    {
        this.WireNodes();
        AddToScenes(carScene1);
        AddToScenes(carScene2);
        AddToScenes(carScene3);
        explode.Visible = false;
        spawnTarget = GetNode(spawnTargetNode);
    }

    private void AddToScenes(PackedScene scene)
    {
        if (scene != null)
        {
            carScenes.Add(scene);
        }
    }

    public override void _Process(float delta)
    {
    }

    private void OnTimeOut()
    {
        var areas = GetOverlappingAreas();
        foreach (var area in areas)
        {
            if (area is Car car && car.GetDirection() == direction)
            {
                scoreManager.Life--;
                scoreManager.PlaySound(honk);
                if (!animationPlayer.IsPlaying())
                {
                    explode.SetVisible(true);
                    animationPlayer.Play("Explode");
                }
                return;
            }
        }
        if (AutoSpawn)
        {
            var roll = random.FloatRange(0, 1);
            if (roll > SpawnChance)
            {
                return;
            }
            Spawn();
        }
    }

    private void Spawn()
    {
        var carScene = carScenes[random.Next(carScenes.Count)];
        var newCar = (Car)carScene.Instance();
        newCar.SetDirection(direction);
        newCar.SetGlobalPosition(GetGlobalPosition());
        spawnTarget.AddChild(newCar);
    }
}
