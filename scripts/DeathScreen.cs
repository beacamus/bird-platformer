using Godot;
using System;

public partial class DeathScreen : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("restart")) {
			var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			animationPlayer.Play("dead");
			GetNode<Timer>("Timer").Start();
		}
	}
	
	public void OnDeath() {
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("dead");
		GetNode<Timer>("Timer").Start();
	}
	
	public void OnTimerTimeout() {
		GetTree().CallDeferred("reload_current_scene");
	}
	
}
