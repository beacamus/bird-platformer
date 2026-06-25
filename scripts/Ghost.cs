using Godot;
using System;

public partial class Ghost : CharacterBody2D
{
	[Export] public float Speed = 50.0f;

	public override void _PhysicsProcess(double delta)
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Player;
		
		var direction = Position.DirectionTo(player.Position);
		Velocity = direction * Speed;
		
		MoveAndSlide();
	}
	
	
}
