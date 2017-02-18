using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour {

	private Vector2 worldPosition;
	private Vector2 screenPosition;
	public Vector2 WorldPosition
	{
		get { return worldPosition;}
		set { worldPosition = value;}
	}

	// Use this for initialization
	void Start () {
		worldPosition = (Vector2)transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		screenPosition.x = Mathf.RoundToInt(worldPosition.x);
		screenPosition.y = Mathf.RoundToInt(worldPosition.y);
		transform.position = screenPosition;
	}
}
