using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

	public static GameCore instance = null;
	public GameObject playerPrefab;
	public GameObject[] blockingTiles;
	public GameObject[] foregroundTiles;
	public GameObject[] backgroundTiles;
	public GameObject[] enemyPrefabs;

	private Transform levelHolder;

	public GameObject player = null;

	public GameObject logo;

	public int minWidth = 6;
	public int maxWidth = 10;
	public int minHeight = 2;
	public int maxHeight = 6;
	public int maxEnemies = 50;
	public int minEnemies = 20;
	public int safeZone = 10;


	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		InitGame();
	}

	void InitGame() {
		if (player == null) {
			player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}

		GenerateLevel(30);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HideLogo() {
		logo.GetComponent<Animator>().SetTrigger("Hide");
	}

	void GenerateLevel(int length) {
		levelHolder = new GameObject("Level").transform;

		GenerateGround(length);
		GenerateForeground(length);
		GenerateBackground(length);
		SpawnEnemies(length);
	}

	void SpawnEnemies(int length) {
		for (int i = 0; i < Random.Range(minEnemies, maxEnemies); i++) {
			GameObject instance = Instantiate(
				enemyPrefabs[0],
				new Vector3(Random.Range(safeZone, length), 0, 0), Quaternion.identity
			) as GameObject;

			instance.transform.SetParent(levelHolder);	
		}
	}

	void GenerateGround(int length) {
		for (int y = 0; y > -1; y --) {
			for (int x = -5; x < length; x ++) {
				GameObject toInstantiate;

				if (y == 0) {
					toInstantiate = blockingTiles[1];
				} else {
					toInstantiate = blockingTiles[2];
				}

				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

				instance.transform.SetParent(levelHolder);
			}
		}

		for (int y = 0; y < 5; y++) {
			for (int x = 0; x < 5; x++ ){
				GameObject toInstantiate;

				toInstantiate = blockingTiles[2];

				GameObject instance = Instantiate(toInstantiate, new Vector3(x -10, y, 0), Quaternion.identity) as GameObject;

				instance.transform.SetParent(levelHolder);

				instance = Instantiate(toInstantiate, new Vector3(length + x, y, 0), Quaternion.identity) as GameObject;	
			}
		}
	}

	void GenerateForeground(int length) {
		bool fence = false;

		for (int x = -5; x < length; x ++) {
			GameObject toInstantiate;

			if (fence) {
				toInstantiate = foregroundTiles[Random.Range(3,4)];
			} else {
				toInstantiate = foregroundTiles[Random.Range(0,1)];
			}

			if (Random.Range(0, 10) == 5 && !fence) {
				toInstantiate = foregroundTiles[2];
				fence = true;
			} else if (Random.Range(0, 10) == 5 && fence) {
				toInstantiate = foregroundTiles[5];
				fence = false;
			}

			GameObject instance = Instantiate(toInstantiate, new Vector3(x,1,0), Quaternion.identity) as GameObject;

			instance.transform.SetParent(levelHolder);
		}
	}

	void GenerateBackground(int length) {
		GenerateBuilding(
			Random.Range(0, length - maxWidth),
			Random.Range(minWidth, maxWidth)
		);
	}

	void GenerateBuilding(int xPosition, int width) {
		int height = Random.Range(minHeight, maxHeight);

		int[,] buildingSquares = new int[width,height + 1];

		//Generate Wall
		for (int y = 0; y < height + 1; y ++) {
			for (int x = 0; x < width; x++) {
				if(x == 0) {
					if (y == height) {
						buildingSquares[x,y] = 1;
					} else {
						buildingSquares[x,y] = 4;
					}
				} else if (x == width -1) {
					if (y == height) {
						buildingSquares[x,y] = 3;
					} else {
						buildingSquares[x,y] = 5;
					}
				} else {
					if (y == height) {
						buildingSquares[x,y] = 2;
					} else {
						buildingSquares[x,y] = 0;
					}
				}
			}
		}

		//Generate Door
		int doorStart = Random.Range(1, width - 2);
		buildingSquares[doorStart,1] = 6;
		buildingSquares[doorStart + 1, 1] = 7;
		buildingSquares[doorStart,0] = 8;
		buildingSquares[doorStart + 1, 0] = 9;

		for (int y = 0; y < height + 1; y ++) {
			for (int x = 0; x < width; x ++) {
				GameObject instance = Instantiate(
					backgroundTiles[buildingSquares[x,y]],
					new Vector3(x + xPosition, y + 1, 0),
					Quaternion.identity
				) as GameObject;

				instance.transform.SetParent(levelHolder);
			}
		}

	}
}
