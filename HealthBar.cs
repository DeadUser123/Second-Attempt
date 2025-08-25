using Godot;
using System;

public partial class HealthBar : ProgressBar
{
    public int MaxHealth = 100;
    private int currentHealth;
    public float seconds = 0.0f;
    private float lifetime = 3.0f;
    public EnemyBase ParentEnemy;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
        MaxValue = MaxHealth;
        Value = currentHealth;
    }

    public override void _Process(double delta)
    {
        if (ParentEnemy != null)
        {
            currentHealth = ParentEnemy.currentHealth;
            MaxHealth = ParentEnemy.MaxHealth;
        }
        Value = currentHealth;
        MaxValue = MaxHealth;

        if (seconds >= lifetime)
        {
            Hide();
        }
        else
        {
            Show();
            seconds += (float)delta;
        }

        if (ParentEnemy != null && ParentEnemy.IsQueuedForDeletion())
        {
            QueueFree();
        }
        else if (ParentEnemy != null)
        {
            var shape = ParentEnemy.GetNode<CollisionShape2D>("CollisionShape2D");
            float height = 50; // fallback

            if (shape.Shape is RectangleShape2D rectShape)
                height = rectShape.Size.Y * shape.Scale.Y;
            
            var barSize = Size; // the current size of the ProgressBar
            float xOffset = -(barSize.X / 2);  // shift left by half width
            float yOffset = -(height / 2 + 15); // place above enemy

            GlobalPosition = ParentEnemy.GlobalPosition + new Vector2(xOffset, yOffset);

            if (shape.Shape is RectangleShape2D rectShapeW)
                CustomMinimumSize = new Vector2(rectShapeW.Size.X * shape.Scale.X, 8);
        }
    }

    public void SetHealth(int hp)
    {
        currentHealth = hp;
        seconds = 0f; // reset timer when updated
        Show();
    }
}