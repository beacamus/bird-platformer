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
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Handle Jump.
		if (Input.IsActionJustPressed("grapple"))
		{
			EmitSignal(SignalName.Grapple);
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
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");

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
		
		
		MoveAndSlide();
		
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
			GD.Print("BOOP: ",body.Name);
			Vector2 left =  new Vector2(-4, 0); // (-1, 0)
			MoveAndCollide(left);
		}
		
	}
