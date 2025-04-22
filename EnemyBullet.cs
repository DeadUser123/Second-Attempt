using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyBullet : CharacterBody2D
{
	[Export] public float Speed = 400f;        // Speed of the enemy
	[Export] public float ShootingInterval = 2f; // Time between shots
	private Vector2 _direction;               // Movement direction
	private Node2D _player;                   // Reference to the player
	private Random rng;
	public override void _Ready()
	{
		rng = new Random();

		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");

		_direction = Vector2.Down;
		// _shootTimer.Start();
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Move the bullet upward
		Vector2 direction = Vector2.Down;
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
				player1.GotHit("A Bullet");
				QueueFree();
			}
		}

		if (GlobalPosition.Y > 1000)
		{
			QueueFree();
		}
	}

	public void GotHit()
	{
		QueueFree();
	}
}
