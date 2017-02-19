using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor {
	public Vector2 FireVector;
	public float ProjectileSpeed;

	void FixedUpdate()
	{
		Move(FireVector);
	}
	public override void Move(Vector2 move_vector)
	{
		move_vector.Scale(new Vector2(1f, 0.833f));
		GetComponent<Rigidbody2D>().velocity = move_vector;
	}
	public void Initialize(Vector2 direction)
	{
		FireVector = direction.normalized * ProjectileSpeed;
		base.Initialize();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy")
		{
			Enemy hit_enemy = coll.gameObject.GetComponent<Enemy>();
			hit_enemy.Die();
			Destroy(gameObject);
			Destroy(EntitySprite.gameObject);
		} else if (coll.gameObject.tag == "Terrain")
		{
			Destroy(gameObject);
			Destroy(EntitySprite.gameObject);
		}
		
	}
}
