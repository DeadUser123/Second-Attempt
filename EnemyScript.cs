using Godot;
using System;
using System.Collections.Generic;
public partial class EnemyScript : EnemyBase
{
    public override void _Ready()
    {
        base._Ready();
        Relocate();
    }
}
