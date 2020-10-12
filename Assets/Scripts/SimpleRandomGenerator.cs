using UnityEngine;

public class SimpleRandomGenerator : MonoBehaviour
{
    public int width;
    public int height;

	private GeneratorHelper helper { 
		get { return GetComponent<GeneratorHelper>(); } 
	}

    public void Generate() {
        Debug.Log($"generate map size {width}x{height}");

		// clean up previous map
		Clear();

		helper.AddTileType("dirt", helper.tilePrefabs["quad"], helper.materials["dirt"]);
		helper.AddTileType("rock", helper.tilePrefabs["cube"], helper.materials["stone"]);
		helper.AddTileType("tree", helper.tilePrefabs["tree-shape"], helper.materials["green"]);
		helper.AddTileType("water", helper.tilePrefabs["quad"], helper.materials["water"]);

		// test the map
		helper.InitMap(width, height, "dirt");

		// add some rocks/trees
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if(Random.value< 0.4f) {
					if (Random.value < 0.25f) {
						helper.SetTile(x, y, "rock");
					} else {
						helper.SetTile(x, y, "tree");
					}
				}
			}
		}

		// add a river
		Vector2 start = new Vector2(0, Mathf.Round(height / 2)-1  +Random.Range(-2, 2));
		Vector2 end = new Vector2(width-1, start.y + Random.Range(-2,2));
		AddRiver(start, end);
	}

	private void AddRiver(Vector2 start, Vector2 end) {
		float dx = end.x - start.x;
		float dy = end.y - start.y;
		float yToX = dy / dx;

		float y = start.y;
		int oldY;
		for (float x = start.x; x <= end.x; x++) {
			helper.SetTile(Mathf.RoundToInt(x), oldY=Mathf.RoundToInt(y), "water");
			y += yToX;

			if (oldY != Mathf.RoundToInt(y)) {
				helper.SetTile(Mathf.RoundToInt(x), Mathf.RoundToInt(y), "water");
			}
		}
	}

	public void Clear() {
		var map = helper.mapContainer;
		while (map.childCount > 0) {
			var tile = map.GetChild(0);
			tile.parent = null;
			DestroyImmediate(tile.gameObject);
		}
	}
}
