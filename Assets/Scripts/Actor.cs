using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseEntity {

	public Sprite[] DirectionSprites;
	private SpriteRenderer renderer;

	protected override void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();
		base.Start();
	}
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public void Move(Vector2 move_vector)
	{
		// parcel up into 90 degrees for now - will do 45 later
		Vector2 up = Vector2.up;
		Vector2 down = -up;
		Vector2 right = Vector2.right;
		Vector2 left = -right;
		Vector2 normal_move = move_vector.normalized;
		if (Vector2.Dot(normal_move, up) > 0.707f)
		{
			renderer.sprite = DirectionSprites[0];
		} else if (Vector2.Dot(normal_move, left) > 0.707f)
		{
			renderer.sprite = DirectionSprites[1];
		} else if (Vector2.Dot(normal_move, down) > 0.707f)
		{
			renderer.sprite = DirectionSprites[2];
		} else if (Vector2.Dot(normal_move, right) > 0.707f)
		{
			renderer.sprite = DirectionSprites[3];
		}
		else {
			Debug.Log("fell through sprite cases" + move_vector);
		}
		worldPosition += move_vector;
	}
}
