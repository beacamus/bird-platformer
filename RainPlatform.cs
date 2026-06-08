using Godot;
using System;

public partial class RainPlatform : StaticBody2D
{
	public Sprite2D sprite;
	public int counter = 0;
	public bool up = false;
	
	[Export] public float WaitTime { get; set; } = 1.0f;
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("Timer").WaitTime = WaitTime;
		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (counter < 9.0f) {
			counter++;
		} else {
			if (up) {
				sprite.Position = new Vector2(sprite.Position.X + 1, sprite.Position.Y);
			} else {
				sprite.Position = new Vector2(sprite.Position.X - 1, sprite.Position.Y);
			}
			counter = 0;
			up = !up;
		}
		return;
	}
	
	public void OnBodyEntered(Node2D body) {
		if (body.Name == "Player") {
			GetNode<Timer>("Timer").Start();
		}
	}
	
	private void OnTimerTimeout()
	{
		QueueFree();
	}
	
}
