using Godot;
using System;

public partial class Death : Area2D
{
	
	public override void _Ready()
	{
	}
	
	public void OnDeathBodyEntered(Node2D body) {
		GetNode<Timer>("Timer").Start();
	}
	
	private void OnTimerTimeout()
	{
		GetTree().ReloadCurrentScene();
	}
}
