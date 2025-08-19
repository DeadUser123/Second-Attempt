using Godot;
using System;

public partial class Score : RichTextLabel
{
	private int score;
	private Node2D camera;
	private Node2D _player;
	private Vector2 offset;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		offset = new Vector2(-576, -324);
		camera = GetNode<Node2D>("/root/Gameplay/Camera2D");
		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
		score = 0;
		this.Text = $"Score: {score}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = _player.GlobalPosition + offset;
	}

	public void ChangeScore(int amount) {
		score += amount;
		this.Text = $"Score: {score}";
	}

	public void SetScore(int new_score) {
		score = new_score;
	}

	public int GetScore() {
		return score;
	}
}
