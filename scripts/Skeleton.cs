using Godot;
using System;

public partial class Skeleton : CharacterBody2D
{
	
	[Export] public bool direction = true;
	[Export] public float Speed = 1.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsOnFloor())
		{
			Vector2 gravity = new Vector2(0,(float)ProjectSettings.GetSetting("physics/2d/default_gravity"));
			Velocity += gravity * (float)delta;
		}
		var sprite2D = GetNode<Sprite2D>("Sprite2D");
		sprite2D.FlipH = !direction;
		if (direction) {
			Velocity = new Vector2((Velocity.X+Speed),Velocity.Y);
		} else {
			Velocity = new Vector2((Velocity.X-Speed),Velocity.Y);
		}
		
		MoveAndSlide();
		
		int collisionCount = GetSlideCollisionCount();
		for (int i = 0; i < collisionCount; i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			Node collider = collision.GetCollider() as Node;
			if (collider is TileMapLayer) {
				Vector2 normal = collision.GetNormal();
				Vector2 hookWorldPos = collision.GetPosition() - normal * 1.0f; // push 1px into the surface
				var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
				var tile = tilemap.LocalToMap(hookWorldPos);
				var tileData = tilemap.GetCellTileData(tile);
				
				if (tileData == null) {
					continue; // no tile here, skip this collision
				}
				
				if ((bool)tileData.GetCustomData("Box")) {
					direction = !direction;
					return;
				}
			}

		}
	}
}
