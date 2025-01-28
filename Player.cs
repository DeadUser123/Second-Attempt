using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private Score scoreText; // Reference to the Score node
	public override void _Ready() {
		scoreText = GetNode<Score>("/root/Main/Score");
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * 0; // 0 because there's no gravity in space lol
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
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
		KinematicCollision2D collision = MoveAndCollide(velocity * (float) delta);
		if (collision != null) {
			Node2D collider = collision.GetCollider() as Node2D;
			if (collider is EnemyScript enemyScript) {
				var enemy = collider as EnemyScript;
				enemy.relocate();
				scoreText.ChangeScore(-200);
			}
		}
	}
}
