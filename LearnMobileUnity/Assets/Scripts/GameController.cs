using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the main gameplay
/// </summary>
public class GameController : MonoBehaviour {

	[Tooltip("A reference to the tile we want to spawn")]
	public Transform tile;

	[Tooltip("A reference to the obstacle we want to spawn")]
	public Transform obstacle;

	[Tooltip("Where the first tile should be placed at")]
	public Vector3 startPoint = new Vector3(0, 0, -5);

	[Tooltip("How many tiles should we create in advance")]
	[Range(3, 15)]
	public int initSpawnNum = 10;

	[Tooltip("How many tiles to spawn initially with no obstacles")]
	public int initNoObstacles = 4;

	/// <summary>
	/// Where the next tile should be spawned at.
	/// </summary>
	private Vector3 nextTileLocation;

	/// <summary>
	/// How should the next tile be rotated?
	/// </summary>
	private Quaternion nextTileRotation;

	// Use this for initialization
	void Start () {
		nextTileLocation = startPoint;
		nextTileRotation = Quaternion.identity;

		for (int i = 0; i < initSpawnNum; ++i)
		{
			SpawnNextTile(i >= initNoObstacles);
		}
	}

	/// <summary>
	/// Will spawn a tile at a certain location and setup the next position
	/// </summary>
	public void SpawnNextTile(bool spawnObstacles = true)
	{
		var newTile = Instantiate(tile, nextTileLocation,
			nextTileRotation);
		// Figure out where and at what rotation we should spawn
		// the next item
		var nextTile = newTile.Find("Next Spawn Point");
		nextTileLocation = nextTile.position;
		nextTileRotation = nextTile.rotation;

		if (!spawnObstacles)
			return;

		var obstaclesSpawnPoints = new List<GameObject>();

		foreach (Transform child in newTile)
		{
			if (child.CompareTag("ObstacleSpawn"))
			{
				obstaclesSpawnPoints.Add(child.gameObject);
			}
		}

		if (obstaclesSpawnPoints.Count > 0)
		{
			var spawnPoint = obstaclesSpawnPoints[Random.Range (0, obstaclesSpawnPoints.Count)];

			var spawnPos = spawnPoint.transform.position;

			var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

			newObstacle.SetParent(spawnPoint.transform);
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
