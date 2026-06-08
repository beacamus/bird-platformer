using Godot;
using System;

public partial class Death : Area2D
{
	public new float Gravity = 0.0f;
	
	public override void _Ready()
	{
		Gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	}
	
	public void OnDeathBodyEntered(Node2D body) {
		GetNode<Timer>("Timer").Start();
	}
	
	private void OnTimerTimeout()
	{
		ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
		GetTree().ReloadCurrentScene();
	}
}
