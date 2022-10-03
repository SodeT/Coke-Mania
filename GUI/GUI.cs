using Godot;
using System;

public class GUI : Control
{
    // Level setup
    [Export] int CokeNumber = 10;
    [Export] float LevelTime = 10f;
    [Export] string LevelPath = "";
    [Export] string NextLevelPath = "";

    float LevelEndTime = 0f;
    float EndDelay = 0.2f;
    bool Next = false;
    bool End = false;

    [Export] NodePath PlayerPath;
    KinematicBody2D Player;

    // Node setup
    [Export] NodePath TimeBarPath;
    ProgressBar TimeBar;

    [Export] NodePath TimeLeftPath;
    Label TimeLeft;

    [Export] NodePath CurrentCokeNumberPath;
    Label CurrentCokeNumber;

    [Export] NodePath AudioFailPath;
    AudioStreamPlayer AudioFail;

    [Export] NodePath AudioJumpPath;
    AudioStreamPlayer AudioJump;

    [Export] NodePath AudioDrinkPath;
    AudioStreamPlayer AudioDrink;

    [Export] NodePath AudioLevelPath;
    AudioStreamPlayer AudioLevel;


    public override void _Ready()
    {
        TimeBar = GetNode<ProgressBar>(TimeBarPath);
        CurrentCokeNumber = GetNode<Label>(CurrentCokeNumberPath);
        TimeLeft = GetNode<Label>(TimeLeftPath);

        Player = GetNode<KinematicBody2D>(PlayerPath);

        AudioFail = GetNode<AudioStreamPlayer>(AudioFailPath);
        AudioJump = GetNode<AudioStreamPlayer>(AudioJumpPath);
        AudioDrink = GetNode<AudioStreamPlayer>(AudioDrinkPath);
        AudioLevel = GetNode<AudioStreamPlayer>(AudioLevelPath);

    }

    public override void _Process(float delta)
    {
        LevelTime -= delta;
        TimeBar.Value = LevelTime;
        TimeLeft.Text = ((int)LevelTime).ToString() + "s left";

        CurrentCokeNumber.Text = ((int)Player.Get("CokeScore")).ToString() + " Cokes";

        if ((bool)Player.Get("DrinkingCoke") && !AudioDrink.Playing && (!End || !Next))
        {
            AudioDrink.Play();
        }

        if ((int)Player.Get("CokeScore") >= CokeNumber && !End)
        {
            Next = true;
        }
        else if (LevelTime < 0f || Input.IsActionPressed("RESET") && !Next)
        {
            End = true;
        }

        if (Next)
        {
            LevelEndTime += delta;
            if (!AudioLevel.Playing)
            {
                AudioLevel.Play();
            }
            if (LevelEndTime > EndDelay)
            {
                GetTree().ChangeScene(NextLevelPath);
            }
        }
        else if (End)
        {
            LevelEndTime += delta;
            if (!AudioFail.Playing)
            {
                AudioFail.Play();
            }
            if (LevelEndTime > EndDelay)
            {
                GetTree().ChangeScene(LevelPath);
            }
        }


    }
}
