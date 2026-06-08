using Godot;
using System;

public partial class Platform : AnimatableBody2D
{
	public Vector2 Velocity { get; private set; }
	private Vector2 _previousPosition;
	
	[Signal]
	public delegate void OnPlatformEventHandler(Platform instance);

	public override void _Ready()
	{
		_previousPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = (GlobalPosition - _previousPosition) / (float)delta;
		_previousPosition = GlobalPosition;
	}
	
	public void OnBodyEntered(Node2D body) {
		if (body.Name == "Player") {
			EmitSignal(SignalName.OnPlatform, this);
		}
	}
	
	public void OnMoveBodyEntered(Node2D body) {
		if (body.Name == "Player") {
			var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			animationPlayer.Play("move");
		}
	}
	
	
}
