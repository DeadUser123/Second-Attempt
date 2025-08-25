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
	private bool enteredOrbit = false;
	private int orbitDirection = -1;

	public override void _Ready()
	{
		base._Ready();
		top = GetNode<Sprite2D>("../top");
		MaxHealth = 20;
		currentHealth = MaxHealth;
		killParent = true;
		scoreValue = 1000;
		speed = 600f;
		relocateAfterDeath = false;
		Relocate();
	}

	public override void _Process(double delta)
	{
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

	protected override void HandleDirection(double delta)
	{
		_direction = _player.GlobalPosition - GlobalPosition;
		_direction = _direction.Normalized();
	}

	protected override void HandleMovement(double delta)
	{
		if ((_player.GlobalPosition - GlobalPosition).Length() > 400)
		{
			base.HandleMovement(delta);
			enteredOrbit = false;
		}
		else
		{
			if (!enteredOrbit)
			{
				enteredOrbit = true;
				orbitDirection = rng.Next(0, 2) == 0 ? 1 : -1;
			}
			Vector2 orbitDir = new Vector2(-orbitDirection * _direction.Y, orbitDirection * _direction.X);

			Velocity = orbitDir * speed;
			GlobalPosition += Velocity * (float)delta;
		}

		top.GlobalPosition = GlobalPosition; 
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
