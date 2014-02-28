using UnityEngine;
using System.Collections;

public class MapBuilder : MonoBehaviour {
	public int width;
	public int height;
	private int heightIncrements;
	private int widthIncrements;
	private int count;

	public GameObject oceanFloor;
	public GameObject ocean;
	public GameObject tile;
	public int[] concentrations = new int[5];

	TerrainProperties[][] scripts;

	void Start() {
		heightIncrements = Mathf.RoundToInt (height/.433f);
		widthIncrements = Mathf.RoundToInt (width /1.5f);
		Vector3 resize = new Vector3 (width + 2, height + 2, 1);
		oceanFloor.transform.localScale = resize;
		ocean.transform.localScale = resize;
		StartCoroutine ("Build");
	}

	IEnumerator Build() {
		CreateTiles ();
		PopulateRandom ();
		AddNeighbors ();
		Place ();
		yield return null;
	}

	void CreateTiles() {
		scripts = new TerrainProperties[heightIncrements + 1][];
		for (int z = 0; z <= heightIncrements; z++) {
			int mod = (int)Mathf.Abs(z%2);
			scripts[z] = new TerrainProperties[widthIncrements - mod + 1];
			for (int x = 0; x <= widthIncrements - mod; x++) {
				Vector3 pos = new Vector3 ((x - widthIncrements/2)*1.5f + mod * .75f, 0, (z - heightIncrements/2) * .433f);
				GameObject t = Instantiate (tile, pos, Quaternion.identity) as GameObject;
				TerrainProperties tp = t.GetComponent<TerrainProperties>() as TerrainProperties;;
				scripts[z][x] = tp;
				count++;
			}
		}		
	}
	
	void PopulateRandom() {
		int[] quantities = new int[concentrations.Length - 1];
		for (int i = 0; i < quantities.Length; i++) {
			float amount = count * (float)concentrations[i]/100;
			quantities[i] = Mathf.RoundToInt(amount);
		}

		for (int i = 0; i < quantities.Length; i++) {
			while(quantities[i] > 0) {
				int z = Random.Range (0,scripts.Length);
				int x = Random.Range (0,scripts[z].Length);
				if (scripts[z][x].type==TerrainProperties.TileTypes.WATER) {
					scripts[z][x].type = (TerrainProperties.TileTypes)i;
					quantities[i]--;
				}
			}
		}
	}

	void AddNeighbors() { 
		for (int i = 0; i < scripts.Length; i++) {
			for (int j = 0; j < scripts[i].Length; j++) {
				SetNeighbors(i,j);
			}
		}
	}

	void Place() { 
		for (int i = 0; i < scripts.Length; i++) {
			for (int j = 0; j < scripts[i].Length; j++) {
				scripts[i][j].randomRotation = Random.Range (0,6);
				scripts[i][j].BuildMesh ();
			}
		}
	}

	void SetNeighbors(int i, int j) {
		int[,] neighborsToSet;
		if (i % 2 == 0) {
			neighborsToSet = new int[,]{{i-2,j},{i-1,j-1},{i+1,j-1},{i+2,j},{i+1,j},{i-1,j}};
		} else {
			neighborsToSet = new int[,]{{i-2,j},{i-1,j},{i+1,j},{i+2,j},{i+1,j+1},{i-1,j+1}};
		}
		
		for(int k = 0; k < neighborsToSet.GetLength (0); k++) {
			if (neighborsToSet[k,0] >= 0 && neighborsToSet[k,0] < scripts.Length
			    && neighborsToSet[k,1] >= 0 && neighborsToSet[k,1] < scripts[neighborsToSet[k,0]].Length) {
				int flipped = (k - 3 >= 0) ? k - 3 : k + 3;
				scripts[neighborsToSet[k,0]][neighborsToSet[k,1]].neighbors[flipped] = scripts[i][j];
			}
		}
	}
}
