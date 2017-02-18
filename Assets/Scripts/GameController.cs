using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public BaseEntity Player;
	public float MoveSpeed = 120.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DoMovement();
	}

	void FixedUpdate () {
	}

	void DoMovement() {
		float x_movement = Input.GetAxis("Horizontal") * MoveSpeed;
		float y_movement = Input.GetAxis("Vertical") * MoveSpeed;
		Vector2 movement = new Vector2(x_movement, y_movement)
						   		* Time.deltaTime;
		Player.WorldPosition += movement;
	}
}
