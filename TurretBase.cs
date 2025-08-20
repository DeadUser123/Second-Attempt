using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class TurretBase : EnemyBase
{
	private Sprite2D top;
	private const float RotationSpeed = 2.0f;
	private double shooting_time;
	private uint burstCount = 5;
	private float spread = 0.75f;
	private float firingInterval = 0.05f;
	public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");

	public override void _Ready()
	{
		base._Ready();
		top = GetNode<Sprite2D>("../top");
		MaxHealth = 10;
		currentHealth = MaxHealth;
		killParent = true;
		scoreValue = 1000;
		relocateAfterDeath = false;
	}

	public override void _Process(double delta)
	{
		HandleHitCooldown(delta);
		shooting_time -= delta;

		Vector2 direction = _player.GlobalPosition - top.GlobalPosition;
		float targetAngle = direction.Angle() + Mathf.Pi / 2;
		float angleDiff = Mathf.AngleDifference(top.GlobalRotation, targetAngle);
		float maxStep = RotationSpeed * (float)delta;

		top.GlobalRotation += Mathf.Clamp(angleDiff, -maxStep, maxStep);

		if (shooting_time <= 0)
		{
			ShootBurst();
			shooting_time = ShootingInterval;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	private async void ShootBurst()
	{
		for (int i = 0; i < burstCount; i++)
		{
			EnemyBullet instance = (EnemyBullet)Bullet.Instantiate();
			instance.GlobalPosition = GlobalPosition;

			float randomOffset = (float)((rng.NextDouble() - 0.5) * spread);
			float angle = top.GlobalRotation + randomOffset - Mathf.Pi / 2;
			instance.SetDirection(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

			GetTree().CurrentScene.AddChild(instance);
			await ToSignal(GetTree().CreateTimer(firingInterval), "timeout");
		}
	}
}
