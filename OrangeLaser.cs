using Godot;
using System;

public partial class OrangeLaser : AnimatableBody2D
{
	//Orange laser doesn't work because it's on body entered - if you're already in the body that doesn't mean anything
	
	[Signal]
	public delegate void OrangeLaseredEventHandler();
	
	[Signal]
	public delegate void OrangeLaseredDoneEventHandler();
	
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
			EmitSignal(SignalName.OrangeLasered);
		}	
	}
	
	public void OnBodyExited(Node2D body)
	{
		if (body.Name == "Player") {
			EmitSignal(SignalName.OrangeLaseredDone);
		}	
	}
	
}
