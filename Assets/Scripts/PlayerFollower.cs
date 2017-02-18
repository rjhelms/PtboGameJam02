using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

	public Actor Player;
	public float CameraZOffset = -10f;
	public float LerpSpeed = 1f;

	private Vector2 world_position;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		world_position = Vector2.Lerp(
						     world_position, 
							 Player.WorldPosition, 
							 Time.deltaTime * LerpSpeed);
		Vector3 new_position = new Vector3(
							       Mathf.RoundToInt(world_position.x),
								   Mathf.RoundToInt(world_position.y),
								   CameraZOffset);
		transform.position = new_position;
	}

	public void SetPlayer (Actor player)
	{
		Player = player;
		world_position = player.WorldPosition;
		Vector3 new_position = new Vector3(
							       Mathf.RoundToInt(world_position.x),
								   Mathf.RoundToInt(world_position.y),
								   CameraZOffset);
		transform.position = new_position;
	}
}
