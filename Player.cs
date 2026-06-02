using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 68.0f;
	public float JumpVelocity = -137.0f;
	public float MaxJumpTime = 0.30f;
	public float JumpIncrement = -875.0f;
	public float Acceleration = 0.5f;
	
	public bool CanCoyoteJump = false;
	public bool WeWereMoving = false;
	public bool Grappled = false;
	public bool GrappledToPlatform = false;
	public float JumpTimer = 0.0f;
	
	private Platform _platform;

	
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
		
		if (GrappledToPlatform) {
			Velocity = _platform?.Velocity ?? Vector2.Zero;
			//GD.Print(Velocity);
			GrappledToPlatform = true;
		}

		
		// Handle Jump.
		if (Input.IsActionJustPressed("grapple"))
		{
			EmitSignal(SignalName.Grapple);
		}
		
		if (Input.IsActionPressed("focus_up"))
		{
			animatedSprite2D.Animation = "up";
			if (!GrappledToPlatform) {
				Velocity = new Vector2(0,Velocity.Y);
			}
			EmitSignal(SignalName.Focus, true, animatedSprite2D.FlipH);
		} else if (Input.IsActionPressed("focus_down")) {
			animatedSprite2D.Animation = "down";
			if (!GrappledToPlatform) {
				Velocity = new Vector2(0,Velocity.Y);
			}
			EmitSignal(SignalName.Focus, false, animatedSprite2D.FlipH);
		} else {
			if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right")) {
				animatedSprite2D.Animation = "walk";
			} else {
				animatedSprite2D.Animation = "idle";
				EmitSignal(SignalName.Idle);
			}
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		// Add the gravity.
		if ((!IsOnFloor()) && !(Grappled))
		{
			Vector2 gravity = new Vector2(0,(float)ProjectSettings.GetSetting("physics/2d/default_gravity"));
			velocity += gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			//GD.Print("Jump Just Pressed!");
			if (IsOnFloor() || CanCoyoteJump || Grappled) {
				JumpTimer = (float)delta; //start the timer
				velocity.Y = JumpVelocity;
				Grappled = false;
				GrappledToPlatform = false; //If we're trying to move then we're no longer grappling the platform
			}
		}
		
		if (Input.IsActionPressed("jump"))
		{
			if (JumpTimer > 0) {
				//GD.Print(velocity.Y);
				velocity.Y += JumpIncrement * (float)delta;
				JumpTimer += (float)delta;
				if (JumpTimer > MaxJumpTime) {
					JumpTimer = 0;
				}
				//GD.Print("JumpTimer: "+JumpTimer);
			}
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = new Vector2(
			Input.GetAxis("move_left", "move_right"),
			Input.IsActionPressed("jump") ? -1 : 0
		);

		if (direction != Vector2.Zero)
		{
			if (Grappled) {
				Grappled = false;
				GrappledToPlatform = false; //If we're trying to move then we're no longer grappling the platform
			}
			velocity.X = direction.X * Speed * Acceleration;
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.Play();
		}
		else // if we aren't moving
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			Acceleration = 0.4f;
			animatedSprite2D.Animation = "idle";
			animatedSprite2D.Stop();
			
		}

		if (!GrappledToPlatform) {
			Velocity = velocity;
		}
		
		var wasOnFloor = IsOnFloor();
		
		if (!(Input.IsActionPressed("focus_up")) && !(Input.IsActionPressed("focus_down"))) // if we aren't focusing
		{
			MoveAndSlide();
		} else if (velocity.Y != 0) { // if we are focusing
			MoveAndSlide();
		}
		
		
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
			
			if ((body is TileMapLayer tileMap)|| (body is Platform))
			{
				Grappled = true;        
				
				if (Input.IsActionPressed("focus_up")) {
					raycast.TargetPosition = new Vector2(0, -1000);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(GlobalPosition.X, hookWorldPos.Y);
					}	
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
				} else if (!(animatedSprite2D.FlipH)) {
					raycast.TargetPosition = new Vector2(1000, 0);
					raycast.ForceRaycastUpdate();
					
					if (raycast.IsColliding())
					{
						Vector2 hookWorldPos = raycast.GetCollisionPoint();
						GlobalPosition = new Vector2(hookWorldPos.X, GlobalPosition.Y);
					}	
				}
				Velocity = new Vector2(0,0);
				if (body is Platform platform) {
					Velocity = platform.Velocity;
					//GD.Print(Velocity);
					_platform = platform;
					GrappledToPlatform = true;
				}
			}
		}
		
		
		public void OnSpeedChanged(float value)
		{
			Speed = value;
		}
		
		public void OnJumpIncrementChanged(float value)
		{
			JumpIncrement = value;
		}
		
		public void OnAccelerationChanged(float value)
		{
			Acceleration = value;
		}
		
		public void OnGravityChanged(float value)
		{
			ProjectSettings.SetSetting("physics/2d/default_gravity",value);
		}
		
		public void OnJumpVelocityChanged(float value)
		{
			JumpVelocity = value;
		}
		
		public void OnMaxJumpTimeChanged(float value)
		{
			MaxJumpTime = value;
		}
		
		
		
	}
