using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyScript : CharacterBody2D {
	[Export] public float speed = 400f;        // Speed of the enemy
    [Export] public float ShootingInterval = 2f; // Time between shots
    private Vector2 _direction;               // Movement direction
    private Timer _shootTimer;                // Timer for shooting
	private double direction_decision_time;
	private Node2D _player;                   // Reference to the player
	private Score scoreText; // Reference to the Score node
	private List<Vector2> directions = new List<Vector2>();
	private Random rng;
	public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");
	private float spawn_immunity = 1.0f;
    public override void _Ready()
	{
		directions.Add(Vector2.Up);
		directions.Add(Vector2.Down);
		directions.Add(Vector2.Left);
		directions.Add(Vector2.Right);
		rng = new Random();

		scoreText = GetNode<Score>("/root/Gameplay/Score");

		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");

		_direction = directions[rng.Next(0, 4)];
		// _shootTimer.Start();
		direction_decision_time = 0;
		Position = new Vector2(-100 + rng.Next(0, 2) * 1250, -100 + rng.Next(0, 2) * 750);
	}

	public void GotHit() {
		// if (spawn_immunity >= 0) return;
		scoreText.ChangeScore(100);

		Node2D instance = (Node2D)Explosion.Instantiate();

		instance.Position = this.GlobalPosition;

		GetTree().CurrentScene.AddChild(instance);

		this.relocate();
	}

	public void relocate() {
		Position = new Vector2(-100 + rng.Next(0, 2) * 1250, -100 + rng.Next(0, 2) * 750);
	}
    public override void _PhysicsProcess(double delta)
    {
		if (spawn_immunity >= 0) spawn_immunity -= (float)delta;
		direction_decision_time += delta;
		if (direction_decision_time > 0.2) {
			if (rng.Next(0, 5) == 0) {
				_direction = directions[rng.Next(0, 4)];
			}
			direction_decision_time = 0;
		}
		if (this.GlobalPosition.Y < 0) {
			_direction = Vector2.Down;
			direction_decision_time = 0;
		}
		if (this.GlobalPosition.Y > 650)
		{
			_direction = Vector2.Up;
			direction_decision_time = 0;
		}
		if (this.GlobalPosition.X < 0)
		{
			_direction = Vector2.Right;
			direction_decision_time = 0;
		}
		if (this.GlobalPosition.X > 1150)
		{
			_direction = Vector2.Left;
			direction_decision_time = 0;
		}
		Position += _direction * speed * (float) delta;
	}
}
