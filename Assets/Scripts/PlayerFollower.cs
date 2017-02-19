using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

	private Vector2 velocity;
	public PlayerEntity Player;
	public float CameraZOffset = -10f;
	public float LerpSpeed = 1f;

	[SerializeField]
	private Vector2 world_position;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		world_position = Vector2.SmoothDamp(world_position, Player.CameraTarget,
											ref velocity, LerpSpeed,
											Mathf.Infinity, Time.deltaTime);
		Vector3 screen_position = new Vector3(
							       Mathf.RoundToInt(world_position.x),
								   Mathf.RoundToInt(world_position.y),
								   CameraZOffset);
		transform.position = screen_position;
	}

	public void SetPlayer (PlayerEntity player)
	{
		Player = player;
		world_position = player.ScreenPosition;
		Vector3 screen_position = new Vector3(
							       Mathf.RoundToInt(world_position.x),
								   Mathf.RoundToInt(world_position.y),
								   CameraZOffset);
		transform.position = screen_position;
	}
}
