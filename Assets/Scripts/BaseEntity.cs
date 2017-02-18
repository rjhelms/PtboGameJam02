using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour {

	[SerializeField]
	protected Vector2 worldPosition;
	protected Vector2 screenPosition;
	[SerializeField]
	protected Vector2 screenPositionOffset;
	public Vector2 WorldPosition
	{
		get { return worldPosition;}
		set { worldPosition = value;}
	}

	// Use this for initialization
	protected virtual void Start () {
		worldPosition = (Vector2)transform.position;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		screenPosition.x = Mathf.RoundToInt(worldPosition.x);
		screenPosition.y = Mathf.RoundToInt(worldPosition.y);
		transform.position = screenPosition + screenPositionOffset;
	}
}
