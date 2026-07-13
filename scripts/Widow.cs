using Godot;
using System;
using System.Linq;

public partial class Widow : CharacterBody2D
{
	[Export] public float Speed = 1.0f;
	
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
	
	private bool leg1stepping = false;
	private bool leg2stepping = false;
	private bool leg3stepping = false;
	private bool leg4stepping = false;
	
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
	
	private float playerDirection = 1.0f;
	
	private float floor1 = -1.0f;
	private float floor2 = -1.0f;
	private float floor3 = -1.0f;
	private float floor4 = -1.0f;	
	
	private float floor = -1.0f;
	
	private bool goingup = false;
	private bool goingdown = false;
	
	
	
	
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
		
		var raycast1 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg1/midleg1/lowerleg1/RayCast2D");
		var raycast2 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg2/midleg2/lowerleg2/RayCast2D");
		var raycast3 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg3/midleg3/lowerleg3/RayCast2D");
		var raycast4 = GetNode<ShapeCast2D>("Skeleton2D/torso/upperleg4/midleg4/lowerleg4/RayCast2D");
		
		var headsprite = GetNode<Sprite2D>("Sprite2D");
		
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
		
		
		// Which direction is the player in?
		var currentPlayer = GetTree().GetFirstNodeInGroup("player") as Player;
		var widowPos = new Vector2(Position.X,(Position.Y+137.0f));
		if (widowPos.DirectionTo(currentPlayer.Position).X < 0) {
			playerDirection = -1;
		} else {
			playerDirection = 1;
		}
		
		if (widowPos.DistanceTo(currentPlayer.Position) < 100) { // If the player and widow are too close to each other don't do anything because usually the player would die
			return;
		}
		
		

//----------------------------------------------------------------------------------------------------------------------------------------
		
		if (stillpickingup1) {
			if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow"))  {
				lowerleg1.Position = new Vector2((lowerleg1.Position.X-Speed),(lowerleg1.Position.Y));
			} else {
				stillpickingup1 = false;
			}
		} else if (leg1pickup) { /// if we are picking up our leg
			var globalPos = lowerleg1.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition); 
			if (playerDirection == 1) {
				if (direction2.X > 0) { //  if widow is only slightly in front of us
					if (distance < 90.0f) {
						if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup1 = true;
						}
						leg1pickup = false;
					}
				} else {
					if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow")) 
					{	
						stillpickingup1 = true;
					}
					leg1pickup = false;
				}
			} else {
				if (direction2.X > 0) { // IF THE WIDOW IS behind me when I'm going left
					if (distance > 120.0f) {
						if (raycast1.IsColliding() && (raycast1.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup1 = true;
						}
						leg1pickup = false;
					}
				}
			}

			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			lowerleg1.Position = new Vector2((lowerleg1.Position.X),(lowerleg1.Position.Y-(direction.X*Speed)));

		} else if (!leg1planted) {
			if (raycast1.IsColliding()) {
				if (raycast1.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg1 = true;
					leg1planted = true;
					Vector2 hookWorldPos = raycast1.GetCollisionPoint(0);
					var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
					Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(hookWorldPos));
					floor1 = mapCoords.Y;
					leg1stepping = false;
				}
			} else {
				leg1stepping = true;
				lowerleg1.Position = new Vector2((lowerleg1.Position.X+Speed),(lowerleg1.Position.Y));
			}
		} else if (leg1planted) {
			if (hitleg1) {
				var globalPos = lowerleg1.GlobalPosition;
				var globalPosX = new Vector2(globalPos.X,0);
				var globalPositionX = new Vector2(GlobalPosition.X,0);
				var distance = globalPosX.DistanceTo(globalPositionX);
				var direction = globalPos.DirectionTo(GlobalPosition);
				if (playerDirection == 1) {
					if (direction.X > 0) {
						if (distance > 120.0f) {
							leg1planted = false;
							leg1pickup = true;
						}
					}
				} else {
					if (direction.X < 0) { // if the widow is in front of me when I'm going left
						if (distance > 0.0f) {
							leg1planted = false;
							leg1pickup = true;
						}
					} else { // when the widow is behind me but it's moving to overtake me
						if (distance < 90) { // if it's kind of close to me
							leg1planted = false;
							leg1pickup = true;
						}
					}
				}
			}	
		}
//----------------------------------------------------------------------------------------------------------------------------------------
		if (stillpickingup2) {
			if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow"))  {
				lowerleg2.Position = new Vector2((lowerleg2.Position.X-Speed),(lowerleg2.Position.Y));
			} else {
				stillpickingup2 = false;
			}
		} else if (leg2pickup) { /// if we are picking up our leg
			var globalPos = lowerleg2.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X > 0) { //  if widow is only slightly in front of us
					if (distance < 40.0f) {
						if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup2 = true;
						}
						leg2pickup = false;
					}
				} else {
					if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow")) 
					{	
						stillpickingup2 = true;
					}
					leg2pickup = false;
				}
			} else {
				if (direction2.X > 0) { // IF THE WIDOW IS "behind" when I'm going left
					if (distance > 60.0f) {
						if (raycast2.IsColliding() && (raycast2.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup2 = true;
						}
						leg2pickup = false;
					}
				}
			}
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			lowerleg2.Position = new Vector2((lowerleg2.Position.X),(lowerleg2.Position.Y-(direction.X*Speed)));

		} else if (!leg2planted) {
			if (raycast2.IsColliding()) {
				if (raycast2.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg2 = true;
					leg2planted = true;
					Vector2 hookWorldPos = raycast2.GetCollisionPoint(0);
					var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
					Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(hookWorldPos));
					floor2 = mapCoords.Y;
					leg2stepping = false;
				}
			} else {
				leg2stepping = true;
				lowerleg2.Position = new Vector2((lowerleg2.Position.X+Speed),(lowerleg2.Position.Y));
			}
		} else if (leg2planted) {
			if (hitleg2) {
				var globalPos = lowerleg2.GlobalPosition;
				var globalPosX = new Vector2(globalPos.X,0);
				var globalPositionX = new Vector2(GlobalPosition.X,0);
				var distance = globalPosX.DistanceTo(globalPositionX);
				var direction = globalPos.DirectionTo(GlobalPosition);
				if (playerDirection == 1) {
					if (direction.X > 0) {
						if (distance > 60.0f) {
							leg2planted = false;
							leg2pickup = true;
						}
					}
				} else {
					if (direction.X < 0) { // if the widow is in front of me when I'm going left
						if (distance > 0.0f) {
							leg2planted = false;
							leg2pickup = true;
						}
					} else { // when the widow is behind me but it's moving to overtake me
						if (distance < 40) { // if it's kind of close to me
							leg2planted = false;
							leg2pickup = true;
						}
					}
					
					
					
				}
			}
			
		}
//----------------------------------------------------------------------------------------------------------------------------------------
		if (stillpickingup3) {
			if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow"))  {
				lowerleg3.Position = new Vector2((lowerleg3.Position.X-Speed),(lowerleg3.Position.Y));
			} else {
				stillpickingup3 = false;
			}
		} else if (leg3pickup) { /// if we are picking up our leg
			var globalPos = lowerleg3.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X < 0) { // If the widow is behind me
					if (distance > 40.0f) {
						if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup3 = true;
						}
						leg3pickup = false;
					}
				}
			} else {
				if (direction2.X < 0) { // Given the widow is in front of me
					if (distance < 40.0f) {
						if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup3 = true;
						}
						leg3pickup = false;
					}
				} else { // if I'm going left and the widow is behind me
					if (raycast3.IsColliding() && (raycast3.GetCollider(0).GetType().Name != "Widow")) 
					{	
						stillpickingup3 = true;
					}
					leg3pickup = false;
				}
			}
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			lowerleg3.Position = new Vector2((lowerleg3.Position.X),(lowerleg3.Position.Y-(direction.X*Speed)));

		} else if (!leg3planted) {
			if (raycast3.IsColliding()) {
				if (raycast3.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg3 = true;
					leg3planted = true;
					Vector2 hookWorldPos = raycast3.GetCollisionPoint(0);
					var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
					Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(hookWorldPos));
					floor3 = mapCoords.Y;
					leg3stepping = false;
				}
			} else {
				leg3stepping = true;
				lowerleg3.Position = new Vector2((lowerleg3.Position.X+Speed),(lowerleg3.Position.Y)); //Move your leg down until you hit something
			}
		} else if (leg3planted) {
			if (hitleg3) {
				var globalPos = lowerleg3.GlobalPosition;
				var globalPosX = new Vector2(globalPos.X,0);
				var globalPositionX = new Vector2(GlobalPosition.X,0);
				var distance = globalPosX.DistanceTo(globalPositionX);
				var direction = globalPos.DirectionTo(GlobalPosition);
				if (playerDirection == 1) {
					if (direction.X > 0) { // If the widow is in front of me
						if (distance > 0.0f) {
							leg3planted = false;
							leg3pickup = true;
						}
					}
				} else {
					if (direction.X < 0) { // If the widow is in front of me when I'm going left
						if (distance > 60.0f) {
							leg3planted = false;
							leg3pickup = true;
						}
					}
				}
			}
			
		}
		
		
//----------------------------------------------------------------------------------------------------------------------------------------		
		if (stillpickingup4) {
			if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow"))  {
				lowerleg4.Position = new Vector2((lowerleg4.Position.X-Speed),(lowerleg4.Position.Y));
			} else {
				stillpickingup4 = false;
			}
		} else if (leg4pickup) { /// if we are picking up our leg
			var globalPos = lowerleg4.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X < 0) { // IF THE WIDOW IS BEHIND ME
					if (distance > 90.0f) {
						if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup4 = true;
						}
						leg4pickup = false;
					}
				}
			} else {
				if (direction2.X < 0) { // Given the widow is in front of me
					if (distance < 90.0f) {
						if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow")) 
						{	
							stillpickingup4 = true;
						}
						leg4pickup = false;
					}
				} else { // If I'm going left and WIDOW is behind me
					if (raycast4.IsColliding() && (raycast4.GetCollider(0).GetType().Name != "Widow")) 
					{	
						stillpickingup4 = true;
					}
					leg4pickup = false;
				}
			}

			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			lowerleg4.Position = new Vector2((lowerleg4.Position.X),(lowerleg4.Position.Y-(direction.X*Speed)));

		} else if (!leg4planted) {
			if (raycast4.IsColliding()) {
				if (raycast4.GetCollider(0).GetType().Name != "Widow") { //If we are not colliding with ourselves
					hitleg4 = true;
					leg4planted = true;
					Vector2 hookWorldPos = raycast4.GetCollisionPoint(0);
					var tilemap = GetTree().GetFirstNodeInGroup("tilemap") as TileMapLayer;
					Vector2I mapCoords = tilemap.LocalToMap(tilemap.ToLocal(hookWorldPos));
					floor4 = mapCoords.Y;
					leg4stepping = false;
				}
			} else {
				leg4stepping = true;
				lowerleg4.Position = new Vector2((lowerleg4.Position.X+Speed),(lowerleg4.Position.Y));
			}
		} else if (leg4planted) {
			if (hitleg4) {
				var globalPos = lowerleg4.GlobalPosition;
				var globalPosX = new Vector2(globalPos.X,0);
				var globalPositionX = new Vector2(GlobalPosition.X,0);
				var distance = globalPosX.DistanceTo(globalPositionX);
				var direction = globalPos.DirectionTo(GlobalPosition);
				if (playerDirection == 1) {
					if (direction.X > 0) { //IF the widow is in front of me
						if (distance > 0.0f) {
							leg4planted = false;
							leg4pickup = true;
						}
					}
				} else {
					if (direction.X < 0) { //IF the widow is in front of me and I'm going left
						if (distance > 120.0f) {
							leg4planted = false;
							leg4pickup = true;
						}
					}
				}
			}

			
		}
		
//----------------------------------------------------------------------------------------------------------------------------------------

		if (Truth(leg1planted, leg2planted, leg3planted, leg4planted) >= 2) { // if at least 2 legs are planted
			//Move widow body forward
			var leg1 = lowerleg1.GlobalPosition;
			var leg2 = lowerleg2.GlobalPosition;
			var leg3 = lowerleg3.GlobalPosition;
			var leg4 = lowerleg4.GlobalPosition;
			
			var leg1rotation = lowerleg1.GlobalRotationDegrees;
			var leg2rotation = lowerleg2.GlobalRotationDegrees;
			var leg3rotation = lowerleg3.GlobalRotationDegrees;
			var leg4rotation = lowerleg4.GlobalRotationDegrees;
			
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var widowPosition = new Vector2(Position.X,(Position.Y+137.0f));
			var direction = widowPosition.DirectionTo(player.Position);
			//Position = new Vector2((Position.X + (direction.X)),(Position.Y));
			//
			//if ((floor1 != -1.0f) && (floor2 != -1.0f) && (floor3 != -1.0f) & (floor4 != -1.0f)) {
				//if (floor1 == floor2 && floor2 == floor3 && floor3 == floor4) {
					//floor = floor1;
				//}
				//if (playerDirection == 1) {
					//if ((floor4 < floor) && (floor3 < floor)) {
						//// GOING UP -y
						//goingup = true;
						//goingdown = false;
					//} else if ((floor4 > floor) && (floor3 > floor)) {
						//// GOING DOWN +y
						//goingdown = true;
						//goingup = false;
					//} else if ((floor1 < floor) && (floor2 < floor)) { // if our 1 and 2 are on a higher level and therefore we are going down
						//goingup = false;
						//goingdown = true;
					//} else { // GOING STEADY
						//goingup = false;
						//goingdown = false;
					//}
					////If floor4 and floor3 are on different floor that is < floor 1 and 2 then move the widow up (so - y)
					//// if different floor but > than others then move the widow body down
				//} else {
					//if ((floor1 < floor) && (floor2 < floor)) {
						//// GOING UP -y
						//goingup = true;
						//goingdown = false;
					//} else if ((floor1 > floor) && (floor2 > floor)) {
						//// GOING DOWN +y
						//goingdown = true;
						//goingup = false;
					//} else if ((floor3 < floor) && (floor4 < floor)) { // if our 3 and 4 are on a higher level and therefore we are going down
						//goingup = false;
						//goingdown = true;
					//} else { // GOING STEADY
						//goingup = false;
						//goingdown = false;
					//}
					//
					////If floor1 and floor2 are on different floor that is < floor 3 and 4 then move the widow up (so - y)
					//// if different floor but > than others then move the widow body down
				//}
			//}
			
			//var banana = new Vector2(0,GlobalPosition.Y);
			//var apple = new Vector2(0,lowerleg1.GlobalPosition.Y);
			//if (playerDirection == 1) {
				//apple = new Vector2(0,lowerleg4.GlobalPosition.Y);
			//}
			//
			//if (goingup) {
				//if ((apple.DistanceTo(banana) < 60.0f)) {
					//Position = new Vector2(Position.X,(Position.Y - Speed));
				//} else if ((apple.DirectionTo(banana).Y > 0)) { // if the leg is above the widow head
					//Position = new Vector2(Position.X,(Position.Y - Speed));
				//}
			//} else if (goingdown) {
				//if (apple.DistanceTo(banana) > 25.0f) {
					//Position = new Vector2(Position.X,(Position.Y + Speed));
				//}
			//} else {
			//}
			
			lowerleg1.GlobalPosition = leg1;
			lowerleg2.GlobalPosition = leg2;
			lowerleg3.GlobalPosition = leg3;
			lowerleg4.GlobalPosition = leg4; 
			
			
			var upperconnection1 = WiggleLeg(lowerleg1, midleg1, upperleg1);
			lowerleg1.GlobalPosition = leg1;
			
			var upperconnection2 = WiggleLeg(lowerleg2, midleg2, upperleg2);
			lowerleg2.GlobalPosition = leg2;
			
			var upperconnection3 = WiggleLeg(lowerleg3, midleg3, upperleg3);
			lowerleg3.GlobalPosition = leg3;
			
			var upperconnection4 = WiggleLeg(lowerleg4, midleg4, upperleg4);
			lowerleg4.GlobalPosition = leg4;
			
			
			Position = new Vector2((upperconnection1.X + 87.982f),(upperconnection1.Y - 5.321f));
			
			WiggleLeg(lowerleg1, midleg1, upperleg1);
			WiggleLeg(lowerleg2, midleg2, upperleg2);
			WiggleLeg(lowerleg3, midleg3, upperleg3);
			WiggleLeg(lowerleg4, midleg4, upperleg4);
			
			lowerleg1.GlobalPosition = leg1;
			lowerleg2.GlobalPosition = leg2;
			lowerleg3.GlobalPosition = leg3;
			lowerleg4.GlobalPosition = leg4;


			

			
			//---------------------------------------------------------------------------------------------------------------------------------------
						// GET CENTRE POINT OF LOWER LIMB
			
			
			
			
			
			
		} else {
		}
		

		
		MoveAndSlide();
	}
	
	public Vector2 WiggleLeg(Bone2D lowerleg, Bone2D midleg, Bone2D upperleg) {

			
			//JUST SORTING LIMB ONE FIRST
			
			// GET CENTRE POINT OF LOWER LIMB
			var angle = lowerleg.GlobalRotationDegrees;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(angle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(angle));
			var centrepoint_lowerlimb = new Vector2((lowerleg.GlobalPosition.X+x_offset_lower),(lowerleg.GlobalPosition.Y+y_offset_lower));;
			var hypotenuse_lower = lowerleg.GetLength() / 2;

			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;
			var bottompoint_lowerlimb = new Vector2((centrepoint_lowerlimb.X+(x_offset_lower*2)),(centrepoint_lowerlimb.Y+(y_offset_lower*2)));
			var toppoint_lowerlimb = new Vector2((lowerleg.GlobalPosition.X),(lowerleg.GlobalPosition.Y));
			

			
			var hypotenuse_mid = midleg.GetLength() / 2;
			var midangle = midleg.GlobalRotationDegrees;
			var x_offset_mid = Mathf.Cos(Mathf.DegToRad(midangle));
			var y_offset_mid = Mathf.Sin(Mathf.DegToRad(midangle));
			x_offset_mid = x_offset_mid * hypotenuse_mid;
			y_offset_mid = y_offset_mid * hypotenuse_mid;
			var midleg_globalrotationdegrees = midangle;
			var midleg_globalposition = new Vector2((toppoint_lowerlimb.X-(x_offset_mid*2)),(toppoint_lowerlimb.Y-(y_offset_mid*2)));
			
			// OKAY NEED TO HAVE THE GLOBAL POSITION OF EACH EGG AS THE TOP POINT
			
			var hypotenuse_upper = upperleg.GetLength() / 2;
			var upperangle = upperleg.GlobalRotationDegrees;
			var x_offset_upper = Mathf.Cos(Mathf.DegToRad(upperangle));
			var y_offset_upper = Mathf.Sin(Mathf.DegToRad(upperangle));
			x_offset_upper = x_offset_upper * hypotenuse_upper;
			y_offset_upper = y_offset_upper * hypotenuse_upper;
			upperleg.GlobalPosition = new Vector2((midleg_globalposition.X-(x_offset_upper*2)),(midleg_globalposition.Y-(y_offset_upper*2)));

			midleg.GlobalPosition = midleg_globalposition;
			
			return upperleg.GlobalPosition;
			
			

	}
	
	

	

	
	public static int Truth(params bool[] booleans)
	{
		return booleans.Count(b => b);
	}
	
	

	
	
}
