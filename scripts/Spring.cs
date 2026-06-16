using Godot;
using System;

public partial class Spring : StaticBody2D
{
	
	[Signal]
	public delegate void SprungEventHandler(string direction);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D body) {
		if (body.Name == "Player") {
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite2D.Play("sprung");
			EmitSignal(SignalName.Sprung, "hi");
		}
	}
}
