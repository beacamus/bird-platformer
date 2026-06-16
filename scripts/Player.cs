using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 440.0f;
	public float JumpVelocity = -455.0f;
	public float MaxJumpTime = 0.30f;
	public float JumpIncrement = -875.0f;
	public float CurrentDelta = 0.0f;
	
	public float LastFallingVelocity { get; private set; }
	
	//Hazards
	public float Wind = 0.0f;
	[Export] public float PublicWind { get; set; } = 10.0f;
	
	public bool InOrangeLaser = false;
	
	public float GrappleSpeed = 360.0f;
	public bool Grappling = false;
	public bool Grappled = false;
	public Vector2 GrapplePos = new Vector2(0,0);
	public Vector2 previousVelocity = new Vector2(0,0);
	
	public int alteredVelocity = 6;
	public bool CanCoyoteJump = false;
	public bool IsFailedGrapple = false;
	public float FailedGrappleTimer = 0.0f;
	public float JumpTimer = 0.0f;
	public float Gravity = 0.0f;
	
	private Platform _platform;
	private RungBody _rungBody;

	private NinePatchRect _hook;
	private CollisionShape2D _collider;
	private Vector2 _originalHookSize;
	private Vector2 _originalColliderSize;
	private Vector2 _originalColliderPosition;
	private Vector2I _tileGrappled;
	private bool _jumpAfterGrapple = false;
	private bool _GrapplingPlatform = false;
	private bool _GrapplingRung = false;
	
	private bool _justGrappled;
	private Vector2 _lastKnownGrapple;

	[Signal]
	public delegate void GrappleEventHandler(Vector2 mousePos, Player player);
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
		// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_GrapplingPlatform) {
			if (Grappled && !(Grappling)) {
				Velocity = _platform.Velocity;
				MoveAndSlide();
			} else {
				//GD.Print("PING");
				GrapplePos = _platform.Position;
			}
		} else if (_GrapplingRung) {
			if (Grappled && !(Grappling)) {
				Velocity = _rungBody.Velocity;
				MoveAndSlide();
			} else {
				//GD.Print("PING2");
				GrapplePos = _rungBody.Position;
			}
		}
		if (InOrangeLaser) {
			if (Velocity != Vector2.Zero) { //if the player is moving through an orange laser
				InOrangeLaser = false;
				GetTree().CallDeferred("reload_current_scene");
			}
		}
		
		if (Input.IsActionJustPressed("restart")) {
			Grappled = false;
			Grappling = false;
			InOrangeLaser = false;
			CanCoyoteJump = false;
			IsFailedGrapple = false;
		}
		
		if (Input.IsActionPressed("focus_down"))
		{
			SetCollisionLayerValue(5, false);
			SetCollisionMaskValue(5, false);
			GD.Print("PING");
		} else {
			SetCollisionLayerValue(5, true);
			SetCollisionMaskValue(5, true);
		}
		
		if (Input.IsActionJustPressed("grapple"))
		{
			_justGrappled = false;
			if (Grappling || Grappled) {
				_justGrappled = true;
				_lastKnownGrapple = GrapplePos;
				_GrapplingPlatform = false;
				_GrapplingRung = false;
				GD.Print("HELLO HELLO!:",GrapplePos);
			}
			if (Grappled || IsFailedGrapple) {
				Grappled = false;
				Grappling = false;
				IsFailedGrapple = false;
				_hook.Size = _originalHookSize;
				GD.Print("HELLO HELLO:",GrapplePos);
			}

			Vector2 mousePos = GetGlobalMousePosition();
			EmitSignal(SignalName.Grapple, GetGlobalMousePosition(), this); 

		}
		
		if (IsFailedGrapple) {
			if (FailedGrappleTimer > 18.0f) {
				IsFailedGrapple = false;
				FailedGrappleTimer = 0.0f;
				_hook.Size = _originalHookSize;
				(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
				_collider.Position = _originalColliderPosition;
			} else {
				_hook.Size = new Vector2((_hook.Size.X + 4), _hook.Size.Y);
				(_collider.Shape as RectangleShape2D).Size = new Vector2(((_collider.Shape as RectangleShape2D).Size.X + 8), (_collider.Shape as RectangleShape2D).Size.Y);
				//Mess with the position of the collider
				_collider.Position = new Vector2((_collider.Position.X+3.3f),_collider.Position.Y);
				FailedGrappleTimer++;
			}
			return;
		}
		if (Grappling) {
			//GD.Print("GRAPPLE POS: "+GrapplePos+" "+_lastKnownGrapple.DistanceTo(GrapplePos));
			//GD.Print(Velocity);
			if (_justGrappled && (_lastKnownGrapple.DistanceTo(GrapplePos) < 30.0f)) {
				Grappling = false;
				_hook.Size = _originalHookSize;
				if (_collider != null)
				{
					(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
					_collider.Position = _originalColliderPosition;
				}
				_justGrappled = false;
				_lastKnownGrapple = GrapplePos;
				//GD.Print("TOO CLOSE");
				return;
			}
			CanCoyoteJump = true;
			GetNode<Timer>("CoyoteTimer").Start();
			if ((Input.IsActionJustPressed("jump"))||(Input.IsActionJustPressed("move_left"))||(Input.IsActionJustPressed("move_right"))) {
				if (Input.IsActionJustPressed("jump")) {
					_jumpAfterGrapple = true;
				}
				Grappling = false;
				_hook.Size = _originalHookSize;
				if (_collider != null)
				{
					(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
					_collider.Position = _originalColliderPosition;
				}
				return;
			}
			Vector2 direction = (GrapplePos - GlobalPosition).Normalized();
			Velocity = direction * GrappleSpeed;
			float x_distance = GlobalPosition.DistanceTo(GrapplePos); 
			Vector2 beforeSize = _hook.Size;
			_hook.Size = new Vector2(_originalHookSize.X+(x_distance*0.2f), _hook.Size.Y);
			Vector2 afterSize = _hook.Size;
			var prevPosition = Position;
			MoveAndSlide(); // if we move with move and slide and use the direction as the grapple pos then we can detect collisions!
			
			if ((Position == GrapplePos) || (GetSlideCollisionCount() != 0) || (((_GrapplingPlatform) || (_GrapplingRung)) && (Position.DistanceTo(GrapplePos) < 5.0f)) ) {
				if (GetSlideCollisionCount() == 0) {
					Grappled = true;
					Grappling = false;
					_hook.Size = _originalHookSize;
					if (_collider != null)
					{
						(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
						_collider.Position = _originalColliderPosition;
					}
				} else {
					KinematicCollision2D collision = GetSlideCollision(0);
					Node collider = collision.GetCollider() as Node;

					if (collider is TileMapLayer tilemap)
					{
						Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(collision.GetPosition()));
						if (mapCoords == _tileGrappled) {
							Grappled = true;
							Grappling = false;
							_hook.Size = _originalHookSize;
							if (_collider != null)
							{
								(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
								_collider.Position = _originalColliderPosition;
							}
						}
					} else if (collider is Platform platform) {
						if (platform == _platform) {
							Grappled = true;
							Grappling = false;
							_hook.Size = _originalHookSize;
							if (_collider != null)
							{
								(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
								_collider.Position = _originalColliderPosition;
							}
						}
					} else if (collider is RungBody rung) {
						if (rung == _rungBody) {
							Grappled = true;
							Grappling = false;
							_hook.Size = _originalHookSize;
							if (_collider != null)
							{
								(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
								_collider.Position = _originalColliderPosition;
							}
						}
					}
					if (prevPosition.DistanceTo(Position) < 0.5f) // if we haven't moved anywhere because our goal is impossible
					{
						Grappled = true;
						Grappling = false;
						_hook.Size = _originalHookSize;
						if (_collider != null)
						{
							(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
							_collider.Position = _originalColliderPosition;
						}
					}
				}
			}
			return;
		}
		

		
	}

	public override void _PhysicsProcess(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
		
		if (
			((Input.IsActionPressed("focus_up")) && ((!Input.IsActionPressed("move_left"))
				&&(!Input.IsActionPressed("move_right"))))
			||
				((Input.IsActionPressed("focus_up"))
				&&
				(Input.IsActionPressed("jump"))
				&&
				(!Input.IsActionPressed("move_left"))
				&&(!Input.IsActionPressed("move_right"))
				)
			)
		{
			animatedSprite2D.Animation = "up";
			
		} else if (
			((Input.IsActionPressed("focus_down")) && ((!Input.IsActionPressed("move_left"))
				&&(!Input.IsActionPressed("move_right"))))
			||
			((Input.IsActionPressed("focus_down"))
			&&
			(Input.IsActionPressed("jump"))
			&&
			(!Input.IsActionPressed("move_left"))
			&&(!Input.IsActionPressed("move_right"))))
		{
			animatedSprite2D.Animation = "down";
		}  else if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right") ||  Input.IsActionPressed("jump")) {
			animatedSprite2D.Animation = "walk";
		}  else {
			animatedSprite2D.Animation = "idle";
		}
		
		animatedSprite2D.Play();
		Vector2 velocity = Velocity;
		
		
		if (Grappled) {
			if (Input.IsActionJustPressed("jump") || Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_right")) {
				Grappled = false;
				_GrapplingPlatform = false;
				_GrapplingRung = false;
				CanCoyoteJump = true;
				GetNode<Timer>("CoyoteTimer").Start();
				_hook.Size = _originalHookSize;
				if (_collider != null)
				{
					(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
					_collider.Position = _originalColliderPosition;
				}
				if (Input.IsActionJustPressed("jump")) {
					_jumpAfterGrapple = true;
				}
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

		if (direction != Vector2.Zero)
		{
			alteredVelocity = 6;
			velocity.X = direction.X * Speed + Wind;
		}
		else // if we aren't moving
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0 + Wind, Speed);
			if (previousVelocity != Vector2.Zero && (previousVelocity.X != 0) && (alteredVelocity > 0)) {
				alteredVelocity--;
				velocity.X = previousVelocity.X;
			}
			
		}

		
		Velocity = velocity;
		
		var wasOnFloor = IsOnFloor();
		
		LastFallingVelocity = Velocity.Y;
		
		if (_jumpAfterGrapple) {
			JumpAfterGrapple();
			_jumpAfterGrapple = false;
		}
		
		MoveAndSlide();
		
		previousVelocity = Velocity;
		
		
		if (wasOnFloor && !IsOnFloor() && (velocity.Y >= 0)) {
			CanCoyoteJump = true;
			GetNode<Timer>("CoyoteTimer").Start();
		}

	
		if (velocity.X != 0)
		{
			CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			var previousFlip = animatedSprite2D.FlipH;
			var currentFlip = velocity.X < 0;
			if (previousFlip != currentFlip) {
				var position = collisionShape.Position.X;
				collisionShape.Position = new Vector2(-position,collisionShape.Position.Y);
			}
			animatedSprite2D.FlipH = currentFlip;
		}
	
		
	}
	
		private void JumpAfterGrapple() {
			
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			
			//GD.Print("PING!");
			// cast raycast to see if near any walls
			// the wall we are closest to, move away from it
			if (GetSlideCollisionCount() != 0) {
				KinematicCollision2D collision = GetSlideCollision(0);
				Vector2 collisionPosition = collision.GetPosition();
				float directionAway = collisionPosition.DistanceTo(Position);
				//GD.Print("PINGU!: "+directionAway);
				if (directionAway < 65.0f) {
					//GD.Print("PINGU 3!: "+Position);
					if (animatedSprite2D.FlipH) { //if we're facing left
						Position = new Vector2((Position.X+directionAway),Position.Y);
					} else {
						Position = new Vector2((Position.X-directionAway),Position.Y);
					}
					//GD.Print("PINGU 4!: "+Position);
				}
			}
		}
		
		private void OnCoyoteTimerTimeout()
		{
			CanCoyoteJump = false;
		}
		
		
		
		public void OnSpeedChanged(float value) //Used as part of the HUD
		{
			Speed = value;
		}
		
		public void OnJumpIncrementChanged(float value) //Used as part of the HUD
		{
			JumpIncrement = value;
		}
		
		public void OnGravityChanged(float value) //Used as part of the HUD
		{
			ProjectSettings.SetSetting("physics/2d/default_gravity",value);
		}
		
		public void OnJumpVelocityChanged(float value) //Used as part of the HUD
		{
			JumpVelocity = value;
		}
		
		public void OnMaxJumpTimeChanged(float value) //Used as part of the HUD
		{
			MaxJumpTime = value;
		}
		
		public void OnGrapple(Vector2 position,  NinePatchRect ninePatchRect, Vector2I mapCoord) {
			Grappling = true;
			if (position != Vector2.Zero) {
				GrapplePos = position;
			}
			_hook = ninePatchRect;
			_tileGrappled = mapCoord;
		}
		
		public void FailedGrapple(NinePatchRect ninePatchRect, CollisionShape2D collider) {
			_hook = ninePatchRect;
			_collider = collider;
			(collider.Shape as RectangleShape2D).Size = new Vector2(10, (collider.Shape as RectangleShape2D).Size.Y);
			ninePatchRect.Size = new Vector2(10, _hook.Size.Y);
			IsFailedGrapple = true;
				
		}
		
		public void GrappleTooShort() {
		}
		
		public void SetHookSize(NinePatchRect ninePatchRect, CollisionShape2D collider) {
			_hook = ninePatchRect;
			_originalHookSize = ninePatchRect.Size;
			_originalColliderPosition = collider.Position;
			var shape = collider.Shape as RectangleShape2D;
			if (shape != null)
			{
				_originalColliderSize = shape.Size;
			}
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
				GetTree().CallDeferred("reload_current_scene");
			} else {
				InOrangeLaser = true;
			}
		}
		
		public void OrangeLaseredDone()
		{
			InOrangeLaser = false;
		}
		
		public void OnPlatform(AnimatableBody2D platform, NinePatchRect ninePatchRect)
		{
			//GD.Print("HELLO 4");
			_hook = ninePatchRect;
			_originalHookSize = ninePatchRect.Size;
			_platform = (Platform)platform;
			_GrapplingPlatform = true;
			Grappling = true;
			GrapplePos = _platform.Position;
			
		}
		
		public void OnRung(RungBody rungBody)
		{
			_rungBody = rungBody;
			_GrapplingRung = true;
			Grappling = true;
			GrapplePos =  _rungBody.Position;
			
		}
		
		public void Sprung(string direction)
		{
			Velocity = new Vector2(Velocity.X,-500.0f);
		}
		
		public void Bananas(Node2D body) { // NEED to trigger the early grapple code, not trigger it from the player end!!
			if (body.Name != "Player") {
				//tilemap.ToLocal(hookWorldPos)
				if (IsFailedGrapple) {
					IsFailedGrapple = false;
					_hook.Size = _originalHookSize;
					(_collider.Shape as RectangleShape2D).Size = _originalColliderSize;
					_collider.Position = _originalColliderPosition;
					EmitSignal(SignalName.Grapple, GetGlobalMousePosition(), this);
				}
			}

		}
		
		
	}
