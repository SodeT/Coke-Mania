using Godot;
using System;

public class Start : Node2D
{

    [Export] string LevelPath = "";

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("START"))
        {
            GetTree().ChangeScene(LevelPath);
        }

    }
}
