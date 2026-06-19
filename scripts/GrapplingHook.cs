using Godot;
using System;

public partial class GrapplingHook : Node2D
{
	
	[Signal]
	public delegate void SetHookSizeEventHandler(NinePatchRect ninePatchRect, CollisionShape2D collider);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var ninePatchRect = GetNode<NinePatchRect>("Area2D/NinePatchRect"); 
		var collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D"); 
		EmitSignal(SignalName.SetHookSize,ninePatchRect,collisionShape);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
	}
	
	[Signal]
	public delegate void GoToEventHandler(Vector2 position, NinePatchRect ninePatchRect, Vector2I tile);
	
	[Signal]
	public delegate void FailedGrappleEventHandler(NinePatchRect ninePatchRect, CollisionShape2D collider);
	
	[Signal]
	public delegate void GrappleTooShortEventHandler();
	
	public void OnBodyEntered(Node2D body)
	{
		var player = GetParent() as Player;
		if (player != null)
		{
			player.Bananas(body);
		}
	}
	
	public void OnGrapple(Vector2 globalMouse, Player player)
	{
		var raycast = GetNode<RayCast2D>("RayCast2D");
		
		raycast.GlobalRotation = 0f;
		raycast.AddException((CollisionObject2D)GetParent()); 
		
		Vector2 direction = (globalMouse - GlobalPosition).Normalized(); // Get the direction of what 1 unit in the mouse direction would be
   		Vector2 targetWorld = GlobalPosition + direction * 200f; //Multiply that by 50
		raycast.TargetPosition = raycast.ToLocal(targetWorld); //Do global to local space
		
		raycast.ForceRaycastUpdate();
		
		var ninePatchRect = GetNode<NinePatchRect>("Area2D/NinePatchRect"); 
		var collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D"); 
		
		if (raycast.IsColliding()) 
		{
			if (raycast.GetCollider().GetType().Name == "Platform") {
				Platform bananaq =(Platform)raycast.GetCollider(); // or however you reference it
				bananaq.TriggerBodyEntered(player, ninePatchRect);
			} else if (raycast.GetCollider().GetType().Name == "Rung") {
				Rung bananaq =(Rung)raycast.GetCollider(); // or however you reference it
				bananaq.OnTriggerEnteredRungEntered(this);
			} else if ((raycast.GetCollider().GetType().Name == "Area2D")) {
				EmitSignal(SignalName.FailedGrapple,ninePatchRect, collisionShape);
				return;
			}
			Vector2 hookWorldPos = raycast.GetCollisionPoint();
			var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
			Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(hookWorldPos));
			if (GlobalPosition.DistanceTo(raycast.GetCollisionPoint()) < 30.0f) {
				
				EmitSignal(SignalName.GrappleTooShort);
			} else {
				EmitSignal(SignalName.GoTo, hookWorldPos, ninePatchRect, mapCoords);
			}
		} else {
			EmitSignal(SignalName.FailedGrapple,ninePatchRect, collisionShape);
		}
	}
}
