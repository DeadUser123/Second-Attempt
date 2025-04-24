using Godot;
using System;

public partial class Explosion : Node2D
{
	public float size = 1f;
	private float time = 0f;
	private Area2D area;

	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");

		var collisionShape = area.GetNode<CollisionShape2D>("CollisionShape2D");
		if (collisionShape.Shape is CircleShape2D circle)
		{
			circle.Radius *= size;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		time += (float)delta;
		if (time > 0.5f)
		{
			QueueFree();
		}

		// var overlappingBodies = area.GetOverlappingBodies();
		// foreach (var collider in overlappingBodies)
		// {
		// 	if (collider is EnemyScript enemyScript)
		// 	{
		// 		var enemy = collider as EnemyScript;
		// 		enemy.GotHit();
		// 		QueueFree();
		// 	}
		// 	else if (collider is Missile missile)
		// 	{
		// 		var enemy = collider as Missile;
		// 		enemy.GotHit();
		// 		QueueFree();
		// 	}
		// 	else if (collider is EnemyShooter enemyShooter)
		// 	{
		// 		var enemy = collider as EnemyShooter;
		// 		enemy.GotHit();
		// 		QueueFree();
		// 	}
		// 	else if (collider is EnemyBullet c)
		// 	{
		// 		var enemy = collider as EnemyBullet;
		// 		enemy.GotHit();
		// 		QueueFree();
		// 	}
		// 	else if (collider is Player player)
		// 	{
		// 		var player1 = collider as Player;
		// 		player1.GotHit("You Got Caught in the Blast");
		// 		QueueFree();
		// 	}
		// }
	}
}
