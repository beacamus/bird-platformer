using Godot;
using System;

public partial class Platform : AnimatableBody2D
{
	public Vector2 Velocity { get; private set; }
	private Vector2 _previousPosition;

	public override void _Ready()
	{
		_previousPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = (GlobalPosition - _previousPosition) / (float)delta;
		_previousPosition = GlobalPosition;
	}
}
