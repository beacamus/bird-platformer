using Godot;
using System;

public partial class Portal : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D body) {
		GetNode<Timer>("Timer").Start();
	}
	
	private void OnTimerTimeout()
	{
		if (GetTree().GetCurrentScene().GetName() == "Main") {
			GetTree().ChangeSceneToFile("res://scene_2.tscn");
		} else if (GetTree().GetCurrentScene().GetName() == "scene2") {
			GetTree().ChangeSceneToFile("res://main.tscn");
		}  else if (GetTree().GetCurrentScene().GetName() == "level_1") {
			GetTree().ChangeSceneToFile("res://main.tscn");
		}
	}
	
}
