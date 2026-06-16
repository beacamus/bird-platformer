using Godot;
using System;

public partial class PressurePlate : Area2D
{
	
	[Signal]
	public delegate void PressurePlatedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player") {
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite2D.Animation = "pressed";
			EmitSignal(SignalName.PressurePlated);
		}	
	}
	
	public void OnBodyExited(Node2D body)
	{
		if (body.Name == "Player") {
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite2D.Animation = "unpressed";
		}	
	}
}
