using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyBullet : CharacterBody2D
{
	[Export] private float Speed = 400f;        // Speed of the enemy
	[Export] private float ShootingInterval = 2f; // Time between shots
	private Node2D _player;                   // Reference to the player
	private Random rng;
	private double lifetime = 10.0d;
	private bool directionSet = false;
	public override void _Ready()
	{
		rng = new Random();

		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");

		if (!directionSet)
		{
			GlobalRotation = GlobalPosition.AngleToPoint(_player.GlobalPosition);
		}
		// _shootTimer.Start();
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Move the bullet upward
		Vector2 direction = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation));
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null)
		{
			Node2D collider = collision.GetCollider() as Node2D;
			if (collider is Player player)
			{
				var player1 = collider as Player;
				player1.GotHit("You Were Shot");
				QueueFree();
			}
		}

		if (GlobalPosition.Y > 1000)
		{
			QueueFree();
		}

		lifetime -= delta;
		if (lifetime <= 0)
		{
			QueueFree();
		}
	}

	public void GotHit()
	{
		QueueFree();
	}

	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	public void SetDirection(Vector2 direction)
	{
		directionSet = true;
		GlobalRotation = direction.Normalized().Angle();  // make the sprite face the direction
	}
}
