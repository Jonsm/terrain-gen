using UnityEngine;
using System.Collections;

public class TerrainProperties : MonoBehaviour {
	public enum TileTypes {MOUNTAIN, VOLCANO, JUNGLE, PLAIN, WATER};
	public GameObject[] meshes = new GameObject[5];
	public Texture[] textures = new Texture[5];

	public TileTypes type;
	public GameObject obj;
	public TerrainProperties[] neighbors;
	public int randomRotation;

	private GameObject mesh;
	private Texture texture;
	private int[] landNeighbors;

	public GameObject lava;
	public float lavaOffset;
	
	void Awake () {
		landNeighbors = new int[]{0,0,0,0,0,0};
		neighbors = new TerrainProperties[6];
		obj = gameObject;
		type = TileTypes.WATER;
	}

	public void BuildMesh () {
		if (type != TileTypes.WATER) {
			setLandNeighbors();
			setMeshAndTextures();
			Quaternion rotation = Quaternion.Euler (0,randomRotation * 60f,0);
			GameObject mesh1 = Instantiate (mesh, transform.position, rotation) as GameObject;
			mesh1.renderer.material.SetFloat ("_ShowBeach1", Mathf.Abs (landNeighbors[0] - 1));
			mesh1.renderer.material.SetFloat ("_ShowBeach2", Mathf.Abs (landNeighbors[1] - 1));
			mesh1.renderer.material.SetFloat ("_ShowBeach3", Mathf.Abs (landNeighbors[2] - 1));
			mesh1.renderer.material.SetFloat ("_ShowBeach4", Mathf.Abs (landNeighbors[3] - 1));
			mesh1.renderer.material.SetFloat ("_ShowBeach5", Mathf.Abs (landNeighbors[4] - 1));
			mesh1.renderer.material.SetFloat ("_ShowBeach6", Mathf.Abs (landNeighbors[5] - 1));
			mesh1.renderer.material.SetTexture ("_MainTex", texture);
			mesh1.transform.parent = gameObject.transform;
		}
	}

	private void setLandNeighbors() {
		for (int i = 0; i < neighbors.Length; i++) {
			if (neighbors[i]!=null && 
			    neighbors[i].type != TileTypes.WATER) landNeighbors[i] = 1;
		}

		int[] neighborsRotated = new int[6];
		int j = 5-randomRotation;
		for (int k = 5; k >= 0; k--) {
			if (j == -1) j = 5;
			neighborsRotated[j] = landNeighbors[k];
			j--;
		}
		landNeighbors = neighborsRotated;
	}

	private void setMeshAndTextures() {
		switch (type) {
			case TileTypes.VOLCANO: 
				spawnLava ();
				goto case TileTypes.MOUNTAIN;
			case TileTypes.MOUNTAIN:
				mesh = meshes[(int)type];
				texture = textures[(int)type];
				break;
			case TileTypes.JUNGLE:
			case TileTypes.PLAIN:
				int random = Random.Range (2,5);
				mesh = meshes[random];
				texture = textures[random];
				break;
		}
	}

	private void spawnLava() {
		Quaternion rot = Quaternion.Euler (90, 0, 0);
		Vector3 pos = transform.position + lavaOffset * Vector3.up;
		GameObject l = Instantiate (lava, pos, rot) as GameObject;
		l.transform.parent = gameObject.transform;
	}
}
