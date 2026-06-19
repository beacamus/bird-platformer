using Godot;
using System;

public partial class Rung : Area2D
{
	
	[Signal]
	public delegate void OnRungBodyEventHandler();

	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
	}
	
		
	public void OnTriggerEnteredRungEntered(Node2D body)
	{
		if (body.Name == "GrapplingHook") {
			EmitSignal(SignalName.OnRungBody);
		}
	}

}
