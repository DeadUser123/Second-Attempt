using Godot;
using System;

public partial class EnemyScript : CharacterBody2D {
	[Export] public float speed = 100f;        // Speed of the enemy
    [Export] public float ShootingInterval = 2f; // Time between shots
    private Vector2 _direction;               // Movement direction
    private Timer _shootTimer;                // Timer for shooting
	private Node2D _player;                   // Reference to the player
	private Score scoreText; // Reference to the Score node
    public override void _Ready()
    {
		
		scoreText = GetNode<Score>("/root/Main/Score");

		_player = GetNode<Node2D>("/root/Main/CharacterBody2D/CharacterBody2D");

		// _direction = new Vector2(0, 1); // Move downward initially
        // _shootTimer.Start();
    }

	public void GotHit() {
		scoreText.ChangeScore(100);
	}
    public override void _PhysicsProcess(double delta)
    {
		// Position += _direction * speed * delta;
	}
}
