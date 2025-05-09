using Godot;
using System;

public partial class CameraMovement : Node2D { // make everything move and keep player still for freedom of movement
    public override void _Ready() {
    }

    public override void _Process(double delta) {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        GlobalPosition -= direction * (float)delta;
    }
}