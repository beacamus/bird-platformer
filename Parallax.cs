using Godot;
using System;

public partial class Parallax : Node2D
{
	private Player _player;
	private Camera2d _camera;
	private Sprite2D _layerA;
	private Sprite2D _layerB;
	
	[Export] public float _layerASpeedHorizontal = 1.5f;
	[Export] public float _layerASpeedVertical = 1.5f;
	[Export] public float _layerBSpeedHorizontal = 0.3f;
	[Export] public float _layerBSpeedVertical = 0.3f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		_camera = GetTree().GetFirstNodeInGroup("camera") as Camera2d;
		_layerA = GetNode<Sprite2D>("layerA");
		_layerB = GetNode<Sprite2D>("layerB");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (_layerA.GlobalPosition.X < (_camera.GlobalPosition.X - 960.0f)) {
			//_layerA.GlobalPosition = new Vector2(_player.GlobalPosition.X,_layerA.GlobalPosition.Y);
			//GD.Print("OFF LEFT!");
		//} else if (_layerA.GlobalPosition.X > (_camera.GlobalPosition.X + 960.0f)) {
			//_layerA.GlobalPosition = new Vector2(_player.GlobalPosition.X,_layerA.GlobalPosition.Y);
			//GD.Print("OFF RIGHT!");
		//}
		//if (_layerB.GlobalPosition.X < (_camera.GlobalPosition.X - 960.0f)) {
			//_layerB.GlobalPosition = new Vector2(_player.GlobalPosition.X,_layerB.GlobalPosition.Y);
			//GD.Print("OFF LEFT!");
		//} else if (_layerB.GlobalPosition.X > (_camera.GlobalPosition.X + 960.0f)) {
			//_layerB.GlobalPosition = new Vector2(_player.GlobalPosition.X,_layerB.GlobalPosition.Y);
			//GD.Print("OFF RIGHT!");
		//}
		if (_player.Velocity != Vector2.Zero) {
			var x = _player.Velocity.X;
			var y = _player.Velocity.Y;
			_layerA.Position -= new Vector2((x * _layerASpeedHorizontal * (float)delta),(y * _layerASpeedVertical * (float)delta));
			_layerB.Position -= new Vector2((x * _layerBSpeedHorizontal * (float)delta),(y * _layerBSpeedVertical * (float)delta));
		}
	}
	
	// How do I say, when you run off the end of the camera loop back around?
	//LimitLeft
	
	
}
