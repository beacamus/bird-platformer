using Godot;
using System;

//Gravity needs to reset if we reset the scene.

public partial class Player : CharacterBody2D
{
	public float Speed = 68.0f;
	public float JumpVelocity = -137.0f;
	public float MaxJumpTime = 0.30f;
	public float JumpIncrement = -875.0f;
	public float Acceleration = 1.0f;
	public float CurrentDelta = 0.0f;
	
	public float LastFallingVelocity { get; private set; }
	
	//Hazards
	public float Wind = 0.0f;
	[Export] public float PublicWind { get; set; } = 10.0f;
	
	public bool InOrangeLaser = false;
	
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
		if (InOrangeLaser) {
			if (Velocity != Vector2.Zero) { //if the player is moving through an orange laser
				InOrangeLaser = false;
				GD.Print("Setting Gravity line 56");
				ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
				GetTree().CallDeferred("reload_current_scene");
			}
		}
		
		if (Input.IsActionJustPressed("restart")) {
			Grappled = false;
			Grappling = false;
			InOrangeLaser = false;
			CanCoyoteJump = false;
			WeWereMoving = false;
			IsFailedGrapple = false;
			ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
			GetTree().CallDeferred("reload_current_scene");
		}
		
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
		
	}

	public override void _PhysicsProcess(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (Input.IsActionPressed("focus_up"))
		{
			animatedSprite2D.Animation = "up";
			
		} else if (Input.IsActionPressed("focus_down")) {
			animatedSprite2D.Animation = "down";

		} else {
			if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right")) {
				animatedSprite2D.Animation = "walk";
			} else {
				animatedSprite2D.Animation = "idle";
			}
		}
		
		Vector2 velocity = Velocity;
		
		
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
				velocity.Y += JumpVelocity;
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
		
		if ((Input.GetAxis("move_left","move_right") != 0) && ((Input.IsActionPressed("focus_down"))||(Input.IsActionPressed("focus_up")))) {
			direction = new Vector2(0,Input.IsActionPressed("jump") ? -1 : 0);
		}

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed + Wind;
			if (!Input.IsActionPressed("focus_up") && !Input.IsActionPressed("focus_down")) {
				animatedSprite2D.Animation = "walk";
			}
			
			animatedSprite2D.Play();
		}
		else // if we aren't moving
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0 + Wind, Speed);
			Acceleration = 0.4f;
			if (!Input.IsActionPressed("focus_up") && !Input.IsActionPressed("focus_down")) {
				animatedSprite2D.Animation = "idle";
			}
			animatedSprite2D.Stop();
			
		}

		
		Velocity = velocity;
		
		var wasOnFloor = IsOnFloor();
		
		LastFallingVelocity = Velocity.Y;
		
		MoveAndSlide();
		
		
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
		
		//Hazards
		public void OnWindEntered(Node2D body)
		{
			Wind = PublicWind;
		}
		
		public void OnWindExited(Node2D body)
		{
			Wind = 0.0f;
		}
		
		public void OrangeLasered()
		{
			if (Velocity != Vector2.Zero) { //if the player is moving through an orange laser
				ProjectSettings.SetSetting("physics/2d/default_gravity",Gravity);
				GetTree().CallDeferred("reload_current_scene");
			} else {
				InOrangeLaser = true;
			}
		}
		
		public void OrangeLaseredDone()
		{
			InOrangeLaser = false;
		}
		
		public void OnPlatform(AnimatableBody2D platform)
		{
			_platform = (Platform)platform;
			Velocity = _platform.Velocity;
		}
		
		public void Sprung(string direction)
		{
			Velocity = new Vector2(Velocity.X,(Velocity.Y-350.0f));
		}
		
		
	}
