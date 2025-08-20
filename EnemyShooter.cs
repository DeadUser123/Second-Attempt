using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyShooter : EnemyBase
{
	private double shooting_time;
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");
	public static PackedScene Missile { get; } = GD.Load<PackedScene>("res://Missile.tscn");

	public override void _Ready()
	{
		base._Ready();
		Relocate();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		shooting_time += delta;

		if (shooting_time > 3)
		{
			Shoot();
			shooting_time = rng.Next(-2, 2);
		}
	}

	private void Shoot()
	{
		Node2D instance = rng.Next(0, 10) == 0 ? (Node2D)Missile.Instantiate() : (Node2D)Bullet.Instantiate();
		instance.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(instance);
	}
}
