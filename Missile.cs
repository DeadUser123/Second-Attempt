using Godot;
using System;

public partial class Missile : CharacterBody2D
{
	private Node2D _player;

	private Vector2 acceleration;
	private Vector2 velocity;
	private Random rng;
	private Score scoreText;
	public override void _Ready()
	{
		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
		scoreText = GetNode<Score>("/root/Gameplay/Score");
		acceleration = new Vector2(0, 0);
		rng = new Random();
		double angle = 2 * Math.PI * rng.NextDouble();
		velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		velocity *= 1000; // make the intial one really large for dramatic effect
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 destination = _player.GlobalPosition;
		acceleration = destination - GlobalPosition;
		velocity += acceleration.LimitLength((float)(100 * Math.Sqrt(2.0)));
		velocity = velocity.LimitLength((float)(600 * Math.Sqrt(2.0)));
		Position += velocity * (float)delta;
		Rotation = velocity.Angle() + (float)Math.PI / 2;
	}

	public void GotHit()
	{
		scoreText.ChangeScore(423);
		QueueFree();
	}
}
