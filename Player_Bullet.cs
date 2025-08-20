using Godot;
using System;

public partial class Player_Bullet : CharacterBody2D
{
	public const int damage = 1;
	public const float speed = 1600.0f;
	private double lifetime = 5.0d;

	public override void _Process(double delta)
	{
		Vector2 velocity = Velocity;

		// Move bullet in facing direction
		Vector2 direction = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation));
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
		}
		else
		{
			velocity = Velocity.MoveToward(Vector2.Zero, speed);
		}

		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null)
		{
			Node2D collider = collision.GetCollider() as Node2D;

			if (collider is EnemyBase enemy)
			{
				enemy.GotHit(damage);
				QueueFree();
			}
			else if (collider is EnemyBullet enemyBullet)
			{
				enemyBullet.GotHit();
				QueueFree();
			}
			else if (collider is Missile missile)
			{
				missile.GotHit();
				QueueFree();
			}

		}

		lifetime -= delta;
		if (lifetime <= 0)
			QueueFree();
	}
}
