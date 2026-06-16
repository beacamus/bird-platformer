using Godot;
using System;

public partial class Hud : CanvasLayer
{

	[Signal]
	public delegate void SpeedChangedEventHandler(float value);
	
	[Signal]
	public delegate void MinJumpChangedEventHandler(float value);
	
	[Signal]
	public delegate void MaxJumpTimeChangedEventHandler(float value);
	
	[Signal]
	public delegate void GravityChangedEventHandler(float value);
	
	[Signal]
	public delegate void JumpIncrementChangedEventHandler(float value);
	
	[Signal]
	public delegate void AccelerationChangedEventHandler(float value);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnHSliderSpeedValueChanged(float value) {
		var message = GetNode<Label>("SpeedLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.SpeedChanged, value);
	}
	
	public void OnHSliderMinJumpValueChanged(float value) {
		var message = GetNode<Label>("MinJumpLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.MinJumpChanged, value);
	}
	
	public void OnHSliderMaxJumpTimeValueChanged(float value) {
		var message = GetNode<Label>("MaxJumpTimeLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.MaxJumpTimeChanged, value);
	}
	
	public void OnHSliderGravityValueChanged(float value) {
		var message = GetNode<Label>("GravityLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.GravityChanged, value);
	}
	
	public void OnHSliderJumpIncrementValueChanged(float value) {
		var message = GetNode<Label>("JumpIncrementLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.JumpIncrementChanged, value);
	}
	
	public void OnHSliderAccelerationValueChanged(float value) {
		var message = GetNode<Label>("AccelerationLabel");
		message.Text = string.Format("{0:N2}", value);
		EmitSignal(SignalName.AccelerationChanged, value);
	}

}
