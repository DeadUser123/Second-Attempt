using Godot;
using System;

public partial class ToGameButton : Button
{
    // Called when the node enters the scene tree for the first time.
    [Export] public string NextScenePath = "res://Gameplay.tscn";

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile(NextScenePath);
    }
}
