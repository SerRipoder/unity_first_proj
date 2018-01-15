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
	public float dodgeSpeed = 10f;

	[Tooltip("How fast ball moves automaticaly")]
	[Range(0, 10)]
	public float rollSpeed = 5f;

	[Header("Swipe Properties")]
	[Tooltip("How far will the player move upon swiping")]
	public float swipeMove = 2f;
	[Tooltip("How far must the player swipe before we will execute the action (in pixel space)")]
	public float minSwipeDistance = 2f;
	/// <summary>
	/// Stores the starting position of mobile touch events
	/// </summary>
	private Vector2 touchStart;

	public enum MobileHorizMovement
	{
		Accelerometer,
		ScreenTouch
	}
	public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		var horizontalSpeed = 0f; //Input.GetAxis("Horizontal") * dodgeSpeed;

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		// Check if we're moving to the side
		horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

		if (Input.GetMouseButton(0))
		{
			horizontalSpeed = CalculateMovement(Input.mousePosition);
		}
		//Check if we are running on a mobile device
		#elif UNITY_IOS || UNITY_ANDROID
		if (horizMovement == MobileHorizMovement.Accelerometer)
			horizontalSpeed = Input.acceleration.x * dodgeSpeed;

		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];

			if (horizMovement == MobileHorizMovement.ScreenTouch)
				horizontalSpeed = CalculateMovement(touch.position);
			SwipeTeleport(touch);
			TouchObjects(touch);
		}
		#endif
		//Check if Input has regis
		rb.AddForce(horizontalSpeed, 0, rollSpeed);
	}

	/// <summary>
	/// Will figure out where to move the player horizontally
	/// </summary>
	/// <param name="pixelPos">The position the player has
	/// touched/clicked on</param>
	/// <returns>The direction to move in the x axis</returns>
	float CalculateMovement(Vector3 pixelPos)
	{
		// Converts to a 0 to 1 scale
		var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);
		float xMove = 0;

		if (worldPos.x < 0.5f)
			xMove = -1;
		else
			xMove = 1;


		return xMove * dodgeSpeed;
	}

	///<summary>
	///Will teleport the player if swiped to the left or right
	///</summary>
	///<param name="touch">Current touch event</param>
	private void SwipeTeleport(Touch touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			touchStart = touch.position;
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			Vector2 touchEnd = touch.position;
			float x = touchEnd.x - touchStart.x;
			if (Mathf.Abs(x) < minSwipeDistance)
				return;
			
			Vector3 moveDirection;
			if (x < 0)
				moveDirection = Vector3.left;
			else
				moveDirection = Vector3.right;

			RaycastHit hit;
			// Only move if we wouldn't hit something
			if (!rb.SweepTest(moveDirection, out hit, swipeMove))
				rb.MovePosition(rb.position + (moveDirection * swipeMove));
		}
	}

	/// <summary>
	/// Will determine if we are touching a game object and if so
	/// call events for it
	/// </summary>
	/// <param name="touch">Our touch event</param>
	private static void TouchObjects(Touch touch)
	{
		// Convert the position into a ray
		Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

		RaycastHit hit;

		if (Physics.Raycast(touchRay, out hit))
			hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
	}
}
