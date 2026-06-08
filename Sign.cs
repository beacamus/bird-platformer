using Godot;
using System;

public partial class Sign : Area2D
{
	[Export] public string Text { get; set; } = "This is a sign!";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D body) {
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "on";
		var signText = GetNode<Label>("Label");
		signText.Text = Text;
		signText.Visible = true;
	}
	
	public void OnBodyExited(Node2D body) {
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "off";
		var signText = GetNode<Label>("Label");
		signText.Visible = false;
	}
	
}
