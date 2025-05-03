using Godot;
using System;

public partial class TestSpawner : Node2D
{
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://TestProjectile.tscn");

	public static PackedScene Enemy { get; } = GD.Load<PackedScene>("res://EnemyTest.tscn");
	public static PackedScene EnemyShooter { get; } = GD.Load<PackedScene>("res://EnemyShooter.tscn");
	public static PackedScene Laser { get; } = GD.Load<PackedScene>("res://Laser.tscn");

	private Node2D _player;
	private float laser_cooldown = 0f;

	public void SpawnObject()
	{
		if (Bullet == null || _player == null) return;

		Node2D instance = (Node2D)Bullet.Instantiate();

		instance.Position = _player.GlobalPosition;

		GetTree().CurrentScene.AddChild(instance);
		
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
		for (int i = 0; i < 3; i++)
		{
			Node2D instance = (Node2D)Enemy.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", instance);
		}
		for (int i = 0; i < 5; i++)
		{
			Node2D instance = (Node2D)EnemyShooter.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", instance);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (laser_cooldown > 0)
		{
			laser_cooldown -= (float)delta;
		}
		else if (laser_cooldown < 0)
		{
			laser_cooldown = 0;
		}
		if (Input.IsActionJustPressed("Test_Key")) {
			SpawnObject();
		} else if (Input.IsActionJustPressed("Laser")) {
			if (laser_cooldown > 0) {
				return;
			}
			Node2D instance = (Node2D)Laser.Instantiate();
			instance.GlobalPosition = _player.GlobalPosition;
			GetTree().CurrentScene.AddChild(instance);
			laser_cooldown = 2f;
		}
	}
}
