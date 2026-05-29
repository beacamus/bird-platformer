using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 125.0f;
	public const float JumpVelocity = -350.0f;
	public bool CanCoyoteJump = false;
	public float Acceleration = 0.4f;
	public bool WeWereMoving = false;
	
	[Signal]
	public delegate void GrappleEventHandler();
	
	[Signal]
	public delegate void FlipEventHandler(bool left);
	
	[Signal]
	public delegate void FocusEventHandler(bool up, bool left);
	
	[Signal]
	public delegate void IdleEventHandler();
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		// Handle Jump.
		if (Input.IsActionJustPressed("grapple"))
		{
			EmitSignal(SignalName.Grapple);
		}
		
		if (Input.IsActionPressed("focus_up"))
		{
			animatedSprite2D.Animation = "up";
			Velocity = new Vector2(0,Velocity.Y);
			EmitSignal(SignalName.Focus, true, animatedSprite2D.FlipH);
		} else if (Input.IsActionPressed("focus_down")) {
			animatedSprite2D.Animation = "down";
			Velocity = new Vector2(0,Velocity.Y);
			EmitSignal(SignalName.Focus, false, animatedSprite2D.FlipH);
		} else {
			animatedSprite2D.Animation = "idle";
			EmitSignal(SignalName.Idle);
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor() || CanCoyoteJump) {
				velocity.Y = JumpVelocity;
			}
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = new Vector2(
			Input.GetAxis("move_left", "move_right"),
			Input.IsActionPressed("jump") ? -1 : 0
		);

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed * Acceleration;
			animatedSprite2D.Play();
		}
		else // if we aren't moving
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			Acceleration = 0.4f;
			animatedSprite2D.Stop();
			
		}

		Velocity = velocity;
		
		var wasOnFloor = IsOnFloor();
		
		if (!(Input.IsActionPressed("focus_up")) && !(Input.IsActionPressed("focus_down"))) // if we aren't focusing
		{
			MoveAndSlide();
		} else if (velocity.Y != 0) { // if we are focusing
			MoveAndSlide();
		}
		
		
		var gravity = GetGravity();
		
		if (wasOnFloor && !IsOnFloor() && (velocity.Y >= 0)) {
			CanCoyoteJump = true;
			GetNode<Timer>("CoyoteTimer").Start();
		}

		if ((!WeWereMoving) && (direction != Vector2.Zero)) { //if we were previously not moving and now we are
			GetNode<Timer>("Acceleration").Start();
		}
		
		if (direction != Vector2.Zero)
		{
			WeWereMoving = true;
		}
		else // if we aren't moving
		{
			WeWereMoving = false;
		}
	
		if (velocity.X != 0)
		{
			var previousFlip = animatedSprite2D.FlipH;
			var currentFlip = velocity.X < 0;
			animatedSprite2D.FlipH = currentFlip;
			if (previousFlip != currentFlip) {
				EmitSignal(SignalName.Flip, currentFlip);
			}
		}
	
		
	}
		
		private void OnCoyoteTimerTimeout()
		{
			CanCoyoteJump = false;
		}
		
		private void OnAccelerationTimeout()
		{
			Acceleration = 1.0f;
		}
		
		public void OnHookBodyEntered(Node2D body) {
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			var raycast = GetNode<RayCast2D>("RayCast2D"); 
			
			if (body.Name == "Player") {
				return;
			}
			
			if (body is TileMapLayer tileMap)
			{        
				if (Input.IsActionPressed("focus_up")) {
					raycast.TargetPosition = new Vector2(0, -1000);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(GlobalPosition.X, hookWorldPos.Y);
					}	
					GetNode<Timer>("CoyoteTimer").Start();
					CanCoyoteJump = true;
				} else if (Input.IsActionPressed("focus_down")) {
					raycast.TargetPosition = new Vector2(0, 1000);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(GlobalPosition.X, hookWorldPos.Y);
					}	
				} else if (animatedSprite2D.FlipH) {
					raycast.TargetPosition = new Vector2(-1000, 0);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(hookWorldPos.X, GlobalPosition.Y);
					}	
					GetNode<Timer>("CoyoteTimer").Start();
					CanCoyoteJump = true;
				} else if (!(animatedSprite2D.FlipH)) {
					raycast.TargetPosition = new Vector2(1000, 0);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(hookWorldPos.X, GlobalPosition.Y);
					}	
					GetNode<Timer>("CoyoteTimer").Start();
					CanCoyoteJump = true;
				}
			}
		}
		
	}
