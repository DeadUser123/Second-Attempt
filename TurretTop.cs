using Godot;
using System;
using System.Collections.Generic;

public partial class TurretTop : Node2D
{

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }
    
    public void Rotate(double angle)
    {
        Rotation += (float)angle;
    }
}
