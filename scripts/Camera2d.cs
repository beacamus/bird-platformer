using Godot;
using System;

public partial class Camera2d : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
		int leftLimit = tilemap.GetUsedCells()[0].X;
		int rightLimit = tilemap.GetUsedCells()[0].X;
		int topLimit = tilemap.GetUsedCells()[0].Y;
		int bottomLimit = tilemap.GetUsedCells()[0].Y;
		foreach (Vector2I tile in tilemap.GetUsedCells()) {
			//GD.Print(tilemap.GetCellTileData(tile).GetCustomData("Edge"));
			if ((bool)tilemap.GetCellTileData(tile).GetCustomData("Edge") == true) { //if the tile is an edge tile
				Vector2 realPos = tilemap.MapToLocal(tile); // Map to local puts from tilemap coords into local pixel space
				if (realPos.X < leftLimit) {
					leftLimit = (int) realPos.X;
				} else if (realPos.X > rightLimit) {
					rightLimit = (int) realPos.X;
				}
				if (realPos.Y < topLimit) {
					topLimit = (int) realPos.Y;
				} else if (realPos.Y > bottomLimit) {
					bottomLimit = (int) realPos.Y;
				}
			}
		}
		LimitEnabled = true;
		LimitLeft = leftLimit;
		LimitRight = rightLimit;
		LimitTop = topLimit;
		LimitBottom = bottomLimit + 64;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Player;
		Position = player.Position;
	}
	
	
}
