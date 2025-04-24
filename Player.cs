using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 600.0f;
	public const float JumpVelocity = -400.0f;
	private Score scoreText; // Reference to the Score node
	public override void _Ready()
	{
		scoreText = GetNode<Score>("/root/Gameplay/Score");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		if (this.GlobalPosition.Y < 0 && velocity.Y < 0) {
			velocity.Y = 0;
		} else if (this.GlobalPosition.Y > 650 && velocity.Y > 0) {
			velocity.Y = 0;
		} else if (this.GlobalPosition.X < 0 && velocity.X < 0) {
			velocity.X = 0;
		} else if (this.GlobalPosition.X > 1150 & velocity.X > 0) {
			velocity.X = 0;
		}

		Velocity = velocity;
		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null)
		{
			Node2D collider = collision.GetCollider() as Node2D;
			if (collider is EnemyScript enemyScript)
			{
				GetTree().SetMeta("death_message", "You Were Killed By A Generic Enemy");
				GetTree().SetMeta("score", scoreText.GetScore());
				GetTree().ChangeSceneToFile("res://GameOver.tscn");
			}
			else if (collider is Missile missile)
			{
				GetTree().SetMeta("death_message", "You Were Shot Down By Missile");
				GetTree().SetMeta("score", scoreText.GetScore());
				GetTree().ChangeSceneToFile("res://GameOver.tscn");
			}
			else if (collider is EnemyShooter enemyShooter)
			{
				GetTree().SetMeta("death_message", "You Collided With a Shooter");
				GetTree().SetMeta("score", scoreText.GetScore());
				GetTree().ChangeSceneToFile("res://GameOver.tscn");
			}
		}
	}

	public void GotHit(String death_message)
	{
		GetTree().SetMeta("death_message", death_message);
		GetTree().SetMeta("score", scoreText.GetScore());
		GetTree().ChangeSceneToFile("res://GameOver.tscn");
	}
}
