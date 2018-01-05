using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour {
	
	/// <summary>
	/// A reference to the Rigidbody component
	/// </summary>
	private Rigidbody rb;

	[Tooltip("How fast the ball moves left/right")]
	public float dodgeSpeed = 5;

	[Tooltip("How fast ball moves automaticaly")]
	[Range(0, 10)]
	public float rollSpeed = 5;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
		rb.AddForce(horizontalSpeed, 0, rollSpeed);
	}
}
