using System.Collections.Generic;
using UnityEngine;

public class SimpleRandomGenerator : BaseGenerator {
    public override void Generate() {
        Debug.Log($"generate map size {width}x{height}");

		// clean up previous map
		helper.Clear();

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
		Vector2 end = new Vector2(width, start.y + Random.Range(-2,2));
		foreach (var subriver in MidpointDisplace(start, end, 4)) {
			AddRiver(subriver.start, subriver.end);
		}
	}

	private void AddRiver(Vector2 start, Vector2 end) {
		float x1 = Mathf.Clamp(start.x, 0, width - 1);
		float x2 = Mathf.Clamp(end.x, 0, width - 1);
		float y1 = Mathf.Clamp(start.y, 0, height - 1);
		float y2 = Mathf.Clamp(end.y, 0, height-1);

		float dx = x2 - x1;
		float dy = y2 - y1;
		float yToX = dy / dx;

		float y = y1;
		int oldY;
		for (float x = x1; x <= x2; x++) {
			helper.SetTile(Mathf.RoundToInt(x), oldY=Mathf.RoundToInt(y), "water");
			y += yToX;

			if (oldY != Mathf.RoundToInt(y)) {
				helper.SetTile(Mathf.RoundToInt(x), Mathf.RoundToInt(y), "water");
			}
		}
	}

	internal struct SubRiver {
		internal Vector2 start;
		internal Vector2 end;
		internal SubRiver(Vector2 start, Vector2 end) {
			this.start = start;
			this.end = end;
		}
	}

	List<SubRiver> MidpointDisplace(Vector2 start, Vector2 end, float scale) {
		var result = new List<SubRiver>();

		// halt condition, we are done
		if (scale < 2) {
			result.Add(new SubRiver(start, end));
			return result;
		}

		Vector2 mid;
		Vector2 direction;
		float amount;
		
		// Find the midpoint
		mid = (start + end) / 2;
		
		// This is the displacement direction.
		// We are displacing along the y axis.
		direction = new Vector2(0, 1);
		
		// Find the amount to displace.
		// We start with a random number -1..1
		amount = Random.value<0.5f?-1:1;
		
		// We now scale by the current scale amount
		amount *= scale;
		
		// We now update our scale amount.  If we left the
		// scale unchanged, we'd end up with increasingly
		// spiky line.
		// Because we cut the length of the line in half with
		// every iteration, multiplying the scale by .5 will
		// maintain the same "spikiness" at all levels.  A value
		// less than .5 will cause things to quickly smooth out.
		scale *= 0.5f;
		
		// And do the displacement.
		mid += direction * amount;
		
		// Now, create & displace the two resulting
		// linesegments am & mb...
		result.AddRange(MidpointDisplace(start, mid, scale));
		result.AddRange(MidpointDisplace(mid, end, scale));

		return result;
	}
}
