using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyShooter : CharacterBody2D
{
	[Export] public float speed = 400f;        // Speed of the enemy
	[Export] public float ShootingInterval = 2f; // Time between shots
	private Vector2 _direction;               // Movement direction
	private double shooting_time;                // Timer for shooting
	private double direction_decision_time;
	private Node2D _player;                   // Reference to the player
	private Score scoreText; // Reference to the Score node
	private List<Vector2> directions = new List<Vector2>();
	private Random rng;
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");
	public static PackedScene Missile { get; } = GD.Load<PackedScene>("res://Missile.tscn");
	public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");
	private bool isRecentlyHit = false;
	private float hitCooldown = 0.1f; // Prevent being hit more than once every 0.1s
	private uint originalcollisionlayer;
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
		shooting_time = 0;
		GlobalPosition = new Vector2(-100 + rng.Next(0, 2) * 1250, -100 + rng.Next(0, 2) * 750);
	}

	public void GotHit()
	{
		if (isRecentlyHit) {
			hitCooldown = 0.1f;
			return;
		}

		isRecentlyHit = true;

		originalcollisionlayer = CollisionLayer;
		CollisionLayer = 0;
		Node2D instance = (Node2D)Explosion.Instantiate();

		instance.GlobalPosition = this.GlobalPosition;

		GetTree().CurrentScene.AddChild(instance);
		scoreText.ChangeScore(100);
		this.Relocate();
		shooting_time = rng.Next(-2, 2);
		CollisionLayer = originalcollisionlayer;
	}

	public void Relocate()
	{
		GlobalPosition = new Vector2(-100 + rng.Next(0, 2) * 1250, -100 + rng.Next(0, 2) * 750);
	}
	public void Shoot()
	{
		if (Bullet == null || Missile == null) return;
		Node2D instance;
		if (rng.Next(0, 10) != 0)
		{
			instance = (Node2D)Bullet.Instantiate();
		}
		else
		{
			instance = (Node2D)Missile.Instantiate();
		}

		instance.GlobalPosition = this.GlobalPosition;

		GetTree().CurrentScene.AddChild(instance);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (isRecentlyHit && hitCooldown <= 0)
		{
			isRecentlyHit = false;
		} else if (isRecentlyHit) {
			hitCooldown -= (float)delta;
		}
		direction_decision_time += delta;
		shooting_time += delta;
		if (direction_decision_time > 0.2)
		{
			if (rng.Next(0, 5) == 0)
			{
				_direction = directions[rng.Next(0, 4)];
			}
			direction_decision_time = 0;
		}
		if (this.Position.Y < 0)
		{
			_direction = Vector2.Down;
			direction_decision_time = 0;
		}
		if (this.Position.Y > 300)
		{
			_direction = Vector2.Up;
			direction_decision_time = 0;
		}
		if (this.Position.X < 0)
		{
			_direction = Vector2.Right;
			direction_decision_time = 0;
		}
		if (this.Position.X > 1150)
		{
			_direction = Vector2.Left;
			direction_decision_time = 0;
		}
		Position += _direction * speed * (float)delta;

		if (shooting_time > 3)
		{
			Shoot();
			shooting_time = rng.Next(-2, 2);
		}
	}
	
}
