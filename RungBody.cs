using Godot;
using System;

public partial class RungBody : AnimatableBody2D
{
	public Vector2 Velocity { get; private set; }
	private Vector2 _previousPosition;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_previousPosition = GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Velocity = (GlobalPosition - _previousPosition) / (float)delta;
		_previousPosition = GlobalPosition;
	}
	
	[Signal]
	public delegate void OnRungEventHandler(RungBody rungBody);
	
	public void OnRungBody()
	{
		EmitSignal(SignalName.OnRung, this);
		
	}
	
}
