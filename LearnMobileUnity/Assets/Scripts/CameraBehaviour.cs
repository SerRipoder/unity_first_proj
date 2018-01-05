using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will adjust the camera to follow and face a target
/// </summary>
public class CameraBehaviour : MonoBehaviour {

	[Tooltip("What object should camera be looking at")]
	public Transform target;

	[Tooltip("How offset will the camera be to the target")]
	public Vector3 offset;
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update () {
		if (target != null)
		{
			transform.position = target.position + offset;

			// Change the rotation to face target
			transform.LookAt (target);
		}
	}
}
