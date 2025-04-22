using Godot;
using System;

public partial class Explosion : CharacterBody2D
{
	public float max_size = 1f;
	public float curr_size = 0f;
	private float transparency = 1f;

	public override void _Ready()
	{
		Scale = new Vector2(0, 0);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (curr_size < max_size)
		{
			curr_size += 4f * (float)delta;
		}
		else if (curr_size < 1.5f * max_size || transparency > 0)
		{
			if (curr_size < 1.5f * max_size) curr_size += 1f * (float)delta;
			if (transparency > 0) transparency -= 1f * (float)delta;
		}
		else
		{
			QueueFree();
		}
		Scale = new Vector2(curr_size, curr_size);
		Modulate = new Color(1, 1, 1, transparency);
	}
}
