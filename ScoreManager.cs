using Godot;
using System;

public class ScoreManager : Node2D
{
    private const int MAX_CONCURRENT_SOUND = 2;
    private int concurrentSound = 0;

    public void PlaySoundStart()
    {
        concurrentSound++;
        // GD.Print("PlaySoundStart(); concurrentSound = ", concurrentSound);
    }

    public void PlaySoundEnd()
    {
        if (concurrentSound <= 0)
        {
            GD.Print("WARNING: PlaySoundEnd() called when concurrentSound <= 0");
        }
        concurrentSound--;
        // GD.Print("PlaySoundEnd(); concurrentSound = ", concurrentSound);
    }

    public Boolean ShouldPlaySound()
    {
        return concurrentSound < MAX_CONCURRENT_SOUND;
    }

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here

    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
