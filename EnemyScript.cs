using Godot;
using System;

public partial class EnemyScript : CharacterBody2D
{
	private Score scoreText; // Reference to the Score node
    public override void _Ready()
    {
        Area2D area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
		scoreText = GetNode<Score>("/root/Main/Score");
    }

	private void OnBodyEntered(Node body) {
		GD.Print("Hit!");
		scoreText.ChangeScore(100);
	}
    public override void _Process(double delta)
	{
		
	}
}
