using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles spawning a new tile and destroying this one
/// upon the player reaching the end
/// </summary>
public class TileEndBehaviour : MonoBehaviour {

	[Tooltip("How much time to wait before destroying " +
		"the tile after reaching the end")]
	public float destroyTime = 1.5f;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.GetComponent<PlayerBehaviour>())
		{
			GameObject.FindObjectOfType<GameController>().SpawnNextTile();

			Destroy(transform.parent.gameObject, destroyTime);
		}
	}
}
