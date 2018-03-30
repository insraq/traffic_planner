using Godot;
using System;

public class ScoreManager : Node2D
{
    private const int MAX_CONCURRENT_SOUND = 2;
    private int concurrentSound = 0;

    public void PlaySoundStart()
    {
        concurrentSound++;
    }

    public void PlaySoundEnd()
    {
        if (concurrentSound <= 0)
        {
            GD.Print("WARNING: PlaySoundEnd() called when concurrentSound <= 0");
        }
        concurrentSound--;
    }

    public Boolean ShouldPlaySound()
    {
        return concurrentSound < MAX_CONCURRENT_SOUND;
    }
}
