using Godot;
using System;

public partial class DeathMessage : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GetTree().HasMeta("death_message")) {
			this.Text = (string)GetTree().GetMeta("death_message");
			GetTree().RemoveMeta("death_message");
		} else {
			this.Text = "Your Death Was Not Remembered";
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
