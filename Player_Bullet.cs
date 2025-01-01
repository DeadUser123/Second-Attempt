using Godot;
using System;

public partial class Player_Bullet : CharacterBody2D
{
	public const float Speed = 300.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnBodyEntered(Node body) {
		GD.Print("oof!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Vector2.Up;
		if (direction != Vector2.Zero) {
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
