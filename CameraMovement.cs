using Godot;
using System;

public partial class CameraMovement : Node2D {
    
    private Node2D _player;
    public override void _Ready()
    {
        _player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
    }

    public override void _Process(double delta) {
        GlobalPosition = _player.GlobalPosition;
    }
}