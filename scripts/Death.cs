using Godot;
using System;

public partial class Death : Area2D
{
	
	[Signal]
	public delegate void OnDeathEventHandler();
	
	public override void _Ready()
	{
	}
	
	public void OnDeathBodyEntered(Node2D body) {
		if (body.Name == "Player") {
			EmitSignal(SignalName.OnDeath);
		}
	}
	
}
