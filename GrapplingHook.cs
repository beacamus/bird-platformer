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
		LookAt(GetGlobalMousePosition());
	}
	
	[Signal]
	public delegate void GoToEventHandler(Vector2 position, NinePatchRect ninePatchRect);
	
	[Signal]
	public delegate void FailedGrappleEventHandler(NinePatchRect ninePatchRect);
	
	public void OnGrapple(Vector2 globalMouse)
	{
		var raycast = GetNode<RayCast2D>("RayCast2D");
		
		raycast.GlobalRotation = 0f;
		raycast.AddException((CollisionObject2D)GetParent()); 
		raycast.TargetPosition = raycast.ToLocal(globalMouse);
		float distance = GlobalPosition.DistanceTo(globalMouse);
		raycast.ForceRaycastUpdate();
		var ninePatchRect = GetNode<NinePatchRect>("NinePatchRect"); 
		if (raycast.IsColliding() && (distance <= 200)) // If we're grappling something 200 pixels away or less
		{
			Vector2 hookWorldPos = raycast.GetCollisionPoint();
			
			EmitSignal(SignalName.GoTo, hookWorldPos, ninePatchRect);
		} else {
			EmitSignal(SignalName.FailedGrapple,ninePatchRect);
		}
	}
}
