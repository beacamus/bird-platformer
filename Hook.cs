using Godot;
using System;

public partial class Hook : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
		// We also specified this function name in PascalCase in the editor's connection window.
	public void OnGrapple()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print("One");
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		GD.Print("Two");
		animatedSprite2D.Play();
		animationPlayer.Play("grapple");

	}
	
	
}
