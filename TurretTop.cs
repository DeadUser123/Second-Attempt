using Godot;
using System;
using System.Collections.Generic;

public partial class TurretTop : Sprite2D
{

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void Rotate(double angle)
    {
        GlobalRotation += (float)angle;
    }
    
    public new float GetRotation()
    {
        return GlobalRotation;
    }
}
