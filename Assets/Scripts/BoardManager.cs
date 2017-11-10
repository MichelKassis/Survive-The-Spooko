using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {


	public class count
	{
		public int minimum;
		public int maximum;

		public count(int min, int max){

			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public count wallCount = new count (5,9);
	public count foodCount = new count (1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	//Make the Squares (grid) of game by foor loops after clearing it according to number of columns and rows (making ossible positions)
	void InitaliseList(){

		gridPositions.Clear ();

		for (int x = 1; x<columns-1;x++){

			for(int y= 1; y<rows-1;y++)
			{
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	//layout background and border tiles in the list
	void boardSetup(){

		boardHolder = new GameObject ("Board").transform;

		//laying(instantiating) the inner square of the floor tiles for grass/zombies 
		for (int x = -1; x< columns + 1;x++){

			for(int y= -1; y < rows+1;y++)
			{
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				//instatite the borders
				if (x == -1 || x == columns || y == -1 || y == rows) {

					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				}
					//Border/Tile instance
					GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(boardHolder);

			}
		}
	}
		

	//returning a random Position and making sure it doesnt return that number again to avoid spawning objects at same tile
	Vector3 RandomPosition(){

		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
}


	//Spawning the tiles at generated random position of objects
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range (minimum, maximum +1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray [Random.Range(0,tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);


		}
	}

	public void SetupScene(int Level){
		boardSetup ();
		InitaliseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log (Level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);

	}


}
