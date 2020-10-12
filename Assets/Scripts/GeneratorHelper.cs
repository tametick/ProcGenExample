using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHelper : MonoBehaviour
{
    public GenericDictionary<string, Material> materials;
    public GenericDictionary<string, GameObject> tilePrefabs;
    GameObject[,] map;
	public Transform mapContainer;

    public void InitMap(int width, int height, string defaultTile= "cube") {
        map = new GameObject[width, height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				AddTile(x, y, defaultTile);
			}
		}
	}

	private void AddTile(int x, int y, string tileType) {
		var tile = map[x, y] = Instantiate(tilePrefabs[tileType], mapContainer);
		tile.SetActive(true);
		tile.transform.position = new Vector3(y, 0, -x);
		tile.name = $"<{x}, {y}> {tileType}";
	}

	public void SetTile(int x, int y, string tileType) {
		// prototype of the new tile type we want to change <x,y> to
		var newTilePrototype = tilePrefabs[tileType];
		var tile = map[x, y];
		tile.GetComponent<MeshFilter>().sharedMesh = newTilePrototype.GetComponent<MeshFilter>().sharedMesh;
		tile.GetComponent<Renderer>().sharedMaterial = newTilePrototype.GetComponent<Renderer>().sharedMaterial;
		tile.transform.rotation = newTilePrototype.transform.rotation;
		tile.name = $"<{x}, {y}> {tileType}";
	}

	public void AddTileType(string tileName, GameObject prototype, Material material) {
		var newTileType = Instantiate(prototype, transform);
		newTileType.GetComponent<Renderer>().material = material;
		newTileType.name = tileName;
		if (tilePrefabs.ContainsKey(tileName)) {
			DestroyImmediate(tilePrefabs[tileName]);
			tilePrefabs.Remove(tileName);
		}

		tilePrefabs.Add(tileName, newTileType);
		newTileType.SetActive(false);
	}

}
