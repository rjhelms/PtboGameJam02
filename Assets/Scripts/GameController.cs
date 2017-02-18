using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public Actor Player;
	public float MoveSpeed = 120.0f;

	private PlayerFollower playerFollower;
	// Use this for initialization
	void Start () {
		playerFollower = FindObjectOfType<PlayerFollower>();
		Debug.Log(playerFollower);
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

		if (movement.magnitude > 0)
		{
			Player.Move(movement);
		}
	}

	public void RegisterPlayer (GameObject player)
	{
		Player = player.GetComponent<Actor>();
		Player.Initialize();
		Debug.Log(Player);
		playerFollower.SetPlayer(Player);
	}
}
