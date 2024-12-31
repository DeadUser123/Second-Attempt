using Godot;
using System;

public partial class EnemyCollisionDetection : CharacterBody2D
{

    public override void _Ready()
    {
        Area2D area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
    }

	private void OnBodyEntered(Node body) {
		GD.Print("Hit!");
	}
    public override void _Process(double delta)
	{
		
	}
}
