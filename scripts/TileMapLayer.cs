using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GD.Print(GetUsedCells());
		foreach (var tile in GetUsedCells()) 
		{
			//If has any neighbours
			bool leftN = false;
			bool rightN = false;
			bool topN = false;
			bool bottomN = false;
			
			Vector2I neighbor = new Vector2I(tile.X - 1, tile.Y);

			if (GetUsedCells().Contains(neighbor))
			{
				leftN = true;
			}
			
			neighbor = new Vector2I(tile.X + 1, tile.Y);
			
			if (GetUsedCells().Contains(neighbor))
			{
				rightN = true;
			}
			
			neighbor = new Vector2I(tile.X, tile.Y - 1);
			
			if (GetUsedCells().Contains(neighbor))
			{
				topN = true;
			}
			
			neighbor = new Vector2I(tile.X, tile.Y + 1);
			
			if (GetUsedCells().Contains(neighbor))
			{
				bottomN = true;
			}
			
			//If has any neighbours
			
			
			
		  var theTile = this.GetCellTileData(tile);
		  // Load the shader resource
		  var shader = GD.Load<Shader>("res://tile_system.gdshader");
		  var sheet = GD.Load<Texture2D>("res://art/carrion_tiles_duplicate.png");
		  var sheetSize = sheet.GetSize(); // Vector2, e.g. (32, 32)
		
			// Region in pixel coords -> convert to UV (0-1) space
			Vector2 regionPos = new Vector2(386, 95);
			Vector2 regionSize = new Vector2(32, 32);

			Vector4 topLeftRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(284, 95);
			regionSize = new Vector2(32, 32);
			
			Vector4 topRightRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(316, 202);
			regionSize = new Vector2(32, 32);
			
			Vector4 bottomLeftRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(214, 133);
			regionSize = new Vector2(32, 32);
			
			Vector4 bottomRightRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(423, 271);
			regionSize = new Vector2(32, 32);
			
			Vector4 clearRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(423, 95);
			regionSize = new Vector2(32, 32);
			
			Vector4 topRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(214, 271);
			regionSize = new Vector2(32, 32);
			
			Vector4 bottomRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(386, 202);
			regionSize = new Vector2(32, 32);
			
			Vector4 leftRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			regionPos = new Vector2(284, 133);
			regionSize = new Vector2(32, 32);
			
			Vector4 rightRegion = new Vector4(
				regionPos.X / sheetSize.X,
				regionPos.Y / sheetSize.Y,
				regionSize.X / sheetSize.X,
				regionSize.Y / sheetSize.Y
			);
			
			
			
		
		
		  var shaderMaterialOne = new ShaderMaterial();
		  shaderMaterialOne.Shader = shader;
		  shaderMaterialOne.SetShaderParameter("red",0.1f);
		  shaderMaterialOne.SetShaderParameter("test_texture", sheet);
		  shaderMaterialOne.SetShaderParameter("top_left_region", clearRegion);
		  shaderMaterialOne.SetShaderParameter("top_right_region", clearRegion);
		  shaderMaterialOne.SetShaderParameter("bottom_left_region", clearRegion);
		  shaderMaterialOne.SetShaderParameter("bottom_right_region", clearRegion);
		

		
		
		  var shaderMaterialTwo = new ShaderMaterial();
		  shaderMaterialTwo.Shader = shader;
		  shaderMaterialTwo.SetShaderParameter("green",0.2f);
		  shaderMaterialTwo.SetShaderParameter("test_texture", sheet);
		  shaderMaterialTwo.SetShaderParameter("top_left_region", topRegion);
		  shaderMaterialTwo.SetShaderParameter("top_right_region", topRegion);
		  shaderMaterialTwo.SetShaderParameter("bottom_left_region", clearRegion);
		  shaderMaterialTwo.SetShaderParameter("bottom_right_region", clearRegion);
		
		  var shaderMaterialThree = new ShaderMaterial();
		  shaderMaterialThree.Shader = shader;
		  shaderMaterialThree.SetShaderParameter("blue",0.3f);
		  shaderMaterialThree.SetShaderParameter("test_texture", sheet);
		  shaderMaterialThree.SetShaderParameter("top_left_region", topRegion);
		  shaderMaterialThree.SetShaderParameter("top_right_region", topRightRegion);
		  shaderMaterialThree.SetShaderParameter("bottom_left_region", clearRegion);
		  shaderMaterialThree.SetShaderParameter("bottom_right_region", rightRegion);
		
		  var shaderMaterialFour = new ShaderMaterial();
		  shaderMaterialFour.Shader = shader;
		  shaderMaterialFour.SetShaderParameter("red",0.4f);
		  shaderMaterialFour.SetShaderParameter("test_texture", sheet);
		  shaderMaterialFour.SetShaderParameter("top_left_region", clearRegion);
		  shaderMaterialFour.SetShaderParameter("top_right_region", rightRegion);
		  shaderMaterialFour.SetShaderParameter("bottom_left_region", clearRegion);
		  shaderMaterialFour.SetShaderParameter("bottom_right_region", rightRegion);
		
		  var shaderMaterialFive = new ShaderMaterial();
		  shaderMaterialFive.Shader = shader;
		  shaderMaterialFive.SetShaderParameter("green",0.5f);
		  shaderMaterialFive.SetShaderParameter("test_texture", sheet);
		  shaderMaterialFive.SetShaderParameter("top_left_region", clearRegion);
		  shaderMaterialFive.SetShaderParameter("top_right_region", rightRegion);
		  shaderMaterialFive.SetShaderParameter("bottom_left_region", bottomRegion);
		  shaderMaterialFive.SetShaderParameter("bottom_right_region", bottomRightRegion);
		
		  var shaderMaterialSix = new ShaderMaterial();
		  shaderMaterialSix.Shader = shader;
		  shaderMaterialSix.SetShaderParameter("blue",0.6f);
		  shaderMaterialSix.SetShaderParameter("test_texture", sheet);
		  shaderMaterialSix.SetShaderParameter("top_left_region", clearRegion);
		  shaderMaterialSix.SetShaderParameter("top_right_region", clearRegion);
		  shaderMaterialSix.SetShaderParameter("bottom_left_region", bottomRegion);
		  shaderMaterialSix.SetShaderParameter("bottom_right_region", bottomRegion);
		
		  var shaderMaterialSeven = new ShaderMaterial();
		  shaderMaterialSeven.Shader = shader;
		  shaderMaterialSeven.SetShaderParameter("red",0.7f);
		  shaderMaterialSeven.SetShaderParameter("test_texture", sheet);
		  shaderMaterialSeven.SetShaderParameter("top_left_region", leftRegion);
		  shaderMaterialSeven.SetShaderParameter("top_right_region", clearRegion);
		  shaderMaterialSeven.SetShaderParameter("bottom_left_region", bottomLeftRegion);
		  shaderMaterialSeven.SetShaderParameter("bottom_right_region", bottomRegion);
		
		  var shaderMaterialEight = new ShaderMaterial();
		  shaderMaterialEight.Shader = shader;
		  shaderMaterialEight.SetShaderParameter("green",0.8f);
		  shaderMaterialEight.SetShaderParameter("test_texture", sheet);
		  shaderMaterialEight.SetShaderParameter("top_left_region", leftRegion);
		  shaderMaterialEight.SetShaderParameter("top_right_region", clearRegion);
		  shaderMaterialEight.SetShaderParameter("bottom_left_region", leftRegion);
		  shaderMaterialEight.SetShaderParameter("bottom_right_region", clearRegion);

		
		  var shaderMaterialNine = new ShaderMaterial();
		  shaderMaterialNine.Shader = shader;
		  shaderMaterialNine.SetShaderParameter("blue",0.9f);
		  shaderMaterialNine.SetShaderParameter("test_texture", sheet);
		  shaderMaterialNine.SetShaderParameter("top_left_region", topLeftRegion);
		  shaderMaterialNine.SetShaderParameter("top_right_region", topRegion);
		  shaderMaterialNine.SetShaderParameter("bottom_left_region", leftRegion);
		  shaderMaterialNine.SetShaderParameter("bottom_right_region", clearRegion);
		
		  var shaderMaterialTen = new ShaderMaterial();
		  shaderMaterialTen.Shader = shader;
		  shaderMaterialTen.SetShaderParameter("red",0.0f);
		  shaderMaterialTen.SetShaderParameter("test_texture", sheet);
		  shaderMaterialTen.SetShaderParameter("top_left_region", topLeftRegion);
		  shaderMaterialTen.SetShaderParameter("top_right_region", topRightRegion);
		  shaderMaterialTen.SetShaderParameter("bottom_left_region", bottomLeftRegion);
		  shaderMaterialTen.SetShaderParameter("bottom_right_region", bottomRightRegion);
		
		  var shaderMaterialEleven = new ShaderMaterial();
		  shaderMaterialEleven.Shader = shader;
		  shaderMaterialEleven.SetShaderParameter("red",0.0f);
		  shaderMaterialEleven.SetShaderParameter("test_texture", sheet);
		  shaderMaterialEleven.SetShaderParameter("top_left_region", leftRegion);
		  shaderMaterialEleven.SetShaderParameter("top_right_region", rightRegion);
		  shaderMaterialEleven.SetShaderParameter("bottom_left_region", leftRegion);
		  shaderMaterialEleven.SetShaderParameter("bottom_right_region", rightRegion);
		
		  var shaderMaterialTwelve = new ShaderMaterial();
		  shaderMaterialTwelve.Shader = shader;
		  shaderMaterialTwelve.SetShaderParameter("red",0.0f);
		  shaderMaterialTwelve.SetShaderParameter("test_texture", sheet);
		  shaderMaterialTwelve.SetShaderParameter("top_left_region", topRegion);
		  shaderMaterialTwelve.SetShaderParameter("top_right_region", topRegion);
		  shaderMaterialTwelve.SetShaderParameter("bottom_left_region", bottomRegion);
		  shaderMaterialTwelve.SetShaderParameter("bottom_right_region", bottomRegion);
		
		  var shaderMaterialThirteen = new ShaderMaterial();
		  shaderMaterialThirteen.Shader = shader;
		  shaderMaterialThirteen.SetShaderParameter("red",0.0f);
		  shaderMaterialThirteen.SetShaderParameter("test_texture", sheet);
		  shaderMaterialThirteen.SetShaderParameter("top_left_region", leftRegion);
		  shaderMaterialThirteen.SetShaderParameter("top_right_region", rightRegion);
		  shaderMaterialThirteen.SetShaderParameter("bottom_left_region", bottomLeftRegion);
		  shaderMaterialThirteen.SetShaderParameter("bottom_right_region", bottomRightRegion);
		
		  var shaderMaterialFourteen = new ShaderMaterial();
		  shaderMaterialFourteen.Shader = shader;
		  shaderMaterialFourteen.SetShaderParameter("red",0.0f);
		  shaderMaterialFourteen.SetShaderParameter("test_texture", sheet);
		  shaderMaterialFourteen.SetShaderParameter("top_left_region", topRegion);
		  shaderMaterialFourteen.SetShaderParameter("top_right_region", topRightRegion);
		  shaderMaterialFourteen.SetShaderParameter("bottom_left_region", bottomRegion);
		  shaderMaterialFourteen.SetShaderParameter("bottom_right_region", bottomRightRegion);
		
		  var shaderMaterialFifthteen = new ShaderMaterial();
		  shaderMaterialFifthteen.Shader = shader;
		  shaderMaterialFifthteen.SetShaderParameter("red",0.0f);
		  shaderMaterialFifthteen.SetShaderParameter("test_texture", sheet);
		  shaderMaterialFifthteen.SetShaderParameter("top_left_region", topLeftRegion);
		  shaderMaterialFifthteen.SetShaderParameter("top_right_region", topRightRegion);
		  shaderMaterialFifthteen.SetShaderParameter("bottom_left_region", leftRegion);
		  shaderMaterialFifthteen.SetShaderParameter("bottom_right_region", rightRegion);
		
		  var shaderMaterialSixteen = new ShaderMaterial();
		  shaderMaterialSixteen.Shader = shader;
		  shaderMaterialSixteen.SetShaderParameter("red",0.0f);
		  shaderMaterialSixteen.SetShaderParameter("test_texture", sheet);
		  shaderMaterialSixteen.SetShaderParameter("top_left_region", topLeftRegion);
		  shaderMaterialSixteen.SetShaderParameter("top_right_region", topRegion);
		  shaderMaterialSixteen.SetShaderParameter("bottom_left_region", bottomLeftRegion);
		  shaderMaterialSixteen.SetShaderParameter("bottom_right_region", bottomRegion);
		
		
		
		  if (leftN && topN && rightN && bottomN) {
			var atlasCoords = new Vector2I(1, 1);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialOne.SetShaderParameter("tile",1);
	   		SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialOne;
		  } else if (leftN && rightN && bottomN && (!topN)) {
			var atlasCoords = new Vector2I(1, 0);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialTwo.SetShaderParameter("tile",2);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialTwo;
		  } else if (leftN && bottomN && (!topN) && (!rightN)) {
			var atlasCoords = new Vector2I(2, 0);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialThree.SetShaderParameter("tile",3);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialThree;
		  } else if (leftN && topN && bottomN && (!rightN)) {
			var atlasCoords = new Vector2I(2, 1);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialFour.SetShaderParameter("tile",4);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialFour;
		  } else if (leftN && topN && (!rightN) && (!bottomN)) {
			var atlasCoords = new Vector2I(2, 2);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialFive.SetShaderParameter("tile",5);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialFive;
		  } else if (leftN && topN && rightN && (!bottomN)) {
			var atlasCoords = new Vector2I(1, 2);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialSix.SetShaderParameter("tile",6);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialSix;
		  } else if (topN && rightN && (!bottomN) && (!leftN)) {
			var atlasCoords = new Vector2I(0, 2);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialSeven.SetShaderParameter("tile",7);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialSeven;
		  } else if (topN && rightN && bottomN && (!leftN)) {
			var atlasCoords = new Vector2I(0, 1);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialEight.SetShaderParameter("tile",8);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialEight;
		  } else if (rightN && bottomN && (!leftN) && (!topN)) {
			var atlasCoords = new Vector2I(0, 0);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialNine.SetShaderParameter("tile",9);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialNine;
		  } else if (topN && bottomN && (!leftN) && (!rightN)) {
			var atlasCoords = new Vector2I(4, 0);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialEleven.SetShaderParameter("tile",11);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialEleven;
		  } else if (rightN && leftN && (!bottomN) && (!topN)) {
			var atlasCoords = new Vector2I(4, 1);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialTwelve.SetShaderParameter("tile",12);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialTwelve;
		  } else if (topN && (!rightN) && (!leftN) && (!bottomN)) {
			var atlasCoords = new Vector2I(4, 2);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialThirteen.SetShaderParameter("tile",13);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialThirteen;
		  } else if (leftN && (!rightN) && (!bottomN) && (!topN)) {
			var atlasCoords = new Vector2I(5, 0);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialFourteen.SetShaderParameter("tile",14);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialFourteen;
		  } else if (bottomN && (!rightN) && (!leftN) && (!topN)) {
			var atlasCoords = new Vector2I(5, 1);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialFifthteen.SetShaderParameter("tile",15);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialFifthteen;
		  } else if (rightN && (!leftN) && (!bottomN) && (!topN)) {
			var atlasCoords = new Vector2I(5, 2);
			int sourceId = GetCellSourceId(tile);
			shaderMaterialSixteen.SetShaderParameter("tile",16);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialSixteen;
		  } else {
			var atlasCoords = new Vector2I(3, 0);
			int sourceId = GetCellSourceId(tile);
			SetCell(tile, sourceId, atlasCoords);
			GetCellTileData(tile).Material = shaderMaterialTen;
		  }

		}
		
		//GD.Print("GRAPES");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
