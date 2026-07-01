using Godot;
using System;

public partial class Widow : CharacterBody2D
{
	[Export] public float Speed = 50.0f;
	
	private Bone2D torso;
	
	private Bone2D upperleg1;
	private Bone2D midleg1;
	private Bone2D lowerleg1;
	
	private Bone2D upperleg2;
	private Bone2D midleg2;
	private Bone2D lowerleg2;
	
	private Bone2D upperleg3;
	private Bone2D midleg3;
	private Bone2D lowerleg3;
	
	private Bone2D upperleg4;
	private Bone2D midleg4;
	private Bone2D lowerleg4;
	
	private bool leg1planted = false;
	private bool leg2planted = false;
	private bool leg3planted = false;
	private bool leg4planted = false;
	
	private bool leg1pickup = false;
	private bool leg2pickup = false;
	private bool leg3pickup = false;
	private bool leg4pickup = false;
	
	private bool stillpickingup1 = false;
	private bool stillpickingup2 = false;
	private bool stillpickingup3 = false;
	private bool stillpickingup4 = false;
	
	private bool hitleg1 = false;
	private bool hitleg2 = false;
	private bool hitleg3 = false;
	private bool hitleg4 = false;
	
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		torso = GetNode<Bone2D>("Skeleton2D/torso"); 
		upperleg1 = GetNode<Bone2D>("Skeleton2D/torso/upperleg1"); 
		midleg1 = GetNode<Bone2D>("Skeleton2D/torso/upperleg1/midleg1"); 
		lowerleg1 = GetNode<Bone2D>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1"); 
		upperleg2 = GetNode<Bone2D>("Skeleton2D/torso/upperleg2"); 
		midleg2 = GetNode<Bone2D>("Skeleton2D/torso/upperleg2/midleg2"); 
		lowerleg2 = GetNode<Bone2D>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2"); 
		upperleg3 = GetNode<Bone2D>("Skeleton2D/torso/upperleg3"); 
		midleg3 = GetNode<Bone2D>("Skeleton2D/torso/upperleg3/midleg3"); 
		lowerleg3 = GetNode<Bone2D>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3"); 
		upperleg4 = GetNode<Bone2D>("Skeleton2D/torso/upperleg4"); 
		midleg4 = GetNode<Bone2D>("Skeleton2D/torso/upperleg4/midleg4"); 
		lowerleg4 = GetNode<Bone2D>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4"); 
	}

	public override void _PhysicsProcess(double delta)
	{

		//var player = GetTree().GetFirstNodeInGroup("player") as Player;
		//var direction = Position.DirectionTo(player.Position);
		//Velocity = direction * Speed;
		
		var previousPosition = Position;
		
		//var yMovement = 0.0f;
		

		
		var raycast1 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/RayCast2D");
		var raycast2 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/RayCast2D");
		var raycast3 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/RayCast2D");
		var raycast4 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/RayCast2D");
		
		var uppersprite1 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg1/Sprite2D");
		var midsprite1 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg1/midleg1/Sprite2D");
		var lowersprite1 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/Sprite2D");
		
		var uppersprite2 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg2/Sprite2D");
		var midsprite2 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg2/midleg2/Sprite2D");
		var lowersprite2 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/Sprite2D");
		
		var uppersprite3 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg3/Sprite2D");
		var midsprite3 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg3/midleg3/Sprite2D");
		var lowersprite3 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/Sprite2D");
		
		var uppersprite4 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg4/Sprite2D");
		var midsprite4 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg4/midleg4/Sprite2D");
		var lowersprite4 = GetNode<Sprite2D>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/Sprite2D");
		
		
		if (stillpickingup1) {
			if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow"))  {
				upperleg1.Position = new Vector2((upperleg1.Position.X-1.0f),(upperleg1.Position.Y));
			} else {
				stillpickingup1 = false;
			}
		} else if (leg1pickup) { /// if we are picking up our leg
			// slowly -x from the upper leg position
			// get the direction of the player
			// move in the y direction of the player (a bit)
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			upperleg1.Position = new Vector2((upperleg1.Position.X+direction.Y),(upperleg1.Position.Y-direction.X));
			uppersprite1.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			midsprite1.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			lowersprite1.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
		} else if (!leg1planted) {
			uppersprite1.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			midsprite1.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			lowersprite1.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			if (raycast1.IsColliding()) {
				if (raycast1.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg1 = true;
					GetNode<Timer>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/GaitTimer").Start();
					leg1planted = true;
				}
			} else {
				upperleg1.Position = new Vector2((upperleg1.Position.X+1.0f),(upperleg1.Position.Y));
				// if we are placing down our leg
				// slowly +x from the upper leg position
			}
		} else if (leg1planted) {
			if (hitleg1) {
				//IF WE ARE PLANTED ON THE GROUND
				// THIS IS WHERE WE MOVE BASED ON OUR PLANTED LIMB?
				// Stick the leg in the SAME position as when it was first planted
				uppersprite1.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				midsprite1.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				lowersprite1.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
			}	
		}
		
		if (stillpickingup2) {
			if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow"))  {
				upperleg2.Position = new Vector2((upperleg2.Position.X-1.0f),(upperleg2.Position.Y));
			} else {
				stillpickingup2 = false;
			}
		} else if (leg2pickup) { /// if we are picking up our leg
			// slowly -x from the upper leg position
			// get the direction of the player
			// move in the y direction of the player (a bit)
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			upperleg2.Position = new Vector2((upperleg2.Position.X+direction.Y),(upperleg2.Position.Y-direction.X));
			uppersprite2.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			midsprite2.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			lowersprite2.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
		} else if (!leg2planted) {
			uppersprite2.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			midsprite2.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			lowersprite2.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			if (raycast2.IsColliding()) {
				if (raycast2.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg2 = true;
					GetNode<Timer>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/GaitTimer").Start();
					leg2planted = true;
				}
			} else {
				upperleg2.Position = new Vector2((upperleg2.Position.X+1.0f),(upperleg2.Position.Y));
				// if we are placing down our leg
				// slowly +x from the upper leg position
			}
		} else if (leg2planted) {
			if (hitleg2) {
				//IF WE ARE PLANTED ON THE GROUND
				// THIS IS WHERE WE MOVE BASED ON OUR PLANTED LIMB?
				// Stick the leg in the SAME position as when it was first planted
				uppersprite2.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				midsprite2.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				lowersprite2.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
			}

			
		}
		
		
		if (stillpickingup3) {
			if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow"))  {
				upperleg3.Position = new Vector2((upperleg3.Position.X-1.0f),(upperleg3.Position.Y));
			} else {
				stillpickingup3 = false;
			}
		} else if (leg3pickup) { /// if we are picking up our leg
			// slowly -x from the upper leg position
			// get the direction of the player
			// move in the y direction of the player (a bit)
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			upperleg3.Position = new Vector2((upperleg3.Position.X+direction.Y),(upperleg3.Position.Y-direction.X));
			uppersprite3.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			midsprite3.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			lowersprite3.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
		} else if (!leg3planted) {
			uppersprite3.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			midsprite3.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			lowersprite3.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			if (raycast3.IsColliding()) {
				if (raycast3.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg3 = true;
					GetNode<Timer>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/GaitTimer").Start();
					leg3planted = true;
				}
			} else {
				upperleg3.Position = new Vector2((upperleg3.Position.X+1.0f),(upperleg3.Position.Y));
				// if we are placing down our leg
				// slowly +x from the upper leg position
			}
		} else if (leg3planted) {
			if (hitleg3) {
				//IF WE ARE PLANTED ON THE GROUND
				// THIS IS WHERE WE MOVE BASED ON OUR PLANTED LIMB?
				// Stick the leg in the SAME position as when it was first planted
				uppersprite3.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				midsprite3.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				lowersprite3.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
			}

			
		}
		
		
		if (stillpickingup4) {
			if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow"))  {
				upperleg4.Position = new Vector2((upperleg4.Position.X-1.0f),(upperleg4.Position.Y));
			} else {
				stillpickingup4 = false;
			}
		} else if (leg4pickup) { /// if we are picking up our leg
			// slowly -x from the upper leg position
			// get the direction of the player
			// move in the y direction of the player (a bit)
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			upperleg4.Position = new Vector2((upperleg4.Position.X+direction.Y),(upperleg4.Position.Y-direction.X));
			uppersprite4.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			midsprite4.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
			lowersprite4.Modulate = new Color(0.0f, 0.0f, 0.922f, 1.0f);
		} else if (!leg4planted) {
			uppersprite4.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			midsprite4.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			lowersprite4.Modulate = new Color(0.2f, 0.89f, 0.318f, 1.0f);
			if (raycast4.IsColliding()) {
				if (raycast4.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg4 = true;
					GetNode<Timer>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/GaitTimer").Start();
					leg4planted = true;
				}
			} else {
				upperleg4.Position = new Vector2((upperleg4.Position.X+1.0f),(upperleg4.Position.Y));
				// if we are placing down our leg
				// slowly +x from the upper leg position
			}
		} else if (leg4planted) {
			if (hitleg4) {
				//IF WE ARE PLANTED ON THE GROUND
				// THIS IS WHERE WE MOVE BASED ON OUR PLANTED LIMB?
				// Stick the leg in the SAME position as when it was first planted
				uppersprite4.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				midsprite4.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				lowersprite4.Modulate = new Color(0.786f, 0.095f, 0.385f, 1.0f);
				
				//var upperlegpos = upperleg4.Position;
				//upperleg4.ApplyRest();
				//var upperlegrest = upperleg4.Position;
				//var direction = upperlegrest.DirectionTo(upperlegpos);
				//yMovement += direction.Y;
				/////GD.Print("DIRECTION: "+direction);
				/////GD.Print("previous position: "+Position);
				//Position = new Vector2((Position.X-direction.X), (Position.Y));
				/////GD.Print("new position: "+Position);
				//
				//upperleg4.Position = upperlegpos;
				//// Move widow based on the normalised direction of limb movement
				//// Stick the leg in the SAME position as when it was first planted
			}

			
		}
		
		//Velocity = displacement/time
		//var currentPosition = Position;
		//Position = previousPosition;
		//var displacement = previousPosition.DistanceTo(currentPosition);
		//Velocity = new Vector2((displacement.X / (float)delta),(displacement.Y / (float)delta));
		
				// Add the gravity.
		//if (!IsOnFloor())
		//{
			//Vector2 gravity = new Vector2(0,(float)ProjectSettings.GetSetting("physics/2d/default_gravity"));
			//Velocity += gravity * (float)delta;
		//}
		
		//var prevleg1 = upperleg1.Position;
		//var prevleg2 = upperleg2.Position;
		//var prevleg3 = upperleg3.Position;
		//var prevleg4 = upperleg4.Position;
		//if (yMovement < 0) {
			//Position = new Vector2((Position.X),(Position.Y + yMovement));
			//upperleg1.Position = prevleg1;
			//upperleg2.Position = prevleg2;
			//upperleg3.Position = prevleg3;
			//upperleg4.Position = prevleg4;
		//}
		
		var upper1 = upperleg1.GlobalPosition;
		var upper2 = upperleg2.GlobalPosition;
		var upper3 = upperleg3.GlobalPosition;
		var upper4 = upperleg4.GlobalPosition;
		
		GlobalPosition = (upperleg1.GlobalPosition + upperleg2.GlobalPosition + upperleg3.GlobalPosition + upperleg4.GlobalPosition) / 4.0f;
		
		GlobalPosition = new Vector2(GlobalPosition.X,(GlobalPosition.Y));
		
		upperleg1.GlobalPosition = upper1;
		upperleg2.GlobalPosition = upper2;
		upperleg3.GlobalPosition = upper3;
		upperleg4.GlobalPosition = upper4;
		
		MoveAndSlide();
	}
	
	private void OnLegOneTimerTimeout()
	{
		leg1planted = false;
		GetNode<Timer>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/PickUpTimer").Start();
		leg1pickup = true;
		MoveAndSlide();
	}
	
	private void OnLegOnePickUpTimeout()
	{
		var raycast1 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/RayCast2D");
		if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow")) 
		{	
			stillpickingup1 = true;
		}
		leg1pickup = false;
		
	}
	
	private void OnLegTwoPickUpTimeout()
	{
		var raycast2 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/RayCast2D");
		if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow")) 
		{	
			stillpickingup2 = true;
		}
		leg2pickup = false;
	}
	
	private void OnLegThreePickUpTimeout()
	{
		var raycast3 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/RayCast2D");
		if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow")) 
		{	
			stillpickingup3 = true;
		}
		leg3pickup = false;
	}
	
	private void OnLegFourPickUpTimeout()
	{
		var raycast4 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/RayCast2D");
		if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow")) 
		{	
			stillpickingup4 = true;
		}
		leg4pickup = false;
	}
	
	
	private void OnLegTwoTimerTimeout()
	{
		leg2planted = false;
		GetNode<Timer>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/PickUpTimer").Start();
		leg2pickup = true;
		MoveAndSlide();
	}
	
	private void OnLegThreeTimerTimeout()
	{
		leg3planted = false;
		GetNode<Timer>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/PickUpTimer").Start();
		leg3pickup = true;
		MoveAndSlide();
	}
	
	private void OnLegFourTimerTimeout()
	{
		leg4planted = false;
		GetNode<Timer>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/PickUpTimer").Start();
		leg4pickup = true;
		MoveAndSlide();
	}
	
	

	
	
}
