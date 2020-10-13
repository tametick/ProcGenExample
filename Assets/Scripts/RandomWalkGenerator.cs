using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalkGenerator : BaseGenerator {
	public override void Generate() {
		// clean up previous map
		helper.Clear();

		helper.AddTileType("dirt", helper.tilePrefabs["quad"], helper.materials["dirt"]);
		helper.AddTileType("rock", helper.tilePrefabs["cube"], helper.materials["stone"]);

		helper.InitMap(width, height, "rock");

		GenerateCave(width/2, height/2, width*height/4);
	}

	private void GenerateCave(int digX, int digY, int minTilesDug) {
		int tries = 10000;
		int dug = 0;
		while (dug < minTilesDug && tries-- >0) {
			// long term we probably want to add a Tile component with this data instead
			if (helper.GetTile(digX, digY).name.EndsWith("rock")) {
				helper.SetTile(digX, digY, "dirt");
				dug++;
			}

			int rand = Random.Range(0, 4);
			var dir = (Direction)rand;
			switch (dir) {
				case Direction.Up:
					digY++;
					break;
				case Direction.Down:
					digY--;
					break;
				case Direction.Right:
					digX++;
					break;
				case Direction.Left:
					digX--;
					break;
			}

			digX = Mathf.Clamp(digX, 1, width - 2);
			digY = Mathf.Clamp(digY, 1, height - 2);
		}
	}

	enum Direction {
		Up,
		Right,
		Down,
		Left
	}
}
