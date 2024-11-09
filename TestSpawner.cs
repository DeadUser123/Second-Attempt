using Godot;
using System;

public partial class TestSpawner : Node2D
{
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://TestProjectile.tscn");

	private Node2D _player;
	
	public void SpawnObject() {
        if (Bullet == null || _player == null) return;

        Node2D instance = (Node2D)Bullet.Instantiate();

        instance.Position = _player.Position;

        GetTree().CurrentScene.AddChild(instance);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Node2D>("/root/Main/CharacterBody2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Test_Key")) {
			SpawnObject();
		}
	}
}
