using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene

public class ObstacleBehaviour : MonoBehaviour {

	[Tooltip("How long to wait before restarting the game")]
	public float waitTime = 2.0f;

	public GameObject explosion;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.GetComponent<PlayerBehaviour>())
		{
			Destroy(col.gameObject);

			Invoke("ResetGame", waitTime);
		}
	}

	/// <summary>
	/// Will restart the currently loaded level
	/// </summary>
	void ResetGame()
	{
		// Restarts the current level
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	/// <summary>
	/// If the object is tapped, we spawn an explosion and
	/// destroy this object
	/// </summary>
	void PlayerTouch()
	{
		if (explosion != null)
		{
			var particles = Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy(particles, 1.0f);
		}

		Destroy(this.gameObject);
	}
}
