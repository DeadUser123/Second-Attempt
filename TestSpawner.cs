using Godot;
using System;
using System.Collections.Generic;

public partial class TestSpawner : Node2D
{
	public static PackedScene Enemy { get; } = GD.Load<PackedScene>("res://EnemyTest.tscn");
	public static PackedScene EnemyShooter { get; } = GD.Load<PackedScene>("res://EnemyShooter.tscn");
	private static PackedScene Stars { get; } = GD.Load<PackedScene>("res://Star.tscn");

	[Export] private Node2D camera; // drag your Camera2D here
	[Export] private int starCount = 200; 
	[Export] private float spawnRadius = 3000f;

	private List<Node2D> stars = new();

	public override void _Ready()
	{
		camera = GetNode<Node2D>("/root/Gameplay/Camera2D");
		// Spawn enemies
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

		// Spawn initial stars
		GD.Randomize();
		for (int i = 0; i < starCount; i++)
		{
			SpawnStar();
		}
	}

	public override void _Process(double delta)
	{
		Vector2 camPos = camera.GlobalPosition;

		foreach (var star in stars)
		{
			// If a star drifts too far from camera, recycle it
			if (star.GlobalPosition.DistanceTo(camPos) > spawnRadius * 1f)
			{
				star.GlobalPosition = camPos + RandomOffset();
			}
		}
	}

	private void SpawnStar()
	{
		var star = Stars.Instantiate<Node2D>();
		GetTree().CurrentScene.CallDeferred("add_child", star);

		star.GlobalPosition = camera.GlobalPosition + RandomOffset();
		stars.Add(star);
	}

	private Vector2 RandomOffset()
	{
		return new Vector2(
			(float)GD.RandRange(-spawnRadius, spawnRadius),
			(float)GD.RandRange(-spawnRadius, spawnRadius)
		);
	}
}
