using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

	private Vector2 velocity;
	public PlayerEntity Player;
	public float CameraZOffset = -10f;
	public float LerpSpeed = 1f;
	public Transform Pointer;
	[SerializeField]
	private Vector2 world_position;
	private GameController controller;
	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<GameController>();
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
		if (controller.Target != null)
		{
			RaycastHit2D pointer_location = Physics2D.Linecast(
				transform.position, 
				(Vector2)controller.Target.transform.position + new Vector2(16, 16),
				LayerMask.GetMask("UI"));
			if (pointer_location.collider == null)
			{
				controller.Pointer.enabled = false;
			} else {
				if (pointer_location.collider.name == "Pointer_Collider_Top")
				{
					controller.Pointer.sprite = controller.PointerSprites[0];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Left")
				{
					controller.Pointer.sprite = controller.PointerSprites[1];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Bottom")
				{
					controller.Pointer.sprite = controller.PointerSprites[2];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Right")
				{
					controller.Pointer.sprite = controller.PointerSprites[3];
				}
				controller.Pointer.enabled = true;
				Pointer.position = pointer_location.point;
			}
		}
		Pointer.position = new Vector3(
			Mathf.RoundToInt(Pointer.position.x),
			Mathf.RoundToInt(Pointer.position.y),
			Mathf.RoundToInt(Pointer.position.z));
	}

	public void SetPlayer (PlayerEntity player)
	{
		Player = player;
		world_position = player.OffsetPosition;
		Vector3 screen_position = new Vector3(
							       Mathf.RoundToInt(world_position.x),
								   Mathf.RoundToInt(world_position.y),
								   CameraZOffset);
		transform.position = screen_position;
	}
}
