using Godot;
using System;

public partial class BlueLaser : Area2D
{
	public CollisionShape2D collider;
	public Sprite2D sprite;
	public int counter = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (counter < 59.0f) {
			counter++;
		} else {
			collider.Disabled = !collider.Disabled;
			sprite.Visible = !sprite.Visible;
			counter = 0;
		}
		return;
	}
}
