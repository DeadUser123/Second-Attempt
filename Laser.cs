using Godot;
using System;

public partial class Laser : RayCast2D
{
	private Node2D _player; 
	private float max_lifetime = 1f;
	private float time = 0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += (float)delta;
		if (time > max_lifetime)
		{
			QueueFree();
		}

		GlobalPosition = _player.GlobalPosition;
		Rotation = _player.Rotation;

		if (IsColliding())
		{
			Node2D collider = GetCollider() as Node2D;
			if (collider is EnemyBase enemyBase)
			{
				var enemy = collider as EnemyBase;
				enemy.GotHit();
			}
			else if (collider is Missile missile)
			{
				var enemy = collider as Missile;
				enemy.GotHit();
			}
		}

	}
}
