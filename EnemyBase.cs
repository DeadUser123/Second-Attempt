using Godot;
using System;
using System.Collections.Generic;

public abstract partial class EnemyBase : CharacterBody2D
{
    [Export] public float speed = 400f;
    [Export] public float ShootingInterval = 2f;
    [Export] public int MaxHealth = 1;   // Default health
    protected int currentHealth;

    protected Vector2 _direction;
    protected double direction_decision_time;
    protected Node2D _player;
    protected Node2D camera;
    protected Score scoreText;
    protected Random rng;
    protected bool isRecentlyHit = false;
    protected float hitCooldown = 0.1f;
    protected uint originalcollisionlayer;
    protected bool stayOnScreen = true;
    protected bool killParent = false;
    protected int scoreValue = 100;
    protected bool relocateAfterDeath = true;

    public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");

    protected List<Vector2> directions = new List<Vector2> {
        Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right
    };

    public override void _Ready()
    {
        rng = new Random();
        scoreText = GetNode<Score>("/root/Gameplay/Score");
        _player = GetNode<Node2D>("/root/Gameplay/CharacterBody2D/CharacterBody2D");
        camera = GetNode<Node2D>("/root/Gameplay/Camera2D");

        _direction = directions[rng.Next(0, directions.Count)];
        direction_decision_time = 0;

        currentHealth = MaxHealth;
    }

    public virtual void GotHit(int amount = 1)
    {
        // GD.Print(currentHealth);
        if (isRecentlyHit) return;

        isRecentlyHit = true;
        hitCooldown = 0.1f;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        originalcollisionlayer = CollisionLayer;
        CollisionLayer = 0;

        scoreText.ChangeScore(scoreValue);

        Node2D instance = (Node2D)Explosion.Instantiate();
        instance.GlobalPosition = GlobalPosition;
        GetTree().CurrentScene.AddChild(instance);

        if (relocateAfterDeath)
        {
            Relocate();
            currentHealth = MaxHealth;
        }
        else
        {
            if (killParent)
                GetParent().QueueFree();
            else
                QueueFree();
        }

        CollisionLayer = originalcollisionlayer;
    }

    protected virtual void Relocate()
    {
        GlobalPosition = GetSpawnPosition(1500, 2000);
        GlobalPosition += camera.GlobalPosition;
    }

    protected Vector2 GetSpawnPosition(float minDistance, float maxDistance)
    {
        double angle = rng.NextDouble() * Math.PI * 2;
        float distance = (float)(minDistance + rng.NextDouble() * (maxDistance - minDistance));
        return new Vector2(
            (float)(Math.Cos(angle) * distance),
            (float)(Math.Sin(angle) * distance)
        );
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleHitCooldown(delta);
        HandleDirection(delta);
        HandleMovement(delta);
    }

    protected void HandleHitCooldown(double delta)
    {
        if (isRecentlyHit)
        {
            hitCooldown -= (float)delta;
            if (hitCooldown <= 0)
            {
                isRecentlyHit = false;
                hitCooldown = 0.1f;
            }
        }
    }

    protected void HandleDirection(double delta)
    {
        direction_decision_time += delta;
        if (direction_decision_time > 0.2 && rng.Next(0, 5) == 0)
        {
            _direction = directions[rng.Next(0, directions.Count)];
            direction_decision_time = 0;

        }
        if (stayOnScreen)
        {
            Vector2 screenSize = GetViewportRect().Size;
            if (GlobalPosition.X < 0)
            {
                _direction.X = Mathf.Abs(_direction.X);
            }
            else if (GlobalPosition.X > screenSize.X)
            {
                _direction.X = -Mathf.Abs(_direction.X);
            }
            if (GlobalPosition.Y < 0)
            {
                _direction.Y = Mathf.Abs(_direction.Y);
            }
            else if (GlobalPosition.Y > screenSize.Y)
            {
                _direction.Y = -Mathf.Abs(_direction.Y);
            }
        }
    }

    protected void HandleMovement(double delta)
    {
        GlobalPosition += _direction * speed * (float)delta;
    }
}
