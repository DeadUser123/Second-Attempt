using Godot;
using System;

public partial class Player_Bullet : CharacterBody2D
{
	public const float Speed = 1600.0f;
	private double lifetime = 5.0d;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
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
			if (collider is EnemyScript enemyScript)
			{
				var enemy = collider as EnemyScript;
				enemy.GotHit();
				QueueFree();
			}
			else if (collider is Missile missile)
			{
				var enemy = collider as Missile;
				enemy.GotHit();
				QueueFree();
			}
			else if (collider is EnemyShooter enemyShooter)
			{
				var enemy = collider as EnemyShooter;
				enemy.GotHit();
				QueueFree();
			}
			else if (collider is EnemyBullet c)
			{
				var enemy = collider as EnemyBullet;
				enemy.GotHit();
				QueueFree();
			}
		}
		lifetime -= delta;
		if (lifetime <= 0) {
			QueueFree();
		}
	}

}
