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
	
	private bool hitleg1 = false;
	private bool hitleg2 = false;
	private bool hitleg3 = false;
	private bool hitleg4 = false;
	
	private float playerDirection = 1.0f;
	
	//private float ALLOWED_HEIGHT = 177.228f;
	private float ALLOWED_HEIGHT = 175.0f;
	
	private float LEG_OUTER_FAR = 160.0f;
	private float LEG_OUTER_NEAR = 100.0f;
	private float LEG_INNER_FAR = 60.0f;
	private float LEG_INNER_NEAR = 40.0f;
	
	private int frame = 0;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		frame = 1;
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
		
		var foot1 = GetFoot(lowerleg1);
		var foot2 = GetFoot(lowerleg2);
		var foot3 = GetFoot(lowerleg3);
		var foot4 = GetFoot(lowerleg4);
		
		
		
	}

	public override void _PhysicsProcess(double delta)
	{
		if (frame == 1) { // colliders don't work in frame 1, so just start from frame 2
			frame = 2;
			return;
		}
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
		
		//GD.Print("4Lower Leg: ",lowerleg4.GlobalPosition);
		//GD.Print("4Mid Leg: ",midleg4.GlobalPosition);
		//GD.Print("4Upper Leg: ",upperleg4.GlobalPosition);
		
		var currentlegpos1 = GetFoot(lowerleg1);
		var currentlegpos2 = GetFoot(lowerleg2);
		var currentlegpos3 = GetFoot(lowerleg3);
		var currentlegpos4 = GetFoot(lowerleg4);
		
		var newlegpos1 = GetFoot(lowerleg1);
		var newlegpos2 = GetFoot(lowerleg2);
		var newlegpos3 = GetFoot(lowerleg3);
		var newlegpos4 = GetFoot(lowerleg4);
		
		var overextended1 = false;
		var overextended2 = false;
		var overextended3 = false;
		var overextended4 = false;
		
		
		
		// Which direction is the player in?
		var currentPlayer = GetTree().GetFirstNodeInGroup("player") as Player;
		var widowPos = new Vector2(Position.X,(Position.Y+137.0f));
		if (widowPos.DirectionTo(currentPlayer.Position).X < 0) {
			playerDirection = -1;
			//midsprite1.Modulate = new Color(0.883f, 0.28f, 0.579f, 1.0f);
		} else {
			playerDirection = 1;
			//midsprite1.Modulate = new Color(0.33f, 0.58f, 0.579f, 1.0f);
		}
		
		if (widowPos.DistanceTo(currentPlayer.Position) < 100) { // If the player and widow are too close to each other don't do anything because usually the player would die
			return;
		}
		
		

//----------------------------------------------------------------------------------------------------------------------------------------
		
		if (leg1pickup) { /// if we are picking up our leg
			//lowersprite1.Modulate = new Color(0.025f, 0.548f, 0.643f, 1.0f);
			var globalPos = lowerleg1.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition); 
			//GD.Print("PING!");
			//GD.Print("distance: "+distance);
			//GD.Print("direction2: "+direction2.X);
			//GD.Print("player direction: "+playerDirection);
			if (playerDirection == 1) {
				if (direction2.X > 0) { //  if widow is only slightly in front of us
					if (distance < LEG_OUTER_NEAR) {
						leg1pickup = false;
					}
				} else {
					leg1pickup = false;
				}
			} else {
				if (direction2.X > 0) { // IF THE WIDOW IS behind me when I'm going left
					if (distance > LEG_OUTER_FAR) {
						leg1pickup = false;
					}
				}
			}

			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var direction = currentlegpos1.DirectionTo(player.Position);
			newlegpos1 = new Vector2((currentlegpos1.X+(direction.X*Speed)),(currentlegpos1.Y+(direction.Y*Speed))); // + y because we should continue in a - direction

		} else if (!leg1planted && !leg1pickup) {
			var upper_leg_pos = upperleg1.GlobalPosition;
			var lower_leg_upper_pos = new Vector2((lowerleg1.GlobalPosition.X),(lowerleg1.GlobalPosition.Y+Speed));
			
			var lowerangle = lowerleg1.GlobalRotationDegrees;
			var hypotenuse_lower = lowerleg1.GetLength() / 2;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;

			var bottompoint_lowerlimb = new Vector2((lower_leg_upper_pos.X+(x_offset_lower*2)),(lower_leg_upper_pos.Y+(y_offset_lower*2)));
			
			
			if (bottompoint_lowerlimb.DistanceTo(upper_leg_pos) >= ALLOWED_HEIGHT) {
				overextended1 = true;
				//GD.Print("1 overextended");
			} else {
				newlegpos1 = new Vector2((currentlegpos1.X),(currentlegpos1.Y+(Speed))); //+ speed which is pos because we're stepping DOWN
			}
		} else if (leg1planted) { //If we are planted
			var globalPos = lowerleg1.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction.X > 0) {
					if (distance > LEG_OUTER_FAR) {
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
					if (distance < LEG_OUTER_NEAR) { // if it's kind of close to me
						leg1planted = false;
						leg1pickup = true;
					}
				}
			}	
		}
//----------------------------------------------------------------------------------------------------------------------------------------
		if (leg2pickup) { /// if we are picking up our leg
			var globalPos = lowerleg2.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X > 0) { //  if widow is only slightly in front of us
					if (distance < LEG_INNER_NEAR) {
						leg2pickup = false;
					}
				} else {
					leg2pickup = false;
				}
			} else {
				if (direction2.X > 0) { // IF THE WIDOW IS "behind" when I'm going left
					if (distance > LEG_INNER_FAR) {
						leg2pickup = false;
					}
				}
			}
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var direction = currentlegpos2.DirectionTo(player.Position);
			newlegpos2 = new Vector2((currentlegpos2.X+(direction.X*Speed)),(currentlegpos2.Y+(direction.Y*Speed))); // + y because we should continue in a - direction

		} else if (!leg2planted && !leg2pickup) {
			//ALLOWED_HEIGHT
			//
			var upper_leg_pos = upperleg2.GlobalPosition;
			var lower_leg_upper_pos = new Vector2((lowerleg2.GlobalPosition.X),(lowerleg2.GlobalPosition.Y+Speed));
			
			var lowerangle = lowerleg2.GlobalRotationDegrees;
			var hypotenuse_lower = lowerleg2.GetLength() / 2;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;

			var bottompoint_lowerlimb = new Vector2((lower_leg_upper_pos.X+(x_offset_lower*2)),(lower_leg_upper_pos.Y+(y_offset_lower*2)));
			
			
			if (bottompoint_lowerlimb.DistanceTo(upper_leg_pos) >= ALLOWED_HEIGHT) {
				overextended2 = true;
				//GD.Print("2 overextended");
			} else {
				newlegpos2 = new Vector2((currentlegpos2.X),(currentlegpos2.Y+(Speed))); //+ speed which is pos because we're stepping DOWN
			}
		} else if (leg2planted) {
			var globalPos = lowerleg2.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction = globalPos.DirectionTo(GlobalPosition);
			//GD.Print("2Distance: "+distance);
			//GD.Print("2Direction: "+direction);
			//GD.Print("2Player Direction: "+playerDirection);
			if (playerDirection == 1) {
				if (direction.X > 0) {
					if (distance > LEG_INNER_FAR) {
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
					if (distance < LEG_INNER_NEAR) { // if it's kind of close to me
						leg2planted = false;
						leg2pickup = true;
					}
				}
				
				
				
			}
			
		}
//----------------------------------------------------------------------------------------------------------------------------------------
		if (leg3pickup) { /// if we are picking up our leg
			var globalPos = lowerleg3.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X < 0) { // If the widow is behind me
					if (distance > LEG_INNER_FAR) {
						leg3pickup = false;
					}
				}
			} else {
				if (direction2.X < 0) { // Given the widow is in front of me
					if (distance < LEG_INNER_NEAR) {
						leg3pickup = false;
					}
				} else { // if I'm going left and the widow is behind me
					leg3pickup = false;
				}
			}
			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var direction = currentlegpos3.DirectionTo(player.Position);
			newlegpos3 = new Vector2((currentlegpos3.X+(direction.X*Speed)),(currentlegpos3.Y+(direction.Y*Speed))); // + y because we should continue in a - direction

		} else if (!leg3planted && !leg3pickup) {
			//ALLOWED_HEIGHT
			//
			var upper_leg_pos = upperleg3.GlobalPosition;
			var lower_leg_upper_pos = new Vector2((lowerleg3.GlobalPosition.X),(lowerleg3.GlobalPosition.Y+Speed));
			
			var lowerangle = lowerleg3.GlobalRotationDegrees;
			var hypotenuse_lower = lowerleg3.GetLength() / 2;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;

			var bottompoint_lowerlimb = new Vector2((lower_leg_upper_pos.X+(x_offset_lower*2)),(lower_leg_upper_pos.Y+(y_offset_lower*2)));
			
			
			if (bottompoint_lowerlimb.DistanceTo(upper_leg_pos) >= ALLOWED_HEIGHT) {
				overextended3 = true;
				//GD.Print("3 overextended");
			} else {
				newlegpos3 = new Vector2((currentlegpos3.X),(currentlegpos3.Y+(Speed))); //+ speed which is pos because we're stepping DOWN
			}
		} else if (leg3planted) {
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
				} else {
					if (distance < LEG_INNER_NEAR) { // if it's kind of close to me
						leg3planted = false;
						leg3pickup = true;
					}
				}
			} else {
				if (direction.X < 0) { // If the widow is in front of me when I'm going left
					if (distance > LEG_INNER_FAR) {
						leg3planted = false;
						leg3pickup = true;
					}
				}
			}
			
			
		}
		
		
//----------------------------------------------------------------------------------------------------------------------------------------		
		if (leg4pickup) { /// if we are picking up our leg
			//midsprite4.Modulate = new Color(0.883f, 0.58f, 0.579f, 1.0f);
			var globalPos = lowerleg4.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction2 = globalPos.DirectionTo(GlobalPosition);
			if (playerDirection == 1) {
				if (direction2.X < 0) { // IF THE WIDOW IS BEHIND ME
					if (distance > LEG_OUTER_FAR) {
						//GD.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!BANANA!");
						leg4pickup = false;
					}
				}
			} else {
				if (direction2.X < 0) { // Given the widow is in front of me
					if (distance < LEG_OUTER_NEAR) {
						//GD.Print("BANANA!!");
						leg4pickup = false;
					}
				} else { // If I'm going left and WIDOW is behind me
					//GD.Print("BANANA!!");
					leg4pickup = false;
				}
			}

			var player = GetTree().GetFirstNodeInGroup("player") as Player;
			var direction = currentlegpos4.DirectionTo(player.Position);
			newlegpos4 = new Vector2((currentlegpos4.X+(direction.X*Speed)),(currentlegpos4.Y+(direction.Y*Speed))); // + y because we should continue in a - direction
			

		} else if (!leg4planted && !leg4pickup) {
			//ALLOWED_HEIGHT
			//
			var upper_leg_pos = upperleg4.GlobalPosition;
			var lower_leg_upper_pos = new Vector2((lowerleg4.GlobalPosition.X),(lowerleg4.GlobalPosition.Y+Speed));
			
			var lowerangle = lowerleg4.GlobalRotationDegrees;
			var hypotenuse_lower = lowerleg4.GetLength() / 2;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;

			var bottompoint_lowerlimb = new Vector2((lower_leg_upper_pos.X+(x_offset_lower*2)),(lower_leg_upper_pos.Y+(y_offset_lower*2)));
			
			
			if (bottompoint_lowerlimb.DistanceTo(upper_leg_pos) >= ALLOWED_HEIGHT) {
				overextended4 = true;
				//GD.Print("4 overextended");
			} else {
				//GD.Print("4 ping");
				newlegpos4 = new Vector2((currentlegpos4.X),(currentlegpos4.Y+(Speed))); //+ speed which is pos because we're stepping DOWN
			}
		} else if (leg4planted) {
			var globalPos = lowerleg4.GlobalPosition;
			var globalPosX = new Vector2(globalPos.X,0);
			var globalPositionX = new Vector2(GlobalPosition.X,0);
			var distance = globalPosX.DistanceTo(globalPositionX);
			var direction = globalPos.DirectionTo(GlobalPosition);
			//midsprite4.Modulate = new Color(0.1f, 0.88f, 0.579f, 1.0f);
			if (playerDirection == 1) {
				if (direction.X > 0) { //IF the widow is in front of me
					if (distance > 0.0f) {
						leg4planted = false;
						leg4pickup = true;
					}
				} else { // if the widow is behind me and we are going right
					if (distance < LEG_OUTER_NEAR) { // if it's kind of close to me ????????????????????????????????????????????????????????????????
						leg4planted = false;
						leg4pickup = true;
					}
				}
			} else {
				if (direction.X < 0) { //IF the widow is in front of me and I'm going left
					if (distance > LEG_OUTER_FAR) {
						leg4planted = false;
						leg4pickup = true;
					}
				}
			}

			
		}
		
		//GD.Print(lowerleg1.GlobalPosition);
		//GD.Print(lowerleg2.GlobalPosition);
		//GD.Print(lowerleg3.GlobalPosition);
		//GD.Print(lowerleg4.GlobalPosition);
		
		//GD.Print("whyarewewaitingwhyarewewaiting");
		
		lowerleg1.GlobalPosition = CanIMoveMyLeg(currentlegpos1, newlegpos1, leg1pickup, leg1planted, lowerleg1, upperleg1, "1");
		lowerleg2.GlobalPosition = CanIMoveMyLeg(currentlegpos2, newlegpos2, leg2pickup, leg2planted, lowerleg2, upperleg2, "2");
		lowerleg3.GlobalPosition = CanIMoveMyLeg(currentlegpos3, newlegpos3, leg3pickup, leg3planted, lowerleg3, upperleg3, "3");
		lowerleg4.GlobalPosition = CanIMoveMyLeg(currentlegpos4, newlegpos4, leg4pickup, leg4planted, lowerleg4, upperleg4, "4");
		
		//GD.Print(lowerleg1.GlobalPosition);
		//GD.Print(lowerleg2.GlobalPosition);
		//GD.Print(lowerleg3.GlobalPosition);
		//GD.Print(lowerleg4.GlobalPosition);
		
		//GD.Print("whyarewewaitingwhyarewewaiting");
		
		
//----------------------------------------------------------------------------------------------------------------------------------------
		

		
		var leg1 = lowerleg1.GlobalPosition;
		var leg2 = lowerleg2.GlobalPosition;
		var leg3 = lowerleg3.GlobalPosition;
		var leg4 = lowerleg4.GlobalPosition;
		
		var leg1rotation = lowerleg1.GlobalRotationDegrees;
		var leg2rotation = lowerleg2.GlobalRotationDegrees;
		var leg3rotation = lowerleg3.GlobalRotationDegrees;
		var leg4rotation = lowerleg4.GlobalRotationDegrees;
		
		
		lowerleg1.GlobalPosition = leg1;
		lowerleg2.GlobalPosition = leg2;
		lowerleg3.GlobalPosition = leg3;
		lowerleg4.GlobalPosition = leg4; 
		
		
		//Position = new Vector2((upperconnection1.X + 87.982f),(upperconnection1.Y - 5.321f));
		var foot1 = GetFoot(lowerleg1);
		var foot2 = GetFoot(lowerleg2);
		var foot3 = GetFoot(lowerleg3);
		var foot4 = GetFoot(lowerleg4);
		
		//if (leg4planted) {
			//GD.Print(" 4 IS PLANTED ");
		//} else if (leg4pickup) {
			//GD.Print(" 4 IS PICKUP ");
		//} else {
			//GD.Print(" 4 IS STEPPING ");
		//}
		
		if ((Truth(leg1planted, leg2planted, leg3planted, leg4planted) >= 2)||(Truth(overextended1, overextended2, overextended3, overextended4) >= 2)) {
			//GD.Print("OH HAI");
			Position = new Vector2((Position.X + (playerDirection * Speed)), (((foot1.Y + foot2.Y + foot3.Y + foot4.Y) / 4f) - 105f));
		} else {
			//Position = (foot1 + foot2 + foot3 + foot4) / 4f;
			Position = new Vector2(Position.X, Position.Y); //if legs are uppy then don't move
		}
		
		//Position = new Vector2(((foot1.X + foot2.X + foot3.X + foot4.X) / 4f),Position.Y);
		

		
		// For leg 1
		upperleg1.GlobalPosition = new Vector2((Position.X - 87.993f), (Position.Y + 2.321f));
		upperleg2.GlobalPosition = new Vector2((Position.X - 37.004f), (Position.Y - 0.865f));
		upperleg3.GlobalPosition = new Vector2((Position.X + 31.996f), (Position.Y - 1.081f));
		upperleg4.GlobalPosition = new Vector2((Position.X + 93.01f), (Position.Y + 2.321f));
		
		lowerleg1.GlobalPosition = leg1;
		lowerleg1.GlobalRotationDegrees = leg1rotation;
		
		lowerleg2.GlobalPosition = leg2;
		lowerleg2.GlobalRotationDegrees = leg2rotation;
		
		lowerleg3.GlobalPosition = leg3;
		lowerleg3.GlobalRotationDegrees = leg3rotation;
		
		lowerleg4.GlobalPosition = leg4;
		lowerleg4.GlobalRotationDegrees = leg4rotation;
		
		GD.Print("1: ",currentlegpos1);
		GD.Print("1: ",newlegpos1);
		GD.Print("2: ",currentlegpos2);
		GD.Print("2: ",newlegpos2);
		GD.Print("3: ",currentlegpos3);
		GD.Print("3: ",newlegpos3);
		GD.Print("4: ",currentlegpos4);
		GD.Print("4: ",newlegpos4);
		

		var h1 = upperleg1.GlobalPosition.DistanceTo(GetFoot(lowerleg1)) ;
		if (h1 > ALLOWED_HEIGHT) {
			GD.Print(lowerleg1.GlobalPosition);
			lowerleg1.GlobalPosition = new Vector2(lowerleg1.GlobalPosition.X,(lowerleg1.GlobalPosition.Y - 30.0f));
			GD.Print(lowerleg1.GlobalPosition);
			GD.Print("UH OH! 1");
		}
		
		var h2 = upperleg2.GlobalPosition.DistanceTo(GetFoot(lowerleg2)) ;
		if (h2 > ALLOWED_HEIGHT) {
			GD.Print(lowerleg2.GlobalPosition);
			lowerleg2.GlobalPosition = new Vector2(lowerleg2.GlobalPosition.X,(lowerleg2.GlobalPosition.Y - 30.0f));
			GD.Print(lowerleg2.GlobalPosition);
			GD.Print("UH OH! 2");
		}
		
		var h3 = upperleg3.GlobalPosition.DistanceTo(GetFoot(lowerleg3)) ;
		if (h3 > ALLOWED_HEIGHT) {
			GD.Print(lowerleg3.GlobalPosition);
			lowerleg3.GlobalPosition = new Vector2(lowerleg3.GlobalPosition.X,(lowerleg3.GlobalPosition.Y - 30.0f));
			GD.Print(lowerleg3.GlobalPosition);
			GD.Print("UH OH! 3");
		}
		
		var h4 = upperleg4.GlobalPosition.DistanceTo(GetFoot(lowerleg4));
		GD.Print(h4);
		if (h4 > ALLOWED_HEIGHT) {
			lowerleg4.GlobalPosition = new Vector2(lowerleg4.GlobalPosition.X,(lowerleg4.GlobalPosition.Y - 30.0f));
			GD.Print(lowerleg4.GlobalPosition);
			GD.Print("UH OH! 4");
			GD.Print(upperleg4.GlobalPosition.DistanceTo(GetFoot(lowerleg4)));
		}
		
		CalculateLeg(lowerleg1, midleg1, upperleg1, "1");		
		CalculateLeg(lowerleg2, midleg2, upperleg2, "2");
		CalculateLeg(lowerleg3, midleg3, upperleg3, "3");
		CalculateLeg(lowerleg4, midleg4, upperleg4, "4");
		
		var foot444 = GetFoot(lowerleg4);
		
		GD.Print("lower leg 4: ",lowerleg4.GlobalPosition);
		
		MoveAndSlide();
		
		GD.Print("lower leg 4: ",lowerleg4.GlobalPosition);
		//GetTree().Paused = true;
	}
	
	public Vector2 GetFoot(Bone2D lowerleg) {
		var lowerangle = lowerleg.GlobalRotationDegrees;
		var hypotenuse_lower = lowerleg.GetLength() / 2;
		var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
		var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
		x_offset_lower = x_offset_lower * hypotenuse_lower;
		y_offset_lower = y_offset_lower * hypotenuse_lower;

		var bottompoint_lowerlimb = new Vector2((lowerleg.GlobalPosition.X+(x_offset_lower*2)),(lowerleg.GlobalPosition.Y+(y_offset_lower*2)));
		return bottompoint_lowerlimb;
	}
	

	
	public void CalculateLeg(Bone2D lowerleg, Bone2D midleg, Bone2D upperleg, string leg) {
		
			
			var point_upper = upperleg.GlobalPosition;

			//GD.Print(leg);
			//JUST SORTING LIMB ONE FIRST
			
			// GET CENTRE POINT OF LOWER LIMB
			var lowerangle = lowerleg.GlobalRotationDegrees;
			var hypotenuse_lower = lowerleg.GetLength() / 2;
			var x_offset_lower = Mathf.Cos(Mathf.DegToRad(lowerangle));
			var y_offset_lower = Mathf.Sin(Mathf.DegToRad(lowerangle));
			x_offset_lower = x_offset_lower * hypotenuse_lower;
			y_offset_lower = y_offset_lower * hypotenuse_lower;

			var centrepoint_lowerlimb = new Vector2((lowerleg.GlobalPosition.X+x_offset_lower),(lowerleg.GlobalPosition.Y+y_offset_lower));;

			var bottompoint_lowerlimb = new Vector2((centrepoint_lowerlimb.X+(x_offset_lower)),(centrepoint_lowerlimb.Y+(y_offset_lower)));
			//GD.Print("Bottom Point of Lower Limb Originally: "+bottompoint_lowerlimb);
			var point_lower = bottompoint_lowerlimb;
			
			// mid point of fixed point and foot
			var midpoint = (upperleg.GlobalPosition + bottompoint_lowerlimb) /2f;
			
			var hypotenuse = upperleg.GlobalPosition.DistanceTo(bottompoint_lowerlimb); //produces a float
			
			var mid_unit_vector = upperleg.GlobalPosition.DirectionTo(bottompoint_lowerlimb);
			
			var hypotenuse_mid = midleg.GetLength() / 2;
			
			var myAngle = Mathf.RadToDeg(Mathf.Atan2(mid_unit_vector.Y,mid_unit_vector.X));
			
			var midangle = myAngle;
			
			var x_offset_mid = mid_unit_vector.X * hypotenuse_mid;
			var y_offset_mid = mid_unit_vector.Y * hypotenuse_mid;
			var midleg_globalposition = new Vector2((midpoint.X-(x_offset_mid)),(midpoint.Y-(y_offset_mid)));

			// h = x + 2xcos(A)
			
			var h = upperleg.GlobalPosition.DistanceTo(bottompoint_lowerlimb);
			GD.Print("h: "+h);
			if (h > ALLOWED_HEIGHT) {
				GetTree().Paused = true;
			}
		
			
			var x = midleg.GetLength();
			//GD.Print("h: "+h);
			//GD.Print("x: "+x);
			
			var cosA = ((h -x) / (2*x));
			//GD.Print("cosA: "+cosA);
			
			var A = Mathf.RadToDeg(Mathf.Acos(cosA)) + 180.0f;
			//GD.Print("A: "+A);
			
			var xsinA = x * Mathf.Sin(Mathf.DegToRad(A));
			//GD.Print("xsinA: "+xsinA);
			
			var xcosA = x * Mathf.Cos(Mathf.DegToRad(A));
			//GD.Print("xcosA: "+xcosA);
			
			//GD.Print("apples: "+midleg_globalposition);
			var perpendicular_offset = new Vector2((midleg_globalposition.X+(xsinA)),(midleg_globalposition.Y)); // 1
			if ((leg == "3") || (leg == "4")) {
				perpendicular_offset = new Vector2((midleg_globalposition.X-(xsinA)),(midleg_globalposition.Y)); // 1
			}
			//GD.Print("perpendicular apples: "+perpendicular_offset);

						
			var upperangle = upperleg.GlobalRotationDegrees;
			
						
			var midleg_lowerpoint = new Vector2((perpendicular_offset.X+(x_offset_mid*2)),(perpendicular_offset.Y+(y_offset_mid*2)));
			// top point of the foot is lowerleg.GlobalPosition
			
			var lowerleg_unit_vector = midleg_lowerpoint.DirectionTo(bottompoint_lowerlimb);
			var myAngle2 = Mathf.RadToDeg(Mathf.Atan2(lowerleg_unit_vector.Y,lowerleg_unit_vector.X));
			
			
			var upperleg_toppoint = upperleg.GlobalPosition;
			var upperleg_unit_vector = upperleg_toppoint.DirectionTo(perpendicular_offset);
			var myAngle3 = Mathf.RadToDeg(Mathf.Atan2(upperleg_unit_vector.Y,upperleg_unit_vector.X));
			
			///-----------------------------------------------------------------------------------------------

			var hypotenuse_upper = upperleg.GetLength() / 2;
			var x_offset_upper = Mathf.Cos(Mathf.DegToRad(upperangle));
			var y_offset_upper = Mathf.Sin(Mathf.DegToRad(upperangle));
			x_offset_upper = x_offset_upper * hypotenuse_upper;
			y_offset_upper = y_offset_upper * hypotenuse_upper;

			var bottompoint_upperlimb = new Vector2((upperleg.GlobalPosition.X+(x_offset_upper*2)),(upperleg.GlobalPosition.Y+(y_offset_upper*2)));
			
			upperleg.GlobalRotationDegrees = myAngle3;
			upperleg.GlobalPosition = point_upper;
			midleg.GlobalRotationDegrees = myAngle; // add 180 otherwise itll be facing upside down
			midleg.GlobalPosition = perpendicular_offset;
			lowerleg.GlobalRotationDegrees = myAngle2;
			
						// GET CENTRE POINT OF LOWER LIMB
			var lowerangle2 = lowerleg.GlobalRotationDegrees;
			var hypotenuse_lower2 = lowerleg.GetLength() / 2;
			var x_offset_lower2 = Mathf.Cos(Mathf.DegToRad(lowerangle2));
			var y_offset_lower2 = Mathf.Sin(Mathf.DegToRad(lowerangle2));
			x_offset_lower2 = x_offset_lower2 * hypotenuse_lower2;
			y_offset_lower2 = y_offset_lower2 * hypotenuse_lower2;
			
			//bottompoint is point_lower
			
			var new_lower_top_point = new Vector2((point_lower.X-(x_offset_lower2*2)),(point_lower.Y-(y_offset_lower2 * 2)));
			lowerleg.GlobalPosition = new_lower_top_point;
			
			var centrepoint_lowerlimb2 = new Vector2((lowerleg.GlobalPosition.X+x_offset_lower2),(lowerleg.GlobalPosition.Y+y_offset_lower2));;
			var bottompoint_lowerlimb2 = new Vector2((centrepoint_lowerlimb2.X+(x_offset_lower2)),(centrepoint_lowerlimb2.Y+(y_offset_lower2)));

			
			return;

	}
	
	public static int Truth(params bool[] booleans)
	{
		return booleans.Count(b => b);
	}
	
	public Vector2 CanIMoveMyLeg(Vector2 currentlegpos, Vector2 newlegpos, bool legpickup, bool legplanted, Bone2D leg, Bone2D upperleg, string theLeg) {
		var returnable = leg.GlobalPosition;
		if (currentlegpos == newlegpos) {
			//GD.Print("LEG PLANTED");
		} else {
			// create a raycast for the vector between the two
			var spaceState = GetWorld2D().DirectSpaceState;
			var query = PhysicsRayQueryParameters2D.Create(currentlegpos, newlegpos);
			var result = spaceState.IntersectRay(query);
			if (result.Count > 0)
			{
				//GD.Print("LEG Hit at point: ", result["position"]);
				if (!legpickup && !legplanted) {
					if (theLeg == "1") {
						leg1planted = true;
					} else if (theLeg == "2") {
						leg2planted = true;
					} else if (theLeg == "3") {
						leg3planted = true;
					} else if (theLeg == "4") {
						leg4planted = true;
					}
					
				}
				// if there is a hit
				// if pickup
				// setup y movement only ray
				// setup x movement only ray
				//
				if (newlegpos.DistanceTo(upperleg.GlobalPosition) < ALLOWED_HEIGHT) { // if I'm not overextended
					if (legpickup) {
						var y_new_without_x = new Vector2(currentlegpos.X, newlegpos.Y);
						var y_ray = PhysicsRayQueryParameters2D.Create(currentlegpos, y_new_without_x); //create a ray that doesn't contain x at all
						var resultY = spaceState.IntersectRay(y_ray);
						
						var x_new_without_y = new Vector2(newlegpos.X,currentlegpos.Y);
						var x_ray = PhysicsRayQueryParameters2D.Create(currentlegpos, x_new_without_y); //create a ray that doesn't contain y at all
						var resultX = spaceState.IntersectRay(x_ray);
						
						if ((resultX.Count > 0)&&(resultY.Count > 0)) { //if we collide on both axes do nothing
							//GD.Print("DO NOTHING");
						} else if ((resultX.Count > 0)) { // if we only hit on the X axis we should still move by the Y
							var lowerangle2 = leg.GlobalRotationDegrees;
							var hypotenuse_lower2 = leg.GetLength() / 2;
							var x_offset_lower2 = Mathf.Cos(Mathf.DegToRad(lowerangle2));
							var y_offset_lower2 = Mathf.Sin(Mathf.DegToRad(lowerangle2));
							x_offset_lower2 = x_offset_lower2 * hypotenuse_lower2;
							y_offset_lower2 = y_offset_lower2 * hypotenuse_lower2;
							returnable = new Vector2((leg.GlobalPosition.X),(newlegpos.Y-(y_offset_lower2 * 2)));
						} else if ((resultY.Count > 0)) { // if we only hit on the Y axis we should still move by the X
							var lowerangle2 = leg.GlobalRotationDegrees;
							var hypotenuse_lower2 = leg.GetLength() / 2;
							var x_offset_lower2 = Mathf.Cos(Mathf.DegToRad(lowerangle2));
							var y_offset_lower2 = Mathf.Sin(Mathf.DegToRad(lowerangle2));
							x_offset_lower2 = x_offset_lower2 * hypotenuse_lower2;
							y_offset_lower2 = y_offset_lower2 * hypotenuse_lower2;
							returnable = new Vector2((newlegpos.X-(x_offset_lower2 * 2)),(leg.GlobalPosition.Y));
						}
					}
				} 
				
				
			} else {
				if (newlegpos.DistanceTo(upperleg.GlobalPosition) < ALLOWED_HEIGHT) { // if I'm not overextended
					var lowerangle2 = leg.GlobalRotationDegrees;
					var hypotenuse_lower2 = leg.GetLength() / 2;
					var x_offset_lower2 = Mathf.Cos(Mathf.DegToRad(lowerangle2));
					var y_offset_lower2 = Mathf.Sin(Mathf.DegToRad(lowerangle2));
					x_offset_lower2 = x_offset_lower2 * hypotenuse_lower2;
					y_offset_lower2 = y_offset_lower2 * hypotenuse_lower2;
					returnable = new Vector2((newlegpos.X-(x_offset_lower2 * 2)),(newlegpos.Y-(y_offset_lower2 * 2)));
				}
				
			}
		}
		return returnable;
	}
	
	
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------------------------------------------------------------
	
	

	
	

	
	
}
