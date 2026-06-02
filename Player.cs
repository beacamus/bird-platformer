using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 68.0f;
	public float JumpVelocity = -137.0f;
	public float MaxJumpTime = 0.30f;
	public float JumpIncrement = -875.0f;
	public float Acceleration = 0.5f;
	public float CurrentDelta = 0.0f;
	
	public float GrappleSpeed = 200.0f;
	public bool Grappling = false;
	public Vector2 GrapplePos = new Vector2(0,0);
	
	public bool CanCoyoteJump = false;
	public bool WeWereMoving = false;
	public float JumpTimer = 0.0f;
	public float Gravity = 0.0f;
	
	private Platform _platform;

	[Signal]
	public delegate void GrappleEventHandler();
	
	[Signal]
	public delegate void FlipEventHandler(bool left);
	
	[Signal]
	public delegate void FocusEventHandler(bool up, bool left);
	
	[Signal]
	public delegate void IdleEventHandler();
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	}
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Grappling) {
			ProjectSettings.SetSetting("physics/2d/default_gravity",0);
			GlobalPosition = GlobalPosition.MoveToward(GrapplePos, GrappleSpeed * (float)delta);
			if (Position == GrapplePos) {
				ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
				Grappling = false;
			}
			GD.Print("Current Position: ",Position);
			return;
		}
		
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
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
		if (!IsOnFloor())
		{
			Vector2 gravity = new Vector2(0,(float)ProjectSettings.GetSetting("physics/2d/default_gravity"));
			velocity += gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			//GD.Print("Jump Just Pressed!");
			if (IsOnFloor() || CanCoyoteJump) {
				JumpTimer = (float)delta; //start the timer
				velocity.Y = JumpVelocity;
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

		Velocity = velocity;
		
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
		
		public void OnGrapple(Vector2 position) {
			Grappling = true;
			GrapplePos = position;
		}
		
		
		
	}
