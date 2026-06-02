using Godot;
using System;

public partial class GrapplingHook : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	[Signal]
	public delegate void GoToEventHandler(Vector2 position);
	
	public void OnGrapple()
	{
		var raycast = GetNode<RayCast2D>("RayCast2D"); 
		raycast.TargetPosition = GetLocalMousePosition();
		raycast.ForceRaycastUpdate();
		GD.Print("Mouse Position"+GetLocalMousePosition());
		if (raycast.IsColliding())
		{
			Vector2 hookWorldPos = raycast.GetCollisionPoint();
			Vector2 globalPos = new Vector2(hookWorldPos.X, hookWorldPos.Y);
			GD.Print("Target Position"+globalPos);
			EmitSignal(SignalName.GoTo, globalPos);
		}	
	}
}
