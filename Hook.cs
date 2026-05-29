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
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animatedSprite2D.Play();
		animationPlayer.Play("grapple");

	}
	
	public void OnFlip(bool left)
	{
		Scale = new Vector2(-Scale.X, Scale.Y);
		if (left) {
			//subtract 20 from the position.x value
			Position += new Vector2(-15, 0);
		} else {
			//add 20 to the position.x value
			Position += new Vector2(15, 0);
		}
	}
	
	public void OnFocus(bool up, bool left)
	{
		if (up) {
			if (left) {
				RotationDegrees = 90;
			} else {
				RotationDegrees = -90;
			}
			
		} else {
			if (left) {
				RotationDegrees = -90;
			} else {
				RotationDegrees = 90;
			}
		}
	}
	
	public void OnIdle()
	{
		RotationDegrees = 0;
	}
	
	
}
