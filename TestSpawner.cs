using Godot;
using System;

public partial class TestSpawner : Node2D
{
	
	public TestSpawner DuplicateSelf() {
		TestSpawner duplicate = (TestSpawner)Duplicate();
		return duplicate;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Test_Key")) {
			var duplicateNode = DuplicateSelf();
			duplicateNode.Position = new Vector2(200, 100);
			GetParent().AddChild(duplicateNode);
		}
	}
}
