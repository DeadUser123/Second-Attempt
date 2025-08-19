using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 600.0f;
    public const float JumpVelocity = -400.0f;
    private Score scoreText;

    // Projectile prefabs
    public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://TestProjectile.tscn");
    public static PackedScene Laser { get; } = GD.Load<PackedScene>("res://Laser.tscn");

    private float laserCooldown = 0f;

    public override void _Ready()
    {
        scoreText = GetNode<Score>("/root/Gameplay/Score");
        AddToGroup("players");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Movement
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

        // Clamp inside screen
        // if (this.GlobalPosition.Y < 0 && velocity.Y < 0) velocity.Y = 0;
        // else if (this.GlobalPosition.Y > 650 && velocity.Y > 0) velocity.Y = 0;
        // else if (this.GlobalPosition.X < 0 && velocity.X < 0) velocity.X = 0;
        // else if (this.GlobalPosition.X > 1150 & velocity.X > 0) velocity.X = 0;

        // ✅ Rotate towards mouse
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 toMouse = mousePos - GlobalPosition;
        Rotation = toMouse.Angle() + Mathf.Pi / 2;

        // ✅ Handle cooldown
        if (laserCooldown > 0)
            laserCooldown -= (float)delta;
        else if (laserCooldown < 0)
            laserCooldown = 0;

        // ✅ Handle shooting input
        if (Input.IsActionJustPressed("Test_Key"))
        {
            ShootBullet(Rotation);
        }
        else if (Input.IsActionJustPressed("Laser"))
        {
            ShootLaser();
        }

        Velocity = velocity;
        KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);
        if (collision != null)
        {
            Node2D collider = collision.GetCollider() as Node2D;
            if (collider is EnemyScript)
            {
                GetTree().SetMeta("death_message", "You Were Killed By A Generic Enemy");
                GetTree().SetMeta("score", scoreText.GetScore());
                GetTree().ChangeSceneToFile("res://GameOver.tscn");
            }
            else if (collider is Missile)
            {
                GetTree().SetMeta("death_message", "You Were Shot Down By Missile");
                GetTree().SetMeta("score", scoreText.GetScore());
                GetTree().ChangeSceneToFile("res://GameOver.tscn");
            }
            else if (collider is EnemyShooter)
            {
                GetTree().SetMeta("death_message", "You Collided With a Shooter");
                GetTree().SetMeta("score", scoreText.GetScore());
                GetTree().ChangeSceneToFile("res://GameOver.tscn");
            }
        }
    }

    // === New Shooting Functions ===
    private void ShootBullet(double rotation)
    {
        if (Bullet == null) return;
        Node2D instance = (Node2D)Bullet.Instantiate();
        instance.GlobalPosition = GlobalPosition;
		instance.GlobalRotation = (float) rotation - Mathf.Pi / 2;
        GetTree().CurrentScene.AddChild(instance);
    }

    private void ShootLaser()
    {
        if (Laser == null || laserCooldown > 0) return;
        Node2D instance = (Node2D)Laser.Instantiate();
        instance.GlobalPosition = GlobalPosition;
        GetTree().CurrentScene.AddChild(instance);
        laserCooldown = 2f; // 2 seconds cooldown
    }

    public void GotHit(String death_message)
    {
        GetTree().SetMeta("death_message", death_message);
        GetTree().SetMeta("score", scoreText.GetScore());
        GetTree().ChangeSceneToFile("res://GameOver.tscn");
    }
}
