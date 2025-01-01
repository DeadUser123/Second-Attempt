using Godot;
using System;

public partial class Score : RichTextLabel
{
	private int score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		this.Text = $"Score: {score}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeScore(int amount) {
		score += amount;
		this.Text = $"Score: {score}";
	}
}
