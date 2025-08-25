using Godot;
using System;

public partial class EnemyBomber : EnemyBase
{
    public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");

    private enum BomberState { Approach, Attack, Retreat }
    private BomberState currentState = BomberState.Approach;

    private float stateTimer = 0f;
    private float retreatDuration = 2.5f;
    private float attackCooldown = 1.0f;
    private bool strafeRight = true;
    private float acceleration = 1600f;
    private float maxSpeed = 800f;
    private Vector2 desiredDir = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
		Relocate();
        speed = maxSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        stateTimer -= (float)delta;

        switch (currentState)
        {
            case BomberState.Approach:
                if (_player != null && GlobalPosition.DistanceTo(_player.GlobalPosition) < 500f)
                {
                    currentState = BomberState.Attack;
                    stateTimer = attackCooldown;
                }
                break;

            case BomberState.Attack:
                Shoot();
                currentState = BomberState.Retreat;
                stateTimer = retreatDuration;
                strafeRight = !strafeRight; // alternate strafing side
                break;

            case BomberState.Retreat:
                if (_player != null && GlobalPosition.DistanceTo(_player.GlobalPosition) > 700f)
                {
                    currentState = BomberState.Approach;
                }
                break;
        }

        HandleDirection(delta);
        HandleMovement(delta);
    }

    protected override void HandleDirection(double delta)
    {
        if (_player == null) return;

        switch (currentState)
        {
            case BomberState.Approach:
                desiredDir = (_player.GlobalPosition - GlobalPosition).Normalized();
                break;

            case BomberState.Attack:
                desiredDir = Vector2.Zero; // hover briefly
                break;

            case BomberState.Retreat:
                Vector2 away = (GlobalPosition - _player.GlobalPosition).Normalized();
                Vector2 perp = new Vector2(-away.Y, away.X);
                desiredDir = (away + perp * (strafeRight ? 0.6f : -0.6f)).Normalized();
                break;
        }

        if (desiredDir != Vector2.Zero)
        {
            Velocity = Velocity.MoveToward(desiredDir * maxSpeed, acceleration * (float)delta);
        }
        else
        {
            // slow down smoothly when no direction
            Velocity = Velocity.MoveToward(Vector2.Zero, acceleration * (float)delta);
        }

        if (Velocity.Length() > 10f) // prevent jitter when stopped
            GlobalRotation = Velocity.Angle() + Mathf.Pi / 2;
    }

    protected override void HandleMovement(double delta)
    {
        MoveAndSlide();
    }

    public void Shoot()
    {
		for (int i = -1; i <= 1; i++)
		{
			EnemyBullet instance = (EnemyBullet)Bullet.Instantiate();
			instance.GlobalPosition = GlobalPosition;
			instance.SetDirection(GlobalPosition.DirectionTo(_player.GlobalPosition).Rotated(i * Mathf.Pi / 16));
			GetTree().CurrentScene.AddChild(instance);
		}
    }
}