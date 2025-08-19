using Godot;
using System;
using System.Collections.Generic;

public partial class TurretBase : Node2D
{
    [Export] public float speed = 400f;        // Speed of the enemy
    [Export] public float ShootingInterval = 2f; // Time between shots
    private Vector2 _direction;               // Movement direction
    private Timer _shootTimer;                // Timer for shooting
	private double direction_decision_time;
	private Node2D _player;                   // Reference to the player
	private Node2D camera;
	private Score scoreText; // Reference to the Score node
	private List<Vector2> directions = new List<Vector2>();
	private Random rng;
	public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");
	private bool isRecentlyHit = false;
	private float hitCooldown = 0.1f; // Prevent being hit more than once every 0.1s
	private uint originalcollisionlayer;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }
}
