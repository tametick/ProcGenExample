
using UnityEngine;

public class DirectionalCaveGenerator : BaseGenerator {
	public float roughness;
	public float windyness;

	public override void Generate() {
		// clean up previous map
		helper.Clear();

		helper.AddTileType("dirt", helper.tilePrefabs["quad"], helper.materials["dirt"]);
		helper.AddTileType("rock", helper.tilePrefabs["cube"], helper.materials["stone"]);

		helper.InitMap(width, height, "rock");

		GenerateCave(width / 2, 0, Random.Range(3, width / 3));
	}

	private void GenerateCave(int x, int y, int width) {
		for (int yy = y; yy < height; yy++) {
			if(Random.value <= roughness) {
				width += RandomWithoutZero(-2, 2);
				width = Mathf.Clamp(width, 3, this.width / 2);
			}

			if(Random.value <= windyness) {
				x += RandomWithoutZero(-2, 2);
				x = Mathf.Clamp(x, 1, this.width - 2 - width);
			}

			for (int xx = x; xx < x + width; xx++) {
				helper.SetTile(xx, yy, "dirt");
			}
		}
	}

	private int RandomWithoutZero(int min, int max) {
		int val = 0;
		while(val == 0) {
			val = Random.Range(min, max+1);
		}
		return val;
	}
}
