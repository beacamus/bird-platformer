using Godot;
using System;

public partial class BlueLaser : Area2D
{
	public CollisionShape2D collider;
	public Sprite2D sprite;
	public CollisionShape2D staticBodyCollider;
	public int counter = 0;
	
	[Export] public bool PermanentlyOn { get; set; } = false;
	[Export] public bool StartsOff { get; set; } = false;
	[Export] public int onTime { get; set; } = 59;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		staticBodyCollider = GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
		if (StartsOff) {
			collider.Disabled = true;
			sprite.Visible = false;
			staticBodyCollider.Disabled = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!PermanentlyOn) {
			if (counter < onTime) {
				counter++;
			} else {
				collider.Disabled = !collider.Disabled;
				sprite.Visible = !sprite.Visible;
				staticBodyCollider.Disabled = !staticBodyCollider.Disabled;
				counter = 0;
			}
		}
		return;
	}
	
	public  void OnPressurePlated()
	{
		collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, !collider.Disabled);
		sprite.Visible = !sprite.Visible;
		staticBodyCollider.SetDeferred(CollisionShape2D.PropertyName.Disabled, !staticBodyCollider.Disabled);
	}
}
