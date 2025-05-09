using Godot;
using System;

public partial class Globals : Node2D // basically constants to make sure I don't need to type 50 lines every time I want to make a new enemy
{
    public static PackedScene Bullet { get; } = GD.Load<PackedScene>("res://TestProjectile.tscn");

    public static PackedScene Enemy { get; } = GD.Load<PackedScene>("res://EnemyTest.tscn");
    public static PackedScene EnemyShooter { get; } = GD.Load<PackedScene>("res://EnemyShooter.tscn");
    public static PackedScene Laser { get; } = GD.Load<PackedScene>("res://Laser.tscn");

    public static PackedScene Explosion { get; } = GD.Load<PackedScene>("res://Explosion.tscn");
    public static PackedScene Missile { get; } = GD.Load<PackedScene>("res://Missile.tscn");
    public static PackedScene EnemyBullet { get; } = GD.Load<PackedScene>("res://EnemyBullet.tscn");
    public Node2D Player;

    public override void _Ready()
    {
        var players = GetTree().GetNodesInGroup("players");
        if (players.Count > 0) {
            Player = players[0] as Node2D;
        }
    }

}