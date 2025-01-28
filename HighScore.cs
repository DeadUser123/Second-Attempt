using Godot;
using System;

public partial class HighScore : RichTextLabel
{
	private int score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GetTree().HasMeta("score")) {
			score = (int)GetTree().GetMeta("score");
			this.Text = $"Your Score Was {score}";
		} else {
			this.Text = "Your Score Was Not Saved";
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
