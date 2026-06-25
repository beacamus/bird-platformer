using Godot;
using System;

public partial class Death : Area2D
{
	
	public override void _Ready()
	{
	}
	
	public void OnDeathBodyEntered(Node2D body) {
		if (body.Name == "Player") { 
			var player = (Player) body;
			player.Dead();
		}
	}
	
}
