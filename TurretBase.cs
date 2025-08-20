using Godot;
using System;
using System.Collections.Generic;

public partial class TurretBase : CharacterBody2D
{
	[Export] public float speed = 400f;        // Speed of the enemy
	[Export] public float ShootingInterval = 2f; // Time between shots
	private Vector2 _direction;               // Movement direction
	private double shooting_time;                // Timer for shooting
	private double direction_decision_time;
	private Node2D _player;                   // Reference to the player
	private Node2D camera;
	private Score scoreText; // Reference to the Score node
	private List<Vector2> directions = new List<Vector2>();
	private Random rng;
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");
	public static PackedScene Missile { get; } = GD.Load<PackedScene>("res://Missile.tscn");
	public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");
	private bool isRecentlyHit = false;
	private float hitCooldown = 0.1f; // Prevent being hit more than once every 0.1s
	private uint originalcollisionlayer;
	private Sprite2D top;
	private const float RotationSpeed = 2.0f; // Speed of rotation towards the player
	private uint burstCount = 5;
	private float spread = 0.75f;
	private float firingInterval = 0.05f;

	public override void _Ready()
	{
		directions.Add(Vector2.Up);
		directions.Add(Vector2.Down);
		directions.Add(Vector2.Left);
		directions.Add(Vector2.Right);
		rng = new Random();

		scoreText = GetNode<Score>("/root/Gameplay/Score");

		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
		camera = GetNode<Node2D>("/root/Gameplay/Camera2D");

		_direction = directions[rng.Next(0, 4)];
		// _shootTimer.Start();
		direction_decision_time = 0;
		shooting_time = 0;

		top = GetNode<Sprite2D>("../top");
	}

	public override void _Process(double delta)
	{
		shooting_time -= delta;
		Vector2 direction = _player.GlobalPosition - top.GlobalPosition;
		float targetAngle = direction.Angle() + Mathf.Pi / 2;

		float angleDiff = Mathf.AngleDifference(top.GlobalRotation, targetAngle);

		// Clamp the step so we don’t overshoot
		float maxStep = RotationSpeed * (float)delta;
		angleDiff = Mathf.Clamp(angleDiff, -maxStep, maxStep);

		// Apply rotation
		top.GlobalRotation += angleDiff;

		if (shooting_time <= 0)
		{
			ShootBurst();
			shooting_time = ShootingInterval;
		}

		// MOVEMENT
	}

	public void GotHit() // because lasers are complicated
	{
		if (isRecentlyHit)
		{
			return;
		}

		isRecentlyHit = true;
		hitCooldown = 0.1f;

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

	private Vector2 GetRandomPosition(float minDistance, float maxDistance)
	{
		// Random angle in radians
		double angle = rng.NextDouble() * Math.PI * 2;

		// Random distance between min and max
		float distance = (float)(minDistance + rng.NextDouble() * (maxDistance - minDistance));

		// Polar → Cartesian
		Vector2 offset = new Vector2(
			(float)(Math.Cos(angle) * distance),
			(float)(Math.Sin(angle) * distance)
		);

		return offset;
	}

	public void Relocate()
	{
		GlobalPosition = GetRandomPosition(1500, 2000);
		GlobalPosition += camera.GlobalPosition;
	}

	public async void ShootBurst()
	{
		if (Bullet == null) return;

		for (int i = 0; i < burstCount; i++)
		{
			EnemyBullet instance = (EnemyBullet)Bullet.Instantiate();
			instance.GlobalPosition = this.GlobalPosition;

			float randomOffset = (float)((rng.NextDouble() - 0.5) * spread);
			float angle = top.GlobalRotation + randomOffset - Mathf.Pi / 2;
			instance.SetDirection(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

			GetTree().CurrentScene.AddChild(instance);

			// Small delay between bullets
			await ToSignal(GetTree().CreateTimer(firingInterval), "timeout"); 
		}
	}
}
