using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseEntity {

	public Sprite[] DirectionSprites;
	private SpriteRenderer sprite_renderer;
	private float slice_angle;
	private Vector2[] rotation_vectors;
	
	protected override void Start ()
	{
		slice_angle = Mathf.Cos(Mathf.Deg2Rad * 22.5f);
		rotation_vectors[0] = Vector2.up;
		rotation_vectors[1] = (Vector2.up - Vector2.right).normalized;
		rotation_vectors[2] = -Vector2.right;
		rotation_vectors[3] = (-Vector2.up - Vector2.right).normalized;
		rotation_vectors[4] = -Vector2.up;
		rotation_vectors[5] = (-Vector2.up + Vector2.right).normalized;
		rotation_vectors[6] = Vector2.right;
		rotation_vectors[7] = (Vector2.up + Vector2.right).normalized;
		sprite_renderer = GetComponent<SpriteRenderer>();
		base.Start();
	}
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public void Move(Vector2 move_vector)
	{
		Vector2 normal_move = move_vector.normalized;
		int target_sprite_index = -1;
		for (int i = 0; i < 8; i++)
		{
			if (Vector2.Dot(normal_move, rotation_vectors[i]) > slice_angle)
			{
				target_sprite_index = i;
			}
		}
		if (target_sprite_index == -1)
		{
			Debug.Log("fell through sprite cases" + move_vector);
		} else {
			sprite_renderer.sprite = DirectionSprites[target_sprite_index];
		}
		worldPosition += move_vector;
	}
}
