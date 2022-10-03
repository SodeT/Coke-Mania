using Godot;
using System;

public class GUI : Control
{
    // Level setup
    [Export] int CokeNumber = 10;
    [Export] float LevelTime = 10f;
    [Export] string LevelPath;
    [Export] string NextLevelPath;

    [Export] NodePath PlayerPath;
    KinematicBody2D Player;

    // Node setup
    [Export] NodePath TimeBarPath;
    ProgressBar TimeBar;

    [Export] NodePath TimeLeftPath;
    Label TimeLeft;

    [Export] NodePath CurrentCokeNumberPath;
    Label CurrentCokeNumber;



    public override void _Ready()
    {
        TimeBar = GetNode<ProgressBar>(TimeBarPath);
        CurrentCokeNumber = GetNode<Label>(CurrentCokeNumberPath);
        TimeLeft = GetNode<Label>(TimeLeftPath);

        Player = GetNode<KinematicBody2D>(PlayerPath);

    }

    public override void _Process(float delta)
    {
        LevelTime -= delta;
        TimeBar.Value = LevelTime;
        TimeLeft.Text = ((int)LevelTime).ToString() + "s left";

        CokeNumber.Text = Player.Get("CokeScore").ToString() + " Cokes";

        if ((int)Player.Get("CokeScore") >= CokeNumber)
        {
            GetTree().ChangeScene(NextLevelPath);
        }
        else if (LevelTime < 0f)
        {
            GetTree().ChangeScene(LevelPath);
        }
    }
}
