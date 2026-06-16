using Godot;
using System;

public partial class Rung : AnimatableBody2D
{
	public Vector2 Velocity { get; private set; }
	private Vector2 _previousPosition;
	
	[Signal]
	public delegate void OnRungEventHandler(Rung instance, NinePatchRect ninePatchRect);

	public override void _Ready()
	{
		_previousPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = (GlobalPosition - _previousPosition) / (float)delta;
		_previousPosition = GlobalPosition;
	}
	
		//if (body.Name == "Player") {
			//var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			//animationPlayer.Play("move");
		//}
		
	public void TriggerBodyEntered(Node2D body, NinePatchRect ninePatchRect)
	{
		EmitSignal(SignalName.OnRung, this, ninePatchRect);
	}

}
