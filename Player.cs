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
	
	public float GrappleSpeed = 120.0f;
	public bool Grappling = false;
	public bool Grappled = false;
	public Vector2 GrapplePos = new Vector2(0,0);
	
	public bool CanCoyoteJump = false;
	public bool WeWereMoving = false;
	public bool IsFailedGrapple = false;
	public float FailedGrappleTimer = 0.0f;
	public float JumpTimer = 0.0f;
	public float Gravity = 0.0f;
	
	private Platform _platform;

	private NinePatchRect _hook;
	private Vector2 _originalHookSize;

	[Signal]
	public delegate void GrappleEventHandler(Vector2 mousePos);
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	}
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsFailedGrapple) {
			if (FailedGrappleTimer > 18.0f) {
				IsFailedGrapple = false;
				FailedGrappleTimer = 0.0f;
				_hook.Size = _originalHookSize;
			} else {
				_hook.Size = new Vector2((_hook.Size.X + 10), _hook.Size.Y);
				FailedGrappleTimer++;
			}
			return;
		}
		if (Grappling) {
			CanCoyoteJump = true;
			GetNode<Timer>("CoyoteTimer").Start();
			if ((Input.IsActionJustPressed("jump"))||(Input.IsActionJustPressed("move_left"))||(Input.IsActionJustPressed("move_right"))) {
				ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
				Grappling = false;
				_hook.Size = _originalHookSize;
				return;
			}
			ProjectSettings.SetSetting("physics/2d/default_gravity",0);
			Vector2 direction = (GrapplePos - GlobalPosition).Normalized();
			Velocity = direction * GrappleSpeed;
			float x_distance = GlobalPosition.DistanceTo(GrapplePos); 
			Vector2 beforeSize = _hook.Size;
			_hook.Size = new Vector2(_originalHookSize.X+x_distance, _hook.Size.Y);
			Vector2 afterSize = _hook.Size;
			MoveAndSlide(); // if we move with move and slide and use the direction as the grapple pos then we can detect collisions!
			if ((Position == GrapplePos) || (GetSlideCollisionCount() != 0) ) {
				Grappled = true;
				Grappling = false;
				_hook.Size = _originalHookSize;
			}
			return;
		}
		
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (Input.IsActionJustPressed("grapple"))
		{
			if (Grappled) {
				Grappled = false;
				Grappling = false;
				_hook.Size = _originalHookSize;
			}
			Vector2 mousePos = GetGlobalMousePosition();
			EmitSignal(SignalName.Grapple, GetGlobalMousePosition());
		}
		

		
		if (Input.IsActionPressed("focus_up"))
		{
			animatedSprite2D.Animation = "up";
			Velocity = new Vector2(0,Velocity.Y);
		} else if (Input.IsActionPressed("focus_down")) {
			animatedSprite2D.Animation = "down";
			Velocity = new Vector2(0,Velocity.Y);
		} else {
			if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right")) {
				animatedSprite2D.Animation = "walk";
			} else {
				animatedSprite2D.Animation = "idle";
			}
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (Grappled) {
			if (Input.IsActionJustPressed("jump") || Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_right")) {
				ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
				Grappled = false;
				CanCoyoteJump = true;
				GetNode<Timer>("CoyoteTimer").Start();
				_hook.Size = _originalHookSize;
			} else {
				Velocity = new Vector2(0,0);
				return;
			}
		}
		
		// Add the gravity.
		if (!IsOnFloor())
		{
			Vector2 gravity = new Vector2(0,(float)ProjectSettings.GetSetting("physics/2d/default_gravity"));
			velocity += gravity * (float)delta;
		}
		



		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor() || CanCoyoteJump) {
				JumpTimer = (float)delta; //start the timer
				velocity.Y = JumpVelocity;
			}
		}
		
		if (Input.IsActionPressed("jump"))
		{
			if (JumpTimer > 0) {
				velocity.Y += JumpIncrement * (float)delta;
				JumpTimer += (float)delta;
				if (JumpTimer > MaxJumpTime) {
					JumpTimer = 0;
				}
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
		
		public void OnGrapple(Vector2 position,  NinePatchRect ninePatchRect) {
			Grappling = true;
			GrapplePos = position;
			_hook = ninePatchRect;
			_originalHookSize = ninePatchRect.Size;
		}
		
		public void FailedGrapple(NinePatchRect ninePatchRect) {
			_hook = ninePatchRect;
			_originalHookSize = ninePatchRect.Size;
			ninePatchRect.Size = new Vector2(10, _hook.Size.Y);
			IsFailedGrapple = true;
				
		}
		
		
	}
