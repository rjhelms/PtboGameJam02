using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public PlayerEntity Player;
	public float MoveSpeed = 120.0f;

	private PlayerFollower playerFollower;
	// Use this for initialization
	void Start () {
		playerFollower = FindObjectOfType<PlayerFollower>();
		Debug.Log(playerFollower);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate () {
		DoMovement();
	}

	void DoMovement() {
		float x_movement = Input.GetAxis("Horizontal") * MoveSpeed;
			// fudge aspect ratio correction for movement
		float y_movement = Input.GetAxis("Vertical") * MoveSpeed / 1.2f; 
		Vector2 movement = new Vector2(x_movement, y_movement);
		Player.Move(movement);
	}

	public void RegisterPlayer (GameObject player)
	{
		Player = player.GetComponent<PlayerEntity>();
		Player.Initialize();
		Debug.Log(Player);
		playerFollower.SetPlayer(Player);
	}
}
